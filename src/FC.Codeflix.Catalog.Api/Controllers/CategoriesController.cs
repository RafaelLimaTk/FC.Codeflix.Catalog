using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FC.Codeflix.Catalog.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryModelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody][Required] CreateCategoryRequest createCategoryRequest,
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(createCategoryRequest, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute][Required] Guid id,
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(new GetCategoryRequest(id), cancellationToken);
        return Ok(response);
    }
}
