using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{

    public class Stat
    {
        public Appliance Appliance { get; set; }
        public float AveragePower { get; set; }
        public int EventCount { get; set; }
        public LastEvent LastEvent { get; set; }
        public int TimeOn { get; set; }
        public int Energy { get; set; }
        public float UsagePercentage { get; set; }
        public dynamic Guesses { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> GroupIds { get; set; }
        public string Id { get; set; }
    }

}
