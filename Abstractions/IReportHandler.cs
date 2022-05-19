using Arcane.Itec.Data;
using System.Collections.Generic;

namespace Arcane.Itec.Abstractions
{
    public interface IReportHandler
    {
        Dictionary<string, PSR> GetPSRs();

        Dictionary<string, PSR> HandleAgencyPsr(string[] reportPsrAgency);

        Dictionary<string, PSR> HandleSimRemuneration(string[] reportSimPaying);

        Dictionary<string, PSR> HandleSORemuneration(string[] reportSOPaying, int sellTargetSO);
    }
}