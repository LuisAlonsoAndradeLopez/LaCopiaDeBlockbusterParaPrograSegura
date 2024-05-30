using Microsoft.EntityFrameworkCore;
using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backendnet.Middlewares;
using backendnet.Services;

var builder = WebApplication.CreateBuilder(args);

//Support for generate JWT
//Comment this line if don't want authentication
builder.Services.AddScoped<JwtTokenService>();

//Support for MySQL
var connectionString = builder.Configuration.GetConnectionString("DataContext");
builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//Support for Identity
//Start of the comment
builder.Services.AddIdentity<CustomIdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    //Password handler
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<IdentityContext>();

//Support for JWT
builder.Services
    .AddHttpContextAccessor()   //For access to HttpContext()
    .AddAuthorization() // For autorize the access in every method
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>    //For autenticate with JWT
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  //From appSettings
            ValidAudience = builder.Configuration["Jwt:Audience"],  //From appSettings
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
//End of the comment

//Support for CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy
    (
        policy =>
        {
            policy.WithOrigins("http://localhost:3001", "http://localhost:8080")
                .AllowAnyHeader()
                .WithMethods("GET", "POST", "PUT", "DELETE");
        }
    );
});

// Adding the service por Email
builder.Services.AddSingleton<EmailService>();

// Adding Memory Cache
builder.Services.AddMemoryCache();

//Adding the functionality for the controllers
builder.Services.AddControllers();

//Adding API documentation
builder.Services.AddSwaggerGen();

//Build the webapp
var app = builder.Build();

//Show swagger only in developement enviorment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Adding a middleware for error handler
//Start of comment
app.UseExceptionHandler("/error");

//Use routes for controllers endpoints
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//Adding the middleware for refresh the token
app.UseSlidingExpirationJwt();
//End of the comment

app.UseCors();

//Establish the use of the routes without specifies anyone by default
app.MapControllers();

app.Run();
