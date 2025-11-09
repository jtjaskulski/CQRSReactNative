namespace SolutionOrdersReact.Server.Dto
{
    public class OrderItemDto
    {
        public int IdOrderItem { get; set; }
        public string? ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}