using System.Security.Claims;
using CalendarioAcademico.Models;
using CalendarioAcademico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CalendarioAcademico.Controllers;
[Authorize] public class HomeController(Store store) : Controller {
 public IActionResult Index(int? year,int? month) {
 var today=DateTime.Today; var y=year??today.Year; var m=month??today.Month;
 if(y<1901 || y>2099 || m<1 || m>12) return BadRequest();
 var uid=User.FindFirstValue(ClaimTypes.NameIdentifier)!;
 return View(store.Read(db=>new Dashboard(db.Subjects.Where(x=>x.OwnerId==uid).ToList(),db.Activities.Where(x=>x.OwnerId==uid).OrderBy(x=>x.Date).ToList(),new DateTime(y,m,1))));
 }
 [AllowAnonymous] public IActionResult Error() => Problem("Ocurrió un error. Revisa los registros del servidor e inténtalo de nuevo.");
}
