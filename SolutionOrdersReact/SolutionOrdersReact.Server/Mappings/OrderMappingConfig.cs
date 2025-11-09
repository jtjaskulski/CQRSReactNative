using Mapster;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Mappings
{
    public class OrderMappingConfig
    {
        public static void Configure()
        {
            // Mapowanie z Order do OrderDto
            TypeAdapterConfig<Order, OrderDto>
                .NewConfig()
                .Map(dest => dest.IdOrder, src => src.IdOrder)
                .Map(dest => dest.DataOrder, src => src.DataOrder)
                .Map(dest => dest.ClientName, src => src.Client != null ? src.Client.Name : null)
                .Map(dest => dest.WorkerName,
                    src => src.Worker != null ? $"{src.Worker.FirstName} {src.Worker.LastName}" : null)
                .Map(dest => dest.Notes, src => src.Notes)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.Items, src => src.OrderItems.Select(oi => new OrderItemDto
                {
                    IdOrderItem = oi.IdOrderItem,
                    ItemName = oi.Item.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Item.Price
                }).ToList());

            //    // Mapowanie z CreateOrderCommand do Order (zakładając strukturę komendy)
            //    TypeAdapterConfig<CreateOrderCommand, Order>
            //        .NewConfig()
            //        .Map(dest => dest.DataOrder, src => src.DataOrder)
            //        .Map(dest => dest.IdClient, src => src.IdClient)
            //        .Map(dest => dest.IdWorker, src => src.IdWorker)
            //        .Map(dest => dest.Notes, src => src.Notes)
            //        .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
            //        .Ignore(dest => dest.IdOrder) // IdOrder jest generowane przez bazę danych
            //        .Ignore(dest => dest.Client)
            //        .Ignore(dest => dest.Worker)
            //        .Ignore(dest => dest.OrderItems);
            //}
        }
    }
}