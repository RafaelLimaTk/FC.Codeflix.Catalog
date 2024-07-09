﻿using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
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
        [FromBody][Required] UpdateCategoryApiRequest updateCategoryApiInput,
        CancellationToken cancellationToken
    )
    {
        var request = new UpdateCategoryRequest(
            id,
            updateCategoryApiInput.Name,
            updateCategoryApiInput.Description,
            updateCategoryApiInput.IsActive
        );
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListCategoriesRequest();
        if (page.HasValue) input.Page = page.Value;
        if (perPage.HasValue) input.PerPage = perPage.Value;
        if (!string.IsNullOrWhiteSpace(search)) input.Search = search;
        if (!string.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (dir.HasValue) input.Dir = dir.Value;

        var response = await _mediator.Send(input, cancellationToken);
        return Ok(response);
    }
}
