using Infrastructure.Guards;

namespace Infrastructure.Tests.Guards
{
    public sealed class GuardTests : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);

        [Fact]
        public void ArgumentNotNull_WhenParamIsNull_ThrowsArgumentNullException()
        {
            string? param = null;
            Assert.Throws<ArgumentNullException>(() => Guard.ArgumentNotNull(param!, nameof(param)));
        }

        [Fact]
        public void ArgumentNotNull_Ok()
        {
            string? param = "test";
            Guard.ArgumentNotNull(param, nameof(param));
            Assert.True(true);
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_WhenParamIsNull_ThrowsArgumentNullException()
        {
            string? param = null;
            Assert.Throws<ArgumentNullException>(() => Guard.ArgumentNotNullOrEmpty(param!, nameof(param)));
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_WhenParamIsEmpty_ThrowsArgumentException()
        {
            string? param = string.Empty;
            Assert.Throws<ArgumentException>(() => Guard.ArgumentNotNullOrEmpty(param, nameof(param)));
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_OK()
        {
            string? param = "test";
            Guard.ArgumentNotNullOrEmpty(param, nameof(param));
            Assert.True(true);
        }

        [Fact]
        public void ArgumentItemsNotNull_WhenCollectionIsNull_ThrowsArgumentNullException()
        {
            List<string?>? collections = null;
            Assert.Throws<ArgumentNullException>(() => Guard.ArgumentItemsNotNull(collections!, nameof(collections)));
        }

        [Fact]
        public void ArgumentItemsNotNull_WhenCollectionIsEmtpy_ThrowsArgumentException()
        {
            List<string?>? collections = [];
            Assert.Throws<ArgumentException>(() => Guard.ArgumentItemsNotNull(collections!, nameof(collections)));
        }

        [Fact]
        public void ArgumentItemsNotNull_WhenItemIsNull_ThrowsArgumenNulltException()
        {
            List<string?>? collections = [null!];
            Assert.Throws<ArgumentNullException>(() => Guard.ArgumentItemsNotNull(collections!, nameof(collections)));
        }

        [Fact]
        public void ArgumentItemsNotNull_OK()
        {
            List<string?>? collections = ["test"];
            Guard.ArgumentItemsNotNull(collections!, nameof(collections));
            Assert.True(true);
        }
    }
}