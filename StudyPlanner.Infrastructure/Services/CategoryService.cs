using Microsoft.EntityFrameworkCore;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Tasks)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Tasks)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

		public async Task DeleteCategoryAsync(int id)
		{
			var category = await _context.Categories
				.Include(c => c.Tasks)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category != null)
			{
				foreach (var task in category.Tasks)
				{
					task.CategoryId = null;
				}

				_context.Categories.Remove(category);
				await _context.SaveChangesAsync();
			}
		}
	}
}