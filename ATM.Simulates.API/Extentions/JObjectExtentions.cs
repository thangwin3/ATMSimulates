using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ATM.Simulates.API.Extentions
{
    public static class JObjectExtention
    {
        /// <summary>
        /// Remove data json null or empty and sort properties follow alphabet
        /// </summary>
        public static void OrderByProperties(this JObject jObj)
        {
            var props = jObj.Properties().OrderBy(x => x.Name).ToList();

            jObj.RemoveAll();

            foreach (var prop in props)
            {
                if (!prop.Value.IsNullOrEmpty())
                {
                    jObj.Add(prop);

                    if (prop.Value is JObject)
                    {
                        (prop.Value as JObject).OrderByProperties();
                    }
                    else if (prop.Value is JArray)
                    {
                        foreach (var pro in prop.Value as JArray)
                        {
                            if (pro is JObject)
                                (pro as JObject).OrderByProperties();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove data json null or empty and sort properties follow alphabet
        /// </summary>
        public static void OrderByProperties(this JArray jarray)
        {
            foreach (var jobj in jarray)
            {
                if (jobj is JObject)
                    (jobj as JObject).OrderByProperties();
            }
        }
    }

    public static class JTokenExtention
    {
        /// <summary>
        /// Remove data json null or empty and sort properties follow alphabet
        /// </summary>
        public static void OrderByProperties(this JToken token)
        {
            if (token is JObject)
            {
                (token as JObject).OrderByProperties();
            }
            else if (token is JArray)
            {
                (token as JArray).OrderByProperties();
            }
        }

        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}
