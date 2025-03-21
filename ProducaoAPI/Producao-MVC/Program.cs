using Microsoft.AspNetCore.Components.Authorization;
using Producao_MVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthAPI>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CookieHandler>();
builder.Services.AddScoped<FormaAPI>();
builder.Services.AddScoped<MaquinaAPI>();
builder.Services.AddScoped<ProdutoAPI>();
builder.Services.AddScoped<MateriaPrimaAPI>();
builder.Services.AddScoped<ProcessoProducaoAPI>();
builder.Services.AddScoped<AuthAPI>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<CookieHandler>();
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
