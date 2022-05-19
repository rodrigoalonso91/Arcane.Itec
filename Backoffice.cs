using Arcane.Itec.Abstractions;
using Arcane.Itec.Data;
using Arcane.Itec.ReportManager;
using System.Collections.Generic;

namespace Arcane.Itec
{
    public class Backoffice
    {
        public IReportHandler _reportHandler;

        public Backoffice() : this(new CsvReportHandler())
        {
        }

        internal Backoffice(IReportHandler reportHandler)
        {
            _reportHandler = reportHandler;
        }

        public void Handle(string[] reportPsrAgency, string[] reportSimPayment, string[] reportSoPayment, int saleTarget)
        {
            _reportHandler.HandleAgencyPsr(reportPsrAgency);
            _reportHandler.HandleSimRemuneration(reportSimPayment);
            _reportHandler.HandleSORemuneration(reportSoPayment, saleTarget);
        }

        public List<NonCompliantClients> GetNonCompliantClients()
        {
            var outputList = new List<NonCompliantClients>();
            foreach (var psr in _reportHandler.GetPSRs().Values)
            {
                if (!psr.SimOk)
                {
                    var nonCompliant = new NonCompliantClients
                    {
                        Id = psr.Id,
                        WalkerName = psr.WalkerName,
                        ClientName = psr.ClientName,
                        Sale = "No aplica",
                        Type = "SIM"
                    };
                    outputList.Add(nonCompliant);
                }
                if (!psr.SelloutOk)
                {
                    var nonCompliant = new NonCompliantClients
                    {
                        Id = psr.Id,
                        WalkerName = psr.WalkerName,
                        ClientName = psr.ClientName,
                        Sale = psr.MonthlySale.ToString(),
                        Type = "SELLOUT"
                    };
                    outputList.Add(nonCompliant);
                }
            }
            return outputList;
        }
    }
}