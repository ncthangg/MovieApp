﻿using AutoMapper;
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
            CreateMap<Category, ResponseCategoryDto>();
            CreateMap<Movie, ResponseMovieDto>();

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


            CreateMap<MovieRate, ResponseMovieRateDto>();
        }
    }
}
