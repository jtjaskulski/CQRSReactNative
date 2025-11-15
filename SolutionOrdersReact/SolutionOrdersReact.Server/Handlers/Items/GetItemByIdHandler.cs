using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class GetItemByIdHandler
    {
        private readonly ApplicationDbContext _context;

        public GetItemByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(
            GetItemByIdQuery request,
            CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.UnitOfMeasurement)
                .Where(i => i.IdItem == request.Id)    // Filtr po Id
                .Select(i => new ItemDto
                {
                    IdItem = i.IdItem,
                    Name = i.Name,
                    Description = i.Description,
                    IdCategory = i.IdCategory,
                    CategoryName = i.Category.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    IdUnitOfMeasurement = i.IdUnitOfMeasurement,
                    UnitName = i.UnitOfMeasurement != null ? i.UnitOfMeasurement.Name : null,
                    Code = i.Code,
                    IsActive = i.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);  // Pierwszy lub null

            return item;
        }
    }
}