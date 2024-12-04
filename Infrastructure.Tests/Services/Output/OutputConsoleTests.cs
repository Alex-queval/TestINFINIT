using Infrastructure.Services.Output;
using Models.Data;

namespace Infrastructure.Tests.Services.Output
{
    public sealed class OutputConsoleTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);

        private readonly RepositoryInfo _RepositoryInfo = new("test", "test")
        {
            TotalJsFile = 1,
            TotalChars = 50,
            TotalCharsCount = new SortedDictionary<char, int>()
            {
                { 'a', 30 },
                { 'e', 10 },
                { 'i', 5 },
                { 'o', 3 },
                { 'u', 1 },
                { 'y', 1 },
            },
            JsFiles = new List<JsFileInfo>()
            {
                new("test", "test")
                {
                    DownloadPath = "http://google.com",
                    TotalChars = 50,
                    CharsCount = new SortedDictionary<char, int>()
                    {
                        { 'a', 30 },
                        { 'e', 10 },
                        { 'i', 5 },
                        { 'o', 3 },
                        { 'u', 1 },
                        { 'y', 1 },
                    }
                },
            }
        };

        private OutputConsole CreateTarget() => new();


        [Fact]
        public void Constructor_OK() => CreateTarget();


        [Fact]
        public void Output_Ok()
        {
            var target = CreateTarget();

            RepositoryInfo? repositoryInfo = _RepositoryInfo;

            target.Output(repositoryInfo);

            Assert.True(true);
        }
    }
}