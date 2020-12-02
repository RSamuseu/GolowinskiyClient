using AutoMapper;
using GolovinskyAPI.Data.Models.Background;
using GolovinskyAPI.Logic.Models.Background;

namespace GolovinskyAPI.Logic.Profiles
{
    public class BackgroundProfile : Profile
    {
        public BackgroundProfile()
        {
            CreateMap<BackgroundPostBase64ViewModel, Background>()
                .ForMember(x => x.orient, opt => opt.MapFrom(a => a.Orientation));
            CreateMap<Background, BackgroundPostBase64ViewModel>()
                .ForMember(x => x.Orientation, opt => opt.MapFrom(a => a.orient));

            CreateMap<BackgroundPutBase64ViewModel, Background>()
                .ForMember(x => x.orient, opt => opt.MapFrom(a => a.Orientation));
            CreateMap<Background, BackgroundPutBase64ViewModel>()
                .ForMember(x => x.Orientation, opt => opt.MapFrom(a => a.orient));

            CreateMap<BackgroundPostFileViewModel, Background>()
                .ForMember(x => x.orient, opt => opt.MapFrom(a => a.Orientation));
            CreateMap<Background, BackgroundPostFileViewModel>()
                .ForMember(x => x.Orientation, opt => opt.MapFrom(a => a.orient));

            CreateMap<BackgroundPutFileViewModel, Background>()
                .ForMember(x => x.orient, opt => opt.MapFrom(a => a.Orientation));
            CreateMap<Background, BackgroundPutFileViewModel>()
                .ForMember(x => x.Orientation, opt => opt.MapFrom(a => a.orient));

            CreateMap<BackgroundBase64DeleteViewModel, Background>();
            CreateMap<Background, BackgroundBase64DeleteViewModel>();

            CreateMap<BackgroundFileDeleteViewModel, Background>();
            CreateMap<Background, BackgroundFileDeleteViewModel>();
        }
    }
}
