namespace Arcane.Itec.Abstractions
{
    public interface IEmployee
    {
        string Name { get; set; }
        int PsrAmount { get; set; }
        int SimAmount { get; set; }
        string SimReward { get; set; }
        string SimPercentage { get; set; }
        int SelloutAmount { get; set; }
        string SelloutReward { get; set; }
        string SelloutPercentage { get; set; }
        int VolumeAmount { get; set; }
        string VolumeReward { get; set; }
        string TotalSalary { get; set; }
    }
}