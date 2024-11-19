using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLua;

namespace DynamicScores
{
    public class ObjectToInferredTypesConverter : JsonConverter<object>
    {
        public override object Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number when reader.TryGetInt64(out var l) => l,
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.String when reader.TryGetDateTime(out var datetime) => datetime,
                JsonTokenType.String => reader.GetString()!,
                _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            object objectToWrite,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
        }
    }

    public class JsonScoreProcessor
    {
        private readonly Lua _luaState;

        public JsonScoreProcessor()
        {
            _luaState = new Lua();
            _luaState.DoString("import = function () end");
        }

        public double GetScoreDouble(string scoreJson, string luaScript)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new ObjectToInferredTypesConverter());
            dynamic data = JsonSerializer.Deserialize<ExpandoObject>(scoreJson, options);
            _luaState["score"] = data;
            var res = (double)(long)_luaState.DoString(luaScript)[0];
            return res;
        }
    }
}