using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;
public record DeleteVideoRequest(Guid VideoId) : IRequest<Unit>;
