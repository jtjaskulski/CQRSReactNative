using MediatR;

namespace SolutionOrdersReact.Server.Requests.Items.Commands
{
    public class DeleteItemCommand : IRequest<Unit>
    {
        public int IdItem { get; set; }

        public DeleteItemCommand(int idItem)
        {
            IdItem = idItem;
        }
    }
}
