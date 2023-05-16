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

var builder = WebApplication.CreateBuilder(args);



//ConfigurationBinder for oauth google
    var configuration = builder.Configuration;


    var connectionString = builder.Configuration.GetConnectionString("jenjennewborncareContextConnection") ?? throw new InvalidOperationException("Connection string 'jenjennewborncareContextConnection' not found.");
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

).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    //googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");

});

var app = builder.Build();

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
