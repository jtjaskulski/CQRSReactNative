using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Handlers.Items;

public class GetAllItemsHandler(ApplicationDbContext context) : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
{
    public async Task<List<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await context.Items
            .Include(i => i.Category)
            .Include(i => i.UnitOfMeasurement)
            .Where(i => i.IsActive)
            .Select(i => new ItemDto
            {
                IdItem = i.IdItem,
                Name = i.Name,
                Description = i.Description,
                CategoryName = i.Category.Name,
                Price = i.Price,
                Quantity = i.Quantity,
                UnitName = i.UnitOfMeasurement != null ? i.UnitOfMeasurement.Name : null,
                Code = i.Code,
                IsActive = i.IsActive
            })
            .ToListAsync(cancellationToken);

        return items;
    }
}