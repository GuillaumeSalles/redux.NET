using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Redux
{
    public interface IStore<TState> : IObservable<TState>
    {
        void Dispatch(IAction action);
    }

    public class Store<TState> : IStore<TState>
    {
        private Subject<IAction> _dispatcher = new Subject<IAction>();
        private ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);

        public Store(TState initialState, Func<TState, IAction, TState> reducer)
        {
            _dispatcher
                .Scan(initialState, reducer)
                .StartWith(initialState)
                .Subscribe(_stateSubject);
        }

        public void Dispatch(IAction action)
        {
            _dispatcher.OnNext(action);
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject.Subscribe(observer);
        }
    }
}
