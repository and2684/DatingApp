using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AppUser, MemberDto>() // Из AppUser в MemberDto
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos!.FirstOrDefault(x => x.IsMain)!.Url)) // Записываем в PhotoUrl главное фото
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())); // Считаем возраст юзера
                
            CreateMap<Photo, PhotoDto>(); // Из Photo в PhotoDto

            CreateMap<MemberUpdateDto, AppUser>();         
            
            CreateMap<RegisterDto, AppUser>();   

            CreateMap<AppUser, LikeDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos!.FirstOrDefault(x => x.IsMain)!.Url)) // Записываем в PhotoUrl главное фото
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())); // Считаем возраст юзера

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos!.FirstOrDefault(x => x.IsMain)!.Url)) // Фотка отправителя
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos!.FirstOrDefault(x => x.IsMain)!.Url)); // Фотка получателя
        }
    }
}