using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenderAnalyzer.DataModels
{
    class GenderGuesserModel
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "Male")]
        public string Male { get; set; }

        [JsonProperty(PropertyName = "Female")]
        public string Female { get; set; }
    }
}
