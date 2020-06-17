using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TournamentManager
{
    public class TeamIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TTeam.ITeamId t = (TTeam.ITeamId)value;
            
            writer.WriteValue(t.Id );
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TTeam.ITeamId);
        }
    }
}
