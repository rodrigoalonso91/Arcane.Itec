using System;
using System.Linq;

namespace Arcane.Itec.ItecUtils
{
    public class Utils
    {
        private const string _template = "+{obj} {type}";

        public static string MakeHintText(string obj, string type)
        {
            return _template.Replace("{obj}", obj).Replace("{type}", type);
        }

        public static string ExtractNumber(string input)
        {
            return new String(input.Where(Char.IsDigit).ToArray());
        }

        public static string NormalizeIdCode(string[] arrSplitedItems, int index)
        {
            if (arrSplitedItems[index].Substring(0, 1) != "0")
                return arrSplitedItems[index].Insert(0, "0");

            return arrSplitedItems[index];
        }

        public static string GetEfectivity(int totalClients, int clientsOk)
        {
            var result = (clientsOk * 100) / totalClients;
            return result.ToString() + '%';
        }
    }
}