using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Redux
{
    public interface IStore<TState> : IObservable<TState>
    {
        void Dispatch(ISignal signal);
    }

    public class Store<TState> : IStore<TState>
    {
        private Subject<ISignal> _dispatcher = new Subject<ISignal>();
        private ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);

        public Store(TState initialState, Func<TState, ISignal, TState> reducer)
        {
            _dispatcher
                .Scan(initialState, reducer)
                .Subscribe(_stateSubject);
        }

        public void Dispatch(ISignal signal)
        {
            _dispatcher.OnNext(signal);
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject.Subscribe(observer);
        }
    }
}
