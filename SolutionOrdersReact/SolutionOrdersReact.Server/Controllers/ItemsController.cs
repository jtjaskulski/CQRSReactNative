using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Requests.Items.Commands;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Controllers
{
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

        /// <summary>
        /// Tworzy nowy produkt
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
        {
            var itemId = await mediator.Send(command);

            // HTTP 201 Created z Location header
            return CreatedAtAction(
                nameof(GetById),
                new { id = itemId },
                new { id = itemId, message = "Produkt został utworzony" }
            );
        }

        /// <summary>
        /// Pobiera produkt po ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetItemByIdQuery(id);
            var result = await mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = $"Produkt o ID {id} nie został znaleziony" });
            }

            return Ok(result);
        }
        /// <summary>
         /// Aktualizuje produkt
         /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateItemCommand command)
        {
            if (id != command.IdItem)
            {
                return BadRequest(new { message = "ID w URL różni się od ID w body" });
            }

            try
            {
                await mediator.Send(command);
                return NoContent();  // HTTP 204 - sukces bez body
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Usuwa produkt (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteItemCommand(id);

            try
            {
                await mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}