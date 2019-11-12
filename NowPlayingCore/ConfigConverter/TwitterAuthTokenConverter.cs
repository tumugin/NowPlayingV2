using System;
using System.Collections.Generic;
using System.Text;
using CoreTweet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NowPlayingCore.Tsumugi;

namespace NowPlayingCore.ConfigConverter
{
    class TwitterAuthTokenConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var authToken = value as Tokens;
            if (authToken == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName("AccessToken");
            writer.WriteValue(authToken.AccessToken);
            writer.WritePropertyName("AccessTokenSecret");
            writer.WriteValue(authToken.AccessTokenSecret);
            writer.WritePropertyName("UserId");
            writer.WriteValue(authToken.UserId);
            writer.WritePropertyName("ScreenName");
            writer.WriteValue(authToken.ScreenName);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            return Tokens.Create(APIKey.CONSUMER_KEY, APIKey.CONSUMER_SECRET,
                jsonObject["AccessToken"].Value<string>(), jsonObject["AccessTokenSecret"].Value<string>(),
                jsonObject["UserId"].Value<long>(), jsonObject["ScreenName"].Value<string>());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Tokens);
        }
    }
}