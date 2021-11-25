using System.Linq;
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
    private DbSet<DbCompany> _companies { get; set; }
    private DbSet<DbCompanyChanges> _companyChanges { get; set; }

    public IQueryable<DbCompany> Companies => _companies.AsQueryable();
    public IQueryable<DbCompanyChanges> CompanyChanges => _companyChanges.AsQueryable();

    public CompanyServiceDbContext(DbContextOptions<CompanyServiceDbContext> options)
      : base(options)
    {
    }

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

    public void AddCompanyOrChanges<T>(T item)
    {
      if (typeof(DbCompany) == typeof(T))
      {
        DbCompany company = item as DbCompany;
        _companies.Add(company);
      }

      if (typeof(DbCompanyChanges) == typeof(T))
      {
        DbCompanyChanges changes = item as DbCompanyChanges;
        _companyChanges.Add(changes);
      }
    }

    public async Task<DbCompany> GetCompanyAsync()
    {
      return await _companies.FirstOrDefaultAsync();
    }

    public async Task<bool> AnyCompanyAsync()
    {
      return await _companies.AnyAsync();
    }
  }
}
