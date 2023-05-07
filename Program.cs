using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using jenjennewborncare.Data;
using jenjennewborncare.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using jenjennewborncare.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);



//ConfigurationBinder for oauth google
var configuration = builder.Configuration;


var connectionString = builder.Configuration.GetConnectionString("jenjennewborncareContextConnection") ?? throw new InvalidOperationException("Connection string 'jenjennewborncareContextConnection' not found.");
builder.Services.AddDbContext<jenjennewborncareContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddDefaultIdentity<User>(options => { options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
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
