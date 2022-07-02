using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Repositories;
using BirthdayApp.Core.Services;
using BirthdayApp.Infrastructure.Repositories;
using BirthdayApp.Services.FileServices;
using BirthdayApp.Services.IdentityServices;
using Neo4jClient;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieConstants.CookieName).AddCookie(CookieConstants.CookieName, options =>
{
    options.Cookie.Name = CookieConstants.CookieName;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Account/Login";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityConstants.PolicyAdminOnly,
        policy => policy.RequireClaim("Admin"));
    options.AddPolicy(IdentityConstants.PolicyUserOnly,
    policy => policy.RequireClaim("User"));
});

builder.Services.AddSingleton<IPasswordHashService, HMACSHA256PasswordHashService>();
builder.Services.AddSingleton<IFileUploadService, LocalFileUploadService>();
builder.Services.AddSingleton<IUserService, UserService>();
//builder.Services.AddSingleton<IAuthenticateService, UserService>();
builder.Services.AddSingleton<IAuthorizeService, DefaultAuthorizeService>();

string uri = builder.Configuration.GetConnectionString("DatabaseUri");
string username = builder.Configuration.GetConnectionString("DatabaseUsername");
string password = builder.Configuration.GetConnectionString("DatabasePassword");

var client = new BoltGraphClient(uri, username, password);
client.ConnectAsync().Wait();
builder.Services.AddSingleton<IGraphClient>(client);
builder.Services.AddSingleton<IUserRepository, Neo4jUserRepository>();
builder.Services.AddSingleton<ICityRepository, Neo4jCityRepository>();

var assembly = Assembly.Load("BirthdayApp.Services");
builder.Services.AddControllers().AddApplicationPart(assembly);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();