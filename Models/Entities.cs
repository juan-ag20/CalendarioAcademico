using System.ComponentModel.DataAnnotations;
namespace CalendarioAcademico.Models;
public class Student {
 public string Id { get; set; } = Guid.NewGuid().ToString();
 public string Name { get; set; } = "";
 public string Email { get; set; } = "";
 public string PasswordHash { get; set; } = "";
}
public class Subject {
 public int Id { get; set; }
 public string OwnerId { get; set; } = "";
 [Required(ErrorMessage="Escribe el nombre."), StringLength(100)] public string Name { get; set; } = "";
 [StringLength(100)] public string? Teacher { get; set; }
 [StringLength(100)] public string? Schedule { get; set; }
 [StringLength(100)] public string? Room { get; set; }
 [StringLength(2000)] public string? Description { get; set; }
}
public class Activity {
 public int Id { get; set; }
 public string OwnerId { get; set; } = "";
 public int SubjectId { get; set; }
 [Required, StringLength(150)] public string Title { get; set; } = "";
 [Required, RegularExpression("^(Tarea|Examen|Apunte)$")] public string Kind { get; set; } = "Tarea";
 [StringLength(10000)] public string? Description { get; set; }
 public DateTime Date { get; set; } = DateTime.Today;
 [StringLength(100)] public string? Place { get; set; }
 [RegularExpression("^(Baja|Media|Alta)$")] public string Priority { get; set; } = "Media";
 [RegularExpression("^(Pendiente|En proceso|Completado)$")] public string Status { get; set; } = "Pendiente";
}
public class Database {
 public List<Student> Students { get; set; } = new();
 public List<Subject> Subjects { get; set; } = new();
 public List<Activity> Activities { get; set; } = new();
}
public class AccountForm {
 [StringLength(100)] public string? Name { get; set; }
 [Required, EmailAddress] public string Email { get; set; } = "";
 [Required, StringLength(100, MinimumLength=8, ErrorMessage="La contraseña debe tener entre 8 y 100 caracteres.")] public string Password { get; set; } = "";
}
public record Dashboard(List<Subject> Subjects, List<Activity> Activities, DateTime Month);
