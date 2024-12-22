using AutoMapper;
using Azure;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data;
using MovieApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Service.Services
{
    public interface IUserService
    {
        Task<ServiceResult> GetAllUser();
        Task<ServiceResult> GetByUserId(long id);
        Task<ServiceResult> GetByUserEmail(string email);
        Task<ServiceResult> GetByUserName(string name);
        Task<ServiceResult> Create(User x);
        Task<ServiceResult> Update(long id, RequestUserDto x);
        Task<ServiceResult> DeleteByUserId(long id);
        Task<bool> UserExist(long id);
        Task<bool> UserExist(string email);

    }
    public interface IUserLikeService
    {
        Task<ServiceResult> GetByUserId(long id);
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> Like(RequestUserLikeDto x);
        Task<ServiceResult> UnLike(RequestUserLikeDto x);
    }
    public interface IUserRoleService
    {
        Task<ServiceResult> GetAllUserRole();
        Task<ServiceResult> GetByRoleId(long id);
        Task<ServiceResult> GetByRoleName(string name);
        Task<ServiceResult> Search(string name);
        Task<ServiceResult> Create(RequestUserRoleDto role);
        Task<ServiceResult> Update(long id, RequestUserRoleDto role);
        Task<ServiceResult> DeleteByRoleId(long id);
    }
    public interface IUserStatusService
    {
        Task<ServiceResult> GetAllUserStatus();
        Task<ServiceResult> GetByStatusId(long id);
        Task<ServiceResult> GetByStatusName(string name);
        Task<ServiceResult> Search(string name);
        Task<ServiceResult> Create(RequestUserStatusDto x);
        Task<ServiceResult> Update(long id, RequestUserStatusDto status);
        Task<ServiceResult> DeleteByStatusId(long id);
    }
    public interface IUserWatchHistoryService
    {
        Task<ServiceResult> GetByUserId(long id);
        Task<ServiceResult> GetByMovieId(long id);
        Task<ServiceResult> Create(RequestUserWatchHistoryDto x);
        Task<ServiceResult> Update(long id, RequestUserWatchHistoryDto x);
        Task<ServiceResult> Delete(RequestUserWatchHistoryDto x);
    }

    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllUser()
        {
            var result = await _unitOfWork.UserRepository.GetAllAsync();
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByUserId(long id)
        {
            var result = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseUserDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByUserEmail(string email)
        {
            var result = await _unitOfWork.UserRepository.GetByUserEmailAsync(email);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseUserDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByUserName(string name)
        {
            var result = await _unitOfWork.UserRepository.GetByUserNameAsync(name);
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Create(User x)
        {
            var userEmailExist = await this.UserExist(x.Email);
            if (userEmailExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);
            }
            else
            {
                var newUser = new User()
                {
                    Name = x.Name,
                    Email = x.Email,
                    Passwordhash = x.Passwordhash,
                    RoleId = x.RoleId,
                    StatusId = x.StatusId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.UserRepository.CreateAsync(newUser);

                var response = _mapper.Map<ResponseUserDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestUserDto x)
        {
            var userIdExist = await this.UserExist(id);
            if (userIdExist)
            {
                var userExistData = (await this.GetByUserId(id)).Data as User;
                var userUpdate = new User()
                {
                    Name = x.Name,
                    Email = userExistData.Email,
                    Passwordhash = userExistData.Passwordhash,
                    RoleId = x.RoleId,
                    StatusId = x.StatusId,
                    CreatedAt = userExistData.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.UserRepository.UpdateAsync(userUpdate);

                var response = _mapper.Map<ResponseUserDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, null);
            }
        }
        public async Task<ServiceResult> DeleteByUserId(long id)
        {
            var userExist = await this.GetByUserId(id);
            if (userExist == null)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
            }
            else
            {
                var userExistData = userExist.Data as User;
                var result = await _unitOfWork.UserRepository.RemoveAsync(userExistData);
                if (!result)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
                }
                var response = _mapper.Map<ResponseUserDto>(userExistData);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        public async Task<bool> UserExist(long id)
        {
            return await _unitOfWork.UserRepository.EntityExistsByPropertyAsync("UserId", id);
        }
        public async Task<bool> UserExist(string email)
        {
            return await _unitOfWork.UserRepository.EntityExistsByPropertyAsync("Email", email);
        }
    }
    public class UserLikeService : IUserLikeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserLikeService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetByUserId(long id)
        {
            var result = await _unitOfWork.UserLikeRepository.GetByUserIdAsync(id);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserLikeDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByMovieId(long id)
        {
            var result = await _unitOfWork.UserLikeRepository.GetByMovieIdAsync(id);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserLikeDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Like(RequestUserLikeDto x)
        {
            var liked = await _unitOfWork.UserLikeRepository.LikeExistAsync(x.MovieId, x.UserId);
            if (!liked)
            {
                var newLike = new UserLike()
                {
                    MovieId = x.MovieId,
                    UserId = x.UserId,
                    CreatedAt = DateTime.UtcNow,
                };
                var result = await _unitOfWork.UserLikeRepository.CreateAsync(newLike);

                var response = _mapper.Map<ResponseUserLikeDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
        }
        public async Task<ServiceResult> UnLike(RequestUserLikeDto x)
        {
            var liked = await _unitOfWork.UserLikeRepository.LikeExistAsync(x.MovieId, x.UserId);
            if (liked)
            {
                var movieLike = new UserLike()
                {
                    MovieId = x.MovieId,
                    UserId = x.UserId,
                };
                var result = await _unitOfWork.UserLikeRepository.RemoveAsync(movieLike);
                if (!result)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
                }
                var response = _mapper.Map<ResponseUserLikeDto>(movieLike);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
        }
    }
    public class UserRoleService : IUserRoleService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserRoleService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllUserRole()
        {
            var result = await _unitOfWork.UserRoleRepository.GetAllAsync();
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserRoleDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByRoleId(long id)
        {
            var result = await _unitOfWork.UserRoleRepository.GetByIdAsync(id);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseUserRoleDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByRoleName(string name)
        {
            var result = await _unitOfWork.UserRoleRepository.GetByRoleNameAsync(name);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseUserRoleDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Search(string name)
        {
            var result = await _unitOfWork.UserRoleRepository.GetListByRoleNameAsync(name);
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserRoleDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Create(RequestUserRoleDto x)
        {
            var roleNameExist = await this.UserRoleExist(x.RoleName);
            if (roleNameExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);
            }
            else
            {
                var roleCreate = new UserRole()
                {
                    RoleName = x.RoleName,
                };
                var result = await _unitOfWork.UserRoleRepository.CreateAsync(roleCreate);

                var response = _mapper.Map<ResponseUserRoleDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestUserRoleDto x)
        {
            var roleIdExist = await this.UserRoleExist(id);
            var roleNameExist = await this.UserRoleExist(x.RoleName);
            if (roleIdExist && !roleNameExist)
            {
                var roleUpdate = new UserRole()
                {
                    RoleId = id,
                    RoleName = x.RoleName,
                };
                var result = await _unitOfWork.UserRoleRepository.UpdateAsync(roleUpdate);

                var response = _mapper.Map<ResponseUserRoleDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
        }
        public async Task<ServiceResult> DeleteByRoleId(long id)
        {
            var userRole = await this.GetByRoleId(id);
            if (userRole.Status < 0)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
            }
            else
            {
                var userRoleData = userRole.Data as UserRole;
                var result = await _unitOfWork.UserRoleRepository.RemoveAsync(userRoleData);
                if (!result)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
                }
                var response = _mapper.Map<ResponseUserRoleDto>(userRoleData);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        private async Task<bool> UserRoleExist(long id)
        {
            return await _unitOfWork.UserRoleRepository.EntityExistsByPropertyAsync("RoleId", id);
        }
        private async Task<bool> UserRoleExist(string name)
        {
            return await _unitOfWork.UserRoleRepository.EntityExistsByPropertyAsync("RoleName", name);
        }
    }
    public class UserStatusService : IUserStatusService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserStatusService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllUserStatus()
        {
            var result = await _unitOfWork.UserStatusRepository.GetAllAsync();
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserStatusDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByStatusId(long id)
        {
            var result = await _unitOfWork.UserStatusRepository.GetByIdAsync(id);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseUserStatusDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByStatusName(string name)
        {
            var result = await _unitOfWork.UserStatusRepository.GetByStatusNameAsync(name);
            if (result == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseUserStatusDto>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Search(string name)
        {
            var result = await _unitOfWork.UserStatusRepository.GetListByStatusNameAsync(name);
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserStatusDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Create(RequestUserStatusDto x)
        {
            var statusExist = await this.UserStatusExist(x.StatusName);
            if (statusExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);
            }
            else
            {
                var statusCreate = new UserStatus()
                {
                    StatusName = x.StatusName,
                };
                var result = await _unitOfWork.UserStatusRepository.CreateAsync(statusCreate);

                var response = _mapper.Map<ResponseUserStatusDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestUserStatusDto x)
        {
            var statusIdExist = await this.UserStatusExist(x.StatusName);
            var statusNameExist = await this.UserStatusExist(x.StatusName);
            if (statusIdExist && !statusNameExist)
            {
                var statusUpdate = new UserStatus()
                {
                    StatusId = id,
                    StatusName = x.StatusName,
                };
                var result = await _unitOfWork.UserStatusRepository.UpdateAsync(statusUpdate);

                var response = _mapper.Map<ResponseUserStatusDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, null);
            }
        }
        public async Task<ServiceResult> DeleteByStatusId(long id)
        {
            var status = await this.GetByStatusId(id);
            if (status.Status < 0)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
            }
            else
            {
                var statusData = status.Data as UserStatus;
                var result = await _unitOfWork.UserStatusRepository.RemoveAsync(statusData);

                var response = _mapper.Map<ResponseUserStatusDto>(statusData);
                if (!result)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
                }

                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, status);
            }
        }

        private async Task<bool> UserStatusExist(long id)
        {
            return await _unitOfWork.UserStatusRepository.EntityExistsByPropertyAsync("StatusId", id);
        }
        private async Task<bool> UserStatusExist(string name)
        {
            return await _unitOfWork.UserStatusRepository.EntityExistsByPropertyAsync("StatusName", name);
        }
    }
    public class UserWatchHistoryService : IUserWatchHistoryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserWatchHistoryService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetByUserId(long id)
        {
            var result = await _unitOfWork.UserWatchHistoryRepository.GetByUserIdAsync(id);
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserWatchHistoryDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> GetByMovieId(long id)
        {
            var result = await _unitOfWork.UserWatchHistoryRepository.GetByMovieIdAsync(id);
            if (!result.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseUserWatchHistoryDto>>(result);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Create(RequestUserWatchHistoryDto x)
        {
            var movieWatched = await this.MovieWatched(x);
            if (movieWatched.Status < 0)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);
            }
            else
            {
                var request = _mapper.Map<UserWatchHistory>(x);
                var result = await _unitOfWork.UserWatchHistoryRepository.CreateAsync(request);

                var response = _mapper.Map<ResponseUserWatchHistoryDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestUserWatchHistoryDto x)
        {
            var movieWatched = await this.MovieWatched(x);
            if (movieWatched.Status < 0)
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, null);
            }
            else
            {
                var request = _mapper.Map<UserWatchHistory>(x);
                var result = await _unitOfWork.UserWatchHistoryRepository.UpdateAsync(request);

                var response = _mapper.Map<ResponseUserWatchHistoryDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, x);
            }
        }
        public async Task<ServiceResult> Delete(RequestUserWatchHistoryDto x)
        {
            var movieWatched = await this.MovieWatched(x);
            if (movieWatched.Status < 0)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
            }
            else
            {
                var request = _mapper.Map<UserWatchHistory>(x);

                var result = await _unitOfWork.UserWatchHistoryRepository.RemoveAsync(request);
                if (!result)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, null);
                }

                var response = _mapper.Map<ResponseUserWatchHistoryDto>(result);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        private async Task<ServiceResult> MovieWatched(RequestUserWatchHistoryDto x)
        {
            var watched = await _unitOfWork.UserWatchHistoryRepository.MovieWatchedAsync((long)x.MovieId, x.UserId);
            if (!watched)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, x);
            }
        }

    }
}
