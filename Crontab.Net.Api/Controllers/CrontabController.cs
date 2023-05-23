using Crontab.Net.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crontab.Net.Api.Controllers;

[ApiController]
[Route("cron-tab")]
public class CrontabController : ControllerBase
{
    private readonly IMediator _mediator;

    public CrontabController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CrontabItemDto>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new CrontabListRequest());

        return Ok(result);
    }

    [HttpPost("insert")]
    public IActionResult Insert([FromBody] CrontabItemInsertDto crontabItemDto) =>
        Ok(_mediator.Send(new CrontabInsertRequest { CrontabItemDto = crontabItemDto }));

    [HttpPut("update")]
    public IActionResult Update([FromBody] CrontabItemUpdateDto crontabItemDto)
    {
        _mediator.Send(new CrontabUpdateRequest { CrontabItemDto = crontabItemDto });
        return Ok();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(int index)
    {
        _mediator.Send(new CrontabDeleteRequest(index));
        return Ok();
    }

    [HttpDelete("clear")]
    public IActionResult Clear()
    {
        throw new NotImplementedException();
    }
}