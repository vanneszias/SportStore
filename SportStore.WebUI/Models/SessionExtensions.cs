using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace SportStore.WebUI.Models;

public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        return data == null ? default : JsonSerializer.Deserialize<T>(data);
    }
} 