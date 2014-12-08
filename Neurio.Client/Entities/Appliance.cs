using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{
    public class Appliance
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string LocationId { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Id { get; set; }
    }
}
