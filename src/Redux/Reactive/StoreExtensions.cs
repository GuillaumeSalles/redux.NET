using System;
using System.Reactive.Linq;

namespace Redux.Reactive
{
    public static class StoreExtensions
    {
        public static IObservable<T> ObserveState<T>(this IStore<T> store)
        {
            return Observable.FromEvent<T>(
                h => store.StateChanged += h,
                h => store.StateChanged -= h);
        }
    }
}
