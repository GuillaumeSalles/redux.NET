using System;
using System.Reactive.Subjects;

namespace Redux
{
    public interface IStore<TState> : IObservable<TState>
    {
        IAction Dispatch(IAction action);

        TState GetState();
    }
        
    public class Store<TState> : IStore<TState>
    {
        private readonly object _syncRoot = new object();
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<TState> _reducer;
        private readonly ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private TState _lastState;

        public Store(Reducer<TState> reducer, TState initialState = default(TState))
        {
            _reducer = reducer;

            _lastState = initialState;
            _stateSubject.OnNext(_lastState);
        }

        public IAction Dispatch(IAction action)
        {
            lock (_syncRoot)
            {
                _lastState = _reducer(_lastState, action);
            }
            _stateSubject.OnNext(_lastState);
            return action;
        }

        public TState GetState()
        {
            return _lastState;
        }
        
        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject
                .Subscribe(observer);
        }
    }
}
