using Arcane.Itec.Abstractions;

namespace Arcane.Itec
{
    public class Employee : IEmployee
    {
        public string Name { get; set; }
        public int PsrAmount { get; set; }
        public int SimAmount { get; set; }
        public int SimReward { get; set; }
        public double SimPercentage { get; set; }
        public int SelloutAmount { get; set; }
        public int SelloutReward { get; set; }
        public double SelloutPercentage { get; set; }
        public int VolumeAmount { get; set; }
        public int VolumeReward { get; set; }
        public int TotalSalary { get; set; }
    }
}