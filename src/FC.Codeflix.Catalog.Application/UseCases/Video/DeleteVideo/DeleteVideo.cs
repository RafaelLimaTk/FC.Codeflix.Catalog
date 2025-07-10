using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Interfaces;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;

public class DeleteVideo : IDeleteVideo
{
    private readonly IVideoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVideo(
        IVideoRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteVideoRequest request,
        CancellationToken cancellationToken)
    {
        var video = await _repository.Get(request.VideoId, cancellationToken);
        var trailerFilePath = video.Trailer?.FilePath;
        var mediaFilePath = video.Media?.FilePath;

        await _repository.Delete(video, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}