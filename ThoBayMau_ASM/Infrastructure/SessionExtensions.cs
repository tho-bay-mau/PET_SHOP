using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aram.Infrastructure
{
	public static class SessionExtensions
	{
		public static void SetJson(this ISession session, string key, object value)
		{
			var options = new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.IgnoreCycles
			};
			session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value, options));
		}

		public static T? GetJson<T>(this ISession session, string key)
		{
			var sessionData = session.GetString(key);
			if (sessionData == null) return default(T);

			var options = new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.IgnoreCycles
			};
			return System.Text.Json.JsonSerializer.Deserialize<T>(sessionData, options);
		}
	}
}