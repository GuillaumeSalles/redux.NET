namespace Redux.Tests
{
    public class FakeAction<T>
    {
        public T Value { get; set; }

        public FakeAction(T value)
        {
            Value = value;
        }
    }
}
