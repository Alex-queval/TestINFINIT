using Infrastructure.Services.FileFilter;

namespace Infrastructure.Tests.Services.FileFilter
{
    public sealed class TsFileFilterTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);


        private TsFileFilter CreateTarget() => new();


        [Fact]
        public void Constructor_OK() => CreateTarget();

        [Fact]
        public void Match_WhenFileNotTs_ReturnFalse()
        {
            var target = CreateTarget();

            string? fileName = "test.txt";
            var result = target.Match(fileName);

            Assert.False(result);
        }

        [Fact]
        public void Match_WhenFileIsTs_ReturnTrue()
        {
            var target = CreateTarget();

            string? fileName = "test.ts";
            var result = target.Match(fileName);

            Assert.True(result);
        }

        [Fact]
        public void Match_WhenFileIsTsx_ReturnTrue()
        {
            var target = CreateTarget();

            string? fileName = "test.tsx";
            var result = target.Match(fileName);

            Assert.True(result);
        }
    }
}