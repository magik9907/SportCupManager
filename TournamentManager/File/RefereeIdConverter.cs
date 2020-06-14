using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TournamentManager
{
    class RefereeIdConverter : JsonConverter
    {
        public void Write(JsonWriter writer, List<TPerson.Referee> value, JsonSerializer serializer)
        {
            List<int> id = new List<int>();

            foreach (var x in value)
            {
                id.Add((x).Id);
            }
            serializer.Serialize(writer, id);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is List<TPerson.Referee> e)
            {
                Write(writer, (List<TPerson.Referee>)value, serializer);
            }
            else
            {

                TPerson.Referee t = (TPerson.Referee)value;

                writer.WriteValue(t.Id);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TPerson.Referee);
        }
    }
}
