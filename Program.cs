using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Zugether.Hubs;
using Zugether.Models;

var builder = WebApplication.CreateBuilder(args);
// �[�� .env ���
Env.Load();
// �K�[�������ҪA��
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
	string? googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
	string? googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
	if (string.IsNullOrEmpty(googleClientId) || string.IsNullOrEmpty(googleClientSecret))
	{
		Console.WriteLine("Google Client ID �� Client Secret ���]�m");
		Environment.Exit(1); // ����ҰʡA�קK���ε{�ǥX��
	}
	googleOptions.ClientId = googleClientId;
	googleOptions.ClientSecret = googleClientSecret;
	var scopes = new[] { "openid", "profile", "email", "https://www.googleapis.com/auth/gmail.send" };
	foreach (var scope in scopes)
	{
		googleOptions.Scope.Add(scope);
	}
	// ���wgoogle api callback path
	googleOptions.CallbackPath = new PathString("/signin-google");
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ZugetherContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connstring")));

// �K�[ SignalR �A��
builder.Services.AddSignalR();
// �K�[ Session �䴩
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.Cookie.HttpOnly = true; // �]�m�� HttpOnly
	options.Cookie.IsEssential = true; // �N Session �]�m�����n Cookie
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
// �t�m������
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// �ҥλ{��
app.UseAuthorization();
// �ҥ� Session
app.UseSession();
// �t�m SignalR ������
app.MapHub<ChatHub>("/chatHub");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
