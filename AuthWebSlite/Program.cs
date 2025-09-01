using AuthWebSlite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthWebSlite
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

			builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

			builder.Services.Configure<IdentityOptions>(options => {
				options.Password.RequireDigit = true; //密碼必須包含數字
				options.Password.RequireLowercase = true; //密碼必須包含小寫字母
				options.Password.RequireNonAlphanumeric = true; //密碼必須包含特殊字元
				options.Password.RequireUppercase = true; //密碼必須包含大寫字母
				options.Password.RequiredLength = 8; //密碼最短長度為8
				options.Password.RequiredUniqueChars = 1; //密碼必須包含1個不同的字元

				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //當帳戶被鎖定時，鎖定5分鐘
				options.Lockout.MaxFailedAccessAttempts = 3; //密碼錯誤3次後，帳戶被鎖定
				options.Lockout.AllowedForNewUsers = true; //新用戶可以被鎖定

				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //允許的使用者名稱字元(允許大小寫英文字、數字、-._@)，其他字元會被拒絕
				options.User.RequireUniqueEmail = true; //Email必須唯一
				options.SignIn.RequireConfirmedEmail = true; //登入前必須先確認Email
			});
			//設定Cookie的相關屬性
			builder.Services.ConfigureApplicationCookie(options => {
				options.Cookie.HttpOnly = true; //設定HttpOnly屬性，防止Client端的Script存取Cookie
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //設定Secure屬性，Cookie只能透過HTTPS傳輸
				options.ExpireTimeSpan = TimeSpan.FromMinutes(5); //Cookie的過期時間為5分鐘。沒有ExpireTimeSpan，Cookie會在瀏覽器關閉時過期
				options.LoginPath = "/Identity/Account/Login"; //設定登入的路徑
				options.AccessDeniedPath = "/Identity/Account/AccessDenied"; //設定存取被拒的路徑
				options.SlidingExpiration = true; //啟用滑動過期時間，每次存取會重新計算過期時間
			});

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

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}
