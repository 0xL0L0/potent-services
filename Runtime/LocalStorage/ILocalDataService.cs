namespace Potency.Services.Runtime.LocalStorage
{
    /// <summary>
    /// This service allows you to save and load various data objects into local storage (PlayerPrefs)
    /// </summary>
    public interface ILocalDataService
    {
        /// <summary>
        /// Gets data of type T
        /// </summary>
        T GetData<T>() where T : class;

        /// <summary>
        /// Saves all current data loaded in memory, into local storage
        /// </summary>
        void SaveAllData();
        
        /// <summary>
        /// Saves data of type T into local storage
        /// </summary>
        void SaveData<T>() where T : class;

        /// <summary>
        /// Tries to load data of type T
        /// If none present, new one will be instantiated in memory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void LoadData<T>() where T : class;
        
        /// <summary>
        /// Deletes all stored local data
        /// </summary>
        void DeleteAllData();
        
        /// <summary>
        /// Deletes data of type T from local storage
        /// </summary>
        void DeleteData<T>() where T : class;
    }
}
