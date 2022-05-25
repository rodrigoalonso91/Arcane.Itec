using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcane.Itec.Abstractions
{
    public interface ISalesPoint
    {
        string Id { get; set; }
        string ClientName { get; set; }
        string Address { get; set; }
        string NumberAddress { get; set; }
    }
}
