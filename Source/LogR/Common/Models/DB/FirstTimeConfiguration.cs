using System;

namespace LogR.Common.Models.DB
{
    public class FirstTimeConfiguration
    {
        public long ID { get; set; }

        public DateTime DoneDate { get; set; }

        public bool Done { get; set; }
    }
}