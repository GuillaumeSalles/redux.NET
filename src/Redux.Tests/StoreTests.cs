﻿using NUnit.Core;
using NUnit.Framework;
using Redux.Reactive;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Redux.Tests
{
    public static class StoreExtensions
    {
        public static void SubscribeAndGetState<TState>(this IStore<TState> store, Action<TState> listener)
        {
            store.StateChanged += () => listener(store.GetState());
        }
    }
    
    public class StoreTests
    {
        [Test]
        public void Should_push_initial_state()
        {
            var sut = new Store<int>(Reducers.PassThrough, 1);
            var spyListener = new SpyListener<int>();

            sut.SubscribeAndGetState(spyListener.Listen);

            CollectionAssert.AreEqual(new[] { 1 }, spyListener.Values);
        }

        [Test]
        public void Should_push_state_on_dispatch()
        {
            var sut = new Store<int>(Reducers.Replace, 1);
            var spyListener = new SpyListener<int>();

            sut.SubscribeAndGetState(spyListener.Listen);
            sut.Dispatch(new FakeAction<int>(2));

            CollectionAssert.AreEqual(new[] { 1, 2 }, spyListener.Values);
        }

        [Test]
        public void Should_only_push_the_last_state_before_subscription()
        {
            var sut = new Store<int>(Reducers.Replace, 1);
            var spyListener = new SpyListener<int>();

            sut.Dispatch(new FakeAction<int>(2));
            sut.SubscribeAndGetState(spyListener.Listen);

            CollectionAssert.AreEqual(new[] { 2 }, spyListener.Values);
        }

        [Test]
        public void Middleware_should_be_called_for_each_action_dispatched()
        {
            var numberOfCalls = 0;
            Middleware<int> spyMiddleware = store => next => action =>
            {
                numberOfCalls++;
                return next(action);
            };

            var sut = new Store<int>(Reducers.Replace, 1, spyMiddleware);
            var spyListener = new SpyListener<int>();

            sut.SubscribeAndGetState(spyListener.Listen);
            sut.Dispatch(new FakeAction<int>(2));

            Assert.AreEqual(1, numberOfCalls);
            CollectionAssert.AreEqual(new[] { 1, 2 }, spyListener.Values);
        }

        [Test]
        [Ignore("This behavior will be handle with a store enhancer in the next release")]
        public void Should_push_state_to_end_of_queue_on_nested_dispatch()
        {
            var sut = new Store<int>(Reducers.Replace, 1);
            var spyListener = new SpyListener<int>();
            sut.SubscribeAndGetState(val =>
            {
                if (val < 5)
                {
                    sut.Dispatch(new FakeAction<int>(val + 1));
                }
                spyListener.Listen(val);
            });

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, spyListener.Values);
        }

        [Test]
        public void GetState_should_return_initial_state()
        {
            var sut = new Store<int>(Reducers.Replace, 1);

            Assert.AreEqual(1, sut.GetState());
        }

        [Test]
        public void GetState_should_return_the_latest_state()
        {
            var sut = new Store<int>(Reducers.Replace, 1);

            sut.Dispatch(new FakeAction<int>(2));

            Assert.AreEqual(2, sut.GetState());
        }

        [Test]
        public async Task Store_should_be_thread_safe()
        {
            var sut = new Store<int>((state, action) => state + 1, 0);

            await Task.WhenAll(Enumerable.Range(0, 1000)
                .Select(_ => Task.Factory.StartNew(() => sut.Dispatch(new FakeAction<int>(0)))));

            Assert.AreEqual(1000, sut.GetState());
        }

        [Test]
        public void Should_push_action_on_dispatch_to_reducer()
        {
            var sut = new Store<int>((state, action) => state + 1, 0);
            object pushedAction = null;
            sut.ActionDispatched += action => pushedAction = action;
            var dispatchedAction = new object();

            sut.Dispatch(dispatchedAction);

            Assert.AreSame(dispatchedAction, pushedAction);
        }

        [Test]
        public void Should_not_push_action_when_middleware_stops()
        {
            Middleware<int> stoppingMiddleware = store => next => action => null;
            var sut = new Store<int>((state, action) => state + 1, 0, stoppingMiddleware);
            var actionWasPushed = false;
            sut.ActionDispatched += action => actionWasPushed = true;

            sut.Dispatch(new object());

            Assert.False(actionWasPushed);
        }
    }
}