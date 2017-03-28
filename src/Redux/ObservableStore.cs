using System;
using System.Reactive.Linq;

namespace Redux.Reactive
{
    public interface IObservableStore<TState> : IObservable<TState>
    {
        TState GetState();

        object Dispatch(object dispatch);
    }

    public class ObservableStore<TState> : IObservableStore<TState>
    {
        private IStore<TState> _innerStore;

        public ObservableStore(IStore<TState> innerStore)
        {
            _innerStore = innerStore;
        }

        public TState GetState()
        {
            return _innerStore.GetState();
        }

        public object Dispatch(object action)
        {
            return _innerStore.Dispatch(action);
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return Observable.FromEvent<TState>(
                h => _innerStore.StateChanged += h,
                h => _innerStore.StateChanged -= h)
                .Subscribe(observer);
        }
    }
}
