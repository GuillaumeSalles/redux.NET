namespace Redux.Tests
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    [Timeout(5000)]
    public class AwaitableStoreTests
    {
        private class IncrementAction
        {
        }

        private class IncrementAsyncAction
        {
        }

        private static int Reducer(int state, object action)
        {
            switch (action)
            {
                case IncrementAction _: return state + 1;
                default: return state;
            }
        }

        private async Task DelayedIncrementSaga(IncrementAsyncAction action, IStore<int> store)
        {
            await Task.Delay(500);
            store.Dispatch(new IncrementAction());
        }

        private Task AsyncThrowingSaga(IncrementAsyncAction action, IStore<int> store)
        {
            throw new Exception();
        }

        private void ThrowingSaga(IncrementAsyncAction action, IStore<int> store)
        {
            throw new Exception();
        }

        private void ImmediateIncrementSaga(IncrementAsyncAction action, IStore<int> store)
        {
            store.Dispatch(new IncrementAction());
        }

        [Test]
        public async Task When_AwaitingDispatch_Should_GetUpdatedState()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void When_NotAwaitingDispatchAsync_Should_GetOldState()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            awaitableStore.DispatchAsync(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(0));
        }

        [Test]
        public async Task When_AwaitingMultipleDispatches_Should_GetFinalState()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());
            await Task.Delay(100);
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());
            await Task.Delay(100);
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(3));
        }

        [Test]
        public void When_NormalDispatch_Should_GetOldState()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            awaitableStore.Dispatch(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(0));
        }

        [Test]
        public void When_SagaNotInvoked_Should_GetNewStateWithNonAwaitedDispatchAsync()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            awaitableStore.Dispatch(new IncrementAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void When_SagaNotInvoked_Should_GetNewStateWithNormalDispatch()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            awaitableStore.Dispatch(new IncrementAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void When_UsingNonAsyncSaga_Should_GetNewStateWithNormalDispatch()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ImmediateIncrementSaga);

            // Act
            awaitableStore.Dispatch(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void When_UsingNonAsyncSaga_Should_GetNewStateWithNonAwaitedAsyncDispatch()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ImmediateIncrementSaga);

            // Act
            awaitableStore.DispatchAsync(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task When_UsingNonAsyncSaga_Should_GetNewStateWithAwaitedAsyncDispatch()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ImmediateIncrementSaga);

            // Act
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void When_SagaThrowsException_Should_BubbleUp()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ThrowingSaga);

            // Act/Assert
            Assert.That(() => awaitableStore.Dispatch(new IncrementAsyncAction()), Throws.Exception);
        }

        [Test]
        public void When_AsyncSagaThrowsException_Should_BubbleUp()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.AsyncThrowingSaga);

            // Act/Assert
            Assert.That(async () => await awaitableStore.DispatchAsync(new IncrementAsyncAction()), Throws.Exception);
        }

        [Test]
        public async Task When_AsyncSagaThrowsException_Expect_DispatchIsStillAwaitable()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);

            // Subscribe a saga that throws an exception, run it and remove it
            IDisposable sub = awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.AsyncThrowingSaga);
            Assert.That(async () => await awaitableStore.DispatchAsync(new IncrementAsyncAction()), Throws.Exception);
            sub.Dispose();

            // Act
            await awaitableStore.DispatchAsync(new IncrementAction());

            // Assert: The test will time out if the async counter was not decremented after the exception
        }

        [Test]
        public void When_SagaUnsubscribed_Should_NotRun()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            IDisposable sub = awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ImmediateIncrementSaga);

            // Sanity check
            awaitableStore.Dispatch(new IncrementAsyncAction());
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));

            // Act
            sub.Dispose();
            awaitableStore.Dispatch(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task When_AsyncSagaUnsubscribed_Should_NotRun()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            IDisposable sub = awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Sanity check
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));

            // Act
            sub.Dispose();
            await awaitableStore.DispatchAsync(new IncrementAsyncAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task When_StoreIsNotAwaitable_Should_WorkAsNormal()
        {
            // Arrange
            var store = new ObservableActionStore<int>(Reducer, 0);
            store.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(store, this.DelayedIncrementSaga);

            // Act
            store.Dispatch(new IncrementAsyncAction());
            
            // Assert
            Assert.That(store.GetState(), Is.EqualTo(0));
            await Task.Delay(600);
            Assert.That(store.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task Should_AllowSagaToRunConcurrently()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsAsyncSaga(awaitableStore, this.DelayedIncrementSaga);

            // Act
            awaitableStore.Dispatch(new IncrementAsyncAction());
            awaitableStore.Dispatch(new IncrementAsyncAction());


            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(0));
            await Task.Delay(600);
            Assert.That(awaitableStore.GetState(), Is.EqualTo(2));
        }
    }
}