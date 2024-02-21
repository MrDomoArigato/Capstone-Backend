using CapstoneApi.Database;
using CapstoneApi.Data;
using CapstoneApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole();

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => dbconnection);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
