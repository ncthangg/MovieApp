using MovieApp.Common.Base;
using MovieApp.Data.Models;
using MovieApp.Data;
using AutoMapper;

namespace MovieApp.Service.Services.High
{
    public interface IUserTokenService
    {
        Task<ServiceResult> Save(long userId, string refreshToken);
        Task<ServiceResult> CheckRefreshTokenValidity(long userId, string refreshToken);
    }
    public class UserTokenService : IUserTokenService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ServiceWrapper _serviceWrapper;
        private readonly HelperWrapper _helperWrapper;
        public UserTokenService(IMapper mapper, ServiceWrapper serviceWrapper, HelperWrapper helperWrapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
            _serviceWrapper = serviceWrapper;
            _helperWrapper = helperWrapper;
        }
        public async Task<ServiceResult> Save(long userId, string refreshToken)
        {
            var userExist = await _serviceWrapper.UserService.UserExist(userId);
            if (userExist)
            {
                var newToken = new UserToken()
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    RefreshTokenExpires = DateTime.UtcNow.AddDays(7),
                    LastLogin = DateTime.UtcNow,
                };
                await _unitOfWork.UserTokenRepository.CreateAsync(newToken);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, newToken);
            }
            else
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);
            }
        }

        public async Task<ServiceResult> CheckRefreshTokenValidity(long userId, string refreshToken)
        {
            // Tìm UserToken dựa trên UserId và RefreshToken
            var userToken = await _unitOfWork.UserTokenRepository.GetByUserIdAndToken(userId, refreshToken);

            // Kiểm tra nếu không tìm thấy token trong cơ sở dữ liệu
            if (userToken == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Refresh token not found", null);
            }

            // Kiểm tra thời gian hết hạn của RefreshToken
            if (userToken.RefreshTokenExpires.HasValue && userToken.RefreshTokenExpires.Value < DateTime.UtcNow)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Refresh token has expired", null);
            }

            // Nếu token còn hạn
            return new ServiceResult(Const.SUCCESS_READ_CODE, "Refresh token is valid", userToken);
        }
    }
}
