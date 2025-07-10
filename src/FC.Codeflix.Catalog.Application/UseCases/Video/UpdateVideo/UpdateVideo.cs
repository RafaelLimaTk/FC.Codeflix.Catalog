using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Interfaces;
using FC.Codeflix.Catalog.Domain.Validations;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.UpdateVideo;

public class UpdateVideo : IUpdateVideo
{
    private readonly IVideoRepository _videoRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICastMemberRepository _castMemberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVideo(
        IVideoRepository videoRepository,
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository,
        ICastMemberRepository castMemberRepository,
        IUnitOfWork unitOfWork)
    {
        _videoRepository = videoRepository;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
        _castMemberRepository = castMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VideoModelResponse> Handle(
        UpdateVideoRequest request,
        CancellationToken cancellationToken)
    {
        var video = await _videoRepository.Get(request.VideoId, cancellationToken);
        video.Update(
            request.Title,
            request.Description,
            request.YearLaunched,
            request.Opened,
            request.Published,
            request.Duration,
            request.Rating);

        var validationHandler = new NotificationValidationHandler();
        video.Validate(validationHandler);
        if (validationHandler.HasErrors())
            throw new EntityValidationException("There are validation errors",
                validationHandler.Errors);

        await ValidateAndAddRelations(request, video, cancellationToken);

        await _videoRepository.Update(video, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return VideoModelResponse.FromVideo(video);
    }

    private async Task ValidateAndAddRelations(
        UpdateVideoRequest input,
        DomainEntity.Video video,
        CancellationToken cancellationToken)
    {
        if (input.GenresIds is not null)
        {
            video.RemoveAllGenres();
            if (input.GenresIds.Count > 0)
            {
                await ValidateGenresIds(input, cancellationToken);
                input.GenresIds!.ToList().ForEach(video.AddGenre);
            }
        }

        if (input.CategoriesIds is not null)
        {
            video.RemoveAllCategories();
            if (input.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(input, cancellationToken);
                input.CategoriesIds!.ToList().ForEach(video.AddCategory);
            }
        }

        if (input.CastMembersIds is not null)
        {
            video.RemoveAllCastMembers();
            if (input.CastMembersIds.Count > 0)
            {
                await ValidateCastMembersIds(input, cancellationToken);
                input.CastMembersIds!.ToList().ForEach(video.AddCastMember);
            }
        }
    }

    private async Task ValidateGenresIds(UpdateVideoRequest input, CancellationToken cancellationToken)
    {
        var persistenceIds = await _genreRepository.GetIdsListByIds(
            input.GenresIds!.ToList(), cancellationToken);
        if (persistenceIds.Count < input.GenresIds!.Count)
        {
            var notFoundIds = input.GenresIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            throw new RelatedAggregateException(
                $"Related genre id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCategoriesIds(UpdateVideoRequest input, CancellationToken cancellationToken)
    {
        var persistenceIds = await _categoryRepository.GetIdsListByIds(
            input.CategoriesIds!.ToList(), cancellationToken);
        if (persistenceIds.Count < input.CategoriesIds!.Count)
        {
            var notFoundIds = input.CategoriesIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCastMembersIds(UpdateVideoRequest input, CancellationToken cancellationToken)
    {
        var persistenceIds = await _castMemberRepository.GetIdsListByIds(
            input.CastMembersIds!.ToList(), cancellationToken);
        if (persistenceIds.Count < input.CastMembersIds!.Count)
        {
            var notFoundIds = input.CastMembersIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            throw new RelatedAggregateException(
                $"Related cast member(s) id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }
}