using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Commands;

namespace SolutionOrdersReact.Server.Handlers.UnitOfMeasurements
{
    public class DeleteUnitOfMeasurementHandler : IRequestHandler<DeleteUnitOfMeasurementCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteUnitOfMeasurementHandler> _logger;

        public DeleteUnitOfMeasurementHandler(
            ApplicationDbContext context,
            ILogger<DeleteUnitOfMeasurementHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            DeleteUnitOfMeasurementCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Usuwanie jednostki miary o ID: {IdUnitOfMeasurement}", request.IdUnitOfMeasurement);

            var unit = await _context.UnitOfMeasurements
                .FirstOrDefaultAsync(u => u.IdUnitOfMeasurement == request.IdUnitOfMeasurement, cancellationToken);

            if (unit == null)
            {
                _logger.LogError("Jednostka miary o ID {IdUnitOfMeasurement} nie została znaleziona", request.IdUnitOfMeasurement);
                throw new KeyNotFoundException($"Jednostka miary o ID {request.IdUnitOfMeasurement} nie istnieje");
            }

            // Soft delete - tylko ustawiamy IsActive = false
            unit.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Usunięto (soft delete) jednostkę miary o ID: {IdUnitOfMeasurement}", request.IdUnitOfMeasurement);

            return Unit.Value;
        }
    }
}

