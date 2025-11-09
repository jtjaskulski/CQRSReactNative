using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    
    }
}