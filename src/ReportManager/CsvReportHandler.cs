using Arcane.Itec.Abstractions;
using Arcane.Itec.Data;
using Arcane.Itec.ItecUtils;
using System.Collections.Generic;

namespace Arcane.Itec.ReportManager
{
    public class CsvReportHandler : IReportHandler
    {
        private readonly Dictionary<string, PSR> PsrFromAgency;

        public CsvReportHandler()
        {
            PsrFromAgency = new Dictionary<string, PSR>();
        }

        public Dictionary<string, PSR> GetPSRs() => PsrFromAgency;
        public Dictionary<string, PSR> HandleAgencyPsr(string[] reportPsrAgency)
        {
            var reportLength = reportPsrAgency.Length;

            for (int i = (int)ReportAgencyIndexs.ReportStartIndex; i < reportLength; i++)
            {
                var reportFields = reportPsrAgency[i].Split(';');

                var client = new PSR
                {
                    Id = reportFields[(int)ReportAgencyIndexs.PsrCode],
                    ClientName = reportFields[(int)ReportAgencyIndexs.PsrName].Replace('"', ' ').Trim(),
                    Address = reportFields[(int)ReportAgencyIndexs.Address],
                    NumberAddress = reportFields[(int)ReportAgencyIndexs.AddressNum],
                    WalkerName = reportFields[(int)ReportAgencyIndexs.WalkerName]
                };
                PsrFromAgency[client.Id] = client;
            }
            return PsrFromAgency;
        }
        public Dictionary<string, PSR> HandleSimRemuneration(string[] reportSimPaying)
        {
            var reportLength = reportSimPaying.Length;
            for (int i = (int)ReportSimPaymentIndexs.ReportStartIndex; i < reportLength; i++)
            {
                var reportFields = reportSimPaying[i].Split(';');
                var psrCode = Utils.NormalizeIdCode(reportFields, ((int)ReportSimPaymentIndexs.PsrCode));

                var commision = reportFields[(int)ReportSimPaymentIndexs.SimCommision];
                commision = Utils.ExtractNumber(commision);

                if (PsrFromAgency.ContainsKey(psrCode) && !string.IsNullOrWhiteSpace(commision))
                {
                    PsrFromAgency[psrCode].SimOk = true;
                }
            }
            return PsrFromAgency;
        }
        public Dictionary<string, PSR> HandleSORemuneration(string[] reportSOPaying, int sellTarget)
        {
            var reportLength = reportSOPaying.Length;

            for (int i = (int)ReportSOPaymentIndexs.ReportStartIndex; i < reportLength; i++)
            {
                var reportFiels = reportSOPaying[i].Split(';');
                var psrCode = Utils.NormalizeIdCode(reportFiels, (int)ReportSOPaymentIndexs.PsrCode);
                var monthlySale = reportFiels[(int)ReportSOPaymentIndexs.TotalSale];

                if (PsrFromAgency.ContainsKey(psrCode))
                {
                    PsrFromAgency[psrCode].MonthlySale = int.Parse(monthlySale);
                    PsrFromAgency[psrCode].SelloutOk = PsrFromAgency[psrCode].MonthlySale >= sellTarget;
                }
            }
            return PsrFromAgency;
        }
    }
}