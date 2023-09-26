using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Areas.PhoenixReport.Helper
{

    public static class RegisterReports
    {
        private static String basePath = "wwwroot";
        public static List<ReportRegistered> GetRegistered(){
            List<ReportRegistered> data = new List<ReportRegistered>();
            
            data.Add(new ReportRegistered()
            {
                Name = "BalanceSheet.mrt",
                FileLoad = $"{basePath}/Reports/BalanceSheet.mrt",
                DictionaryVariables = new List<StiVariable>() {
                    new StiVariable(){
                        Name="legal_entity_id",
                        Type = typeof(string)
                    }
                }
            });

            data.Add(new ReportRegistered()
            {
                Name = "General Ledger.mrt",
                FileLoad = $"{basePath}/Reports/General Ledger.mrt",
                DictionaryVariables = new List<StiVariable>() {
                    new StiVariable(){
                        Name="legal_entity_id",
                        Type = typeof(string)
                    },
                    new StiVariable(){
                        Name="account_id",
                        Type = typeof(string)
                    },
                }
            });

            return data;
        }


        public static StiReport GetReport(this Microsoft.AspNetCore.Mvc.Controller controller,
            String ReportName, List<StiVariable> DictionaryVariables,String ConnectionString)
        {
            var report = new StiReport();
            var reportRegistered = GetRegistered().Where(o => o.Name == ReportName).FirstOrDefault();

            report.Load(StiNetCoreHelper.MapPath(controller, reportRegistered.FileLoad));
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new Stimulsoft.Report.Dictionary.StiSqlDatabase("MS SQL", ConnectionString));

           
            foreach (StiVariable variable in reportRegistered.DictionaryVariables)
            {
                if (DictionaryVariables.Exists(o => o.Name == variable.Name))
                {
                    var param =  DictionaryVariables.Where(o => o.Name == variable.Name).FirstOrDefault();
                    param.Type = variable.Type;
                    report.Dictionary.Variables[param.Name] = param;
                }
            }

            return report;
        }

        public class ReportRegistered
        {
            public String Name { get; set; }
            public String FileLoad { get; set; }

            public List<StiVariable> DictionaryVariables { get; set; }
        }

    }
}
