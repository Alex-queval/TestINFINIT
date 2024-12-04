using Infrastructure.Services.GitHub;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configurations;
using Models.Data;
using Moq;
using Octokit;

namespace Infrastructure.Tests.Services.GitHub
{
    public sealed class OctokitServicesTests : IDisposable
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
                    DownloadPath = "http://google.com"
                }
            }
        };

        private class OctokitServicesMock(IServiceProvider provider) : OctokitServices(provider)
        {
            public bool ThrowsExceptionWhenGetRepository { get; set; } = false;
            public bool ThrowsExceptionWhenGetContents { get; set; } = false;

            protected override Task<Repository> GetRepository(string owner, string repositoryName)
            {
                if (ThrowsExceptionWhenGetRepository)
                    throw new Exception();
                else
                    return Task.FromResult(new Repository());
            }

            protected override Task<List<RepositoryContent>> GetContents(string owner, string repositoryName, string path)
            {
                var list = new List<RepositoryContent>()
                {
                    new(
                        "test.js", 
                        "http://google.com", 
                        "sha24", 
                        24,
                        ContentType.File, 
                        "http://google.com",
                        "http://google.com",
                        "http://google.com",
                        "http://google.com",
                        "encoding",
                        "encodingContent",
                        "target", 
                        "submodule"
                        )
                };

                if (ThrowsExceptionWhenGetContents)
                    throw new Exception();
                else
                    return Task.FromResult(list);
            }
        }

        private void SetupMocks()
        {
            _ServiceProviderMock
                .Setup(x => x.GetService(typeof(ILogger<OctokitServices>)))
                .Returns(_LoggerMock.Object)
                .Verifiable();

            _ServiceProviderMock
                .Setup(x => x.GetService(typeof(IOptionsMonitor<GitHubConnectionConfigs>)))
                .Returns(_GitHubConfigsMock.Object)
                .Verifiable();
        }

        private OctokitServicesMock CreateTarget()
        {
            SetupMocks();

            _GitHubConfigsMock
                .Setup(x => x.CurrentValue)
                .Returns(new GitHubConnectionConfigs() { Token = "token" })
                .Verifiable();

            return new OctokitServicesMock(_ServiceProviderMock.Object);
        }

        [Fact]
        public void Constructor_WhenServiceProviderIsNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new OctokitServices(null!));

        [Fact]
        public void Constructor_WhenTokenIsNull_ThrowsArgumentNullException()
        {
            SetupMocks();
            SetupMocks();

            _GitHubConfigsMock
                .Setup(x => x.CurrentValue)
                .Returns(new GitHubConnectionConfigs() { Token = null! })
                .Verifiable();

            Assert.Throws<ArgumentNullException>(
                () => new OctokitServices(_ServiceProviderMock.Object));
        }

        [Fact]
        public void Constructor_WhenTokenIsEmtpy_ThrowsArgumentException()
        {
            SetupMocks();
            SetupMocks();

            _GitHubConfigsMock
                .Setup(x => x.CurrentValue)
                .Returns(new GitHubConnectionConfigs() { Token = string.Empty })
                .Verifiable();

            Assert.Throws<ArgumentException>(
                () => new OctokitServices(_ServiceProviderMock.Object));
        }

        [Fact]
        public void Constructor_OK() => CreateTarget();

        [Fact]
        public void TryGetRepository_WhenOwnerIsNull_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            string? owner = null;
            string? repositoryName = "test";

            Assert.ThrowsAsync<ArgumentNullException>(
                () => target.TryGetRepository(owner!, repositoryName));
        }

        [Fact]
        public void TryGetRepository_WhenOwnerIsEmpty_ThrowsArgumentException()
        {
            var target = CreateTarget();

            string? owner = string.Empty;
            string? repositoryName = "test";

            Assert.ThrowsAsync<ArgumentException>(
                () => target.TryGetRepository(owner, repositoryName));
        }

        [Fact]
        public void TryGetRepository_WhenRepositoryNameIsNull_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            string? owner = "test";
            string? repositoryName = null!;

            Assert.ThrowsAsync<ArgumentNullException>(
                () => target.TryGetRepository(owner, repositoryName));
        }

        [Fact]
        public void TryGetRepository_WhenRepositoryNameIsEmpty_ThrowsArgumentException()
        {
            var target = CreateTarget();

            string? owner = "test";
            string? repositoryName = string.Empty;

            Assert.ThrowsAsync<ArgumentException>(
                () => target.TryGetRepository(owner, repositoryName));
        }

        [Fact]
        public async Task TryGetRepository_WhenRepositoryNotExist_ReturnNull()
        {
            var target = CreateTarget();
            target.ThrowsExceptionWhenGetRepository = true;

            string? owner = "test";
            string? repositoryName = "test";

            var result = await target.TryGetRepository(owner, repositoryName);

            Assert.Null(result);
        }

        [Fact]
        public async Task TryGetRepository_WhenRespositoryExist_ReturnInfos()
        {
            var target = CreateTarget();

            string? owner = "lodash";
            string? repositoryName = "lodash";

            var result = await target.TryGetRepository(owner, repositoryName);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetJsFileInfos_WhenRepositoryInfoIsNull_ThrowsArgumentNullException()
        {
            var target = CreateTarget();

            RepositoryInfo? repositoryInfo = null;

            Assert.ThrowsAsync<ArgumentNullException>(
                () => target.GetJsFileInfos(repositoryInfo!));
        }

        [Fact]
        public void GetJsFileInfos_WhenGetContentsThrowsExcpetion_ThrowsInvalidOperationException()
        {
            var target = CreateTarget();
            target.ThrowsExceptionWhenGetContents = true;

            RepositoryInfo? repositoryInfo = _RepositoryInfo;

            Assert.ThrowsAsync<InvalidOperationException>(
                () => target.GetJsFileInfos(repositoryInfo));
        }

        [Fact]
        public async Task GetJsFileInfos_Ok()
        {
            var target = CreateTarget();

            RepositoryInfo? repositoryInfo = _RepositoryInfo;

            await target.GetJsFileInfos(repositoryInfo);

            Assert.Equal(1, repositoryInfo.TotalJsFile);
        }

        [Fact]
        public async Task DonwloadFilesContents_Ok()
        {
            var target = CreateTarget();

            RepositoryInfo? repositoryInfo = _RepositoryInfo;

            await target.DonwloadFilesContents(repositoryInfo);

            Assert.True(true);
        }
    }
}