using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PingData
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public long RoundtripTime { get; set; }
        public string Status { get; set; }
    }
}
