using MovieApp.Service.Services;
using MovieApp.Service.Services.Low;

namespace MovieApp.Service
{
    public class ServiceWrapper
    {
        public IUserService UserService { get; set; }
        public IUserRoleService UserRoleService { get; set; }
        public IUserStatusService UserStatusService { get; set; }
        public IUserWatchHistoryService UserWatchHistoryService { get; set; }
        public IUserLikeService UserLikeService { get; set; }
        //==============================
        public IMovieService MovieService { get; set; }
        public IMovieSeasonService MovieSeasonService { get; set; }
        public IMovieEpisodeService MovieEpisodeService { get; set; }
        //==============================
        public IMovieActorService MovieActorService { get; set; }
        public IMovieCategoryService MovieCategoryService { get; set; }
        public IMovieRateService MovieRateService { get; set; }

        //==============================
        public ICategoryService CategoryService { get; set; }
        public ITypeService TypeService { get; set; }
        public IActorService ActorService { get; set; }


        public ServiceWrapper(
             IUserService userService,
             IUserRoleService userRoleService,
             IUserStatusService userStatusService,
             IUserWatchHistoryService userWatchHistory,
             IUserLikeService userLikeService,

             IMovieService movieService,
             IMovieSeasonService movieSeasonService,
             IMovieEpisodeService movieEpisodeService,

             IMovieActorService movieActorService,
             IMovieCategoryService movieCategoryService,
             IMovieRateService movieRateService,

             ICategoryService categoryService,
             ITypeService typeService,
             IActorService actorService

             )
        {
            UserService = userService;
            UserRoleService = userRoleService;
            UserStatusService = userStatusService;
            UserWatchHistoryService = userWatchHistory;
            UserLikeService = userLikeService;

            MovieService = movieService;
            MovieSeasonService = movieSeasonService;
            MovieEpisodeService = movieEpisodeService;

            MovieActorService = movieActorService;
            MovieCategoryService = movieCategoryService;
            MovieRateService = movieRateService;

            CategoryService = categoryService;
            TypeService = typeService;
            ActorService = actorService;
        }
    }
}
