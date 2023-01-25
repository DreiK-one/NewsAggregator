using Moq;
using NewsAggregator.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace NewsAggregator.Domain.Tests.Services.Tests.Helpers
{
    public static class TestFunctions
    {
        public static Mock<IQueryable<T>> GetMockData<T>(IQueryable<T> testData) where T : BaseEntity
        {
            var mockSet = new Mock<IQueryable<T>>();
            mockSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(new CancellationToken())).Returns(new TestAsyncEnumerator<T>(testData.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new TestAsyncQueryProvider<T>(testData.Provider));
            mockSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(testData.Expression);
            mockSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(testData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(testData.GetEnumerator());
            return mockSet;
        }
    }
}
