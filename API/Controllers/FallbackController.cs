using System.IO;
namespace API.Controllers;
public class FallbackController : Controller
{
    public ActionResult Index() => 
        PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","index.html"),"text/HTML");
}
