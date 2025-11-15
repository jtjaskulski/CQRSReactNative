using MediatR;

namespace SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Commands
{
    public class CreateUnitOfMeasurementCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}

