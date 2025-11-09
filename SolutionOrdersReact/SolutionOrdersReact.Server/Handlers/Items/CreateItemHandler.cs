using Mapster;
using MediatR;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Commands;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class CreateItemHandler(ApplicationDbContext context) : IRequestHandler<CreateItemCommand, int>
    {
        public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = request.Adapt<Item>();

            context.Items.Add(item);
            await context.SaveChangesAsync(cancellationToken);

            return item.IdItem;
        }
    }
}