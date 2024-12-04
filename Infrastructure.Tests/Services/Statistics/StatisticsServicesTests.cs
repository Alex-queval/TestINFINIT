using Infrastructure.Services.GitHub;
using Infrastructure.Services.Statistics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configurations;
using Models.Data;
using Moq;
using Octokit;

namespace Infrastructure.Tests.Services.Statistics
{
    public sealed class StatisticsServicesTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);

        private readonly Mock<IServiceProvider> _ServiceProviderMock = new();
        private readonly Mock<ILogger<OctokitServices>> _LoggerMock = new();
        private readonly Mock<IOptionsMonitor<GitHubConnectionConfigs>> _GitHubConfigsMock = new();

        private readonly RepositoryInfo _RepositoryInfo = new("test", "test")
        {
            TotalJsFile = 1,
            JsFiles = new List<JsFileInfo>()
            {
                new("test", "test")
                {
                    Content ="Lorem ipsum odor amet, consectetuer adipiscing elit."
                }
            }
        };



        private StatisticsServices CreateTarget() => new();

        [Fact]
        public void Constructor_OK() => CreateTarget();

        [Fact]
        public void CountWordsForEachFile_WhenRepositoryInfoIsNull_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = null;

            Assert.Throws<ArgumentNullException>(
                () => target.CountWordsForEachFile(repository!));
        }

        [Fact]
        public void CountWordsForEachFile_WhenJsFileIsEmpty_ThrowsArgumentException()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = new("test", "test");

            Assert.Throws<ArgumentException>(
                () => target.CountWordsForEachFile(repository!));
        }

        [Fact]
        public void CountWordsForEachFile_WhenJsFileIsContainsNullValue_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = new("test", "test");
            repository.JsFiles.Add(null!);

            Assert.Throws<ArgumentNullException>(
                () => target.CountWordsForEachFile(repository!));
        }

        [Fact]
        public void CountWordsForEachFile_OK()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = _RepositoryInfo;

            target.CountWordsForEachFile(repository!);

            Assert.Equal(44, repository.JsFiles.First().TotalChars);
            Assert.NotEmpty(repository.JsFiles.First().CharsCount);
        }

        [Fact]
        public void CountWordForRepository_WhenRepositoryInfoIsNull_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = null;

            Assert.Throws<ArgumentNullException>(
                () => target.CountWordForRepository(repository!));
        }

        [Fact]
        public void CountWordForRepository_WhenJsFileIsEmpty_ThrowsArgumentException()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = new("test", "test");

            Assert.Throws<ArgumentException>(
                () => target.CountWordForRepository(repository!));
        }

        [Fact]
        public void CountWordForRepository_WhenJsFileIsContainsNullValue_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = new("test", "test");
            repository.JsFiles.Add(null!);

            Assert.Throws<ArgumentNullException>(
                () => target.CountWordForRepository(repository!));
        }

        [Fact]
        public void CountWordForRepository_OK()
        {
            var target = CreateTarget();

            RepositoryInfo? repository = _RepositoryInfo;

            target.CountWordForRepository(repository!);

            Assert.Equal(44, repository.TotalChars);
            Assert.NotEmpty(repository.TotalCharsCount);
        }
    }
}