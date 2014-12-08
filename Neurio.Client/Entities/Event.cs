using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities
{
    public class Event
    {

        public Appliance Appliance { get; set; }
        public string Status { get; set; }
        public DateTime Start { get; set; }
        public int Energy { get; set; }
        public float AveragePower { get; set; }
        public dynamic Guesses { get; set; }
        public List<string> GroupIds { get; set; }
        public int CycleCount { get; set; }
        public bool IsConfirmed { get; set; }
        public string Id { get; set; }
        public DateTime End { get; set; }
    }


}