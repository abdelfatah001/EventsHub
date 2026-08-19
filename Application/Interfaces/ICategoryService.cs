
using Application.DTOs.Category.Request;
using Application.DTOs.Category.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto?> AddAsync(CategoryRequestDto eventCategory);
        Task<CategoryResponseDto?> GetByIdAsync(int eventCategoryId);
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
        Task<bool> UpdateAsync(int id, CategoryRequestDto eventCategory);
    }
}
