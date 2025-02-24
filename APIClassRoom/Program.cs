using System.Transactions;
using APIClassRoom.Components;
using APIClassRoom.Model;
using APIClassRoom.Service;
using APIClassRoom.Storage;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FluentUI.AspNetCore.Components;
using MudBlazor.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("APIClassRoomContextConnection") ?? throw new InvalidOperationException("Connection string 'APIClassRoomContextConnection' not found.");;

string baseUri = "http://localhost:5146/";
string con = "Data Source=RADOIN_CR;Initial Catalog=MyDb;Integrated Security=true;Encrypt=False;";

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

builder.Services.AddSingleton<ClassStorage>(
    new ClassStorage(con));
builder.Services.AddSingleton<TimeSlotStorage>(
    new TimeSlotStorage(con));
builder.Services.AddSingleton<UserStorage>(
    new UserStorage(con));
builder.Services.AddSingleton<ClassReservationStorage>(
    new ClassReservationStorage(con));
builder.Services.AddSingleton<ClassScheduleStorage>(
    new ClassScheduleStorage(con));
builder.Services.AddSingleton<ClassCompensationStorage>(
    new ClassCompensationStorage(con));
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute()); // ✅ Apply globally
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddScoped<HttpClient>(sp =>
{
    var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5146/") };
    return httpClient;
});

builder.Services.AddMudServices();

/*builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options => {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan=TimeSpan.FromHours(20);
        options.Cookie.Name = "khadija";
    });*/
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=GoogleDefaults.AuthenticationScheme;
}).AddCookie("Cookies", options => {
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan=TimeSpan.FromHours(20);
    options.Cookie.Name = "khadija";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
}).AddGoogle(options =>
{
    options.SignInScheme=CookieAuthenticationDefaults.AuthenticationScheme;
    options.ClientId ="221814378332-inngokjf0ienjpasaa9qjbqot913p7b9.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = new PathString("/signin-google"); 
    options.SaveTokens = true;
    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
 
});
builder.Services.AddFluentUIComponents();
builder.Services.AddBlazorBootstrap();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
if (OperatingSystem.IsWindows()) { TransactionManager.ImplicitDistributedTransactions = true;}
var app = builder.Build();

// ✅ Proper middleware ordering
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();