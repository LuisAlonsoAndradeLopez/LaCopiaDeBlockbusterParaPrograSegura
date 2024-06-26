using frontendnet.Middlewares;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adding Services
builder.Services.AddControllersWithViews();

// Support for Consult the API
var UrlWebAPI = builder.Configuration["UrlWebAPI"];
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<SendBearerDelegatingHandler>();
builder.Services.AddTransient<RefreshTokenDelegatingHandler>();
builder.Services.AddHttpClient<AuthClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); });
builder.Services.AddHttpClient<CategoriesClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<SendBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefreshTokenDelegatingHandler>();
builder.Services.AddHttpClient<UsersClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<SendBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefreshTokenDelegatingHandler>();
builder.Services.AddHttpClient<RolesClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<SendBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefreshTokenDelegatingHandler>();
builder.Services.AddHttpClient<MoviesClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<SendBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefreshTokenDelegatingHandler>();
builder.Services.AddHttpClient<ProfileClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<SendBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefreshTokenDelegatingHandler>();

builder.Services.AddHttpClient<EmailClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<SendBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefreshTokenDelegatingHandler>();

// Suport for Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".frontendnet";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LoginPath = "/Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

var app = builder.Build();

// Middleware for error handlind
app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
