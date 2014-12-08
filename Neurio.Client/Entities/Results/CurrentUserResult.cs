using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurio.Client.Entities.Results
{
    public class CurrentUserResult : BaseResult
    {

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Location> Locations { get; set; }

        public List<Appliance> Appliances { get; set; }
    }






}