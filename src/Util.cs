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
        public static string GetWeather(string location)
        {
            // API test url
            //string url = "https://samples.openweathermap.org/data/2.5/weather?q=" + zip + ",us&appid=b6907d289e10d714a6e88b30761fae22";
            // API us zip url
            //string url = "http://api.openweathermap.org/data/2.5/weather?zip=" + zip + ",us&APPID=d1437fe63b9165a5569c09489f6c69f8&units=Imperial";
            // API url
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + location + "&APPID=d1437fe63b9165a5569c09489f6c69f8&units=Imperial";
            string content = string.Empty;

            try
            {
                // setup client and get content from weather API
                var client = new WebClient();
                content = client.DownloadString(url);
                // Deserialize json
                JObject json = JObject.Parse(content);
                // Pick off the interesting weather bits
                string where = (string)json["name"];
                string description = (string)json["weather"][0]["description"];
                string humidity = (string)json["main"]["humidity"];
                string temp = (string)json["main"]["temp"];
                string temp_low = (string)json["main"]["temp_min"];
                string temp_high = (string)json["main"]["temp_max"];

                // back to caller
                content = where + ", " + description + ", temperature " + temp + ", low " + temp_low + ", high " + temp_high + ", humidity " + humidity;
            }
            catch (Exception ex)
            {
                content = ", error=" + ex;
            }

            return content;
        }
    }
}
