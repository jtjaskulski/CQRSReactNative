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
            var orders = await context.Orders
                .Include(o => o.Client)
                .Include(o => o.Worker)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Select(o => o.Adapt<OrderDto>())
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}