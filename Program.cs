using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using jenjennewborncare.Data;
using jenjennewborncare.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using jenjennewborncare.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using Stripe;
using jenjennewborncare.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using MySqlX.XDevAPI;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


var environment = builder.Environment;

    var config = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.production.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    // Retrieve configuration values specific to Azure AD and Key Vault
    var tenantId = config["AzureAd:TenantId"];
    var clientId = config["AzureAd:ClientId"];
    var clientSecret = config["AzureAd:ClientSecret"];
    var keyVaultUrl = config["AzureAd:KeyVaultUrl"];

    // Establish connection with Azure Key Vault
    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
    var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);

    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), credential);


// Establish MySQL database connection
var connectionString = builder.Configuration.GetConnectionString("jenjennewborncareContextConnections")
    ?? throw new InvalidOperationException("Connection string 'jenjennewborncareContextConnections' not found.");

builder.Services.AddDbContext<jenjennewborncareContext>(options =>
    options.UseMySQL(connectionString));


builder.Services.AddDefaultIdentity<User>(options => { options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
        .AddRoles<IdentityRole>().AddEntityFrameworkStores<jenjennewborncareContext>();

    // Add services to the container.
    builder.Services.AddControllersWithViews();


//configuring api
//builder.Services.AddEndpointsApiExplorer();

//configuring roles
//builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


//configuring email sender
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("EmailSettings"));

// Configure Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];






//access denied page modified
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Errors/AccessDenied");
});

//configuring google authentication
builder.Services.AddAuthentication(

//options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

);

// Configure Google authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});



//builder.Services.Configure<ForwardedHeadersOptions>(options => {
//    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
//});

var app = builder.Build();

//configure ngnix to wors properly

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders=ForwardedHeaders.XForwardedFor|ForwardedHeaders.XForwardedProto

});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseCookiePolicy();

app.UseAuthorization();
app.MapRazorPages();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"


    ) ;


app.Run();
