using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using jenjennewborncare.Data;
using jenjennewborncare.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using jenjennewborncare.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
var builder = WebApplication.CreateBuilder(args);



//ConfigurationBinder for oauth google
var configuration = builder.Configuration;


//var connectionString = builder.Configuration.GetConnectionString("jenjennewborncareContextConnection") ?? throw new InvalidOperationException("Connection string 'jenjennewborncareContextConnection' not found.");
var connectionString = "server=localhost;database=jenjeare_main;uid=jenje_01;pwd=?a5F7ds71";
//var connectionString = "server=192.185.7.2;database=jenjeare_main;uid=jenje_01;pwd=?a5F7ds71";
builder.Services.AddDbContext<jenjennewborncareContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<jenjennewborncareContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


//configuring email sender
//builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("EmailSettings"));



//configuring google authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];

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

app.UseAuthorization();
app.MapRazorPages();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"


    ) ;


app.Run();
