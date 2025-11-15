using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.Items.Queries
{
    public class GetItemByIdQuery : IRequest<ItemDto?>
    {
        public int Id { get; set; }

        public GetItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}
