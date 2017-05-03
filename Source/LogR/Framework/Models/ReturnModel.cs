using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Models
{
    public class ReturnModel<T>
    {
        public T Model { get; set; }
        public String Error { get; set; }
        public int ActiveTab { get; set; }
    }

}
