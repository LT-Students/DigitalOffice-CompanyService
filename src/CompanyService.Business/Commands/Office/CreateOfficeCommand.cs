using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office
{
    public class CreateOfficeCommand : ICreateOfficeCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IOfficeRepository _officeRepository;
        private readonly IDbOfficeMapper _mapper;
        private readonly ICreateOfficeRequestValidator _validator;

        public CreateOfficeCommand(
            IAccessValidator accessValidator,
            IOfficeRepository officeRepository,
            IDbOfficeMapper mapper,
            ICreateOfficeRequestValidator validator)
        {
            _accessValidator = accessValidator;
            _officeRepository = officeRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public OperationResultResponse<Guid> Execute(CreateOfficeRequest request)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enouth rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            DbOffice office = _mapper.Map(request);
            _officeRepository.Add(office);

            return new OperationResultResponse<Guid>
            {
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess,
                Body = office.Id
            };
        }
    }
}
