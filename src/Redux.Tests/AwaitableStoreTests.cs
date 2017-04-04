namespace Redux.Tests
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    [Timeout(5000)]
    public class AwaitableStoreTests
    {
        private class StoreIncrementAction
        {
        }

        private class SagaIncrementAction
        {
        }

        private static int Reducer(int state, object action)
        {
            switch (action)
            {
                case StoreIncrementAction _: return state + 1;
                default: return state;
            }
        }

        private static void BlockingIncrementSaga(SagaIncrementAction action, IStore<int> store)
        {
            Task.Delay(100).Wait();
            store.Dispatch(new StoreIncrementAction());
        }

        private static async Task AsyncIncrementSaga(SagaIncrementAction action, IStore<int> store)
        {
            await Task.Delay(100);
            store.Dispatch(new StoreIncrementAction());
        }

        private static void BlockingThrowingSaga(SagaIncrementAction action, IStore<int> store)
        {
            throw new Exception();
        }

        private static Task AsyncThrowingSaga(SagaIncrementAction action, IStore<int> store)
        {
            throw new Exception();
        }

        #region Updating state using delayed saga

        [Test]
        public async Task AsyncSaga_When_AwaitingDispatchAsync_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            await store.DispatchAsync(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task AsyncSaga_When_NotAwaitingDispatchAsync_Should_GetNewStateAfterSagaCompletes()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.DispatchAsync(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(0));
            await Task.Delay(150);
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task AsyncSaga_When_UsingNormalDispatch_Should_GetNewStateAfterSagaCompletes()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.Dispatch(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(0));
            await Task.Delay(150);
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task AsyncSaga_When_StoreIsNonAwaitable_Should_GetNewStateAfterSagaCompletes()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.Dispatch(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(0));
            await Task.Delay(150);
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task AsyncSaga_When_AwaitingFirstOfSeveralDispatches_Should_AwaitAllDispatchesAndGetFinalState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            Task<object> firstDispatch = store.DispatchAsync(new SagaIncrementAction());
            await Task.Delay(30);
            store.Dispatch(new SagaIncrementAction());
            await Task.Delay(30);
            store.Dispatch(new SagaIncrementAction());

            await firstDispatch;

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(3));
        }

        #endregion

        #region Updating state using blocking saga

        [Test]
        public async Task BlockingSaga_When_AwaitingDispatchAsync_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingIncrementSaga);

            // Act
            await store.DispatchAsync(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void BlockingSaga_When_NotAwaitingDispatchAsync_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingIncrementSaga);

            // Act
            store.DispatchAsync(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void BlockingSaga_When_UsingNormalDispatch_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingIncrementSaga);

            // Act
            store.Dispatch(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void BlockingSaga_When_StoreIsNonAwaitable_Should_GetNewState()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingIncrementSaga);

            // Act
            store.Dispatch(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        #endregion

        #region Updating state directly (bypassing saga)

        [Test]
        public async Task DirectAction_When_AwaitingDispatchAsync_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            await store.DispatchAsync(new StoreIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void DirectAction_When_NotAwaitingDispatchAsync_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.DispatchAsync(new StoreIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void DirectAction_When_UsingNormalDispatch_Should_GetNewState()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.Dispatch(new StoreIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void DirectAction_When_StoreIsNonAwaitable_Should_GetNewState()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.Dispatch(new StoreIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        #endregion

        #region Exceptions - async saga

        [Test]
        public void AsyncThrowingSaga_When_AwaitingDispatchAsync_Should_BubbleUp()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncThrowingSaga);

            // Act/Assert
            Assert.That(async () => await store.DispatchAsync(new SagaIncrementAction()), Throws.Exception);
        }

        [Test]
        public void AsyncThrowingSaga_When_UsingNormalDispatch_Should_BubbleUp()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncThrowingSaga);

            // Act/Assert
            Assert.That(() => store.Dispatch(new SagaIncrementAction()), Throws.Exception);
        }

        [Test]
        public void AsyncThrowingSaga_When_StoreIsNonAwaitable_Should_BubbleUp()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncThrowingSaga);

            // Act/Assert
            Assert.That(() => store.Dispatch(new SagaIncrementAction()), Throws.Exception);
        }

        [Test]
        public async Task AsyncThrowingSaga_When_ExceptionThrownAndCaught_Expect_DispatchIsStillAwaitable()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);

            // Subscribe a saga that throws an exception, run it and remove it
            IDisposable sub = store.Actions.OfType<SagaIncrementAction>()
                .RunsAsyncSaga(store, AsyncThrowingSaga);
            Assert.That(async () => await store.DispatchAsync(new SagaIncrementAction()), Throws.Exception);
            sub.Dispose();

            // Act/Assert: The test will time out if the async counter was not decremented after the exception
            await store.DispatchAsync(new StoreIncrementAction());
        }

        #endregion

        #region Exceptions - blocking saga

        [Test]
        public void BlockingThrowingSaga_When_AwaitingDispatchAsync_Should_BubbleUp()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingThrowingSaga);

            // Act/Assert
            Assert.That(async () => await store.DispatchAsync(new SagaIncrementAction()), Throws.Exception);
        }

        [Test]
        public void BlockingThrowingSaga_When_UsingNormalDispatch_Should_BubbleUp()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingThrowingSaga);

            // Act/Assert
            Assert.That(() => store.Dispatch(new SagaIncrementAction()), Throws.Exception);
        }

        [Test]
        public void BlockingThrowingSaga_When_StoreIsNonAwaitable_Should_BubbleUp()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingThrowingSaga);

            // Act/Assert
            Assert.That(() => store.Dispatch(new SagaIncrementAction()), Throws.Exception);
        }

        [Test]
        public async Task BlockingThrowingSaga_When_ExceptionThrownAndCaught_Expect_DispatchIsStillAwaitable()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);

            // Subscribe a saga that throws an exception, run it and remove it
            IDisposable sub = store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingThrowingSaga);
            Assert.That(async () => await store.DispatchAsync(new SagaIncrementAction()), Throws.Exception);
            sub.Dispose();

            // Act/Assert: The test will time out if the async counter was not decremented after the exception
            await store.DispatchAsync(new StoreIncrementAction());
        }

        #endregion

        #region Unsubscription

        [Test]
        public async Task AsyncSaga_When_Unsubscribed_Should_NotBeInvoked()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            IDisposable sub = store.Actions.OfType<SagaIncrementAction>()
                .RunsAsyncSaga(store, AsyncIncrementSaga);

            // Sanity check
            await store.DispatchAsync(new SagaIncrementAction());
            Assert.That(store.GetState(), Is.EqualTo(1));

            // Act
            sub.Dispose();
            await store.DispatchAsync(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void BlockingSaga_When_Unsubscribed_Should_NotBeInvoked()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            IDisposable sub = store.Actions.OfType<SagaIncrementAction>().RunsSaga(store, BlockingIncrementSaga);

            // Sanity check
            store.Dispatch(new SagaIncrementAction());
            Assert.That(store.GetState(), Is.EqualTo(1));

            // Act
            sub.Dispose();
            store.Dispatch(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        #endregion

        #region Concurrency

        [Test]
        public async Task When_MultipleActionsDispatched_Should_BeProcessedConcurrentlyBySaga()
        {
            // Arrange
            var store = new AwaitableStore<int>(Reducer, 0);
            store.Actions.OfType<SagaIncrementAction>().RunsAsyncSaga(store, AsyncIncrementSaga);

            // Act
            store.Dispatch(new SagaIncrementAction());
            await Task.Delay(50);
            store.Dispatch(new SagaIncrementAction());

            // Assert
            Assert.That(store.GetState(), Is.EqualTo(0));
            await Task.Delay(150);
            Assert.That(store.GetState(), Is.EqualTo(2));
        }
    }

    #endregion
}