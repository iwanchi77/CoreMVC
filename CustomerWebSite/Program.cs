using CustomerWebSite.Data;
using CustomerWebSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebSite
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString));

			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			//註冊 NorthwindContext 服務，並設定使用 SQL Server 資料庫，連接字串從設定檔中取得。
			builder.Services.AddDbContext<NorthwindContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Northwind"));
			});

			//設定 Session，並設定 Session 的 Cookie 名稱、逾時時間等屬性。
			builder.Services.AddSession(options =>
			{
				options.Cookie.Name = ".CustomerWebSite.Session";
				options.IdleTimeout = TimeSpan.FromMinutes(5);
				options.Cookie.IsEssential = true;
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			});

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

			//管線中加入中介軟體，以強制使用 HTTPS、提供靜態檔案、設定路由、授權及使用 Session。
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseSession();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{CustomerID?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}
