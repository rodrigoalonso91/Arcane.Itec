using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcane.Itec.Extentions
{
    public static class ListExtentions
    {
        public static List<int> ConvertValuesToInt(this List<string> objList)
        {
            return objList.Select(x => int.Parse(x)).ToList();
        }
    }
}
