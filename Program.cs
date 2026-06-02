using Microsoft.Data.Sqlite; 

//создаем соединение с файлом бд
using var connection = new SqliteConnection("Data Source=users.db"); 
connection.Open(); 
//users.db создасться сам при запуске

var createTableCommand = connection.CreateCommand();
createTableCommand.CommandText = @"
    CREATE TABLE IF NOT EXISTS Users (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Username TEXT NOT NULL,
        Email TEXT NOT NULL,
        Password TEXT NOT NULL
    );";
createTableCommand.ExecuteNonQuery();
Console.WriteLine("Таблица user создана успешно!"); 

var builder = WebApplication.CreateBuilder(args);

// добавление контроллеров 
builder.Services.AddControllers();

var app = builder.Build();

// Раздача статических файлов (HTML, CSS, JS)
app.UseDefaultFiles(); 
app.UseStaticFiles();  // файлы из папки wwwroot

// Подключение маршрутов контроллеров 
app.MapControllers();

// Перенаправления на html страницы
app.MapGet("/login", () => Results.Redirect("/login_form.html")); //вход
app.MapGet("/register", () => Results.Redirect("/registation_form.html")); //регистрация

// Обработка регистрации
app.MapPost("/api/auth/register", async (RegistrationRequest data) =>
{
    Console.WriteLine($"Регистрация: {data.Username}, Email: {data.Email}, Password: {data.Password}");

    // Добавляем пользователя в таблицу (INSERT INTO)
    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "INSERT INTO Users (Username, Email, Password) VALUES (@username, @email, @password)";
    command.Parameters.AddWithValue("@username", data.Username);
    command.Parameters.AddWithValue("@email", data.Email);
    command.Parameters.AddWithValue("@password", data.Password);
    
    int number = await command.ExecuteNonQueryAsync();
    Console.WriteLine($"В таблицу успешно добавлено {number} строк/a\n"); 
    Console.Write("Регистрация на стороне сервера прошла успешно!\n");
    return Results.Ok("Регистрация выполнена успешно!");
    
}); 

app.MapPost("/api/auth/login", async (LoginRequest data) =>
{
    Console.WriteLine($"Вход: {data.Username}, Password: {data.Password}");
    
    string sql = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
    
    using var command = new SqliteCommand(sql, connection);
    command.Parameters.AddWithValue("@username", data.Username);
    command.Parameters.AddWithValue("@password", data.Password);
    
    using var reader = await command.ExecuteReaderAsync();
    
    if (reader.HasRows) 
    {
        Console.Write("Вход на стороне сервера выболнен успешно!\n");
        return Results.Ok("Вход выполнен успешно!");
    }
    else 
    {
        Console.Write("Пользователя нет в базе данных!\n");
        return Results.NotFound("Пользователь не найден или неверный пароль");
    }
});

app.Run();

public record RegistrationRequest(string Username, string Email, string Password);
public record LoginRequest(string Username, string Password);
