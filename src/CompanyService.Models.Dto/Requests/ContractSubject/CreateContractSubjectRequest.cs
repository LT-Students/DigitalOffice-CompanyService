using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject
{
  public record CreateContractSubjectRequest
  {
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
