using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class BussinesUnitJobLevelDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string job_grade_id { get; set; }
        [StringLength(50)]
        public string job_level_id { get; set; }
        [StringLength(255)]
        public string parent_id { get; set; }
        public string job_grade_name { get; set; }
        public string employee_name { get; set; }
        public string job_level_name { get; set; }
        public string job_title_id { get; set; }
        public string job_title_name { get; set; }
        public string business_unit_group_id { get; set; }
        public string business_unit_group_name { get; set; }
        public string business_unit_subgroup_id { get; set; }
        public string business_unit_subgroup_name { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_division_name { get; set; }
        public string business_unit_departement_id { get; set; }
        public string business_unit_departement_name { get; set; }
        public BusinessUnitGroup BusinessUnitGroup { get; set; }
        public BusinessUnitSubgroup BusinessUnitSubgroup { get; set; }
        public BusinessUnitDivision BusinessUnitDivision { get; set; }
        public BusinessUnitDepartement BusinessUnitDepartement { get; set; }
        public ParentBusinessUnit ParentBusinessUnit { get; set; }
        public DateTime? created_on { get; set; }
        public string username { get; set; }
        public string remarks { get; set; }
    }


    public class BusinessUnitJobLevelChart {

        public string Id { get; set; }
        public string parentId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Image { get; set; }
        public string business_unit_group_id { get; set; }
        public string business_unit_subgroup_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }

    }
    public class ParentBusinessUnit {

        public string id { get; set; }
        public string name { get; set; }
        public string remarks { get; set; }
        public string username { get; set; }
        public BusinessUnitGroup business_unit_group_id { get; set; }
        public BusinessUnitSubgroup business_unit_subgroup_id { get; set; }
        public BusinessUnitDivision business_unit_division_id { get; set; }
        public BusinessUnitDepartement business_unit_departement_id { get; set; }
    }

    public class BusinessUnitGroup {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class BusinessUnitSubgroup
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class BusinessUnitDivision
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class BusinessUnitDepartement
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class BussinesUnitJobLevelNew
    {
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string job_grade_id { get; set; }
        [StringLength(50)]
        public string job_level_id { get; set; }
        [StringLength(255)]
        public string parent_id { get; set; }
         public string job_level_name { get; set; }
        public string business_unit_group_id { get; set; }
        public string business_unit_subgroup_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }
        public string job_title_id { get; set; }
        public string job_title_name { get; set; }
        public string job_grade_name { get; set; }
        public string remarks { get; set; }
    }            
    public class BussinesUnitJobLevelSearch : SearchParameter
    {
        public List<BusinessUnitGroup> ListBusinessUnitGroup { get; set; }
        public string Search_business_unit_id { get; set; }
        public string Search_hrjob_grade_id { get; set; }
        public string Search_hrjob_level_id { get; set; }
        public string Search_parent_id { get; set; }
        public string Search_business_unit_group_id { get; set; }
        public string Search_business_unit_subgroup_id { get; set; }
        public string Search_business_unit_division_id { get; set; }
        public string Search_business_unit_departement_id { get; set; }
        public string search_title_name { get; set; }
        public string search_group_name { get; set; }
        public string search_sub_group_name { get; set; }
        public string search_division_name { get; set; }
        public string search_department_name { get; set; }
        public string search_remarks { get; set; }

        public string search_all { get; set; }

    }

    public class BusinessUnitJobLevelTree : BussinesUnitJobLevelDto
    {
        public List<BusinessUnitJobLevelTree> children { get; set; }
    }
    public class BusinessUnitJobLevelParent : BussinesUnitJobLevelDto
    {
        public BusinessUnitJobLevelParent parent { get; set; }
    }

    public class BusinessUnitJobLevelChild : BussinesUnitJobLevelDto
    {

    }
}