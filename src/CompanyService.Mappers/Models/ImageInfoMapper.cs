using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class ImageInfoMapper : IImageInfoMapper
    {
        public ImageInfo Map(ImageData response)
        {
            if (response == null)
            {
                return null;
            }

            return new ImageInfo
            {
                Id = response.ImageId,
                ParentId = response.ParentId,
                Type = response.Type,
                Content = response.Content,
                Extension = response.Extension,
                Name = response.Name
            };
        }
    }
}
