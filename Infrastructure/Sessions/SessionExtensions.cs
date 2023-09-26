using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.Sessions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static T GetSessionManager<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            //JWT Deserialize Here
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);

        }
    }
}
