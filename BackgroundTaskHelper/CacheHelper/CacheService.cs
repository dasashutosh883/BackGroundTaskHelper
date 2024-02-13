using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;

namespace BackgroundTaskHelper.CacheHelper
{
    public class CacheService:ICacheService
    {
        private IDatabase _db;
        public CacheService()
        {
            ConfigureRedis();
        }
        private void ConfigureRedis()
        {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default!;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public bool AppendData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var redisValue = _db.StringGet(key);
            if (!string.IsNullOrEmpty(redisValue))
            {
                List<T> orginalList = JsonConvert.DeserializeObject<List<T>>(redisValue);
                List<T> appendList = new List<T>(orginalList);
                appendList.Add(value);
                var isSet = _db.StringSet(key, JsonConvert.SerializeObject(appendList),expiryTime);
                return isSet;
            }
            else
            {
                List<T> list = new List<T>();
                list.Add(value);
                var isSet = _db.StringSet(key, JsonConvert.SerializeObject(list), expiryTime);
                return isSet;
            }
           
        }
    }
}
