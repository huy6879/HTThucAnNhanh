using AutoMapper;
using HeThongDatThucAn20.Data;
using HeThongDatThucAn20.ViewModels;

namespace HeThongDatThucAn20.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, Account>().
                ForMember(kh => kh.Fullname, option => option.MapFrom(RegisterVM =>
                RegisterVM.Fullname))
                .ReverseMap();
        }
    }
}
