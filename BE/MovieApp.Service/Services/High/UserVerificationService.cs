using AutoMapper;
using MovieApp.Common.Base;
using MovieApp.Data.Models;
using MovieApp.Data;

namespace MovieApp.Service.Services
{
    public interface IUserVerificationService
    {
        Task<ServiceResult> GetByUserId(long userId);
        Task<ServiceResult> Save(long userId, string verificationCode);
        Task<ServiceResult> CheckVerificationCodeValidity(long userId, string verificationCode);
        Task<ServiceResult> HandleVerificationCode(User user);
    }
    public class UserVerificationService : IUserVerificationService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ServiceWrapper _serviceWrapper;
        private readonly HelperWrapper _helperWrapper;
        public UserVerificationService(IMapper mapper, ServiceWrapper serviceWrapper, HelperWrapper helperWrapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
            _serviceWrapper = serviceWrapper;
            _helperWrapper = helperWrapper;
        }

        public async Task<ServiceResult> GetByUserId(long userId)
        {
            var user = await _unitOfWork.UserVerificationRepository.GetByUserId(userId);
            if (user == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, user);
            }
        }
        public async Task<ServiceResult> Save(long userId, string verificationCode)
        {
            var userExist = await _serviceWrapper.UserService.UserExist(userId);
            if (userExist)
            {
                var code = await _unitOfWork.UserVerificationRepository.GetByUserId(userId);

                if (code == null)
                {
                    var newCode = new UserVerification()
                    {
                        UserId = userId,
                        VerificationCode = verificationCode,
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                        IsUsed = false,
                    };
                    await _unitOfWork.UserVerificationRepository.CreateAsync(newCode);
                    return new ServiceResult(Const.SUCCESS_CREATE_CODE, "Verification code created successfully", newCode);
                }

                // Trường hợp mã đã hết hạn hoặc đã được sử dụng, cập nhật mã mới
                if (code.ExpiresAt < DateTime.UtcNow || code.IsUsed)
                {
                    code.VerificationCode = verificationCode;
                    code.CreatedAt = DateTime.UtcNow;
                    code.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
                    code.IsUsed = false;  // Đảm bảo mã chưa sử dụng

                    await _unitOfWork.UserVerificationRepository.UpdateAsync(code);
                    return new ServiceResult(Const.SUCCESS_CREATE_CODE, "Verification code updated successfully", code);
                }

                return new ServiceResult(Const.SUCCESS_CREATE_CODE, "Verification code is still valid", code);

            }
            else
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, null);
            }
        }
        public async Task<ServiceResult> CheckVerificationCodeValidity(long userId, string verificationCode)
        {
            // Tìm UserToken dựa trên UserId và RefreshToken
            var code = await _unitOfWork.UserVerificationRepository.GetByUserId(userId);

            // Kiểm tra nếu không tìm thấy mã xác thực
            if (code == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Verification code not found", null);
            }

            // Kiểm tra mã xác thực có khớp không
            if (!code.VerificationCode.Equals(verificationCode, StringComparison.OrdinalIgnoreCase))
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Invalid verification code", null);
            }

            // Kiểm tra mã xác thực đã sử dụng chưa
            if (code.IsUsed)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Verification code has already been used", null);
            }

            // Kiểm tra thời gian hết hạn của mã xác thực
            if (code.ExpiresAt < DateTime.UtcNow)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, "Verification code has expired", null);
            }

            return new ServiceResult(Const.SUCCESS_READ_CODE, "Verification code is valid", code);
        }

        public async Task<ServiceResult> HandleVerificationCode(User user)
        {

            var newVerificationCode = _helperWrapper.TokenHelper.GenerateVerificationCode();

            var saveResult = await this.Save(user.UserId, newVerificationCode);
            if (saveResult.Status < 0)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, "Error saving verification code");
            }

            // Gửi email xác thực
            var verificationLink = $"https://yourdomain.com/verify?code={newVerificationCode}";
            await _helperWrapper.EmailHelper.SendVerificationEmailAsync(user.Email, newVerificationCode, verificationLink);

            return new ServiceResult(Const.SUCCESS_CREATE_CODE, "Verification code sent", newVerificationCode);
        }


    }
}
