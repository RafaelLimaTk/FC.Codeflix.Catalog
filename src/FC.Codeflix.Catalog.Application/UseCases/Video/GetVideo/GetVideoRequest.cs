using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.GetVideo;
public record GetVideoRequest(Guid VideoId) : IRequest<VideoModelResponse>;