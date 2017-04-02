using System.Collections.Generic;

namespace Redux.Tests
{
    public class SpyListener<T>
    {
        private readonly List<T> _values = new List<T>();
        public IEnumerable<T> Values => _values;

        public void Listen(T value)
        {
            _values.Add(value);
        }
    }
}