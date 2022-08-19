using System.Reflection;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef
{
  /// <summary>
  /// A class that defines the tables and its properties in the database of CompanyService.
  /// </summary>
  public class CompanyServiceDbContext : DbContext, IDataProvider
  {
    public CompanyServiceDbContext(DbContextOptions<CompanyServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<DbCompany> Companies { get; set; }
    public DbSet<DbCompanyUser> CompaniesUsers { get; set; }
    public DbSet<DbContractSubject> ContractSubjects { get; set; }

    // Fluent API is written here.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.CompanyService.Models.Db"));
    }

    public object MakeEntityDetached(object obj)
    {
      Entry(obj).State = EntityState.Detached;

      return Entry(obj).State;
    }

    public void Save()
    {
      SaveChanges();
    }

    public void EnsureDeleted()
    {
      Database.EnsureDeleted();
    }

    public bool IsInMemory()
    {
      return Database.IsInMemory();
    }

    public async Task SaveAsync()
    {
      await SaveChangesAsync();
    }
  }
}
