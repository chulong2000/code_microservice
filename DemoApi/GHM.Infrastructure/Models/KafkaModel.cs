using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.Infrastructure.Models
{
    public class KafkaModel
    {
        public string action { get; set; }
        public dynamic data { get; set; }
    }

    public class IdModel
    {
        public string Id { get; set; }
    }
}
