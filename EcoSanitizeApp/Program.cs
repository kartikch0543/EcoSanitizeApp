using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcoSanitizeApp.Data;
using EcoSanitizeApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC Controllers and Views
builder.Services.AddControllersWithViews();

// Session support for User Authentication (Sign In / Register)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Database Context (SQLite persistent database synchronized with SQLite Web Admin)
var dbPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "EcoSanitize.db"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Scoped Service Dependency Injection
builder.Services.AddScoped<ISanitizationService, SanitizationService>();

var app = builder.Build();

// Ensure database and initial records are created/verified
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Automatically start SQLite Web Admin on port 8080 in the background if not already running
AutoStartSqliteWebAdmin(dbPath);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Session middleware for authentication
app.UseSession();

app.UseAuthorization();

// Conventional MVC Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Helper method to auto-launch SQLite Web Admin on port 8080
static void AutoStartSqliteWebAdmin(string databaseFilePath)
{
    Task.Run(() =>
    {
        try
        {
            // Check if port 8080 is already active
            using var client = new TcpClient();
            var asyncResult = client.BeginConnect("127.0.0.1", 8080, null, null);
            bool isPortOpen = asyncResult.AsyncWaitHandle.WaitOne(400);
            if (isPortOpen && client.Connected)
            {
                client.EndConnect(asyncResult);
                return; // Already running on port 8080
            }
        }
        catch
        {
            // Port 8080 is free, proceed to launch
        }

        try
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string scriptPath = Path.Combine(appData, "Python", "Python314", "Scripts", "sqlite_web.exe");

            if (!File.Exists(scriptPath))
            {
                var pythonDir = Path.Combine(appData, "Python");
                if (Directory.Exists(pythonDir))
                {
                    var found = Directory.GetFiles(pythonDir, "sqlite_web.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (found != null) scriptPath = found;
                }
            }

            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            if (File.Exists(scriptPath))
            {
                startInfo.FileName = scriptPath;
                startInfo.Arguments = $"\"{databaseFilePath}\" --port 8080 --host 127.0.0.1 -x";
            }
            else
            {
                startInfo.FileName = "python";
                startInfo.Arguments = $"-m sqlite_web \"{databaseFilePath}\" --port 8080 --host 127.0.0.1 -x";
            }

            System.Diagnostics.Process.Start(startInfo);
        }
        catch
        {
            // Non-critical background service; continue smoothly if python/sqlite-web is absent
        }
    });
}
