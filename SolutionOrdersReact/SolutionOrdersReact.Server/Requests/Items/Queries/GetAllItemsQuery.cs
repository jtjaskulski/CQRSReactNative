using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.Items.Queries
{
    public class GetAllItemsQuery : IRequest<List<ItemDto>>
    {
    }
}