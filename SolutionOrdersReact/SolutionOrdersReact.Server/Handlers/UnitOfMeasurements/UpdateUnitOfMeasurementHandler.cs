using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Commands;

namespace SolutionOrdersReact.Server.Handlers.UnitOfMeasurements
{
    public class UpdateUnitOfMeasurementHandler : IRequestHandler<UpdateUnitOfMeasurementCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UpdateUnitOfMeasurementHandler> _logger;

        public UpdateUnitOfMeasurementHandler(
            ApplicationDbContext context,
            ILogger<UpdateUnitOfMeasurementHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            UpdateUnitOfMeasurementCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Aktualizacja jednostki miary o ID: {IdUnitOfMeasurement}", request.IdUnitOfMeasurement);

            var unit = await _context.UnitOfMeasurements
                .FirstOrDefaultAsync(u => u.IdUnitOfMeasurement == request.IdUnitOfMeasurement, cancellationToken);

            if (unit == null)
            {
                _logger.LogError("Jednostka miary o ID {IdUnitOfMeasurement} nie została znaleziona", request.IdUnitOfMeasurement);
                throw new KeyNotFoundException($"Jednostka miary o ID {request.IdUnitOfMeasurement} nie istnieje");
            }

            unit.Name = request.Name;
            unit.Description = request.Description;
            unit.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Zaktualizowano jednostkę miary o ID: {IdUnitOfMeasurement}", request.IdUnitOfMeasurement);

            return Unit.Value;
        }
    }
}

