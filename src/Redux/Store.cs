using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
        private Subject<IAction> _dispatcher = new Subject<IAction>();
        private ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private Middleware<TState>[] _middlewares;

        public Store(TState initialState, Reducer<TState> reducer, params Middleware<TState>[] middlewares)
        {
            _middlewares = middlewares;

            _dispatcher
                .Scan(initialState, (previousState,action) => reducer(previousState,action))
                .StartWith(initialState)
                .Subscribe(_stateSubject);
        }

        public IAction Dispatch(IAction action)
        {
            Dispatcher dispatcher = InnerDispatch;
            foreach(var middleware in _middlewares)
            {
                dispatcher = middleware(this)(dispatcher);
            }
            return dispatcher(action);
        }

        private IAction InnerDispatch(IAction action)
        {
            _dispatcher.OnNext(action);
            return action;
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject.Subscribe(observer);
        }
    }
}
