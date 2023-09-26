using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Services
{
    public class RestClient<T>
    {
        public T GetResponse(string apiaddr, object obj, RestSharp.Method method = Method.POST)
        {
            var client = new RestClient(apiaddr);
            var request = new RestRequest(method);

            //request.AddParameter("application/x-www-form-urlencoded; charset=utf-8", obj, ParameterType.GetOrPost);
            request.AddObject(obj);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);


            return JsonConvert.DeserializeObject<T>(response.Content); ;
            }
        public T GetObject()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { });
        }
    }
}
