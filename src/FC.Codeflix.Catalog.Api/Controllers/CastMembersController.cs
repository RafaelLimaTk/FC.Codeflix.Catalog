using FC.Codeflix.Catalog.Api.ApiModels.CastMember;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMembers;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.Api.Controllers;

[ApiController]
[Route("cast_members")]
public class CastMembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CastMembersController(IMediator mediator) => _mediator = mediator;


    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CastMemberModelResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCastMemberRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { Id = response.Id },
            new ApiResponse<CastMemberModelResponse>(response)
        );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CastMemberModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(new GetCastMemberRequest(id), cancellationToken);
        return Ok(new ApiResponse<CastMemberModelResponse>(output));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CastMemberModelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCastMemberApiRequest apiRequest,
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(
            new UpdateCastMemberRequest(id, apiRequest.Name, apiRequest.Type),
            cancellationToken
        );
        return Ok(new ApiResponse<CastMemberModelResponse>(response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCastMemberRequest(id), cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseList<CastMemberModelResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
          [FromQuery] int? page,
          [FromQuery(Name = "per_page")] int? perPage,
          [FromQuery] string? search,
          [FromQuery] string? dir,
          [FromQuery] string? sort,
          CancellationToken cancellationToken
      )
    {
        var input = new ListCastMembersRequest();
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (search is not null) input.Search = search;
        if (dir is not null) input.Dir = dir.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        if (sort is not null) input.Sort = sort;
        var output = await _mediator.Send(input, cancellationToken);
        return Ok(new ApiResponseList<CastMemberModelResponse>(output));
    }
}
