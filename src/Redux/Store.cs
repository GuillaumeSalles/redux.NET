using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;

namespace Redux
{
    public delegate IAction Dispatcher(IAction action);

    public delegate TState Reducer<TState>(TState previousState, IAction action);

    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);

    public interface IStore<TState> : IObservable<TState>
    {
        IAction Dispatch(IAction action);
    }
        
    public class Store<TState> : IStore<TState>
    {
        private readonly Dispatcher _dispatcher;
        private readonly Subject<IAction> _subjectDispatcher = new Subject<IAction>();
        private readonly ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private readonly Reducer<TState> _reducer;

        public Store(TState initialState, Reducer<TState> reducer, params Middleware<TState>[] middlewares)
        {
            _dispatcher = ApplyMiddlewares(middlewares);

            _reducer = reducer;

            _subjectDispatcher
                .Scan (initialState, SubjectDispatcherAccumulator)
                .StartWith (initialState)
                .Subscribe (_stateSubject);
        }

        TState SubjectDispatcherAccumulator(TState previousState, IAction action)
        {
            return _reducer (previousState, action);
        }

        public IAction Dispatch(IAction action)
        {
            return _dispatcher(action);
        }

        public TState GetState()
        {
            return this.FirstAsync().ToTask().Result;
        }
        
        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject
                .ObserveOn(Scheduler.CurrentThread)
                .Subscribe(observer);
        }

        private Dispatcher ApplyMiddlewares(params Middleware<TState>[] middlewares)
        {
            Dispatcher dispatcher = InnerDispatch;
            foreach (var middleware in middlewares)
            {
                dispatcher = middleware(this)(dispatcher);
            }
            return dispatcher;
        }

        private IAction InnerDispatch(IAction action)
        {
            _subjectDispatcher.OnNext(action);
            return action;
        }
    }
}
