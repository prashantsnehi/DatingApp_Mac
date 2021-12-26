import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { BusyService } from './busy.service';
import { getPaginationHeaders, getPaginatedResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl: string = environment.apiUrl;
  hubUrl: string = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageSource = new BehaviorSubject<Message[]>([]);

  messageThread$ = this.messageSource.asObservable();

  constructor(private http: HttpClient, private busyService: BusyService) { }

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.hubUrl}message?user=${otherUsername}`, {
          accessTokenFactory: () => user.token
        })
        .withAutomaticReconnect()
        .build();
    
    this.hubConnection.start().catch(error => console.log(error)).finally(() => this.busyService.idle());
    this.hubConnection.on("ReceiveMessageThread", messages => {
      this.messageSource.next(messages);
    })

    this.hubConnection.on("NewMesssage", message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.messageSource.next([...messages, message])
      })
    })

    this.hubConnection.on("UpdateGroup", (group: Group) => {
      if(group.connections.some(x => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if(!message.dateRead) {
              message.dateRead = new Date(Date.now())
            }
          })
          this.messageSource.next([...messages])
        })
      }
    })
  }

  stopHubConnection() {
    if(this.hubConnection) {
      this.messageSource.next([]);
      this.hubConnection.stop();
    }
  }

  getMessages(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('Container', container);

    return getPaginatedResult<Message[]>(`${this.baseUrl}messages`, params, this.http);
  }

  getMessageThread = (username: string) => 
    this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);

  // sendMessage = (username: string, content: string) => 
  //   this.http.post<Message>(`${this.baseUrl}messages`, { recipientUsername: username, content});
  async sendMessage(username: string, content: string) {
    return this.hubConnection.invoke("SendMessageAsync", { recipientUserName: username, content })
            .catch(error => console.log(error));
  }

  deleteMessage = (id: number) => 
    this.http.delete(`${this.baseUrl}messages/${id}`);
}
