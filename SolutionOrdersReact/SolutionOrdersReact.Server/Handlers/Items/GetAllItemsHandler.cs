using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class GetAllItemsHandler(ApplicationDbContext _context) : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(
            GetAllItemsQuery request,
            CancellationToken cancellationToken)
        {
            // Query do bazy z Include (EAGER LOADING)
            var items = await _context.Items
                .Include(i => i.Category)              // JOIN z Category
                .Include(i => i.UnitOfMeasurement)     // JOIN z UnitOfMeasurement
                .Where(i => i.IsActive)                // Tylko aktywne
                .OrderBy(i => i.Name)                  // Sortowanie
                .Select(i => new ItemDto               // Projekcja do DTO
                {
                    IdItem = i.IdItem,
                    Name = i.Name,
                    Description = i.Description,
                    IdCategory = i.IdCategory,
                    CategoryName = i.Category.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    IdUnitOfMeasurement = i.IdUnitOfMeasurement,
                    UnitName = i.UnitOfMeasurement != null
                        ? i.UnitOfMeasurement.Name
                        : null,
                    Code = i.Code,
                    IsActive = i.IsActive
                })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}