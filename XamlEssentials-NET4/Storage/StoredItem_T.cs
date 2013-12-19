using System.Reflection;
#if SILVERLIGHT
using System.IO.IsolatedStorage;
#elif WINRT
using Windows.Storage;
#endif

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
        private T _defaultValue;
        private bool _needsRefresh;

        #endregion

        #region Properties

        /// <summary>
        /// The value that should be substituted when the StoredItem is null or not set.
        /// </summary>
        public T DefaultValue 
        {
            get { return _defaultValue; }
        }

        /// <summary>
        /// The name of the item in storage.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The actual value of the item in storage.
        /// </summary>
        public T Value
        {
            get
            {
                if (_needsRefresh)
                {
#if SILVERLIGHT
                    if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(Name, out _value))
                    {
                        IsolatedStorageSettings.ApplicationSettings[Name] = _defaultValue;
#elif WINRT
                    if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(Name))
                    {
                        ApplicationData.Current.LocalSettings.Values[Name] = DefaultValue;
#endif
                        _value = _defaultValue;
                    }
                    _needsRefresh = false;
                }

                return _value;
            }
            set
            {
                if (_value.Equals(value))
                    return;

                _value = value;

                ForceSave();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The default constructor for the StoredItem, which specifies the name and the default value.
        /// </summary>
        /// <param name="name">The name of the item in storage.</param>
        /// <param name="defaultValue">The value that should be substituted when the StoredItem is null or not set.</param>
        public StoredItem(string name, T defaultValue)
        {
            Name = name;
            _defaultValue = defaultValue;

            // If isolated storage doesn't have the value stored yet
#if SILVERLIGHT
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(Name, out _value))
            {
                _value = defaultValue;
                IsolatedStorageSettings.ApplicationSettings[Name] = _defaultValue;
#elif WINRT
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(Name))
            {
                _value = defaultValue;
                ApplicationData.Current.LocalSettings.Values[Name] = _defaultValue;
#endif
            }

            _needsRefresh = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Forces the item to clear the cached value and retrieve the value from IsolatedStorage the next time it is accessed.
        /// </summary>
        public void ForceRefresh()
        {
            _needsRefresh = true;
        }

        /// <summary>
        /// Allows you to Force-Commit the Value to IsolatedStorage. Useful if your Value is a list that is being manipulated.
        /// </summary>
        public void ForceSave()
        {
            //RWM: Make sure when we get the Value that it doesn't pull from IsolatedStorage.
            _needsRefresh = false;
#if SILVERLIGHT
            IsolatedStorageSettings.ApplicationSettings[Name] = _value;
#elif WINRT
            ApplicationData.Current.LocalSettings.Values[Name] = _value;
#endif
            _needsRefresh = true;
        }

        /// <summary>
        /// A string representation of the StoredItem, typically used for debugging.
        /// </summary>
        /// <returns>A string representation of the StoredItem.</returns>
        public override string ToString()
        {
            return string.Format("Name: {0}, Value: {1}, Default: {2}", Name, _value, DefaultValue);
        }

        #endregion

    }
}