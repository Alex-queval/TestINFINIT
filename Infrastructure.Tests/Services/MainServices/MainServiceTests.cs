using Infrastructure.Services.GitHub;
using Infrastructure.Services.MainServices;
using Infrastructure.Services.Output;
using Infrastructure.Services.Statistics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configurations;
using Models.Data;
using Moq;
using Octokit;

namespace Infrastructure.Tests.Services.MainServices
{
    public sealed class MainServiceTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);

        private readonly Mock<IServiceProvider> _ServiceProviderMock = new();
        private readonly Mock<ILogger<MainService>> _LoggerMock = new();
        private readonly Mock<IOptionsMonitor<RepositoryToSearchConfig>> _OptionsMock = new();
        private readonly Mock<IGitHubServices> _GitHubServicesMock = new();
        private readonly Mock<IStatisticServices> _StatisticsServicesMock = new();
        private readonly Mock<IOutputServices> _OutputServicesMock = new();

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

        private void SetupMocks()
        {
            _ServiceProviderMock
                .Setup(x => x.GetService(typeof(ILogger<MainService>)))
                .Returns(_LoggerMock.Object)
                .Verifiable();

            _ServiceProviderMock
                .Setup(x => x.GetService(typeof(IOptionsMonitor<RepositoryToSearchConfig>)))
                .Returns(_OptionsMock.Object)
                .Verifiable();

            _ServiceProviderMock
               .Setup(x => x.GetService(typeof(IGitHubServices)))
               .Returns(_GitHubServicesMock.Object)
               .Verifiable();

            _ServiceProviderMock
               .Setup(x => x.GetService(typeof(IStatisticServices)))
               .Returns(_StatisticsServicesMock.Object)
               .Verifiable();

            _ServiceProviderMock
               .Setup(x => x.GetService(typeof(IOutputServices)))
               .Returns(_OutputServicesMock.Object)
               .Verifiable();

            _GitHubServicesMock
                .Setup(x => x.TryGetRepository(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_RepositoryInfo)
                .Verifiable();
        }

        private MainService CreateTarget()
        {
            SetupMocks();

            _OptionsMock
                .Setup(x => x.CurrentValue)
                .Returns(new RepositoryToSearchConfig() 
                {
                    Owner = "lodash",
                    RepositoryName = "lodash",
                })
                .Verifiable();

            return new MainService(_ServiceProviderMock.Object);
        }

        [Fact]
        public void Constructor_WhenServiceProviderIsNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new OctokitServices(null!));

        [Fact]
        public void Constructor_OK() => CreateTarget();

        [Fact]
        public void Execute_WhenSomethingWentWrong_ThrowsException()
        {
            var target = CreateTarget();
            _GitHubServicesMock
                .Setup(x => x.TryGetRepository(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception())
                .Verifiable();

            Assert.ThrowsAsync<ArgumentNullException>(
                () => target.Execute());
        }

        [Fact]
        public async Task Execute_OK()
        {
            var target = CreateTarget();

            await target.Execute();

            Assert.True(true);
            _OutputServicesMock.Verify(x => x.Output(It.IsAny< RepositoryInfo>()), Times.Once());
        }
    }
}