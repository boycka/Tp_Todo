using Tp_TODO.Filters;
using Tp_TODO.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFileLog, FileLog>();

builder.Services.AddScoped<AuthFilter>();
builder.Services.AddScoped<ThemeFilter>();
builder.Services.AddScoped<LoggingFilter>();


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ThemeFilter>();
    options.Filters.Add<LoggingFilter>();

});
builder.Services.AddSession(
    opt =>
    {
        opt.IdleTimeout = TimeSpan.FromHours(5);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
