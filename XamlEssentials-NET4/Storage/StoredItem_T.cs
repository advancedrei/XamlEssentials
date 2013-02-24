#if SILVERLIGHT
using System.IO.IsolatedStorage;

namespace XamlEssentials.Storage
{

    /// <summary>
    /// An item to be put in IsolatedStorage, usually for application settings.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item to be stored. Should be a primitive like int, bool, etc, but any Serializable item should work.
    /// </typeparam>
    /// <remarks>
    /// Taken from this MSDN sample: http://code.msdn.microsoft.com/wpapps/Alarm-Clock-with-voice-7b749124/view/SourceCode
    /// </remarks>
    public class StoredItem<T>
    {

        #region Private Members

        private T _value;
        private bool _needRefresh;

        #endregion

        #region Properties

        /// <summary>
        /// The actual value of the item in storage.
        /// </summary>
        public T Value
        {
            get
            {
                if (this._needRefresh)
                {
                    if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(this.Name, out this._value))
                    {
                        IsolatedStorageSettings.ApplicationSettings[this.Name] = this.DefaultValue;
                        this._value = this.DefaultValue;
                    }
                    this._needRefresh = false;
                }

                return this._value;
            }
            set
            {
                if (this._value.Equals(value))
                    return;

                // Store the value in isolated storage.
                IsolatedStorageSettings.ApplicationSettings[this.Name] = value;
                this._needRefresh = true;
            }
        }

        /// <summary>
        /// The value that should be substituted when the StoredItem is null or not set.
        /// </summary>
        public T DefaultValue { get; private set; }

        /// <summary>
        /// The name of the item in storage.
        /// </summary>
        public string Name { get; private set; }


        #endregion

        #region Methods

        /// <summary>
        /// The default constructor for the StoredItem, which specifies the name and the default value.
        /// </summary>
        /// <param name="name">The name of the item in storage.</param>
        /// <param name="defaultValue">The value that should be substituted when the StoredItem is null or not set.</param>
        public StoredItem(string name, T defaultValue)
        {
            this.Name = name;
            this.DefaultValue = defaultValue;

            // If isolated storage doesn't have the value stored yet
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(this.Name, out this._value))
            {
                this._value = defaultValue;
                IsolatedStorageSettings.ApplicationSettings[this.Name] = defaultValue;
            }

            this._needRefresh = false;
        }

        /// <summary>
        /// A string representation of the StoredItem, typically used for debugging.
        /// </summary>
        /// <returns>A string representation of the StoredItem.</returns>
        public override string ToString()
        {
            return this.Name
                + " with value: " + this._value.ToString()
                + ", default value: " + this.DefaultValue.ToString();
        }

        /// <summary>
        /// Forces the item to clear the cached value and retrieve the value from IsolatedStorage the next time it is accessed.
        /// </summary>
        public void ForceRefresh()
        {
            this._needRefresh = true;
        }

        #endregion

    }
}


#endif