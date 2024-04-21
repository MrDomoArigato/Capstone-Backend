using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Tests.Services;

public class BudgetServiceTests : IDisposable 
{
    private readonly DbContextOptions<CapstoneContext> _options;
    private readonly CapstoneContext _context;

    public BudgetServiceTests()
    {
        _options = new DbContextOptionsBuilder<CapstoneContext>()
            .UseInMemoryDatabase(databaseName: "TestBudgetDatabase")
            .Options;

        _context = new CapstoneContext(_options);

        SeedData();
    }

    private void SeedData()
    {
        _context.TransactionTypes.AddRange(new List<TransactionType>{
            new(){
                Id = 005000,
                Code = "TEST10",
                Description = "Test Code 1-0"
            },
            new() {
                Id = 005001,
                Code = "TEST11",
                Description = "Test Code 1-1"
            },
            new() {
                Id = 005002,
                Code = "TEST12",
                Description = "Test Code 1-2"
            },
            new() {
                Id = 006000,
                Code = "TEST20",
                Description = "Test Code 2-0"
            },
            new() {
                Id = 006001,
                Code = "TEST21",
                Description = "Test Code 2-1"
            },
            new() {
                Id = 006002,
                Code = "TEST22",
                Description = "Test Code 2-2"
            },
            new() {
                Id = 007000,
                Code = "TEST30",
                Description = "Test Code 3-0"
            },
            new() {
                Id = 007001,
                Code = "TEST31",
                Description = "Test Code 3-1"
            }
        });

        _context.Budgets.AddRange(new List<Budget>{
            new(){
                UserId = "id1",
                BudgetItems = new Dictionary<int, decimal>{
                    {5000, 300.00m},
                    {5001, 100.00m},
                    {5002, 200.00m},
                    {6000, 400.00m},
                    {6001, 400.00m},
                    {7000, 100.00m},
                    {7001, 100.00m}
                }
            },
            new(){
                UserId = "id2",
                BudgetItems = new Dictionary<int, decimal>{
                    {6001, 300.00m},
                    {6002, 400.00m}
                }
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public void CreateBudget_AddsBudgetToDatabase()
    {
        var _service = new BudgetService(_context);
        var startCount = _context.Budgets.Count();

        var result = _service.CreateBudget("create1", [
            [
                new (){
                    Id = 005001,
                    Amount = 100.00m
                },
                new (){
                    Id = 005002,
                    Amount = 200.00m
                }
            ],
            [
                new (){
                    Id = 006001,
                    Amount = 100.00m
                },
                new (){
                    Id = 006002,
                    Amount = 200.00m
                }
            ],
            [
                new() {
                    Id = 007001,
                    Amount = 300.00m
                }
            ]
        ]);

        var endCount = _context.Budgets.Count();

        Assert.NotNull(result);
        Assert.Equal(0, result);
        Assert.Equal(startCount + 1, endCount);
    }

    [Fact]
    public async void GetBudget_ReturnsCorrectBudget()
    {
        var _service = new BudgetService(_context);
        var result = await _service.GetBudget("id1");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(7, result.SelectMany(b => b).Count());
    }

    [Fact]
    public void DeleteBudget_DeletesBudgetOnDatabase()
    {
        var _service = new BudgetService(_context);
        var startCount = _context.Budgets.Count();

        var result = _service.DeleteBudget("id2");

        var endCount = _context.Budgets.Count();

        Assert.NotNull(result);
        Assert.Equal(0, result);
        Assert.Equal(startCount - 1, endCount);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}