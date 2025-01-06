using AutoMapper;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data.Models;
using MovieApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Service.Services.Low
{
    public interface ITypeService
    {
        Task<ServiceResult> GetAllType();
        Task<ServiceResult> GetByTypeId(long id);
        Task<ServiceResult> Search(string name);
        Task<ServiceResult> Create(RequestTypeDto request);
        Task<ServiceResult> Update(long id, RequestTypeDto request);
        Task<ServiceResult> DeleteByTypeId(long id);
    }
    public class TypeService : ITypeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TypeService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllType()
        {
            var types = await _unitOfWork.TypeRepository.GetAllAsync();
            if (!types.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseTypeDto>>(types);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> GetByTypeId(long id)
        {
            var type = await _unitOfWork.TypeRepository.GetByIdAsync(id);
            if (type == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseTypeDto>(type);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Search(string name)
        {
            var types = await _unitOfWork.TypeRepository.GetByTypeNameAsync(name);
            if (!types.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseTypeDto>>(types);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> Create(RequestTypeDto request)
        {
            var typeExist = await TypeExist(request.TypeName);
            if (typeExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var newType = new MovieType()
                {
                    TypeName = request.TypeName
                };
                var result = await _unitOfWork.TypeRepository.CreateAsync(newType);

                var response = _mapper.Map<ResponseTypeDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestTypeDto request)
        {
            var typeIdExist = await TypeExist(id);
            var typeNameExist = await TypeExist(request.TypeName);
            if (typeIdExist && !typeNameExist)
            {
                var updateType = new MovieType()
                {
                    TypeId = id,
                    TypeName = request.TypeName
                };
                var result = await _unitOfWork.TypeRepository.UpdateAsync(updateType);

                var response = _mapper.Map<ResponseTypeDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByTypeId(long id)
        {
            var type = await _unitOfWork.TypeRepository.GetByIdAsync(id);
            if (type == null)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
            }
            else
            {
                await _unitOfWork.TypeRepository.RemoveAsync(type);
                var response = _mapper.Map<ResponseTypeDto>(type);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        private async Task<bool> TypeExist(long id)
        {
            return await _unitOfWork.TypeRepository.EntityExistsByPropertyAsync("TypeId", id);
        }
        private async Task<bool> TypeExist(string name)
        {
            return await _unitOfWork.TypeRepository.EntityExistsByPropertyAsync("TypeName", name);
        }
    }
}
