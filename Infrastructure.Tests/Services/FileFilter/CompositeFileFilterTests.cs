using Infrastructure.Services.FileFilter;

namespace Infrastructure.Tests.Services.FileFilter
{
    public sealed class CompositeFileFilterTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);


        private CompositeFileFilter CreateTarget() => new([new JsFileFilter(), new TsFileFilter()]);


        [Fact]
        public void Constructor_OK() => CreateTarget();

        [Fact]
        public void Match_WhenFileNotMatchAnyStrategies_ReturnFalse()
        {
            var target = CreateTarget();

            string? fileName = "test.txt";
            var result = target.Match(fileName);

            Assert.False(result);
        }

        [Fact]
        public void Match_WhenFileMatchAnyStrategies_ReturnTrue()
        {
            var target = CreateTarget();

            string? fileName = "test.ts";
            var result = target.Match(fileName);

            Assert.True(result);
        }
    }
}