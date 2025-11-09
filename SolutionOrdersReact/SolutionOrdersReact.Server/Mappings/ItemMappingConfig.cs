using Mapster;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Commands;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Mappings
{
    public static class ItemMappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Item, ItemDto>
                .NewConfig()
                .Map(dest => dest.IdItem, src => src.IdItem)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.UnitName, src => src.UnitOfMeasurement != null ? src.UnitOfMeasurement.Name : null)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.IsActive, src => src.IsActive);

            TypeAdapterConfig<CreateItemCommand, Item>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IdCategory, src => src.IdCategory)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.FotoUrl, src => src.FotoUrl)
                .Map(dest => dest.IdUnitOfMeasurement, src => src.IdUnitOfMeasurement)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.IsActive, src => true)
                .Ignore(dest => dest.IdItem)
                .Ignore(dest => dest.Category)
                .Ignore(dest => dest.UnitOfMeasurement)
                .Ignore(dest => dest.OrderItems);
        }
    }
}