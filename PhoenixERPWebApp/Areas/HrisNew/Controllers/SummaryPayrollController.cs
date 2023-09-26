using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using CodeMarvel.Infrastructure.ModelShared;

using FastMember;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Phoenix.WebExtension.RestApi;

using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.HrisNew.Report;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class SummaryPayrollController : Controller
    {
        string url = ApiUrl.SummaryPayrollUrl;
        private readonly ApiClientFactory client;
        string menuUnique = "PAYROLLBYLEGALENTITY";

        public SummaryPayrollController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll/{menuUnique}");
            return View();
        }
        public async Task<IActionResult> GetGenerateList(int month, int year, string legalEntityId, string divisionId)
        {

            var IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll/{menuUnique}");

            var result = await client.GetApiResponse<DataTable>($"{url}/{month}/{year}/{legalEntityId}/{divisionId}");
            if (result.Data != null)
            {


                if (!IsAllowOpenEncryptEmployeePayroll)
                {

                    var JsonData = JsonConvert.SerializeObject(result.Data, Formatting.Indented);
                    List<SummaryPayroll> listObject = JsonConvert.DeserializeObject<List<SummaryPayroll>>(JsonData);


                    foreach (var item in listObject)
                    {
                        item.rpt_Basic_Salary = "***********";
                        item.rpt_Bonus = "***********";
                        item.rpt_THR = "***********";
                        item.rpt_Jaminan_Pensiun_BPJS = "***********";
                        item.rpt_Jamsostek_BPJS_TK = "***********";
                        item.rpt_Overtime = "***********";
                        item.rpt_Phone_Allowance = "***********";
                        item.rpt_Timeslip_Allowance = "***********";
                        item.rpt_housing_allowance = "***********";
                        item.rpt_job_allowance = "***********";
                        item.rpt_Transportation_Allowance = "***********";
                        item.rpt_Tambahan_Rapel = "***********";
                        item.rpt_Gaji_Kotor = "***********";
                        //item.rpt_THR_Bonus = "***********";
                        item.rpt_Tunjangan_Jabatan = "***********";
                        //item.rpt_BPJS_KS_Corp = "***********";
                        //item.rpt_Jamsostek_BPJS_TK_deduction = "***********";
                        item.rpt_Iuran_sukarela_Koperasi_Nava = "***********";
                        item.rpt_Iuran_Wajib_Koperasi_Nava = "***********";
                        //item.rpt_Potongan_Jamsostek = "***********";
                        //item.rpt_Potongan_Pinjaman_Koperasi = "***********";
                        //item.rpt_BPJS_KS_Emp = "***********";
                        //item.rpt_BPJS_TK_Emp = "***********";
                        item.rpt_Pot_Jaminan_Pensiun_BPJS = "***********";
                        //item.rpt_potongan_bpjs_2 = "***********";
                        item.rpt_potongan_bpjs_6 = "***********";
                        item.rpt_potongan_lain_lain = "***********";
                        item.rpt_Potongan_Unpaid_leave = "***********";
                        item.rpt_Loan = "***********";
                        item.rpt_Pph_21 = "***********";
                        item.rpt_THP = "***********";
                        item.rpt_Account_No = "***********";
                        item.rpt_bank_id = "***********";
                    }
                    result.Data = listObject;
                }
                else
                {

                    var JsonData = JsonConvert.SerializeObject(result.Data, Formatting.Indented);
                    List<SummaryPayroll> listObject = JsonConvert.DeserializeObject<List<SummaryPayroll>>(JsonData);
                    List<SummaryPayrollInt> listObjectInt = new List<SummaryPayrollInt>();
                    foreach (var item in listObject)
                    {
                        SummaryPayrollInt d = new SummaryPayrollInt();
                        if (!string.IsNullOrEmpty(item.employee_basic_info_id))
                        {
                            d.employee_basic_info_id = item.employee_basic_info_id;
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Employee_Name))
                        {
                            d.rpt_Employee_Name = item.rpt_Employee_Name;
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Legal_Entity_Name))
                        {
                            d.rpt_Legal_Entity_Name = item.rpt_Legal_Entity_Name;
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Division))
                        {
                            d.rpt_Division = item.rpt_Division;
                        }


                        if (!string.IsNullOrEmpty(item.rpt_Join_Date))
                        {
                            d.rpt_Join_Date = item.rpt_Join_Date;
                        }


                        if (!string.IsNullOrEmpty(item.rpt_Status_Pph21))
                        {
                            d.rpt_Status_Pph21 = item.rpt_Status_Pph21;
                        }


                        if (!string.IsNullOrEmpty(item.rpt_employee_code))
                        {
                            d.rpt_employee_code = item.rpt_employee_code;
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Account_No))
                        {
                            d.rpt_Account_No = item.rpt_Account_No;
                        }

                        if (!string.IsNullOrEmpty(item.rpt_bank_id))
                        {
                            d.rpt_bank_id = item.rpt_bank_id;
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Basic_Salary))
                        {
                            d.rpt_Basic_Salary = float.Parse(item.rpt_Basic_Salary);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_THR))
                        {
                            d.rpt_THR = float.Parse(item.rpt_THR);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Bonus))
                        {
                            d.rpt_Bonus = float.Parse(item.rpt_Bonus);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Jaminan_Pensiun_BPJS))
                        {
                            d.rpt_Jaminan_Pensiun_BPJS = float.Parse(item.rpt_Jaminan_Pensiun_BPJS);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Jamsostek_BPJS_TK))
                        {
                            d.rpt_Jamsostek_BPJS_TK = float.Parse(item.rpt_Jamsostek_BPJS_TK);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Overtime))
                        {
                            d.rpt_Overtime = float.Parse(item.rpt_Overtime);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Phone_Allowance))
                        {
                            d.rpt_Phone_Allowance = float.Parse(item.rpt_Phone_Allowance);
                        }
                        if (!string.IsNullOrEmpty(item.rpt_Timeslip_Allowance))
                        {
                            d.rpt_Timeslip_Allowance = float.Parse(item.rpt_Timeslip_Allowance);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_housing_allowance))
                        {
                            d.rpt_housing_allowance = float.Parse(item.rpt_housing_allowance);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_job_allowance))
                        {
                            d.rpt_job_allowance = float.Parse(item.rpt_job_allowance);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Transportation_Allowance))
                        {
                            d.rpt_Transportation_Allowance = float.Parse(item.rpt_Transportation_Allowance);
                        }
                        if (!string.IsNullOrEmpty(item.rpt_Tambahan_Rapel))
                        {
                            d.rpt_Tambahan_Rapel = float.Parse(item.rpt_Tambahan_Rapel);
                        }



                        if (!string.IsNullOrEmpty(item.rpt_Tunjangan_Jabatan))
                        {
                            d.rpt_Tunjangan_Jabatan = float.Parse(item.rpt_Tunjangan_Jabatan);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Gaji_Kotor))
                        {
                            d.rpt_Gaji_Kotor = float.Parse(item.rpt_Gaji_Kotor);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Iuran_sukarela_Koperasi_Nava))
                        {
                            d.rpt_Iuran_sukarela_Koperasi_Nava = float.Parse(item.rpt_Iuran_sukarela_Koperasi_Nava);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Iuran_Wajib_Koperasi_Nava))
                        {
                            d.rpt_Iuran_Wajib_Koperasi_Nava = float.Parse(item.rpt_Iuran_Wajib_Koperasi_Nava);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Pot_Jaminan_Pensiun_BPJS))
                        {
                            d.rpt_Pot_Jaminan_Pensiun_BPJS = float.Parse(item.rpt_Pot_Jaminan_Pensiun_BPJS);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_potongan_bpjs_6))
                        {
                            d.rpt_potongan_bpjs_6 = float.Parse(item.rpt_potongan_bpjs_6);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_potongan_lain_lain))
                        {
                            d.rpt_potongan_lain_lain = float.Parse(item.rpt_potongan_lain_lain);
                        }
                        if (!string.IsNullOrEmpty(item.rpt_Potongan_Unpaid_leave))
                        {
                            d.rpt_Potongan_Unpaid_leave = float.Parse(item.rpt_Potongan_Unpaid_leave);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Loan))
                        {
                            d.rpt_Loan = float.Parse(item.rpt_Loan);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_Pph_21))
                        {
                            d.rpt_Pph_21 = float.Parse(item.rpt_Pph_21);
                        }

                        if (!string.IsNullOrEmpty(item.rpt_THP))
                        {
                            d.rpt_THP = float.Parse(item.rpt_THP);
                        }

                        listObjectInt.Add(d);
                    }
                    result.Data = listObjectInt;
                }
            }
            //DataTable table = new DataTable();
            //using (var reader = ObjectReader.Create(listObject))
            //{
            //    table.Load(reader);
            //}

            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public ActionResult ExportExcel(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public ActionResult ExportExcelListAccount(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }


    }


}