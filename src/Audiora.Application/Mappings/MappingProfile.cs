using AudioMapper = AutoMapper;
using Audiora.Application.DTOs.Response;
using Audiora.Domain.Entities;

namespace Audiora.Application.Mappings;

public class MappingProfile : AudioMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()))
            .ForMember(d => d.IsPremium, o => o.MapFrom(s => s.IsPremium()));

        CreateMap<Music, MusicResponse>()
            .ForMember(d => d.Genre, o => o.MapFrom(s => s.Genre.ToString()))
            .ForMember(d => d.ArtistName, o => o.MapFrom(s => s.Artist != null ? s.Artist.Name : string.Empty))
            .ForMember(d => d.AlbumTitle, o => o.MapFrom(s => s.Album != null ? s.Album.Title : null))
            .ForMember(d => d.Duration, o => o.MapFrom(s => s.GetFormattedDuration()));
    }
}