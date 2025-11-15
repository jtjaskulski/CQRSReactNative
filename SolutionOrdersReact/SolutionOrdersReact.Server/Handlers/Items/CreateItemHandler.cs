using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Commands;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class CreateItemHandler(ApplicationDbContext context, ILogger<CreateItemHandler> logger) : IRequestHandler<CreateItemCommand, int>
    {
        public async Task<int> Handle(
            CreateItemCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Tworzenie nowego produktu: {Name}", request.Name);

            // Mapowanie Command → Entity
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
                IsActive = true  // Domyślnie aktywny
            };

            // Dodanie do DbContext
            context.Items.Add(item);

            // Zapisanie do bazy
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Utworzono produkt ID: {IdItem}", item.IdItem);

            // Zwracamy ID
            return item.IdItem;
        }
    }
}