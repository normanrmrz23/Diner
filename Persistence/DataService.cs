using System;
using MonkeyCache;
using Newtonsoft.Json;

namespace Diner.Persistence
{
    public interface IDataService
    {
        void InsertObject<T>(string key, T data, DateTimeOffset? absoluteExpiration = null);
        T? Get<T>(string key);
        T? GetValue<T>(string key) where T : struct;
        void RemoveObject(string key);
        void RemoveAll();
    }

    public class DataService : IDataService
    {
        private readonly IBarrel _barrel;
        private readonly JsonSerializerSettings _jsonSettings;

        public DataService(IBarrel barrel)
        {
            _barrel = barrel;
            // TODO make this injectable?
            _jsonSettings = AssignmentJson.Settings;
        }

        public void InsertObject<T>(string key, T data, DateTimeOffset? absoluteExpiration = null)
        {
            var now = SystemTimeService.Now();
            var expiration = absoluteExpiration ?? now.Add(TimeSpan.FromDays(30));
            var duration = expiration - now;
            var json = JsonConvert.SerializeObject(data, _jsonSettings);
            _barrel.Add(key, json, duration);
        }

        public void RemoveObject(string key)
        {
            _barrel.Empty(key);
        }

        public void RemoveAll()
        {
            _barrel.EmptyAll();
        }

        public T? Get<T>(string key)
        {
            return _barrel.Exists(key) ? GetViaJson<T>(key) : default(T);
        }

        public T? GetValue<T>(string key) where T : struct
        {
            if (!_barrel.Exists(key)) return null;

            return GetViaJson<T>(key);
        }

        private T? GetViaJson<T>(string key)
        {
            return _barrel.Get<T>(key /*,_jsonSettings*/);
        }
    }
}
