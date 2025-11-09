using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Requests.Orders.Queries;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IMediator mediator) : ControllerBase
    {







        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllOrdersQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}