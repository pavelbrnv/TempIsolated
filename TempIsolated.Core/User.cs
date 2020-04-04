using System;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Core
{
    public sealed class User
    {
        #region Fields

        private string name;

        #endregion

        #region Properties

        public Guid Id { get; }

        public string Name
        {
            get => name;
            set
            {
                if (value == null)
                {
                    value = Properties.Resources.User;
                }

                if (!string.Equals(name, value, StringComparison.Ordinal))
                {
                    var oldName = name;
                    name = value;
                    OnNameChanged(oldName, name);
                }
            }
        }

        #endregion

        #region Ctor

        public User(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Events

        public event EventHandler<StringValueChangedEventArgs> NameChanged = delegate { };

        #endregion

        #region Events raisers

        private void OnNameChanged(string oldValue, string newValue)
        {
            var args = new StringValueChangedEventArgs(oldValue, newValue);
            NameChanged(this, args);
        }

        #endregion
    }
}
