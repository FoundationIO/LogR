using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Code.Models.DB
{
    public class FirstTimeConfiguration
    {
        public long ID { get; set; }
        public DateTime DoneDate { get; set; }
        public bool Done { get; set; }
    }
}