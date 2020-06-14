using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{/*
    class RefereesIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            value = (List<TPerson.IRefereeId>)value;
            List<int> id = new List<int>();

            foreach (var x in value)
            {
                id.Add(((TPerson.IRefereeId)x).Id);
            }
            serializer.Serialize(writer, id);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<TPerson.IRefereeId>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }*/
}
