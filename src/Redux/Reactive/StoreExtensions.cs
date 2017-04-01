using System;
using System.Reactive.Linq;

namespace Redux.Reactive
{
    public static class StoreExtensions
    {
        public static IObservable<T> ObserveState<T>(this IStore<T> store)
        {
            return Observable.Create<T>(observer =>
            {
                return store.Subscribe(() => observer.OnNext(store.GetState())).Dispose;
            });
        }
    }
}
