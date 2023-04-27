using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using jenjennewborncare.Data;
using jenjennewborncare.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("jenjennewborncareContextConnection") ?? throw new InvalidOperationException("Connection string 'jenjennewborncareContextConnection' not found.");
var connectionString = "server=localhost;database=::;uid=f;::";
//var connectionString = "server=192.185.7.2;database=::;uid=j::";
builder.Services.AddDbContext<jenjennewborncareContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<jenjennewborncareContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
