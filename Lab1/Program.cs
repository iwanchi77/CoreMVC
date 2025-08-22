using Lab1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lab1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the DI container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //HTTP要求管線(明管) 暗管-美觀視角、明管-功能視角 core後變成明管

            //app.Use鐵布衫(); 啟用鐵布衫功能 --提供跨站請求偽造(CSRF)保護，防止未經授權的請求。
            app.UseHttpsRedirection(); //瀏覽HTTP網址 --自動重導至HTTPS網址。作用:保護User資料安全。
			app.UseStaticFiles();      //指定存放網站靜態文件的資料夾為:wwwroot(預設，網站根目錄) --提供CSS、JavaScript、圖片等靜態資源。
			app.UseRouting();          //執行URL Routing/ URL Rewriting。啟用路由功能 --允許ASP.NET Core應用程式根據URL路徑將請求路由到相應的控制器和操作方法。
			app.UseAuthorization();    //啟用授權功能(權限管制-Authenticate=>Aythorize) --確保只有經過身份驗證的用戶才能訪問受保護的資源。


			// Configure the HTTP request pipeline.
			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.MapRazorPages();

            app.Run();
        }
    }
}
