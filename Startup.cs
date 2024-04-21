using CapstoneApi.Authenticate;
using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CapstoneApi
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        public IConfiguration _configuration = configuration;
        public IWebHostEnvironment _env = env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });

            // Load the PEM-encoded certificate string from configuration
            string certificate = _configuration["certificates:certificate"]!;
            RSA rsa = CertLoader.LoadCertificateFromBase64String(certificate)!;

            if(rsa != null)
                // Add authentication services
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = _configuration["JWT:issuer"],
                            ValidAudience = _configuration["JWT:audience"],
                            IssuerSigningKey = new RsaSecurityKey(rsa),
                            ValidateIssuerSigningKey = true
                        };
                    });
            else
                Console.WriteLine("Certificate was not loaded. Please set certificates:certificate in App Settings.");

            var dbconnection = _configuration.GetSection("db-connection").Get<DBConnection>();

            services.AddDbContext<CapstoneContext>(options =>
            {
                options.UseNpgsql($"Host={dbconnection!.Host}:{dbconnection!.Port};Database={dbconnection!.Database};Username={dbconnection!.Username};Password={dbconnection!.Password}");
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IBudgetService, BudgetService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add controllers
            services.AddControllers(options =>
            {
                if(_env.IsProduction())
                    options.Filters.Add(new AuthorizeFilter());
            });
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            if(_env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseAuthentication();

            app.Use(
                async (context, next) => 
                {
                    var stopwatch = Stopwatch.StartNew();
                    await next();
                    stopwatch.Stop();

                    logger.LogInformation($"{ context.Request.Method } { context.Request.Path } responded { context.Response.StatusCode } in { stopwatch.ElapsedMilliseconds }ms");
                }
            );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}