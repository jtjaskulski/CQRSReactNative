using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Requests.Items.Commands;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController(IMediator mediator) : ControllerBase
{
    // GET: api/items
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllItemsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    // POST: api/items
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
    {
        var itemId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = itemId }, itemId);
    }
}