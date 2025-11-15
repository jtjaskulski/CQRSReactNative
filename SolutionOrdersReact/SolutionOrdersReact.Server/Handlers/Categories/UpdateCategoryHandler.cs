using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Categories.Commands;

namespace SolutionOrdersReact.Server.Handlers.Categories
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UpdateCategoryHandler> _logger;

        public UpdateCategoryHandler(
            ApplicationDbContext context,
            ILogger<UpdateCategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Aktualizacja kategorii o ID: {IdCategory}", request.IdCategory);

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.IdCategory == request.IdCategory, cancellationToken);

            if (category == null)
            {
                _logger.LogError("Kategoria o ID {IdCategory} nie została znaleziona", request.IdCategory);
                throw new KeyNotFoundException($"Kategoria o ID {request.IdCategory} nie istnieje");
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Zaktualizowano kategorię o ID: {IdCategory}", request.IdCategory);

            return Unit.Value;
        }
    }
}

