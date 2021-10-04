namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
  public record RedisConfig
  {
    public const string SectionName = "Redis";

    public double CacheLiveInMinutes { get; set; }
  }
}
