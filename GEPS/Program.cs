using GEPS.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new RoleFilter()); // Global olarak RoleFilter'ı ekliyoruz
});
builder.Services.AddHttpClient(); // AddHttpClient() burada eklenmeli.

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum süresi, örneğin 30 dakika
}); // Session desteği ekleniyor
builder.Services.AddScoped<RoleFilter>(); // RoleFilter bağımlılığını ekliyoruz
var app = builder.Build();

// Middleware sırasını doğru şekilde yerleştiriyoruz
app.UseSession(); // Session'ı kullanmaya başlıyoruz
app.UseMiddleware<RoleMiddleware>(); // RoleMiddleware'i buraya ekliyoruz

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // HTTPS yönlendirmesi
app.UseStaticFiles(); // Statik dosyaların sunulması

app.UseRouting(); // Routing'i aktif ediyoruz
app.UseAuthentication(); // Kimlik doğrulama middleware'ini kullanıyoruz
app.UseAuthorization(); // Yetkilendirme middleware'ini kullanıyoruz

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Method override için özel işlem ekliyoruz
app.Use(async (context, next) =>
{
    if (context.Request.Method == "POST" && context.Request.Form["_method"] == "PUT")
    {
        context.Request.Method = "PUT";
    }
    await next();
});