using System;
using System.Collections.Generic;

namespace Redux.Tests
{
    public class MockObserver<T>
    {
        private readonly List<T> _values = new List<T>();
        public IEnumerable<T> Values => _values;

        public void StateChangedHandler(T value)
        {
            _values.Add(value);
        }
    }
}
