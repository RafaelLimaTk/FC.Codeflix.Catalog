using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute][Required] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new DeleteCategoryRequest(id), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        [FromRoute][Required] Guid id,
        [FromBody][Required] UpdateCategoryRequest updateCategoryRequest,
        CancellationToken cancellationToken
    )
    {
        updateCategoryRequest.Id = id;
        var response = await _mediator.Send(updateCategoryRequest, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] ListCategoriesRequest listCategoriesRequest,
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(listCategoriesRequest, cancellationToken);
        return Ok(response);
    }
}
