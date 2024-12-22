using Microsoft.EntityFrameworkCore;
using MovieApp.Data.DBContext;
using MovieApp.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Data
{
    public class UnitOfWork
    {
        private UserRepository _userRepository;
        private UserRoleRepository _userRoleRepository;
        private UserStatusRepository _userStatusRepository;
        private UserTokenRepository _userTokenRepository;
        private UserVerificationRepository _userVerificationRepository;
        private UserWatchHistoryRepository _userWatchHistoryRepository;
        private UserLikeRepository _userLikeRepository;

        private MovieRepository _movieRepository;
        private MovieCategoryRepository _movieCategoryRepository;
        private MovieRateRepository _movieRateRepository;
        private CategoryRepository _categoryRepository;

        private MovieAppDBContext _dbContext;
        public UnitOfWork()
        {
            _dbContext ??= new MovieAppDBContext(); // nếu null thì mới new 
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new Repository.UserRepository(_dbContext);
            }
        }
        public UserRoleRepository UserRoleRepository
        {
            get
            {
                return _userRoleRepository ??= new Repository.UserRoleRepository(_dbContext);
            }
        }
        public UserStatusRepository UserStatusRepository
        {
            get
            {
                return _userStatusRepository ??= new Repository.UserStatusRepository(_dbContext);
            }
        }
        public UserTokenRepository UserTokenRepository
        {
            get
            {
                return _userTokenRepository ??= new Repository.UserTokenRepository(_dbContext);
            }
        }
        public UserVerificationRepository UserVerificationRepository
        {
            get
            {
                return _userVerificationRepository ??= new Repository.UserVerificationRepository(_dbContext);
            }
        }
        public UserWatchHistoryRepository UserWatchHistoryRepository
        {
            get
            {
                return _userWatchHistoryRepository ??= new Repository.UserWatchHistoryRepository(_dbContext);
            }
        }
        public UserLikeRepository UserLikeRepository
        {
            get
            {
                return _userLikeRepository ??= new Repository.UserLikeRepository(_dbContext);
            }
        }


        public MovieRepository MovieRepository
        {
            get
            {
                return _movieRepository ??= new Repository.MovieRepository(_dbContext);
            }
        }
        public MovieCategoryRepository MovieCategoryRepository
        {
            get
            {
                return _movieCategoryRepository ??= new Repository.MovieCategoryRepository(_dbContext);
            }
        }
        public MovieRateRepository MovieRateRepository
        {
            get
            {
                return _movieRateRepository ??= new Repository.MovieRateRepository(_dbContext);
            }
        }
        public CategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new Repository.CategoryRepository(_dbContext);
            }
        }

    }
}
