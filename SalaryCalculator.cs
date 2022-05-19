using Arcane.Itec.Data;
using Arcane.Itec.DTO;
using Arcane.Itec.ItecUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arcane.Itec
{
    public class SalaryCalculator
    {
        private readonly CommissionValue Commission;
        private readonly CommisionRules Rules;
        private int TotalSimReward { get; set; }
        private int TotalSelloutReward { get; set; }
        private int TotalVolumes { get; set; }
        private int TotalVolumeRewards { get; set; }
        private int TotalEmployeeSalarys { get; set; }

        public SalaryCalculator(CommissionValue commission, CommisionRules rules)
        {
            Commission = commission;
            Rules = rules;
        }

        public List<Employee> GetEmployeesSalary(Dictionary<string, PSR> agencyPsr)
        {
            var employeesNames = agencyPsr.Values.Select(x => x.WalkerName)
                                                 .Distinct()
                                                 .ToList();

            var employees = new List<Employee>();
            employeesNames.ForEach(name =>
            {
                var clientFromEmployee = agencyPsr.Values.Where(psr => psr.WalkerName == name);
                employees.Add(new Employee
                {
                    Name = name,
                    PsrAmount = clientFromEmployee.Count(),
                    SimAmount = clientFromEmployee.Where(client => client.SimOk).Count(),
                    SelloutAmount = clientFromEmployee.Where(client => client.SelloutOk).Count(),
                    VolumeAmount = clientFromEmployee.Select(x => x.MonthlySale).Aggregate((result, value) => result + value)
                });
            });

            TotalVolumes = agencyPsr.Values.Select(x => x.MonthlySale)
                                           .Aggregate((result, value) => result + value);

            RemoveUnpaidVolumes(employees);

            employees.ForEach(e =>
            {
                e.SimReward = CalculateSimReward(e);
                e.SelloutReward = CalculateSOReward(e);
                e.SimPercentage = Utils.GetEfectivity(e.PsrAmount, e.SimAmount);
                e.SelloutPercentage = Utils.GetEfectivity(e.PsrAmount, e.SelloutAmount);
                e.VolumeReward = CalculateVolumenReward(e, TotalVolumes);
                e.TotalSalary = CalculateTotalRewards(e);
            });

            var totalSimAmount = agencyPsr.Where(p => p.Value.SimOk).Count();
            var totalSelloutAmount = agencyPsr.Where(p => p.Value.SelloutOk).Count();

            employees.Add(new Employee
            {
                Name = "TOTALES",
                PsrAmount = agencyPsr.Values.Count,
                SimAmount = totalSimAmount,
                SimReward = "$ " + TotalSimReward,
                SimPercentage = Utils.GetEfectivity(agencyPsr.Values.Count, totalSimAmount),
                SelloutAmount = totalSelloutAmount,
                SelloutReward = "$ " + TotalSelloutReward,
                SelloutPercentage = Utils.GetEfectivity(agencyPsr.Values.Count, totalSelloutAmount),
                VolumeAmount = TotalVolumes,
                VolumeReward = "$ " + TotalVolumeRewards,
                TotalSalary = "$ " + TotalEmployeeSalarys
            });
            return employees;
        }

        private void RemoveUnpaidVolumes(List<Employee> employees)
        {
            employees.ForEach(e =>
            {
                if (e.SimAmount < Rules.RequieredPSR || e.SelloutAmount < Rules.RequieredPSR)
                {
                    TotalVolumes -= e.VolumeAmount;
                }
            });
        }

        private string CalculateSimReward(Employee employee)
        {
            var commision = Commission.DefaultSim;
            if (employee.SimAmount >= Rules.SimStep3) commision = Commission.SimStep3;
            else if (employee.SimAmount >= Rules.SimStep2) commision = Commission.SimStep2;
            else if (employee.SimAmount >= Rules.SimStep1) commision = Commission.SimStep1;

            var reward = employee.SimAmount * commision;
            TotalSimReward += reward;
            TotalEmployeeSalarys += reward;
            return "$ " + reward;
        }

        private string CalculateSOReward(Employee employee)
        {
            int commision = Commission.DefaultSellout;
            if (employee.SelloutAmount >= Rules.SelloutStep2) commision = Commission.SelloutStep2;
            else if (employee.SelloutAmount >= Rules.SelloutStep1) commision = Commission.SelloutStep1;

            var reward = employee.SelloutAmount * commision;
            TotalSelloutReward += reward;
            TotalEmployeeSalarys += reward;
            return "$ " + reward;
        }

        private string CalculateVolumenReward(Employee employee, int volume)
        {
            if (employee.SimAmount < Rules.RequieredPSR || employee.SelloutAmount < Rules.RequieredPSR) return "$ 0";

            long multiplyResult = (long)employee.VolumeAmount * Commission.VolumePayment;
            var reward = (int)Math.Round((double)multiplyResult / volume);

            TotalVolumeRewards += reward;
            TotalEmployeeSalarys += reward;
            return "$ " + reward;
        }

        private string CalculateTotalRewards(Employee employee)
        {
            var simReward = int.Parse(Utils.ExtractNumber(employee.SimReward));
            var selloutReward = int.Parse(Utils.ExtractNumber(employee.SelloutReward));
            var volumeReward = int.Parse(Utils.ExtractNumber(employee.VolumeReward));

            var totalReward = simReward + selloutReward + volumeReward;
            return "$ " + totalReward;
        }
    }
}