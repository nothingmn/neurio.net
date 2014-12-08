using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{

    public class Location
    {
        public string Name { get; set; }
        public string Timezone { get; set; }
        public List<Sensor> Sensors { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
    }
}
