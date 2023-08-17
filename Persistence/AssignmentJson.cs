using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Diner.Persistence
{
    public static class AssignmentJson
    {
        public static JsonSerializerSettings Settings => new JsonSerializerSettings().AddAssignmentJsonSerializerSettings();

        public static JsonSerializerSettings AddAssignmentJsonSerializerSettings(this JsonSerializerSettings settings)
        {
#if DEBUG
            settings.Formatting = Formatting.Indented;
#endif
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            settings.DateParseHandling = DateParseHandling.None;  // TODO: should this be DateTimeOffset?
            settings.FloatParseHandling = FloatParseHandling.Decimal;
            settings.NullValueHandling = NullValueHandling.Ignore;
           // settings.Converters.Add(new AssignmentEventConverter());
            settings.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal });
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }
    }
}