using StoreLib.Contexts;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddRazorPages();
builder.Services.AddDbContext<StoreDbContext>(); //Добавление контекста
builder.Services.AddSession(); //добавление сессий
builder.Services.AddHttpContextAccessor(); // добавление Accessor-а для хранения информации о текущем пользователе 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
