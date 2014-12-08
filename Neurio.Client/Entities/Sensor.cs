using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{

    public class Sensor
    {
        public string Name { get; set; }
        public string InstallCode { get; set; }
        public string SensorType { get; set; }
        public string LocationId { get; set; }
        public List<Channel> Channels { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public List<Sample> Samples { get; set; }
        public List<Sample> LiveSamples { get; set; }
        public List<Event> Events { get; set; }

    }
}