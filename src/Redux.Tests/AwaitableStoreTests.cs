namespace Redux.Tests
{
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using NUnit.Framework;

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
            await Task.Delay(1000);
            store.Dispatch(new IncrementAction());
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
            awaitableStore.Dispatch(new IncrementAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public void When_UsingNonAsyncSaga_Should_GetNewStateNonAwaitedAsyncDispatch()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ImmediateIncrementSaga);

            // Act
            awaitableStore.DispatchAsync(new IncrementAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }

        [Test]
        public async Task When_UsingNonAsyncSaga_Should_GetNewStateAwaitedAsyncDispatch()
        {
            // Arrange
            var awaitableStore = new AwaitableStore<int>(Reducer, 0);
            awaitableStore.Actions.OfType<IncrementAsyncAction>().RunsSaga(awaitableStore, this.ImmediateIncrementSaga);

            // Act
            await awaitableStore.DispatchAsync(new IncrementAction());

            // Assert
            Assert.That(awaitableStore.GetState(), Is.EqualTo(1));
        }
    }
}