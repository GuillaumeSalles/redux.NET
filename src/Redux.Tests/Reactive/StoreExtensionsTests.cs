using NUnit.Framework;
using Redux.Reactive;
using System;

namespace Redux.Tests.Reactive
{
    public class StoreExtensionsTests
    {
        [Test]
        public void ObserveState_should_push_store_states()
        {
            var sut = new Store<int>(Reducers.Replace, 1);
            var spyListener = new SpyListener<int>();

            sut.ObserveState().Subscribe(spyListener.Listen);
            sut.Dispatch(new FakeAction<int>(2));

            CollectionAssert.AreEqual(new[] { 1, 2 }, spyListener.Values);
        }

        [Test]
        public void ObserveActions_should_push_actions()
        {
            var sut = new Store<int>(Reducers.Replace, 0);
            object receivedAction = null;

            sut.ObserveActions().Subscribe(action => receivedAction = action);

            var dispatchedAction = new object();
            sut.Dispatch(dispatchedAction);

            Assert.AreSame(receivedAction, dispatchedAction);
        }
    }
}
