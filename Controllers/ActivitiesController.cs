using System.Security.Claims;
using CalendarioAcademico.Models;
using CalendarioAcademico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CalendarioAcademico.Controllers;
[Authorize] public class ActivitiesController(Store store) : Controller {
 private string Uid => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
 public IActionResult Index(int? subjectId,string? kind) {
 ViewBag.Subjects=store.Read(db=>db.Subjects.Where(x=>x.OwnerId==Uid).ToList()); ViewBag.SubjectId=subjectId; ViewBag.Kind=kind;
 return View(store.Read(db=>db.Activities.Where(x=>x.OwnerId==Uid && (!subjectId.HasValue || x.SubjectId==subjectId) && (string.IsNullOrEmpty(kind) || x.Kind==kind)).OrderBy(x=>x.Date).ToList()));
 }
 private void Subjects() => ViewBag.Subjects=store.Read(db=>db.Subjects.Where(x=>x.OwnerId==Uid).ToList());
 public IActionResult Edit(int id=0, int subjectId=0) { Subjects(); var a=id==0?new Activity{SubjectId=subjectId}:store.Read(db=>db.Activities.FirstOrDefault(x=>x.Id==id && x.OwnerId==Uid)); return a is null?NotFound():View(a); }
 [HttpPost] public IActionResult Edit(Activity form) {
 if(form.Date.Year<1901 || form.Date.Year>2099) ModelState.AddModelError("Date","Selecciona una fecha válida entre 1901 y 2099.");
 if(!store.Read(db=>db.Subjects.Any(x=>x.Id==form.SubjectId && x.OwnerId==Uid))) ModelState.AddModelError("SubjectId","Selecciona una de tus materias.");
 if(!ModelState.IsValid) { Subjects(); return View(form); }
 var ok=store.Write(db=> { if(!db.Subjects.Any(x=>x.Id==form.SubjectId && x.OwnerId==Uid)) return false;
 var a=db.Activities.FirstOrDefault(x=>x.Id==form.Id && x.OwnerId==Uid); if(form.Id!=0 && a is null) return false;
 if(a is null) { a=new Activity{Id=db.Activities.Select(x=>x.Id).DefaultIfEmpty().Max()+1,OwnerId=Uid}; db.Activities.Add(a); }
 a.SubjectId=form.SubjectId; a.Title=form.Title; a.Kind=form.Kind; a.Description=form.Description; a.Date=form.Date; a.Place=form.Place; a.Priority=form.Priority; a.Status=form.Status; return true; });
 return ok?RedirectToAction("Index"):NotFound();
 }
 [HttpPost] public IActionResult Delete(int id) { store.Write(db=>db.Activities.RemoveAll(x=>x.Id==id && x.OwnerId==Uid)); return RedirectToAction("Index"); }
}
