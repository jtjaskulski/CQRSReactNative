using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.UnitOfMeasurements.Queries
{
    public class GetUnitOfMeasurementByIdQuery : IRequest<UnitOfMeasurementDto?>
    {
        public int Id { get; set; }

        public GetUnitOfMeasurementByIdQuery(int id)
        {
            Id = id;
        }
    }
}

