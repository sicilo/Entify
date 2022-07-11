using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public  class Minerals : ProcedurePatameter
    {
        public int ID { get; set; }
        public string Mineral { get; set; } = string.Empty;
        public DateTime? CREATED { get; set; }
    }
}
