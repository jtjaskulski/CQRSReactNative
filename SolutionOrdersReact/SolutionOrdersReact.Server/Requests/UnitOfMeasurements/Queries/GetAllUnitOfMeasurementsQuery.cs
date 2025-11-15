using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Queries
{
    public class GetAllUnitOfMeasurementsQuery : IRequest<List<UnitOfMeasurementDto>>
    {
    }
}

