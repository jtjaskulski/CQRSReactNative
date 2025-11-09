using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class GetAllItemsHandler(ApplicationDbContext context) : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await context.Items
                .AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.UnitOfMeasurement)
                .Where(i => i.IsActive)
                .Select(i => i.Adapt<ItemDto>())
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}