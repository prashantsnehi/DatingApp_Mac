import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Member } from '../_models/member';
import { environment } from 'src/environments/environment';

// const httpOptions = {
//   headers: new HttpHeaders({
//   Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
//   })
// }
@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>('https://localhost:5001/api/users');
  }

  getMember(username: string) {
    return this.http.get<Member>('https://localhost:5001/api/users/' + username);
  }
}
