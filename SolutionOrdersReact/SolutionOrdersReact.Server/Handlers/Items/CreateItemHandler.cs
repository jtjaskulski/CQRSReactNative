using MediatR;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Commands;

namespace SolutionOrdersReact.Server.Handlers.Items;

public class CreateItemHandler: IRequestHandler<CreateItemCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateItemHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Name = request.Name,
            Description = request.Description,
            IdCategory = request.IdCategory,
            Price = request.Price,
            Quantity = request.Quantity,
            FotoUrl = request.FotoUrl,
            IdUnitOfMeasurement = request.IdUnitOfMeasurement,
            Code = request.Code,
            IsActive = true
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return item.IdItem;
    }
}