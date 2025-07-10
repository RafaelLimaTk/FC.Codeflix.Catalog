using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.ListVideos;

public interface IListVideos
    : IRequestHandler<ListVideosRequest, ListVideosResponse>
{ }
