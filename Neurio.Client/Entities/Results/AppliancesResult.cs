using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Neurio.Client.Entities.Results
{
    public class AppliancesResult : BaseResult
    {
        [JsonProperty(PropertyName = "Property1")]
        public List<Appliance> Appliances { get; set; }
    }

}