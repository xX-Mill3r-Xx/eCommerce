using eCommerce.Console.Database;

var db = new CursoEfcoreContext();

Console.WriteLine($"Lista de Usuarios da Database: Curso EFCore");

foreach (var usuario in db.Usuarios)
{
    Console.WriteLine($"Nome:{usuario.Nome} | RG:{usuario.Rg} | Email:{usuario.Email}");
}

Console.ReadKey();
