using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Orders.Queries;

namespace SolutionOrdersReact.Server.Handlers.Orders
{
    public class GetAllOrdersHandler(ApplicationDbContext context) : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderEntities = await context.Orders
                .Include(o => o.Client)
                .Include(o => o.Worker)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync(cancellationToken);

            var orders = orderEntities.Adapt<List<OrderDto>>();
            return orders;
        }
    }
}