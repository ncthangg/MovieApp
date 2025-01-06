using AutoMapper;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data.Models;

namespace MovieApp.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ///User
            //Request
            CreateMap<User, RequestUserDto>().ReverseMap();
            CreateMap<UserRole, RequestUserRoleDto>().ReverseMap();
            CreateMap<UserStatus, RequestUserStatusDto>().ReverseMap();
            CreateMap<UserLike, RequestUserLikeDto>().ReverseMap();
            CreateMap<UserWatchHistory, RequestUserWatchHistoryDto>().ReverseMap();

            //Response
            CreateMap<User, ResponseUserDto>();
            CreateMap<UserRole, ResponseUserRoleDto>();
            CreateMap<UserStatus, ResponseUserStatusDto>();
            CreateMap<UserLike, ResponseUserLikeDto>();
            CreateMap<UserWatchHistory, ResponseUserWatchHistoryDto>();

            //Login
            CreateMap<UserToken, ResponseUserTokenDto>();
            CreateMap<UserVerification, ResponseUserVerificationDto>();

            ///Movie
            CreateMap<Actor, ResponseActorDto>();

            CreateMap<Category, ResponseCategoryDto>();
            CreateMap<MovieType, ResponseTypeDto>();

            CreateMap<Movie, ResponseMovieDto>();
            CreateMap<MovieRate, ResponseMovieRateDto>();

            ///Movie_Actor
            //CreateMap<MovieActor, ResponseMovieActorDto>();
            CreateMap<MovieActor, ResponseActorDto>()
                .ForMember(dest => dest.ActorId, opt => opt.MapFrom(src => src.ActorId))
                .IncludeMembers(src => src.Actor);
            CreateMap<MovieActor, ResponseMovieDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .IncludeMembers(src => src.Movie);

            CreateMap<MovieActor, ResponseMovieActorDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.Actor, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.Actor = context.Mapper.Map<List<ResponseActorDto>>(src.Actor);
                });

            CreateMap<IEnumerable<MovieActor>, ResponseMovieActorDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.First().MovieId))
                .ForMember(dest => dest.Actor, opt => opt.MapFrom(src => src));

            CreateMap<IEnumerable<MovieActor>, ResponseActorMovieDto>()
                .ForMember(dest => dest.ActorId, opt => opt.MapFrom(src => src.First().ActorId))
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src));

            ///Movie_Category
            CreateMap<MovieCategory, ResponseCategoryDto>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .IncludeMembers(src => src.Category);
            CreateMap<MovieCategory, ResponseMovieDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .IncludeMembers(src => src.Movie);

            CreateMap<MovieCategory, ResponseMovieCategoryDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.Category, opt => opt.Ignore()) 
                .AfterMap((src, dest, context) =>
                 {
                    dest.Category = context.Mapper.Map<List<ResponseCategoryDto>>(src.Category);
                 });

            CreateMap<IEnumerable<MovieCategory>, ResponseMovieCategoryDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.First().MovieId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src));

            CreateMap<IEnumerable<MovieCategory>, ResponseCategoryMovieDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.First().CategoryId))
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src));


        }
    }
}
