using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Commands;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteItemHandler> _logger;

        public DeleteItemHandler(
            ApplicationDbContext context,
            ILogger<DeleteItemHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            DeleteItemCommand request,
            CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.IdItem == request.IdItem, cancellationToken);

            if (item == null)
            {
                throw new KeyNotFoundException($"Produkt o ID {request.IdItem} nie istnieje");
            }

            _logger.LogInformation("Usuwanie produktu ID: {IdItem}", request.IdItem);

            // Soft delete - tylko ustawiamy IsActive = false
            item.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);

            // Lub hard delete:
            // _context.Items.Remove(item);
            // await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Usunięto produkt ID: {IdItem}", request.IdItem);

            return Unit.Value;
        }
    }
}