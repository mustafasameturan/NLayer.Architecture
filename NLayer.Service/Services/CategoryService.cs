using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repository;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Repositories;

namespace NLayer.Service.Services;

public class CategoryService : Service<Category>, ICategoryService
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductsAsync(int categoryId)
    {
        var category = await _categoryRepository.GetSingleCategoryByIdWithProductsAsync(categoryId);
        var categoryDto = _mapper.Map<CategoryWithProductsDto>(category);

        return CustomResponseDto<CategoryWithProductsDto>.Success(200, categoryDto);
    }
}