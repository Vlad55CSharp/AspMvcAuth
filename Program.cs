using Microsoft.AspNetCore.Identity;
using AspMvcAuth.Models;
using AspMvcAuth.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using AspMvcAuth.CustomPolicy;
using Microsoft.AspNetCore.Authorization;
namespace AspMvcAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
            })
                .AddRoles<ApplicationRole>() //включаем поддержку ролей
                .AddEntityFrameworkStores<ApplicationContext>();
                

			builder.Services.ConfigureApplicationCookie(opts => 
            { 
                opts.LoginPath = "/account/signin";
                opts.AccessDeniedPath = "/AccessDanied"; //путь к странице с информацией о запрете доступа
            });

			//настраиваем политику авторизации
			builder.Services.AddTransient<IAuthorizationHandler, OlderThenHandler>();
			builder.Services.AddAuthorization(options => 
            {
                options.AddPolicy("OnlyRussianAdmin", policy => 
                {
                    policy.RequireRole("ADMIN");
                    policy.RequireClaim("Language", "Russian");
                    policy.AddRequirements(new OlderThenPolicy(18)); //пользователь старше 18 лет
                });
            });

			// Add services to the container.
			builder.Services.AddControllersWithViews();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
