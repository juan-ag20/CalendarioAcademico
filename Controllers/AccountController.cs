using System.Security.Claims;
using CalendarioAcademico.Models;
using CalendarioAcademico.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace CalendarioAcademico.Controllers;
public class AccountController(Store store) : Controller {
 private readonly PasswordHasher<Student> hasher = new();
 [HttpGet] public IActionResult Login() => View(new AccountForm());
 [HttpGet] public IActionResult Register() => View(new AccountForm());
 [HttpPost] public async Task<IActionResult> Register(AccountForm form) {
 if(string.IsNullOrWhiteSpace(form.Name)) ModelState.AddModelError("Name","Escribe tu nombre y apellido.");
 if(!ModelState.IsValid) return View(form);
 var student=store.Write(db => {
 if(db.Students.Any(x=>x.Email.Equals(form.Email.Trim(),StringComparison.OrdinalIgnoreCase))) return null;
 var s=new Student {Name=form.Name!.Trim(),Email=form.Email.Trim().ToLowerInvariant()}; s.PasswordHash=hasher.HashPassword(s,form.Password); db.Students.Add(s); return s;
 });
 if(student is null) { ModelState.AddModelError("","Ese correo ya está registrado."); return View(form); }
 await SignIn(student); return RedirectToAction("Index","Home");
 }
 [HttpPost] public async Task<IActionResult> Login(AccountForm form) {
 if(!ModelState.IsValid) return View(form);
 var s=store.Read(db=>db.Students.FirstOrDefault(x=>x.Email.Equals(form.Email.Trim(),StringComparison.OrdinalIgnoreCase)));
 if(s is null || hasher.VerifyHashedPassword(s,s.PasswordHash,form.Password)==PasswordVerificationResult.Failed) { ModelState.AddModelError("","Correo o contraseña incorrectos."); return View(form); }
 await SignIn(s); return RedirectToAction("Index","Home");
 }
 private Task SignIn(Student s) => HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier,s.Id),new Claim(ClaimTypes.Name,s.Name)},CookieAuthenticationDefaults.AuthenticationScheme)));
 [HttpPost] public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(); return RedirectToAction("Login"); }
}
