using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Queries;

namespace SolutionOrdersReact.Server.Handlers.UnitOfMeasurements
{
    public class GetAllUnitOfMeasurementsHandler : IRequestHandler<GetAllUnitOfMeasurementsQuery, List<UnitOfMeasurementDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetAllUnitOfMeasurementsHandler> _logger;

        public GetAllUnitOfMeasurementsHandler(
            ApplicationDbContext context,
            ILogger<GetAllUnitOfMeasurementsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<UnitOfMeasurementDto>> Handle(
            GetAllUnitOfMeasurementsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pobieranie wszystkich jednostek miary");

            var units = await _context.UnitOfMeasurements
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .Select(u => new UnitOfMeasurementDto
                {
                    IdUnitOfMeasurement = u.IdUnitOfMeasurement,
                    Name = u.Name,
                    Description = u.Description,
                    IsActive = u.IsActive
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Pobrano {Count} jednostek miary", units.Count);

            return units;
        }
    }
}

