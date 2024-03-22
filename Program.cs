using CapstoneApi.Database;
using CapstoneApi.Data;
using CapstoneApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

/* var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole();

# warning TODO: Fix before final release
builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy => {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });
        });

//Gets DB connect from secrets 
var dbconnection = builder.Configuration.GetSection("db-connection").Get<DBConnection>();

// Add services to the container.
builder.Services.AddControllers();
//Sets up database connection for reference
builder.Services.AddDbContext<CapstoneContext>(options => {
                options.UseNpgsql($"Host={dbconnection!.Host}:{dbconnection!.Port};Database={dbconnection!.Database};Username={dbconnection!.Username};Password={dbconnection!.Password}");
            });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.MapGet("/", () => dbconnection);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();
 */


 namespace CapstoneApi
 {
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Use the Startup class to configure the application
                });
    }
 }