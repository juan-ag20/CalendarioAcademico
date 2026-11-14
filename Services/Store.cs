using System.Text.Json;
using CalendarioAcademico.Models;
namespace CalendarioAcademico.Services;
public class Store {
 private readonly object gate = new();
 private readonly string path;
 public Store(IWebHostEnvironment env) { var dir = Path.Combine(env.ContentRootPath,"App_Data"); Directory.CreateDirectory(dir); path = Path.Combine(dir,"calendar.json"); }
 private Database Load() => File.Exists(path) ? JsonSerializer.Deserialize<Database>(File.ReadAllText(path)) ?? throw new InvalidDataException("Archivo de datos inválido") : new();
 public T Read<T>(Func<Database,T> action) { lock(gate) return action(Load()); }
 public T Write<T>(Func<Database,T> action) { lock(gate) { var db=Load(); var result=action(db); var temp=path+".tmp"; File.WriteAllText(temp,JsonSerializer.Serialize(db)); File.Move(temp,path,true); return result; } }
}
