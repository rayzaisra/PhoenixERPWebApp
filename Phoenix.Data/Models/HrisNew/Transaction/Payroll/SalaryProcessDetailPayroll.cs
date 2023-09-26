using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_process_detail_payroll", Schema = "hr")]
    public class SalaryProcessDetailPayroll : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column("salary_process_header_id"), StringLength(50)]
        public string SalaryProcessHeaderId { get; set; }

        [Column("basic_salary")]
        public decimal? BasicSalary { get; set; }

        [Column(nameof(Bonus))]
        public decimal? Bonus { get; set; }

        [Column(nameof(THR))]
        public decimal? THR { get; set; }

        [Column("Jaminan_Pensiun_BPJS")]
        public decimal? JaminanPensiunBPJS { get; set; }

        [Column("Jamsostek_BPJS_TK_addition")]
        public decimal? JamsostekBPJSTKaddition { get; set; }

        [Column(nameof(Overtime))]
        public decimal? Overtime { get; set; }

        [Column("Phone_Allowance")]
        public decimal? PhoneAllowance { get; set; }

        [Column("Tambahan_Rapel")]
        public decimal? TambahanRapel { get; set; }


        [Column("THR_Bonus")]
        public decimal? THRBonus { get; set; }

        [Column("Timesleep_Allowance")]
        public decimal? TimesleepAllowance { get; set; }

        [Column("Transportation_Allowance")]
        public decimal? TransportationAllowance { get; set; }

        [Column("Tunjangan_Jabatan")]
        public decimal? TunjanganJabatan { get; set; }

        [Column("BPJS_KS_Corp")]
        public decimal? BPJSKSCorp { get; set; }

        [Column("Jamsostek_BPJS_TK_deduction")]
        public decimal? JamsostekBPJSTKDeduction { get; set; }

        [Column("Iuran_sukarela_Koperasi_Nava")]
        public decimal? IuranSukarelaKoperasiNava { get; set; }

        [Column("Iuran_Wajib_Koperasi_Nava")]
        public decimal? IuranWajibKoperasiNava { get; set; }

        [Column("Potongan_Jamsostek")]
        public decimal? PotonganJamsostek { get; set; }

        [Column("Potongan_Pinjaman_Koperasi")]
        public decimal? PotonganPinjamanKoperasi { get; set; }

        [Column("BPJS_KS_Emp")]
        public decimal? BPJSKSEmp { get; set; }

        [Column("BPJS_TK_Emp")]
        public decimal? BPJSTKEmp { get; set; }

        [Column("Pot_Jaminan_Pensiun_BPJS")]
        public decimal? PotJaminanPensiunBPJS { get; set; }

        [Column("Loan")]
        public decimal? Loan { get; set; }

        [Column("Pph_21")]
        public decimal? Pph21 { get; set; }

        [Column("THP")]
        public decimal? THP { get; set; }

        [Column("potongan_bpjs_2_persen")]
        public decimal? PotonganBpjs2Persen { get; set; }

        [Column("potongan_lain_lain")]
        public decimal? PotonganLainLain { get; set; }

        [Column("potongan_bpjs_6_persen")]
        public decimal? PotonganBpjs6persen { get; set; }

        [Column("housing_allowance")]
        public decimal? HousingAllowance { get; set; }

        [Column("job_allowance")]
        public decimal? JobAllowance { get; set; }

        [Column("Gaji_Kotor")]
        public decimal? GajiKotor { get; set; }
    }
}