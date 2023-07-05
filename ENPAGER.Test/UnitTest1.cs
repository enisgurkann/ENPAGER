using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ENPAGER.Test;

public class UnitTest1
{
    private readonly TestContext _testContext;

    public UnitTest1()
    {
        _testContext = new TestContext(CreateNewContextOptions());
    }

    private static DbContextOptions<TestContext> CreateNewContextOptions()
    {
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<TestContext>();
        builder.UseInMemoryDatabase( "TestDbInMemory")
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    private List<TestModel> GetDbSet()
    {
        var list = new List<TestModel>();
        list.Add(new TestModel() {Name = "Enis", Surname = "Gürkan"});
        list.Add(new TestModel() {Name = "Sinan", Surname = "Gürkan"});
        list.Add(new TestModel() {Name = "Bill", Surname = "Gates"});
        list.Add(new TestModel() {Name = "Steve", Surname = "Jobs"});
        list.Add(new TestModel() {Name = "Freed", Surname = "Jobs"});
        list.Add(new TestModel() {Name = "Ali", Surname = "Jobs"});
        list.Add(new TestModel() {Name = "Veli", Surname = "Jobs"});
        list.Add(new TestModel() {Name = "Mehmet", Surname = "Jobs"});
        return list;
    }

    [Fact]
    public async Task ToPagedList_Test()
    {
        using (var context = new TestContext(CreateNewContextOptions()))
        {
            var dbList = GetDbSet();
            await _testContext.TestDbSet.AddRangeAsync(dbList);
            await _testContext.SaveChangesAsync();
           
           
            var list = await _testContext
                .TestDbSet
                .ToPagedListAsync(pageNumber:1,pageSize:5);
           
            Assert.Equal(5, list.Items.Count());
            Assert.Equal(8, list.TotalCount);
            Assert.Equal(2, list.TotalPages);
            Assert.Equal(5, list.PageSize);
        }
    }
}