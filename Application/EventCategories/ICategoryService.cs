using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using Domain.Entities;

namespace Application.EventCategories
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto?> AddAsync(CategoryRequestDto eventCategory);
        Task<CategoryResponseDto?> GetByIdAsync(int eventCategoryId);
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
        Task<bool> UpdateAsync(int id, CategoryRequestDto eventCategory);
    }
}
