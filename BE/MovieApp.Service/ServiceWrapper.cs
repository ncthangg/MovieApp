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
        public IMovieCategoryService MovieCategoryService { get; set; }
        public IMovieRateService MovieRateService { get; set; }
        public ICategoryService CategoryService { get; set; }


        public ServiceWrapper(
             IUserService userService,
             IUserRoleService userRoleService,
             IUserStatusService userStatusService,
             IUserWatchHistoryService userWatchHistory,
             IUserLikeService userLikeService,
             IMovieService movieService,
             IMovieCategoryService movieCategoryService,
             IMovieRateService movieRateService,
             ICategoryService categoryService
             )
        {

            UserService = userService;
            UserRoleService = userRoleService;
            UserStatusService = userStatusService;
            UserWatchHistoryService = userWatchHistory;
            UserLikeService = userLikeService;
            MovieService = movieService;
            MovieCategoryService = movieCategoryService;
            MovieRateService = movieRateService;
            CategoryService = categoryService;
        }
    }
}
