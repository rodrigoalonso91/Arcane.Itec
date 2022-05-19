using Arcane.Itec.Abstractions;

namespace Arcane.Itec
{
    public class Employee : IEmployee
    {
        public string Name { get; set; }
        public int PsrAmount { get; set; }
        public int SimAmount { get; set; }
        public string SimReward { get; set; }
        public string SimPercentage { get; set; }
        public int SelloutAmount { get; set; }
        public string SelloutReward { get; set; }
        public string SelloutPercentage { get; set; }
        public int VolumeAmount { get; set; }
        public string VolumeReward { get; set; }
        public string TotalSalary { get; set; }
    }
}