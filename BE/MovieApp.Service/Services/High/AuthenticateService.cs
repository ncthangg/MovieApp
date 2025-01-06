using AutoMapper;
using Microsoft.AspNetCore.Http;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.AuthenticateDtos.Request;
using MovieApp.Common.DTOs.AuthenticateDtos.Response;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data;
using MovieApp.Data.Models;

namespace MovieApp.Service.Services.High
{
    public interface IAuthenticateService
    {
        Task<ServiceResult> Login(RequestLoginDto requestLoginDto);
        Task<ServiceResult> Register(RequestRegisterDto requestRegisterDto);
        Task<ServiceResult> Logout();
    }
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServiceWrapper _serviceWrapper;
        private readonly HelperWrapper _helperWrapper;
        private readonly IUserTokenService _userTokenService;
        private readonly IUserVerificationService _userVerificationService;
        public AuthenticateService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ServiceWrapper serviceWrapper,
            HelperWrapper helperWrapper,
            IUserTokenService userTokenService,
            IUserVerificationService userVerificationService
            )
        {
            _unitOfWork ??= new UnitOfWork();
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _serviceWrapper = serviceWrapper;
            _helperWrapper = helperWrapper;
            _userTokenService = userTokenService;
            _userVerificationService = userVerificationService;
        }

        public async Task<ServiceResult> Login(RequestLoginDto requestLoginDto)
        {
            try
            {
                if (string.IsNullOrEmpty(requestLoginDto.Email) || string.IsNullOrEmpty(requestLoginDto.Password))
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Email và Password không được để trống", null);
                }

                var userExist = await _serviceWrapper.UserService.GetByUserEmail(requestLoginDto.Email);
                var userData = userExist.Data as User;
                if (userData == null || !_helperWrapper.PasswordHelper.VerifyPassword(requestLoginDto.Password, userData.Passwordhash))
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, "User không tồn tại hoặc Password không đúng", null);
                }

                if (!userData.IsVerified)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Email chưa được xác minh.", null);
                }

                // Tạo token
                var token = _helperWrapper.TokenHelper.GenerateJWT(userData, userData.Role.RoleName);
                var refreshToken = _helperWrapper.TokenHelper.GenerateRefreshToken();

                // Lưu Refresh token
                var saveTokenResult = await _userTokenService.Save(userData.UserId, refreshToken);
                if (saveTokenResult.Data != null)
                {
                    return new ServiceResult(Const.ERROR_EXCEPTION, "Lỗi khi lưu Refresh Token", null);
                }
                var tokenData = saveTokenResult.Data as UserToken;

                // Xử lí DTO trả về
                var responseUserDto = _mapper.Map<ResponseUserDto>(userData);
                var responseUserTokenDto = _mapper.Map<ResponseUserTokenDto>(tokenData);

                var response = new ResponseLoginDto
                {
                    ResponseUserDto = responseUserDto,
                    ResponseUserTokenDto = responseUserTokenDto,
                    Token = token
                };

                return new ServiceResult(Const.SUCCESS_READ_CODE, "Login thành công", response);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        public async Task<ServiceResult> Register(RequestRegisterDto request)
        {
            try
            {
                // Kiểm tra người dùng đã tồn tại chưa
                var userExist = await _serviceWrapper.UserService.GetByUserEmail(request.Email);

                var userData = new User();
                if (userExist.Data != null)
                {
                    userData = userExist.Data as User;
                }

                var response = new ResponseRegisterDto
                {
                    ResponseUserDto = null,
                    ResponseUserVerificationDto = null,
                };

                if (userData != null)
                {
                    // Người dùng đã tồn tại -> Response
                    if (userData.IsVerified)
                    {
                        response.ResponseUserDto = _mapper.Map<ResponseUserDto>(userData);
                        return new ServiceResult(Const.ERROR_EXCEPTION, "User đã tồn tại", response);
                    }

                }
                else
                {
                    var createUserResult = await AddAccount(request);
                    if (createUserResult.Status < 0)
                    {
                        return new ServiceResult(Const.ERROR_EXCEPTION, "Register thất bại");
                    }
                    userData = createUserResult.Data as User;
                    if (userData == null)
                    {
                        return new ServiceResult(Const.ERROR_EXCEPTION, "Lỗi Mapping");
                    }
                }

                // Xử lí gửi Verify Code
                var verificationResult = await _userVerificationService.HandleVerificationCode(userData);
                if (verificationResult.Status < 0)
                {
                    return new ServiceResult(Const.ERROR_EXCEPTION, "Gửi code thất bại");
                }

                // Xử lí DTO trả về
                response.ResponseUserDto = _mapper.Map<ResponseUserDto>(userData);
                response.ResponseUserVerificationDto = _mapper.Map<ResponseUserVerificationDto>(verificationResult.Data);

                return new ServiceResult(Const.SUCCESS_CREATE_CODE, "Register thành công, Verification Code đã được gửi.", response);
            }
            catch (Exception ex)
            {
                // Log lỗi tại đây nếu cần
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        private async Task<ServiceResult> AddAccount(RequestRegisterDto requestRegisterDto)
        {
            if (requestRegisterDto.Password.Equals(requestRegisterDto.ConfirmPassword))
            {
                var hashedPassword = _helperWrapper.PasswordHelper.HashPassword(requestRegisterDto.Password);

                var role = await _serviceWrapper.UserRoleService.GetByRoleName("member");
                var roleId = (role.Data as UserRole).RoleId;

                var status = await _serviceWrapper.UserStatusService.GetByStatusName("deactive");
                var statusId = (status.Data as UserStatus).StatusId;

                var newUser = new User()
                {
                    Name = requestRegisterDto.Name,
                    Email = requestRegisterDto.Email,
                    Passwordhash = hashedPassword,
                    RoleId = roleId,
                    StatusId = statusId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _serviceWrapper.UserService.Create(newUser);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, newUser);
            }

            return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);

        }
        public Task<ServiceResult> Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear(); // Xóa toàn bộ session

            // Trả về một thông báo khi đăng xuất thành công
            return Task.FromResult(new ServiceResult(Const.SUCCESS_READ_CODE, "Xóa Session thành công."));
        }

    }
}
