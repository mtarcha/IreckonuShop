using AutoMapper;

namespace IreckonuShop.API.Utilities
{
    public sealed class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<ViewModels.Product, BusinessLogic.Models.Product>();
        }
    }
}