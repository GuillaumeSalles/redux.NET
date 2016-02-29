using System;
using System.Reactive.Linq;
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
        private readonly Subject<IAction> _subjectDispatcher = new Subject<IAction>();
        private readonly ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private TState _lastState;

        public Store(Reducer<TState> reducer, TState initialState = default(TState))
        {
            _subjectDispatcher
                .Scan(initialState, (state, action) => reducer(state, action))
                .StartWith(initialState)
                .Do(state => _lastState = state)
                .Subscribe(_stateSubject);
        }

        public IAction Dispatch(IAction action)
        {
            _subjectDispatcher.OnNext(action);
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
