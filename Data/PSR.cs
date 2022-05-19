using Arcane.Itec.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcane.Itec.Data
{
    public class PSR : ISalesPoint
    {
        public string Id { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string NumberAddress { get; set; } = string.Empty;
        public string WalkerName { get; set; } = string.Empty;
        public int MonthlySale { get; set; } = 0;
        public bool SimOk { get; set; } = false;
        public bool SelloutOk { get; set; } = false;
    }
}
