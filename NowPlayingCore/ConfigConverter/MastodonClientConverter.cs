using System;
using System.Collections.Generic;
using System.Text;
using Mastonet;
using Mastonet.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NowPlayingCore.ConfigConverter
{
    class MastodonClientConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mastodonClient = value as MastodonClient;
            if (mastodonClient == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName("AppRegistration");
            writer.WriteStartObject();
            if (mastodonClient.AppRegistration != null)
            {
                writer.WritePropertyName("ClientId");
                writer.WriteValue(mastodonClient.AppRegistration.ClientId);
                writer.WritePropertyName("ClientSecret");
                writer.WriteValue(mastodonClient.AppRegistration.ClientSecret);
                writer.WritePropertyName("Id");
                writer.WriteValue(mastodonClient.AppRegistration.Id);
                writer.WritePropertyName("Instance");
                writer.WriteValue(mastodonClient.AppRegistration.Instance);
                writer.WritePropertyName("RedirectUri");
                writer.WriteValue(mastodonClient.AppRegistration.RedirectUri);
                writer.WritePropertyName("Scope");
                writer.WriteValue(mastodonClient.AppRegistration.Scope);
            }
            else
            {
                writer.WriteNull();
            }

            writer.WriteEndObject();

            writer.WritePropertyName("AuthToken");
            writer.WriteStartObject();
            if (mastodonClient.AuthToken != null)
            {
                writer.WritePropertyName("AccessToken");
                writer.WriteValue(mastodonClient.AuthToken.AccessToken);
                writer.WritePropertyName("TokenType");
                writer.WriteValue(mastodonClient.AuthToken.TokenType);
                writer.WritePropertyName("Scope");
                writer.WriteValue(mastodonClient.AuthToken.Scope);
                writer.WritePropertyName("CreatedAt");
                writer.WriteValue(mastodonClient.AuthToken.CreatedAt);
            }
            else
            {
                writer.WriteNull();
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var appRegistration = new AppRegistration();
            var authToken = new Auth();
            var jsonObject = JObject.Load(reader);
            appRegistration.ClientId = jsonObject["AppRegistration"]["ClientId"].Value<string>();
            appRegistration.ClientSecret = jsonObject["AppRegistration"]["ClientSecret"].Value<string>();
            appRegistration.Id = jsonObject["AppRegistration"]["Id"].Value<long>();
            appRegistration.Instance = jsonObject["AppRegistration"]["Instance"].Value<string>();
            appRegistration.RedirectUri = jsonObject["AppRegistration"]["RedirectUri"].Value<string>();
            var scope = jsonObject["AppRegistration"]["Scope"].Value<long>();
            appRegistration.Scope = (Scope) Enum.ToObject(typeof(Scope), scope);

            authToken.AccessToken = jsonObject["AuthToken"]["AccessToken"].Value<string>();
            authToken.TokenType = jsonObject["AuthToken"]["TokenType"].Value<string>();
            authToken.Scope = jsonObject["AuthToken"]["Scope"].Value<string>();
            authToken.CreatedAt = jsonObject["AuthToken"]["CreatedAt"].Value<string>();

            return new MastodonClient(appRegistration, authToken);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MastodonClient);
        }
    }
}