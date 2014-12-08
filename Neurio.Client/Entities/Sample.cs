using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{

    public class Sample
    {
        public int ConsumptionPower { get; set; }
        public int ConsumptionEnergy { get; set; }
        public int GenerationPower { get; set; }
        public int GenerationEnergy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
