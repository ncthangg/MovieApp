using AutoMapper;
using MovieApp.Common.Base;
using MovieApp.Common.DTOs.Request;
using MovieApp.Common.DTOs.Response;
using MovieApp.Data.Models;
using MovieApp.Data;

namespace MovieApp.Service.Services.Low
{
    public interface ICategoryService
    {
        Task<ServiceResult> GetAllCategory();
        Task<ServiceResult> GetByCategoryId(long id);
        Task<ServiceResult> Search(string name);
        Task<ServiceResult> Create(RequestCategoryDto request);
        Task<ServiceResult> Update(long id, RequestCategoryDto request);
        Task<ServiceResult> DeleteByCategoryId(long id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<ServiceResult> GetAllCategory()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (!categories.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseCategoryDto>>(categories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> GetByCategoryId(long id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<ResponseCategoryDto>(category);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
        }
        public async Task<ServiceResult> Search(string name)
        {
            var categories = await _unitOfWork.CategoryRepository.GetByCategoryNameAsync(name);
            if (!categories.Any())
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, null);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<ResponseCategoryDto>>(categories);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response, response.Count());
            }
        }
        public async Task<ServiceResult> Create(RequestCategoryDto request)
        {
            var categoryExist = await CategoryExist(request.CategoryName);
            if (categoryExist)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            else
            {
                var newCategory = new Category()
                {
                    CategoryName = request.CategoryName
                };
                var result = await _unitOfWork.CategoryRepository.CreateAsync(newCategory);

                var response = _mapper.Map<ResponseCategoryDto>(result);
                return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response);
            }
        }
        public async Task<ServiceResult> Update(long id, RequestCategoryDto request)
        {
            var categoryIdExist = await CategoryExist(id);
            var categoryNameExist = await CategoryExist(request.CategoryName);
            if (categoryIdExist && !categoryNameExist)
            {
                var updateCategory = new Category()
                {
                    CategoryId = id,
                    CategoryName = request.CategoryName
                };
                var result = await _unitOfWork.CategoryRepository.UpdateAsync(updateCategory);

                var response = _mapper.Map<ResponseCategoryDto>(result);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        public async Task<ServiceResult> DeleteByCategoryId(long id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
            }
            else
            {
                await _unitOfWork.CategoryRepository.RemoveAsync(category);
                var response = _mapper.Map<ResponseCategoryDto>(category);
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, response);
            }
        }
        private async Task<bool> CategoryExist(long id)
        {
            return await _unitOfWork.CategoryRepository.EntityExistsByPropertyAsync("CategoryId", id);
        }
        private async Task<bool> CategoryExist(string name)
        {
            return await _unitOfWork.CategoryRepository.EntityExistsByPropertyAsync("CategoryName", name);
        }
    }
}
