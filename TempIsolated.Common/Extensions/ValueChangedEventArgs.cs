using System;

namespace TempIsolated.Common.Extensions
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; }

        public T NewValue { get; }

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class BooleanValueChangedEventArgs : ValueChangedEventArgs<bool>
    {
        public BooleanValueChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }

    public class StringValueChangedEventArgs : ValueChangedEventArgs<string>
    {
        public StringValueChangedEventArgs(string oldValue, string newValue)
            : base(oldValue, newValue)
        {
        }
    }
}
