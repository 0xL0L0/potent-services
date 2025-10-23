using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Potency.Services.Runtime.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potency.Services.Runtime.LocalStorage
{
    /// <inheritdoc cref="ILocalDataService"/>
    public class LocalDataService : ILocalDataService
    {
        private readonly IDictionary<Type, object> _data = new Dictionary<Type, object>();

        private UnityCallbacksMonoComponent _unityCallbackComponent;
        
        public LocalDataService()
        {
            var serviceObject = Object.Instantiate(new GameObject("LocalDataServiceObject"));
            _unityCallbackComponent = serviceObject.AddComponent<UnityCallbacksMonoComponent>();
            _unityCallbackComponent.OnApplicationChangedFocus += OnApplicationChangedFocus;
            _unityCallbackComponent.OnApplicationHasQuit += OnApplicationHasQuit;
        }

        public T GetData<T>() where T : class
        {
            var type = typeof(T);
            
            if(!_data.ContainsKey(typeof(T)))
            {
                LoadData<T>();
            }

            return _data[type] as T;
        }

        public void SaveAllData()
        {
            foreach (var data in _data)
            {
                PlayerPrefs.SetString(data.Key.Name, JsonConvert.SerializeObject(data.Value));
            }

            PlayerPrefs.Save();
        }

        public void SaveData<T>() where T : class
        {
            var type = typeof(T);
            var jsonData = JsonConvert.SerializeObject(_data[type]);
            PlayerPrefs.SetString(type.Name, jsonData);
            PlayerPrefs.Save();
        }

        public void LoadData<T>() where T : class
        {
            var type = typeof(T);
            var json = PlayerPrefs.GetString(type.Name, "");
            var instance = string.IsNullOrEmpty(json) ? Activator.CreateInstance<T>() : JsonConvert.DeserializeObject<T>(json);
            _data[typeof(T)] = instance;
        }

        public void DeleteAllData()
        {
            _data.Clear();
            PlayerPrefs.DeleteAll();
        }

        public void DeleteData<T>() where T : class
        {
            var type = typeof(T);

            _data.Remove(type);
            PlayerPrefs.DeleteKey(type.Name);
        }

        private void OnApplicationChangedFocus(bool hasFocus)
        {
            if(!hasFocus)
            {
                SaveAllData();
            }
        }
        
        private void OnApplicationHasQuit()
        {
            SaveAllData();
        }
    }
}
