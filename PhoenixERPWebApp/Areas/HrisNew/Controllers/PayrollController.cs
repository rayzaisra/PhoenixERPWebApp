using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
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
    public class PayrollController : Controller
    {
        string url = ApiUrl.PayrollUrl;
        private readonly ApiClientFactory client;
        string menuUnique = "PAYROLLREQ";
        public PayrollController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexApproval()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id, bool isApprove)
        {
            var model = new SalaryProcessHeader();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<SalaryProcessHeader>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft || model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = isApprove == false;
                model.Status = StatusTransaction.Draft;
            }

            ViewBag.processApprove = isApprove && isCurrentApproval;
            model.IsApprove = isApprove;
            //ViewBag.isHcAdmin = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsHCAdmin");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<SalaryProcessHeader>>(url) ?? new List<SalaryProcessHeader>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<SalaryProcessHeader>>($"{url}/approvalList") ?? new List<SalaryProcessHeader>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(SalaryProcessHeader model)
        {
            model.Status = StatusTransaction.Approved;
            model.IsTmp = false;
            var response = await client.PutApiResponse<SalaryProcessHeader>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(SalaryProcessHeader model)
        {
            model.Status = StatusTransaction.Rejected;
            model.IsTmp = false;
            var response = await client.PutApiResponse<SalaryProcessHeader>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SalaryProcessHeader model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            model.IsTmp = false;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SalaryProcessHeader model)
        {
            model.Status = StatusTransaction.Draft;
            model.IsTmp = false;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(SalaryProcessHeader model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<SalaryProcessHeader>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<SalaryProcessHeader>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as SalaryProcessHeader });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateProcess(int month, int year)
        {
            var decrypt = await client.GetApiResponse<int>($"{url}/DecryptPayroll");
            var response = await client.GetApiResponse<SalaryProcessHeader>($"{url}/Generate/{month}/{year}");

            return Json(response, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetGenerateList(int month, int year)
        {
            var result = await client.GetApiResponse<DataTable>($"{url}/{month}/{year}");
            var IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll/{menuUnique}");
            var JsonData = JsonConvert.SerializeObject(result.Data, Formatting.Indented);
            //List<GeneratePayroll> listObject = JsonConvert.DeserializeObject<List<GeneratePayroll>>(JsonData);
            //if (!IsAllowOpenEncryptEmployeePayroll)
            //{
            //    foreach (var item in listObject)
            //    {
            //        item.rpt_Basic_Salary = "***********";
            //        item.rpt_Tunjangan_HP = "***********";
            //        item.rpt_Tunjangan_Rumah_Tangga = "***********";
            //        item.rpt_Loan = "***********";
            //        item.rpt_Koperasi = "***********";
            //        item.rpt_Koperasi_Dana_Talangan = "***********";
            //        item.rpt_Koperasi_Simpan_Pinjam = "***********";
            //        item.rpt_BPJS_TK_Corp = "***********";
            //        item.rpt_BPJS_TK_Emp = "***********";
            //        item.rpt_BPJS_KS_Corp = "***********";
            //        item.rpt_BPJS_KS_Emp = "***********";
            //        item.rpt_Take_Home_Pay = "***********";
            //    }
            //}

            List<GeneratePayrollNew> listObject = JsonConvert.DeserializeObject<List<GeneratePayrollNew>>(JsonData);
            if (!IsAllowOpenEncryptEmployeePayroll)
            {
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
                    

                }
                result.Data = listObject;
            }
            else
            {
                List<GeneratePayrollFloat> listObjectFloat = new List<GeneratePayrollFloat>();
                foreach (var item in listObject)
                {
                    GeneratePayrollFloat d = new GeneratePayrollFloat();
                    if (!string.IsNullOrEmpty(item.employee_basic_info_id))
                    {
                        d.employee_basic_info_id = item.employee_basic_info_id;
                    }

                    if (!string.IsNullOrEmpty(item.rpt_Employee_Name))
                    {
                        d.rpt_Employee_Name = item.rpt_Employee_Name;
                    }

                    if (!string.IsNullOrEmpty(item.rpt_Legal_Entity))
                    {
                        d.rpt_Legal_Entity = item.rpt_Legal_Entity;
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

                    listObjectFloat.Add(d);
                }
                result.Data = listObjectFloat;
            }
           
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> EncryptDataPayroll()
        {
            var encrypt = await client.GetApiResponse<int>($"{url}/EncryptPayroll");
            return Json(encrypt, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}