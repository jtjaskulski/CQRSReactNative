using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Requests.Items.Commands;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Controllers;

public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/items
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllItemsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // POST: api/items
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
    {
        var itemId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = itemId }, itemId);
    }
}