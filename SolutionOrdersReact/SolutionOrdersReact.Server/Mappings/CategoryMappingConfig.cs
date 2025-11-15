using Mapster;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Mappings
{
    public static class CategoryMappingConfig
    {
        public static void Configure()
        {
            // Category -> CategoryDto
            TypeAdapterConfig<Category, CategoryDto>
                .NewConfig()
                .Map(dest => dest.IdCategory, src => src.IdCategory)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsActive, src => src.IsActive);

            // CategoryDto -> Category
            TypeAdapterConfig<CategoryDto, Category>
                .NewConfig()
                .Map(dest => dest.IdCategory, src => src.IdCategory)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsActive, src => src.IsActive);
        }
    }
}

