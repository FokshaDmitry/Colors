using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    [Serializable]
    public class Responce
    {
        public bool succces { get; set; }
        public string? StatusTxt { get; set; }
        public ResponceCode code { get; set;}
        public object? data { get; set; }
    }
}
