using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Police
{
    public class RequestHelper
    {
        public const string API_BASE_URL = @"http://data.police.uk/api/";

        /// <summary>
        /// Get a list of all Forces in the country
        /// </summary>
        /// <returns>A list of all Forces in the country</returns>
        public static IList<Force> GetForces()
        {
            List<Force> forces = new List<Force>();

            IList<IDictionary<string, object>> jsonResponse = RetrieveJSONArray(API_BASE_URL + "forces");

            foreach(IDictionary<string, object> item in jsonResponse)
            {
                string id = (string)item["id"];
                string name = (string)item["name"];

                forces.Add(new Force(id, name));
            }

            return forces;
        }

        public static Force GetSpecificForce(string id)
        {
            Force f;

            IDictionary<string, object> jsonResponse = RetrieveJSONObject(API_BASE_URL + "forces/" + id);

            string name = (string)jsonResponse["name"];

            f = new Force(id, name);

            f.Description = (string)jsonResponse["description"];
            f.URL = (string)jsonResponse["url"];
            f.Telephone = (string)jsonResponse["telephone"];

            foreach (IDictionary<string, object> item in JArraytoDictionaryArray((JArray)jsonResponse["engagement_methods"]))
            {
                string url = (string)item["url"];
                string description = (string)item["description"];
                string title = (string)item["title"];
                Force.Engagement_Method em = new Force.Engagement_Method(url, description, title);
                f.EngagementMethods.Add(em);
            }

            return f;
        }

        public static Force GetSpecificForce(Force f)
        {
            return GetSpecificForce(f.ID);
        }

        
        public static IList<IDictionary<string, object>> JArraytoDictionaryArray(JArray ar)
        {
            return ar.ToObject<IList<IDictionary<string, object>>>();
        }

        /// <summary>
        /// Retrieve a JSON array from a given URL
        /// </summary>
        /// <param name="RequestURL">URL requested</param>
        public static IList<IDictionary<string, object>> RetrieveJSONArray(string RequestURL)
        {
            string response = "";
            using (WebClient wc = new WebClient())
            {
                response = wc.DownloadString(new Uri(RequestURL));
            }

            return JsonToArray(response);
        }

        /// <summary>
        /// Retrieve a JSON object from a given URL
        /// </summary>
        /// <param name="RequestURL">URL requested</param>
        public static IDictionary<string, object> RetrieveJSONObject(string RequestURL)
        {
            string response = "";
            using (WebClient wc = new WebClient())
            {
                response = wc.DownloadString(new Uri(RequestURL));
            }

            return JsonToObject(response);
        }

        private static IDictionary<string, object> JsonToObject(string json)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
        }

        private static IList<IDictionary<string, object>> JsonToArray(string json)
        {
            return JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(json);
        }

        private static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
