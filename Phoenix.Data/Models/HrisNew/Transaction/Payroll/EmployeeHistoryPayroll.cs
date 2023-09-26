using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.HrisNew.Transaction.Payroll
{
    public class EmployeeHistoryPayroll
    {
        public List<EmployeeHistoryPayrollHeader> Header { get; set; }
        public List<EmployeeHistoryPayrollDetail> Detail { get; set; }
    }

    public class EmployeeHistoryPayrollHeader
    {
        public string month_name { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }

    public class EmployeeHistoryPayrollDetail
    {
        public string month_name { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string name_employee { get; set; }
        public string Legal_Entity { get; set; }
        public string Division { get; set; }
        public string Join_Date { get; set; }
        public string Status_Pph21 { get; set; }
        public string Basic_Salary { get; set; }

        public string Bonus { get; set; }
        public string THR { get; set; }
        public string Jaminan_Pensiun_BPJS { get; set; }
        public string Jamsostek_BPJS_TK_addition { get; set; }
        public string Overtime { get; set; }
        public string Phone_Allowance { get; set; }
        public string Tambahan_Rapel { get; set; }
        public string THR_Bonus { get; set; }
        public string Timesleep_Allowance { get; set; }
        public string Transportation_Allowance { get; set; }
        public string Tunjangan_Jabatan { get; set; }
        public string BPJS_KS_Corp { get; set; }
        public string Jamsostek_BPJS_TK_deduction { get; set; }
        public string Iuran_sukarela_Koperasi_Nava { get; set; }
        public string Iuran_Wajib_Koperasi_Nava { get; set; }
        public string Potongan_Jamsostek { get; set; }
        public string Potongan_Pinjaman_Koperasi { get; set; }
        public string BPJS_KS_Emp { get; set; }
        public string BPJS_TK_Emp { get; set; }
        public string Pot_Jaminan_Pensiun_BPJS { get; set; }
        public string Loan { get; set; }
        public string Pph_21 { get; set; }
        public string THP { get; set; }
        public string potongan_bpjs_2_persen { get; set; }
        public string potongan_lain_lain { get; set; }
        public string potongan_bpjs_6_persen { get; set; }
        public string housing_allowance { get; set; }
        public string job_allowance { get; set; }
        public string Gaji_Kotor { get; set; }
    }
}
