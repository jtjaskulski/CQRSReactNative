using MediatR;

namespace SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Commands
{
    public class UpdateUnitOfMeasurementCommand : IRequest<Unit>
    {
        public int IdUnitOfMeasurement { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}

