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
        private int TotalVolumes { get; set; }

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
                e.TotalSalary = GetTotalSalary(e);
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

        private int CalculateSimReward(Employee employee)
        {
            var commision = Commission.DefaultSim;

            if (employee.SimAmount >= Rules.SimStep3)
            {
                commision = Commission.SimStep3;
            }
            else if (employee.SimAmount >= Rules.SimStep2)
            {
                commision = Commission.SimStep2;
            }
            else if (employee.SimAmount >= Rules.SimStep1)
            {
                commision = Commission.SimStep1;
            }

            return employee.SimAmount * commision;
        }

        private int CalculateSOReward(Employee employee)
        {
            int commision = Commission.DefaultSellout;

            if (employee.SelloutAmount >= Rules.SelloutStep2)
            {
                commision = Commission.SelloutStep2;
            }
            else if (employee.SelloutAmount >= Rules.SelloutStep1)
            {
                commision = Commission.SelloutStep1;
            }

            return employee.SelloutAmount * commision;
        }

        private int CalculateVolumenReward(Employee employee, int volume)
        {
            if (employee.SimAmount < Rules.RequieredPSR || employee.SelloutAmount < Rules.RequieredPSR) return 0;

            long multiplyResult = (long)employee.VolumeAmount * Commission.VolumePayment;

            return (int)Math.Round((double)multiplyResult / volume);
        }

        private int GetTotalSalary(Employee employee)
        {
            return employee.SimReward + employee.SelloutReward + employee.VolumeReward;
        }
    }
}