using BussinessObjects.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.AccountRepo;
using Repository.AlbumRepo;
using Repository.CategoryRepo;
using Repository.OrderRepo;
using Repository.OrderDetailRepo;
using Repository.ProductRepo;
using Project_API.Map;
using System;
using System.Text;

namespace Project_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(MapperConfig));
            // cấu hình swagger
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Net 6 API", Version = "v1" });
                /*option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                    }
                 });*/
            });
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("CORSPolicy", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((host) => true));
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ProjectDbContext>().AddDefaultTokenProviders();

            builder.Services.AddDbContext<ProjectDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

            /*builder.Services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false; // email phải confirm mới được đăng nhập
            });*/
            
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IProductRepository, ProductRepository>();


			/*builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => // cấu hình thêm một vào chỗ khác như thời gian sống...
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    // tham số bên dưới
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // nếu được có thể tham số hóa trong appsetting
                    ValidIssuer = builder.Configuration["Jwt:ValidIssuer"], // đường dẫn host
                    ValidAudience = builder.Configuration["Jwt:ValidAudience"],
                    // quantrong
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });*/

			builder.Services.AddScoped<ProjectDbContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("CORSPolicy");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        //tkcustomer: linhmp@gmail.com 123456
        //tkadmin:    admin@gmail.com  123123
    }
}
