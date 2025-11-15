using Mapster;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Mappings
{
    public static class UnitOfMeasurementMappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<UnitOfMeasurement, UnitOfMeasurementDto>
                .NewConfig()
                .Map(dest => dest.IdUnitOfMeasurement, src => src.IdUnitOfMeasurement)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsActive, src => src.IsActive);
        }
    }
}

