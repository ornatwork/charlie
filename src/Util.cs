using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Charlie
{
    public class Util
    {
        /// <summary>
        /// Encodes string to base64
        /// </summary>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decodes base64 to string
        /// </summary>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Gets weather forecast from location.
        /// </summary>
        public static string GetWeather(string zip)
        {
            // API url
            string url = "https://samples.openweathermap.org/data/2.5/weather?q=" + zip + ",us&appid=b6907d289e10d714a6e88b30761fae22";
            //string url = "http://api.openweathermap.org/data/2.5/weather?zip=" + zip + ",us&APPID=d1437fe63b9165a5569c09489f6c69f8";
            string content = string.Empty;

            try
            {
                // setup client and get content from weather API
                var client = new WebClient();
                content = client.DownloadString(url);
                // Deserialize json
                //dynamic results = JsonConvert.DeserializeObject<dynamic>(content);
                JObject json = JObject.Parse(content);
                string where = (string)json["name"];
                string description = (string)json["weather"][0]["description"];
                string humidity = (string)json["main"]["humidity"];

                // back to caller
                content = where + ", " + description + ", humidity " + humidity;
            }
            catch (Exception ex)
            {
                content = ", error=" + ex;
            }

            return content;
        }
    }
}
