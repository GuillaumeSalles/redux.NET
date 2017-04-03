namespace Redux
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;

    public delegate void Saga<TState, in TAction>(TAction action, IStore<TState> store);

    public delegate Task AsyncSaga<TState, in TAction>(TAction action, IStore<TState> store);

    public static class ObservableExtensions
    {
        public static void RunsSaga<TState, TAction>(
            this IObservable<TAction> source,
            IStore<TState> store,
            Saga<TState, TAction> saga)
        {
            source.Subscribe(action => saga(action, store));
        }

        public static void RunsAsyncSaga<TState, TAction>(
            this IObservable<TAction> source,
            AwaitableStore<TState> store,
            AsyncSaga<TState, TAction> saga)
        {
            source.Subscribe(
                async action =>
                {
                    // TODO: Find a way to call AddOperation and RemoveOperation below
                    // without specifying concrete class AwaitableStore above, but also
                    // without giving devs access to AddOperation and RemoveOperation
                    store.AddOperation();
                    await saga(action, (IStore<TState>)store);
                    store.RemoveOperation();
                });
        }
    }

    public interface IObservableActionStore
    {
        IObservable<object> Actions { get; }
    }

    public class ObservableActionStore<TState> : Store<TState>, IObservableActionStore
    {
        private readonly ISubject<object> actionsSubject = new Subject<object>();

        /// <inheritdoc />
        public ObservableActionStore(
            Reducer<TState> reducer,
            TState initialState = default(TState),
            params Middleware<TState>[] middlewares) : base(reducer, initialState, middlewares)
        {
        }

        public IObservable<object> Actions => this.actionsSubject;

        protected override object InnerDispatch(object action)
        {
            object ret = base.InnerDispatch(action);
            this.actionsSubject.OnNext(action);
            return ret;
        }
    }

    public interface IAwaitableStore<TState>
    {

        Task<object> DispatchAsync(object action);
    }

    public class AwaitableStore<TState> : ObservableActionStore<TState>, IAwaitableStore<TState>
    {
        private int numOperations;
        private readonly ISubject<int> numOperationsSubject = new BehaviorSubject<int>(0); // TODO: start with 0

        /// <inheritdoc />
        public AwaitableStore(
            Reducer<TState> reducer,
            TState initialState = default(TState),
            params Middleware<TState>[] middlewares) : base(reducer, initialState, middlewares)
        {
        }

        private IObservable<int> OngoingOperations => this.numOperationsSubject;

        internal void AddOperation()
        {
            this.numOperationsSubject.OnNext(++this.numOperations);
        }

        internal void RemoveOperation()
        {
            this.numOperationsSubject.OnNext(--this.numOperations);
        }

        public async Task<object> DispatchAsync(object action)
        {
            object ret = this.Dispatch(action);
            await this.OngoingOperations.FirstAsync(i => i == 0);
            return Task.FromResult(ret);
        }
    }
}