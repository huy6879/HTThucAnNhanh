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

            CreateMap<Account, AccountVM>()
            .ReverseMap();

            CreateMap<AccountCreateVM, AccountVM>()
                .ReverseMap();

            CreateMap<AccountCreateVM, Account>()
                .ForMember(x => x.Password, opt => opt.Ignore())
                .ForMember(x => x.RandomKey, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<Role, RoleVM>()
                .ReverseMap();
        }
    }
}
