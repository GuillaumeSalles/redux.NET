using System;

namespace Redux
{
    public static class FunctionalHelpers
    {
        public static Func<T,T> Compose<T>(params Func<T,T>[] funcs)
        {
            return arg =>
            {
                for (int i = funcs.Length - 1; i >= 0; i--)
                {
                    arg = funcs[i](arg);
                };

                return arg;
            };
        }
    }
}
