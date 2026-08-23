using Application.DTOs.Category.Request;
using Application.DTOs.Category.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IEventCategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(IEventCategoryRepo categoryService, IMapper mapper) 
        {
            _categoryRepo = categoryService;
            _mapper = mapper;
        }
        public async Task<CategoryResponseDto?> AddAsync(CategoryRequestDto eventCategory)
        {
            if (eventCategory is null)
                throw new ArgumentNullException(nameof(eventCategory), "Event category payload cannot be null.");

            var category = await _categoryRepo.AddAsync(_mapper.Map<EventCategory>(eventCategory));

            return _mapper.Map<CategoryResponseDto?>(category);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int eventCategoryId)
        {
            if (eventCategoryId <= 0)
                throw new ArgumentOutOfRangeException(nameof(eventCategoryId), eventCategoryId, "Event category id must be a positive integer.");

            var category = await _categoryRepo.GetByIdAsync(eventCategoryId);

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<bool> UpdateAsync(int id, CategoryRequestDto eventCategory)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Event category id must be a positive integer.");

            if (eventCategory is null)
                throw new ArgumentNullException(nameof(eventCategory), "Event category update payload cannot be null.");

            var category = await _categoryRepo.GetByIdAsync(id);

            if (category is null)
                throw new KeyNotFoundException($"Event category with ID {id} does not exist.");

            category.Name = eventCategory.Name;
            category.Description = eventCategory.Description;

            return await _categoryRepo.UpdateAsync(category);
        }
    }
}
