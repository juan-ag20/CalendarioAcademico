using System.Security.Claims;
using CalendarioAcademico.Models;
using CalendarioAcademico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CalendarioAcademico.Controllers;
[Authorize] public class SubjectsController(Store store) : Controller {
 private string Uid => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
 public IActionResult Index() => View(store.Read(db=>db.Subjects.Where(x=>x.OwnerId==Uid).ToList()));
 public IActionResult Edit(int id=0) { var s=id==0?new Subject():store.Read(db=>db.Subjects.FirstOrDefault(x=>x.Id==id && x.OwnerId==Uid)); return s is null?NotFound():View(s); }
 [HttpPost] public IActionResult Edit(Subject form) {
 if(!ModelState.IsValid) return View(form);
 var ok=store.Write(db=> { var s=db.Subjects.FirstOrDefault(x=>x.Id==form.Id && x.OwnerId==Uid); if(form.Id!=0 && s is null) return false;
 if(s is null) { s=new Subject{Id=db.Subjects.Select(x=>x.Id).DefaultIfEmpty().Max()+1,OwnerId=Uid}; db.Subjects.Add(s); }
 s.Name=form.Name; s.Teacher=form.Teacher; s.Schedule=form.Schedule; s.Room=form.Room; s.Description=form.Description; return true; });
 return ok?RedirectToAction("Index"):NotFound();
 }
 [HttpPost] public IActionResult Delete(int id) { store.Write(db=> { if(!db.Subjects.Any(x=>x.Id==id && x.OwnerId==Uid)) return false; db.Activities.RemoveAll(x=>x.SubjectId==id && x.OwnerId==Uid); db.Subjects.RemoveAll(x=>x.Id==id && x.OwnerId==Uid); return true; }); return RedirectToAction("Index"); }
}
