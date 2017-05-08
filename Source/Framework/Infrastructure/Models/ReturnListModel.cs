using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Models
{
    public class ReturnListModel<T, K>
    {
        public List<T> Model { get; set; }
        public K Search { get; set; }
        public String Error { get; set; }
        public int ActiveTab { get; set; }
    }
}
