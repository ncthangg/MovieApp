using AutoMapper;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data.Models;
using MovieApp.Data;

namespace MovieApp.Service.Services.Low
{
    public interface IActorService
    {
        Task<ServiceResult> GetAllActor();
        Task<ServiceResult> GetByActorId(long id);
        Task<ServiceResult> Search(string name);
        Task<ServiceResult> Create(RequestActorDto request);
        Task<ServiceResult> Update(long id, RequestActorDto request);
        Task<ServiceResult> DeleteByActorId(long id);
    }
    public class ActorService : IActorService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ActorService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllActor()
        {
            var actors = await _unitOfWork.ActorRepository.GetAllAsync();
            if (!actors.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseActorDto>>(actors);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> GetByActorId(long id)
        {
            var category = await _unitOfWork.ActorRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseActorDto>(category);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Search(string name)
        {
            var categories = await _unitOfWork.ActorRepository.GetByActorNameAsync(name);
            if (!categories.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseActorDto>>(categories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> Create(RequestActorDto request)
        {
            var categoryExist = await ActorExist(request.ActorName);
            if (categoryExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var newActor = new Actor()
                {
                    ActorName = request.ActorName
                };
                var result = await _unitOfWork.ActorRepository.CreateAsync(newActor);

                var response = _mapper.Map<ResponseActorDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestActorDto request)
        {
            var categoryIdExist = await ActorExist(id);
            var categoryNameExist = await ActorExist(request.ActorName);
            if (categoryIdExist && !categoryNameExist)
            {
                var updateActor = new Actor()
                {
                    ActorId = id,
                    ActorName = request.ActorName
                };
                var result = await _unitOfWork.ActorRepository.UpdateAsync(updateActor);

                var response = _mapper.Map<ResponseActorDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByActorId(long id)
        {
            var category = await _unitOfWork.ActorRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
            }
            else
            {
                await _unitOfWork.ActorRepository.RemoveAsync(category);
                var response = _mapper.Map<ResponseActorDto>(category);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        private async Task<bool> ActorExist(long id)
        {
            return await _unitOfWork.ActorRepository.EntityExistsByPropertyAsync("ActorId", id);
        }
        private async Task<bool> ActorExist(string name)
        {
            return await _unitOfWork.ActorRepository.EntityExistsByPropertyAsync("ActorName", name);
        }
    }
}
