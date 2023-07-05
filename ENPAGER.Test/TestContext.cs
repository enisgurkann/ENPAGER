using Microsoft.EntityFrameworkCore;

namespace ENPAGER.Test;

public class TestContext: DbContext
{
    public TestContext() { }
    public TestContext(DbContextOptions<TestContext> options): base(options) { }
    public DbSet<TestModel> TestDbSet { get; set; }
}
