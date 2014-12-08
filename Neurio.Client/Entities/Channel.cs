using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{
    public class Channel
    {
        public string SensorId { get; set; }
        public int channel { get; set; }
        public string ChannelType { get; set; }
        public DateTime Start { get; set; }
        public string Id { get; set; }
    }
}
