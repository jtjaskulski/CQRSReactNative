namespace SolutionOrdersReact.Server.Dto
{
    public class OrderDto
    {
        public int IdOrder { get; set; }
        public DateTime? DataOrder { get; set; }
        public string? ClientName { get; set; }
        public string? WorkerName { get; set; }
        public string? Notes { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}