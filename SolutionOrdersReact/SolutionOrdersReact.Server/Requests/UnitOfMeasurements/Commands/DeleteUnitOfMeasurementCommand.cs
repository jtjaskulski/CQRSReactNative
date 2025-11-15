using MediatR;

namespace SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Commands
{
    public class DeleteUnitOfMeasurementCommand : IRequest<Unit>
    {
        public int IdUnitOfMeasurement { get; set; }

        public DeleteUnitOfMeasurementCommand(int idUnitOfMeasurement)
        {
            IdUnitOfMeasurement = idUnitOfMeasurement;
        }
    }
}

