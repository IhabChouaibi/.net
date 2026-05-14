using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.Services;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SectionName));

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "LivraisonFrontend.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();

builder.Services.AddTransient<ApiAuthorizationHandler>();
builder.Services.AddScoped<SessionManager>();
builder.Services.AddScoped<AuthSessionHelper>();
builder.Services.AddScoped<ClientAccessGuard>();
builder.Services.AddScoped<AdminAccessGuard>();

builder.Services.AddHttpClient("GatewayClient", (_, client) =>
    {
        var apiSettings = builder.Configuration.GetSection(ApiSettings.SectionName).Get<ApiSettings>() ?? new ApiSettings();
        client.BaseAddress = new Uri(apiSettings.GatewayBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(20);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    })
    .AddHttpMessageHandler<ApiAuthorizationHandler>();

builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<IClientApiService, ClientApiService>();
builder.Services.AddScoped<ILivreurApiService, LivreurApiService>();
builder.Services.AddScoped<IColisApiService, ColisApiService>();
builder.Services.AddScoped<IVehiculeApiService, VehiculeApiService>();
builder.Services.AddScoped<IPaiementApiService, PaiementApiService>();
builder.Services.AddScoped<ILivraisonApiService, LivraisonApiService>();
builder.Services.AddScoped<IDashboardApiService, DashboardApiService>();
builder.Services.AddScoped<IProfilApiService, ProfilApiService>();
builder.Services.AddScoped<IProfilClientApiService, ProfilClientApiService>();
builder.Services.AddScoped<ISuiviColisApiService, SuiviColisApiService>();

var app = builder.Build();

app.UseExceptionHandler("/Error");
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/HandleStatusCode", "?code={0}");
if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
