using BLL.Services;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.unitofwork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using scannapp.middleware;
using System.Text;

namespace scannapp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================
            //  Add Services
            // =========================

            // JWT Configuration
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                  options.UseSqlServer(
                     builder.Configuration.GetConnectionString("DefaultConnection"),
                       b => b.MigrationsAssembly("DAL")
     )
 );


            // Identity
            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBranchRepository, BranchRepository>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IPointsRepository, PointsRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserOfferRepository, UserOfferRepository>();

            // UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Controllers & Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // =========================
            //  Build App
            // =========================
            var app = builder.Build();

            //  Seed Roles and Admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                await SeedData.SeedRolesAndAdmin(userManager, roleManager);
            }

            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // =========================
            //  Middleware Pipeline
            // =========================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
            builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
            builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
