var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // 👈 vigtigt

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // 👈 vigtigt

app.Run();
