using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IRequestorService
    {
        Task<InfoEmployee> GetByUserLogin();
        Task<InfoEmployee> GetEmployeeinfoById(string employeeId);
        Task<InfoEmployee> GetEmployeeinfoByName(string employeeName);
        Task<InfoEmployee> GetEmployeeLoaninfoById(string employeeId, string isCorporateloan);
        Task<InfoEmployee> GetEmployeeinfoByIdAndLeaveRequest(string employeeId, string leaveRequestId = null);
        Task<List<InfoEmployee>> getAllEmployeByLeaveRequest(string leaveRequestId);
    }

    public class RequestorService : IRequestorService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public RequestorService(DataContext context)
        {
            this.context = context;
        }

        public async Task<InfoEmployee> GetByUserLogin()
        {
            //var employeeId = context.GetEmployeeId();
            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            var requestor = (from b in context.EmployeeBasicInfos
                             join c in context.LegalEntity on b.LegalEntityId equals c.Id
                             where b.Id == employeeId
                             select new InfoEmployee
                             {
                                 EmployeeId = b.Id,
                                 EmployeeName = b.NameEmployee,
                                 DateEmployed = b.JoinDate,
                                 Phone = b.PhoneNumber,
                                 Email = b.Email,
                                 EmployementStatusId = b.EmployementStatusId,
                                 LegalEntityId = c.Id,
                                 LegalEntityName = c.LegalEntityName,
                             }).FirstOrDefault();
            if (requestor == null)
                throw new Exception("Requestor not registered in master employee, please create employee data.");
            if (requestor != null) requestor = GetEmployeeInfo(requestor);
            return requestor;
        }

        public async Task<InfoEmployee> GetEmployeeinfoById(string employeeId)
        {
            var requestor = (from b in context.EmployeeBasicInfos
                             join c in context.LegalEntity on b.LegalEntityId equals c.Id
                            
                             where b.Id == employeeId
                             select new InfoEmployee
                             {
                                 EmployeeId = b.Id,
                                 EmployeeName = b.NameEmployee,
                                 DateEmployed = b.JoinDate,
                                 Phone = b.PhoneNumber,
                                 Email = b.Email,
                                 EmployementStatusId = b.EmployementStatusId,
                                 LegalEntityId = c.Id,
                                 LegalEntityName = c.LegalEntityName,
                               
                             }).FirstOrDefault();

            if (requestor != null)
            {
                requestor = GetEmployeeInfo(requestor);
            }


            return requestor;
        }

        public async Task<List<InfoEmployee>> getAllEmployeByLeaveRequest(string leaveRequestId)
        {
            List<InfoEmployee> infoEmployees = new List<InfoEmployee>();
            var dt = await context.GetDataTable(string.Format("EXEC uspGetSelectAllEmployeeLeaveBalance"), null);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    InfoEmployee m = new InfoEmployee();
                    m.EmployeeId = dt.Rows[i]["EmployeeId"].ToString();
                    m.EmployeeName = dt.Rows[i]["EmployeeName"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["DateEmployed"].ToString()))
                    {
                        m.DateEmployed = DateTime.Parse(dt.Rows[i]["DateEmployed"].ToString());
                    }
                    m.Phone = dt.Rows[i]["Phone"].ToString();
                    m.Email = dt.Rows[i]["Email"].ToString();
                    m.EmployementStatusId = dt.Rows[i]["EmployementStatusId"].ToString();
                    m.LegalEntityId = dt.Rows[i]["LegalEntityId"].ToString();
                    m.LegalEntityName = dt.Rows[i]["LegalEntityName"].ToString();
                    m.JobTitleId = dt.Rows[i]["JobTitleId"].ToString();
                    m.JobTitle = dt.Rows[i]["JobTitle"].ToString();
                    m.GradeId = dt.Rows[i]["GradeId"].ToString();
                    m.Grade = dt.Rows[i]["Grade"].ToString();
                    m.BalanceLeaveCurrentYear = Convert.ToDecimal(dt.Rows[i]["BalanceLeaveCurrentYear"].ToString());
                    m.BalanceLeaveCarryOver = Convert.ToDecimal(dt.Rows[i]["BalanceLeaveCarryOver"].ToString());
                    infoEmployees.Add(m);
                }
            }


            return infoEmployees;
        }

        public async Task<InfoEmployee> GetEmployeeinfoByIdAndLeaveRequest(string employeeId, string leaveRequestId = null)
        {
            var requestor = (from b in context.EmployeeBasicInfos
                             join c in context.LegalEntity on b.LegalEntityId equals c.Id
                             where b.Id == employeeId
                             select new InfoEmployee
                             {
                                 EmployeeId = b.Id,
                                 EmployeeName = b.NameEmployee,
                                 DateEmployed = b.JoinDate,
                                 Phone = b.PhoneNumber,
                                 Email = b.Email,
                                 EmployementStatusId = b.EmployementStatusId,
                                 LegalEntityId = c.Id,
                                 LegalEntityName = c.LegalEntityName
                             }).FirstOrDefault();

            if (requestor != null)
            {
                requestor = GetEmployeeInfo(requestor);
            }

            if (!string.IsNullOrEmpty(leaveRequestId))
            {

                var balThisYear = (from q in context.LeaveBalances
                                   join r in context.LeaveRequestTypes on q.Type equals r.Id
                                   join s in context.EmployeeBasicInfos on q.EmployeeBasicInfoId equals s.Id
                                   where q.IsDeleted == false && r.Name.ToLower().Contains("annual leave") && r.GradeLevelEmployeeId == s.JobGradeId
                                   && q.EmployeeBasicInfoId == employeeId
                                   orderby q.Year descending
                                   select q.Balance).FirstOrDefault();

                var thisYear = (from q in context.LeaveBalances
                                join r in context.LeaveRequestTypes on q.Type equals r.Id
                                join s in context.EmployeeBasicInfos on q.EmployeeBasicInfoId equals s.Id
                                where q.IsDeleted == false && r.Name.ToLower().Contains("annual leave") && r.GradeLevelEmployeeId == s.JobGradeId
                                && q.EmployeeBasicInfoId == employeeId
                                orderby q.Year descending
                                select q.Year).FirstOrDefault();

                var balLastYear = (from q in context.LeaveBalances
                                   join r in context.LeaveRequestTypes on q.Type equals r.Id
                                   join s in context.EmployeeBasicInfos on q.EmployeeBasicInfoId equals s.Id
                                   where q.IsDeleted == false && r.Name.ToLower().Contains("annual leave") && r.GradeLevelEmployeeId == s.JobGradeId
                                   && q.EmployeeBasicInfoId == employeeId && q.Year == (thisYear - 1)
                                   orderby q.Year descending
                                   select q.Balance).FirstOrDefault();

                if (balThisYear != null)
                {
                    requestor.BalanceLeaveCurrentYear = balThisYear.Value;
                }
                else
                {
                    requestor.BalanceLeaveCurrentYear = 0;
                }

                if (balLastYear != null)
                {
                    requestor.BalanceLeaveCarryOver = balLastYear.Value;
                }
                else
                {
                    requestor.BalanceLeaveCarryOver = 0;
                }

            }

            return requestor;
        }

        public async Task<InfoEmployee> GetEmployeeinfoByName(string employeeName)
        {
            var requestor = (from b in context.EmployeeBasicInfos
                             join c in context.LegalEntity on b.LegalEntityId equals c.Id
                             where b.NameEmployee.Trim().Replace(" ", "").ToLower() == employeeName.Trim().Replace(" ", "").ToLower()
                             select new InfoEmployee
                             {
                                 EmployeeId = b.Id,
                                 EmployeeName = b.NameEmployee,
                                 DateEmployed = b.JoinDate,
                                 Phone = b.PhoneNumber,
                                 Email = b.Email,
                                 EmployementStatusId = b.EmployementStatusId,
                                 LegalEntityId = c.Id,
                                 LegalEntityName = c.LegalEntityName
                             }).FirstOrDefault();

            if (requestor != null)
            {
                requestor = GetEmployeeInfo(requestor);
            }

            return requestor;

        }

        public InfoEmployee GetEmployeeInfo(InfoEmployee employee)
        {
            var basicInfo = context.EmployeeBasicInfos.Where(x => x.Id == employee.EmployeeId).FirstOrDefault();
            int years = DateTime.Now.Year - Convert.ToDateTime(basicInfo.DateBirth).Year;
            DateTime birthdayThisYear = Convert.ToDateTime(basicInfo.DateBirth).AddYears(years);

            employee.DateBirth = basicInfo.DateBirth;
            employee.Age = birthdayThisYear > DateTime.Now ? years - 1 : years;

            if (basicInfo.BusinessUnitJobLevelId != null)
            {
                var businessUnit = context.BusinessUnitJobLevels.Where(x => x.Id == basicInfo.BusinessUnitJobLevelId).FirstOrDefault();
                if (businessUnit != null)
                {
                    employee.JobTitle = context.JobTitles.Where(x => x.Id == businessUnit.JobTitleId).Select(x => x.TitleName).FirstOrDefault();
                    employee.JobTitleId = businessUnit.JobTitleId;
                    employee.Grade = context.JobGrades.Where(x => x.Id == businessUnit.JobGradeId).Select(x => x.GradeName).FirstOrDefault();
                    employee.GradeId = businessUnit.JobGradeId;
                    employee.Division = context.BusinessUnits.Where(x => x.Id == businessUnit.BusinessUnitDivisionId).Select(x => x.UnitName).FirstOrDefault();
                    employee.DivisionId = context.BusinessUnits.Where(x => x.Id == businessUnit.BusinessUnitDivisionId).Select(x => x.Id).FirstOrDefault();
                    employee.BusinessUnitId = businessUnit.BusinessUnitId; //context.BusinessUnits.Where(x => x.Id == businessUnit.BusinessUnitDivisionId).Select(x => x.Id).FirstOrDefault();
                }
            }

            var contract = (from r in context.EmployeeContracts
                            join s in context.Filemasters on r.FilemasterId equals s.Id
                            where r.EmployeeBasicInfoId == employee.EmployeeId
                            select new { r.FilemasterId, s.Name }).FirstOrDefault();

            if (contract != null)
            {
                employee.EmployeeContractDocument = contract.FilemasterId;
                employee.EmployeeContractDocumentName = contract.Name;
            }

            employee.MarriedStatusName = context.MarriedStatuss.Where(x => x.Id == basicInfo.MarriedStatusId).Select(x => x.MarriedName).FirstOrDefault();
            employee.EmployementStatusId = basicInfo.EmployementStatusId;
            employee.EmployementStatus = context.EmployementStatuss.Where(x => x.Id == basicInfo.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefault();
            employee.EmployeeJobDescription = "";
            employee.EmployeeJobDescriptionName = "";

           

            return employee;
        }

        public async Task<InfoEmployee> GetEmployeeLoaninfoById(string employeeId, string isCorporateloan) {
            InfoEmployee employee = new InfoEmployee();
            decimal? sum = 0;
            try
            {
                //var query = context.EmployeeLoanPaids.Where(x => x.EmployeeBasicInfoId == employeeId && x.Status == 0).Select(x=>x.Value).ToList();
                var IsCorporate = false;
                if (isCorporateloan == "1")
                {
                    IsCorporate = true;
                }
                var query = (from a in context.EmployeeLoans
                             join b in context.EmployeeLoanPaids on a.Id equals b.EmployeeLoanId
                             join c in context.LoanCategorys on a.LoanCategoryId equals c.Id
                             where a.IsDeleted == false && b.EmployeeBasicInfoId == employeeId && b.Status == 0 && c.is_Corporate_Loan == IsCorporate
                             select b.Value).ToList(); 
                sum = query.Sum();
            }
            catch 
            {
                sum = 0;
            }

            employee.LoanBalance = sum;

            return employee;
        
        
        }


    }
}