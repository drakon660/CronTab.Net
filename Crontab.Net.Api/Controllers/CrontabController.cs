﻿using Crontab.Net.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crontab.Net.Api.Controllers;

[ApiController]
[Authorize]
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
    [Authorize(IdentityData.AdminUserPolicyName)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Insert([FromBody] CrontabItemInsertDto crontabItemDto)
    {
        _mediator.Send(new CrontabInsertRequest { CrontabItemDto = crontabItemDto });
        return NoContent();
    }
    
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Update([FromBody] CrontabItemUpdateDto crontabItemDto)
    {
        _mediator.Send(new CrontabUpdateRequest { CrontabItemDto = crontabItemDto });
        return NoContent();
    }

    [HttpDelete("delete")]
    [Authorize]
    [RequireClaim(IdentityData.AdminUserClaimName,"true")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(int index)
    {
        _mediator.Send(new CrontabDeleteRequest(index));
        return NoContent();
    }

    [HttpDelete("clear")]
    public IActionResult Clear()
    {
        _mediator.Send(new CrontabDeleteRequest(-1));
        return NoContent();
    }
}