using Infrastructure.Services.FileFilter;

namespace Infrastructure.Tests.Services.FileFilter
{
    public sealed class JsFileFilterTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);


        private JsFileFilter CreateTarget() => new();


        [Fact]
        public void Constructor_OK() => CreateTarget();

        [Fact]
        public void Match_WhenFileNotJs_ReturnFalse()
        {
            var target = CreateTarget();

            string? fileName = "test.txt";
            var result = target.Match(fileName);

            Assert.False(result);
        }

        [Fact]
        public void Match_WhenFileIsJs_ReturnTrue()
        {
            var target = CreateTarget();

            string? fileName = "test.js";
            var result = target.Match(fileName);

            Assert.True(result);
        }

        [Fact]
        public void Match_WhenFileIsJsx_ReturnTrue()
        {
            var target = CreateTarget();

            string? fileName = "test.jsx";
            var result = target.Match(fileName);

            Assert.True(result);
        }
    }
}