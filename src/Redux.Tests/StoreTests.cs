using NUnit.Core;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Redux.Tests
{
    [TestFixture]
    public class StoreTests
    {
        [Test]
        public void Should_push_initial_state()
        {
            var sut = new Store<int>(1, Reducers.PassThrough);
            var mockObserver = new MockObserver<int>();

            sut.Subscribe(mockObserver);

            CollectionAssert.AreEqual(new[] { 1 }, mockObserver.Values);
        }

        [Test]
        public void Should_push_state_on_dispatch()
        {
            var sut = new Store<int>(1, Reducers.Replace);
            var mockObserver = new MockObserver<int>();

            sut.Subscribe(mockObserver);
            sut.Dispatch(new FakeAction<int>(2));

            CollectionAssert.AreEqual(new[] { 1, 2 }, mockObserver.Values);
        }

        [Test]
        public void Should_only_push_the_last_state_before_subscription()
        {
            var sut = new Store<int>(1, Reducers.Replace);
            var mockObserver = new MockObserver<int>();

            sut.Dispatch(new FakeAction<int>(2));
            sut.Subscribe(mockObserver);

            CollectionAssert.AreEqual(new[] { 2 }, mockObserver.Values);
        }

        public Middleware<int> DebugMiddleware = store => next => action => {
            Debug.WriteLine("Before dispatch");
            var newAction = next(action);
            Debug.WriteLine("After dispatch");
            return newAction;
        };

        [Test]
        public void Middleware_should_be_applied()
        {
            var sut = new Store<int>(1, Reducers.Replace, DebugMiddleware);
            sut.Dispatch(new FakeAction<int>(2));
        }
    }
}
