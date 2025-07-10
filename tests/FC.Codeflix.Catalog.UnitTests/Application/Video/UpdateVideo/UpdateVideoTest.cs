using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Extensions;
using FC.Codeflix.Catalog.Domain.Interfaces;
using FluentAssertions;
using Moq;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Video.UpdateVideo;

namespace FC.Codeflix.Catalog.UnitTests.Application.Video.UpdateVideo;

[Collection(nameof(UpdateVideoTestFixture))]
public class UpdateVideoTest
{
    private readonly UpdateVideoTestFixture _fixture;
    private readonly Mock<IVideoRepository> _videoRepository;
    private readonly Mock<IGenreRepository> _genreRepository;
    private readonly Mock<ICategoryRepository> _categoryRepository;
    private readonly Mock<ICastMemberRepository> _castMemberRepository;
    private readonly Mock<IUnitOfWork> _unitofWork;
    private readonly UseCase.UpdateVideo _useCase;

    public UpdateVideoTest(UpdateVideoTestFixture fixture)
    {
        _fixture = fixture;
        _videoRepository = new();
        _genreRepository = new();
        _categoryRepository = new();
        _castMemberRepository = new();
        _unitofWork = new();
        _useCase = new(_videoRepository.Object,
            _genreRepository.Object,
            _categoryRepository.Object,
            _castMemberRepository.Object,
            _unitofWork.Object);
    }

    [Fact(DisplayName = nameof(UpdateVideosBasicInfo))]
    [Trait("Application", "UpdateVideo - Use Cases")]
    public async Task UpdateVideosBasicInfo()
    {
        var exampleVideo = _fixture.GetValidVideo();
        var input = _fixture.CreateValidInput(exampleVideo.Id);
        _videoRepository.Setup(repository =>
            repository.Get(
                It.Is<Guid>(id => id == exampleVideo.Id),
                It.IsAny<CancellationToken>()))
        .ReturnsAsync(exampleVideo);

        VideoModelResponse output = await _useCase.Handle(input, CancellationToken.None);

        _videoRepository.VerifyAll();
        _videoRepository.Verify(repository => repository.Update(
            It.Is<DomainEntity.Video>(video =>
                ((video.Id == exampleVideo.Id) &&
                (video.Title == input.Title) &&
                (video.Description == input.Description) &&
                (video.Rating == input.Rating) &&
                (video.YearLaunched == input.YearLaunched) &&
                (video.Opened == input.Opened) &&
                (video.Published == input.Published) &&
                (video.Duration == input.Duration)))
            , It.IsAny<CancellationToken>())
        , Times.Once);
        _unitofWork.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
    }
}
