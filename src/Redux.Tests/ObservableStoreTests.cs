using NUnit.Framework;
using Redux.Reactive;
using System;

namespace Redux.Tests
{
    public class ObservableStoreTests
    {
        [Test]
        public void Should_push_state_on_dispatch()
        {
            var sut = new ObservableStore<int>(new Store<int>(Reducers.Replace, 1));
            var mockObserver = new MockObserver<int>();

            sut.Subscribe(mockObserver.StateChangedHandler);
            sut.Dispatch(new FakeAction<int>(2));

            CollectionAssert.AreEqual(new[] { 1, 2 }, mockObserver.Values);
        }
    }
}
