using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
	public partial interface IEmployeeBasicInfoServices : IBaseService<EmployeeBasicInfoDto, EmployeeBasicInfo, string>
	{
		GeneralResponseList<EmployeeBasicInfoDto> ListAll(EmployeeBasicInfoSearch filter);
		GeneralResponse<EmployeeBasicInfo> DeleteWithReference(string id);
		GeneralResponse<EmployeeBasicInfoDto> GetWithDetail(string id);
		GeneralResponse<EmployeeBasicInfoDto> GetWithEmployeeId(string id);
		GeneralResponseList<EmployeeBasicInfoDto> GetWithDivisionId(string DivisionId);
		GeneralResponse<EmployeeBasicInfoDto> CreateWithReference(EmployeeBasicInfoNew dto);
		GeneralResponse<EmployeeBasicInfoDto> CreateEmployeeFromCandidate(EmployeeBasicInfoNew dto);
		GeneralResponse<EmployeeBasicInfoNew> CreateNewEmployeeBasic(EmployeeBasicInfoNew dto);
		GeneralResponse<EmployeeBasicInfo> UpdateNewEmployeeBasic(EmployeeBasicInfoNew dto);
	}
	public partial class EmployeeBasicInfoServices : BaseService<EmployeeBasicInfoDto, EmployeeBasicInfo, string>, IEmployeeBasicInfoServices
	{
		private IEFRepository<Family, string> _repoFamily;
		private IEFRepository<EmployeeEmergencyContact, string> _repoEmergency;
		private IEFRepository<Education, string> _repoEducation;
		private IEFRepository<EmployeeTraining, string> _repoTraining;
		private IEFRepository<EmployeeExperience, string> _repoExperience;
		private IEFRepository<BusinessUnitJobLevel, string> _repoBusinessUnitJobLevel;
		private IEFRepository<CandidateRecruitmentEducationInfo, string> _repoCandidateRecruitmentEducationInfo;
		private IEFRepository<CandidateRecruitmentEmploymentHistory, string> _repoCandidateRecruitmentEmploymentHistory;
		private IEFRepository<CandidateRecruitmentSignContract, string> _repoCandidateRecruitmentSignContract;
		private IEFRepository<EmployeeContract, string> _repoEmployeeContract;
		private IEFRepository<EmployeeChecklist, string> _repoEmployeeChecklist;
		private IEFRepository<CandidateRecruitment, string> _repoCandidateRecruitment;
		private IEFRepository<Institution, string> _repoInstitution;
		public EmployeeBasicInfoServices(IEFRepository<EmployeeBasicInfo, string> repo, IEFRepository<Family, string> repoFamily, IEFRepository<EmployeeEmergencyContact, string> repoEmergency,
			IEFRepository<Education, string> repoEducation, IEFRepository<EmployeeTraining, string> repoTraining, IEFRepository<EmployeeExperience, string> repoExperience, IEFRepository<BusinessUnitJobLevel, string> repoBusinessUnitJobLevel, IEFRepository<CandidateRecruitmentEducationInfo, string> repoCandidateRecruitmentEducationInfo, IEFRepository<CandidateRecruitmentEmploymentHistory, string> repoCandidateRecruitmentEmploymentHistory, IEFRepository<CandidateRecruitmentSignContract, string> repoCandidateRecruitmentSignContract, IEFRepository<EmployeeContract, string> repoEmployeeContract, IEFRepository<EmployeeChecklist, string> repoEmployeeChecklist, IEFRepository<CandidateRecruitment, string> repoCandidateRecruitment, IEFRepository<Institution, string> repoInstitution) : base(repo)
		{
			_repo = repo;
			_repoFamily = repoFamily;
			_repoEmergency = repoEmergency;
			_repoEducation = repoEducation;
			_repoTraining = repoTraining;
			_repoExperience = repoExperience;
			_repoBusinessUnitJobLevel = repoBusinessUnitJobLevel;
			_repoCandidateRecruitmentEducationInfo = repoCandidateRecruitmentEducationInfo;
			_repoCandidateRecruitmentEmploymentHistory = repoCandidateRecruitmentEmploymentHistory;
			_repoCandidateRecruitmentSignContract = repoCandidateRecruitmentSignContract;
			_repoEmployeeContract = repoEmployeeContract;
			_repoEmployeeChecklist = repoEmployeeChecklist;
			_repoCandidateRecruitment = repoCandidateRecruitment;
			_repoInstitution = repoInstitution;
	}

		private BusinessUnitGroup newbug(BusinessUnitGroup businessUnitGroup)
		{
			BusinessUnitGroup x = new BusinessUnitGroup();
			if (businessUnitGroup != null)
			{
				x = businessUnitGroup;
			}
			return x;
		}
		private BusinessUnitSubgroup newbugsub(BusinessUnitSubgroup businessUnitSubGroup)
		{
			BusinessUnitSubgroup x = new BusinessUnitSubgroup();
			if (businessUnitSubGroup != null)
			{
				x = businessUnitSubGroup;
			}
			return x;
		}
		private BusinessUnitDivision newbugDiv(BusinessUnitDivision businessUnitGroupDivision)
		{
			BusinessUnitDivision x = new BusinessUnitDivision();
			if (businessUnitGroupDivision != null)
			{
				x = businessUnitGroupDivision;
			}
			return x;
		}
		private BusinessUnitDepartement newbugDepart(BusinessUnitDepartement businessUnitDepartement)
		{
			BusinessUnitDepartement x = new BusinessUnitDepartement();
			if (businessUnitDepartement != null)
			{
				x = businessUnitDepartement;
			}
			return x;
		}

		private ParentBusinessUnit newBugParent(ParentBusinessUnit param)
		{
			ParentBusinessUnit x = new ParentBusinessUnit();
			if (param != null)
			{
				x = param;
			}
			return x;
		}

		public GeneralResponseList<EmployeeBasicInfoDto> ListAll(EmployeeBasicInfoSearch filter)
		{

			GeneralResponseList<EmployeeBasicInfoDto> resp = new GeneralResponseList<EmployeeBasicInfoDto>();
			try
			{
				PagingHelper q = new PagingHelper();
				var que = from tb1 in _repo.GetContext().Set<EmployeeBasicInfo>()
						  join tbk in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb1.business_unit_job_level_id equals tbk.Id
						  into bk1
						  from tbk1 in bk1.DefaultIfEmpty()
						  join tb2 in _repo.GetContext().Set<BusinessUnit>() on tbk1.business_unit_division_id equals tb2.Id
						  into bu
						  from tbu in bu.DefaultIfEmpty()
						  join tb4 in _repo.GetContext().Set<JobTitle>() on tbk1.job_title_id equals tb4.Id
						  into ttl
						  from tbttl in ttl.DefaultIfEmpty()
						  join tb5 in _repo.GetContext().Set<JobGrade>() on tbk1.job_grade_id equals tb5.Id
						  into grd
						  from tblgrd in grd.DefaultIfEmpty()
						  join tb6 in _repo.GetContext().Set<EmployementStatus>() on tb1.employement_status_id equals tb6.Id
						  into employee
						  from tbempoyee in employee.DefaultIfEmpty()
						  join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.Id equals tb7.employee_basic_info_id
						  into au1
						  from tau1 in au1.DefaultIfEmpty()
						  select new EmployeeBasicInfoDto()
						  {
							  Id = tb1.Id,
							  name_employee = tb1.name_employee,
							  division = tbu.unit_name,
							  job_title = tbttl.title_name,
							  grade = tblgrd.grade_name,
							  employee_status = tbempoyee.employment_status_name,
							  application_user_id = tau1.Id

						  };

				resp.RecordsTotal = que.Count();

				que = q.filterContains(que, "division", filter.Search_division);
				que = q.filterContains(que, "job_title", filter.Search_job_title);
				que = q.filterContains(que, "grade", filter.Search_grade);
				que = q.filterContains(que, "employee_status", filter.Search_employee_status);
				que = q.filterContains(que, "name_employee", filter.Search_name);

				resp.RecordsFiltered = que.Count();
				resp.Total = resp.RecordsTotal;

				que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
				que = q.limit(que, filter.GetLimit(), filter.GetOffset());

				resp.Rows = que.ToList();
				resp.Success = true;

			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message;
			}
			return resp;

		}

		public GeneralResponse<EmployeeBasicInfoDto> CreateWithReference(EmployeeBasicInfoNew dto)
		{
			GeneralResponse<EmployeeBasicInfoDto> respon = new GeneralResponse<EmployeeBasicInfoDto>();
			try
			{
				EmployeeBasicInfo newBasicInfo = new EmployeeBasicInfo();
				PropertyMapper.All(dto, newBasicInfo);
				_repo.Create(false, newBasicInfo);

				#region Save Family
				if (dto.listFamily != null || dto.listFamily.Count > 0)
				{
					foreach (FamilyNew data in dto.listFamily)
					{
						Family Famz = new Family();
						Famz.employee_basic_info_id = newBasicInfo.Id;
						Famz.family_status_id = data.family_status_id;
						Famz.married_status_id = data.married_status_id;
						Famz.institution_id = data.institution_id;
						Famz.religion_id = data.religion_id;
						Famz.description = data.description;
						Famz.phone_no = data.phone_no;
						Famz.other_no = data.other_no;
						Famz.birthday = DateTime.Parse(data.birthday, CultureInfo.InvariantCulture);
						Famz.address = data.address;
						Famz.gender = data.gender;
						Famz.name = data.name;
						_repoFamily.Create(false, Famz);
					}
				}
				#endregion

				#region Save Emergency Contact
				if (dto.listEmergency != null && dto.listEmergency.Count > 0)
				{
					foreach (EmployeeEmergencyContactNew item in dto.listEmergency)
					{
						EmployeeEmergencyContact ec = new EmployeeEmergencyContact();
						ec.employee_basic_info_id = newBasicInfo.Id;
						ec.address = item.address;
						ec.full_name = item.full_name;
						ec.other_no = item.other_no;
						ec.phone_no = item.phone_no;
						ec.family_status_id = item.family_status_id;
						_repoEmergency.Create(false, ec);
					}
				}
				#endregion

				#region Save Education
				if (dto.listEducation != null && dto.listEducation.Count > 0)
				{
					foreach (EducationNew edu in dto.listEducation)
					{
						Education e = new Education();
						e.address = edu.address;
						e.education_level_id = edu.education_level_id;
						e.employee_basic_info_id = newBasicInfo.Id;
						e.institution_id = edu.institution_id;
						e.majors = edu.majors;
						e.qualification = edu.qualification;
						e.start_education = edu.start_education;
						e.end_education = edu.end_education;
						_repoEducation.Create(false, e);
					}
				}
				#endregion

				#region Save Training
				if (dto.listTraining != null && dto.listTraining.Count > 0)
				{
					foreach (EmployeeTrainingNew t in dto.listTraining)
					{
						EmployeeTraining train = new EmployeeTraining();
						train.address = t.address;
						train.description = t.description;
						train.employee_basic_info_id = newBasicInfo.Id;
						train.start_training = t.start_training;
						train.end_training = t.end_training;
						train.expired_certificate = DateTime.Parse(t.expired_certificate, CultureInfo.InvariantCulture);
						train.filemaster_id = t.filemaster_id;
						train.institution_id = t.institution_id;
						train.name_training = t.name_training;
						train.no_certification = t.no_certification;
						_repoTraining.Create(false, train);
					}
				}
				#endregion

				#region Save Experience
				if (dto.listExperience != null && dto.listExperience.Count > 0)
				{
					foreach (EmployeeExperienceNew e in dto.listExperience)
					{
						EmployeeExperience exp = new EmployeeExperience();
						exp.address = e.address;
						exp.company_name = e.company_name;
						exp.employee_basic_info_id = newBasicInfo.Id;
						exp.phone = e.phone;
						exp.position = e.position;
						exp.remark = e.remark;
						exp.start_date = e.start_date;
						exp.end_date = e.end_date;

						_repoExperience.Create(false, exp);
					}
				}
				#endregion

				_repo.SaveChanges();
				_repoFamily.SaveChanges();
				_repoEmergency.SaveChanges();
				_repoEducation.SaveChanges();
				_repoTraining.SaveChanges();
				_repoExperience.SaveChanges();

				_repo.Dispose();
				_repoFamily.Dispose();
				_repoEmergency.Dispose();
				_repoEducation.Dispose();
				_repoTraining.Dispose();
				_repoExperience.Dispose();

				respon.Success = true;
				respon.Message = "Sucess";
			}
			catch (Exception)
			{
				throw;
			}
			return respon;
		}

		public GeneralResponse<EmployeeBasicInfo> DeleteWithReference(string id)
		{
			GeneralResponse<EmployeeBasicInfo> response = new GeneralResponse<EmployeeBasicInfo>();
			try
			{
				#region Delete Family
				IQueryable<FamilyDto> FamzQuery = (from tb1 in _repo.GetContext().Set<Family>()
												   where tb1.employee_basic_info_id == id
												   select new FamilyDto()
												   {
													   Id = tb1.Id,
													   employee_basic_info_id = tb1.employee_basic_info_id
												   });
				List<FamilyDto> ListFamily = FamzQuery.ToList();
				foreach (FamilyDto famz in ListFamily)
				{
					_repoFamily.RemoveByID(false, famz.Id);
				}
				#endregion

				#region Delete Employee Emergency Contact
				IQueryable<EmployeeEmergencyContactDto> EmergencyQuery = (from tb1 in _repo.GetContext().Set<EmployeeEmergencyContact>()
																		  where tb1.employee_basic_info_id == id
																		  select new EmployeeEmergencyContactDto()
																		  {
																			  Id = tb1.Id,
																			  employee_basic_info_id = tb1.employee_basic_info_id
																		  });
				List<EmployeeEmergencyContactDto> ListEmergency = EmergencyQuery.ToList();

				foreach (EmployeeEmergencyContactDto emergency in ListEmergency)
				{
					_repoEmergency.RemoveByID(false, emergency.Id);
				}
				#endregion

				#region Delete Education
				IQueryable<EducationDto> EducationQuery = (from tb1 in _repo.GetContext().Set<Education>()
														   where tb1.employee_basic_info_id == id
														   select new EducationDto()
														   {
															   Id = tb1.Id,
															   employee_basic_info_id = tb1.employee_basic_info_id
														   });
				List<EducationDto> ListEducation = EducationQuery.ToList();

				foreach (EducationDto edu in ListEducation)
				{
					_repoEducation.RemoveByID(false, edu.Id);
				}
				#endregion

				#region Delete Employee Training
				IQueryable<EmployeeTrainingDto> TrainingQuery = (from tb1 in _repo.GetContext().Set<EmployeeTraining>()
																 where tb1.employee_basic_info_id == id
																 select new EmployeeTrainingDto()
																 {
																	 Id = tb1.Id,
																	 employee_basic_info_id = tb1.employee_basic_info_id
																 });
				List<EmployeeTrainingDto> ListTraining = TrainingQuery.ToList();

				foreach (EmployeeEmergencyContactDto emergency in ListEmergency)
				{
					_repoEmergency.RemoveByID(false, emergency.Id);
				}
				#endregion

				#region Delete Experience
				IQueryable<EmployeeExperienceDto> ExperienceQuery = (from tb1 in _repo.GetContext().Set<EmployeeExperience>()
																	 where tb1.employee_basic_info_id == id
																	 select new EmployeeExperienceDto()
																	 {
																		 Id = tb1.Id,
																		 employee_basic_info_id = tb1.employee_basic_info_id
																	 });
				List<EmployeeExperienceDto> ListExperience = ExperienceQuery.ToList();

				foreach (EmployeeExperienceDto exp in ListExperience)
				{
					_repoExperience.RemoveByID(false, exp.Id);
				}
				#endregion

				_repo.RemoveByID(false, id);


				_repo.SaveChanges();
				_repoEducation.SaveChanges();
				_repoEmergency.SaveChanges();
				_repoExperience.SaveChanges();
				_repoFamily.SaveChanges();
				_repoTraining.SaveChanges();

				_repo.Dispose();
				_repoEducation.Dispose();
				_repoEmergency.Dispose();
				_repoExperience.Dispose();
				_repoFamily.Dispose();
				_repoTraining.Dispose();

				response.Success = true;
				response.Message = "Sucess";
				response.Code = "00";

			}
			catch (Exception x)
			{
				response.Success = false;
				response.Code = "500";
				response.Message = x.Message.ToString();
			}
			return response;
		}

		public GeneralResponse<EmployeeBasicInfoDto> CreateEmployeeFromCandidate(EmployeeBasicInfoNew dto)
		{
			GeneralResponse<EmployeeBasicInfoDto> resp = new GeneralResponse<EmployeeBasicInfoDto>();
			try
			{
				EmployeeBasicInfo newBasicInfo = new EmployeeBasicInfo();
				PropertyMapper.All(dto, newBasicInfo);
				newBasicInfo.business_unit_job_level_id = dto.business_unit_job_level_id;
				newBasicInfo.legal_entity_id = dto.legal_entity_id;
				newBasicInfo.name_employee = dto.first_name + " " + dto.last_name;
				newBasicInfo.location_id = dto.location_id;
				newBasicInfo.home_address = dto.address;
				newBasicInfo.address_identity_card = dto.address;
				newBasicInfo.join_date = dto.start_date;



				List<CandidateRecruitmentEducationInfoDto> ListcandidateEducationInfo = new List<CandidateRecruitmentEducationInfoDto>();
				List<CandidateRecruitmentEmploymentHistoryDto> Listcandidatemploymenthistory = new List<CandidateRecruitmentEmploymentHistoryDto>();


				IQueryable<CandidateRecruitmentEducationInfoDto> que = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEducationInfo>()
																	   join tb2 in _repo.GetContext().Set<EducationLevel>() on tb1.education_level_id equals tb2.Id
																	   join tb3 in _repo.GetContext().Set<Institution>() on tb1.institution_id equals tb3.Id
																	   where tb1.candidate_recruitment_id == dto.candicate_recruitment_id
																	   select new CandidateRecruitmentEducationInfoDto()
																	   {
																		   Id = tb1.Id,
																		   candidate_recruitment_id = tb1.candidate_recruitment_id,
																		   created_on = tb1.created_on,
																		   institution_id = tb1.institution_id,
																		   education_level_id = tb1.education_level_id,
																		   institution_name = tb3.institution_name,
																		   last_education = tb2.education_name,
																		   location = tb1.location,
																		   major = tb1.major,
																		   year_from = tb1.year_from,
																		   year_to = tb1.year_to
																	   };
				ListcandidateEducationInfo = que.ToList();

				IQueryable<CandidateRecruitmentEmploymentHistoryDto> que2 = from tb1 in _repo.GetContext().Set<CandidateRecruitmentEmploymentHistory>()
																			where tb1.candidate_recruitment_id == dto.candicate_recruitment_id
																			select new CandidateRecruitmentEmploymentHistoryDto()
																			{
																				Id = tb1.Id,
																				candidate_recruitment_id = tb1.candidate_recruitment_id,
																				created_on = tb1.created_on,
																				company_address = tb1.company_address,
																				company_name = tb1.company_name,
																				company_phone = tb1.company_phone,
																				main_responsibilities = tb1.main_responsibilities,
																				year_from = tb1.year_from,
																				year_to = tb1.year_to,
																				achievements = tb1.achievements,
																				position = tb1.position
																			};

				Listcandidatemploymenthistory = que2.ToList();

				IQueryable<CandidateRecruitmentSignContract> que3 = from tb1 in _repo.GetContext().Set<CandidateRecruitmentSignContract>()
																	where tb1.candidate_recruitment_id == dto.candicate_recruitment_id
																	select new CandidateRecruitmentSignContract()
																	{
																		Id = tb1.Id,
																		candidate_recruitment_id = tb1.candidate_recruitment_id,
																		created_on = tb1.created_on,
																		filemaster_id = tb1.filemaster_id,
																		start_date = tb1.start_date,
																		end_date = tb1.end_date,
																		contracttype = tb1.contracttype
																	};
				CandidateRecruitmentSignContract candidatesigncontract = que3.FirstOrDefault();

				if (candidatesigncontract != null)
				{
					EmployeeContract employeecontract = new EmployeeContract();
					employeecontract.candidate_recruitment_id = dto.candicate_recruitment_id;
					employeecontract.employee_basic_info_id = newBasicInfo.Id;
					employeecontract.filemaster_id = candidatesigncontract.filemaster_id;
					employeecontract.start_date = candidatesigncontract.start_date;
					employeecontract.end_date = candidatesigncontract.end_date;

					_repoEmployeeContract.Create(false, employeecontract);
					_repoCandidateRecruitmentSignContract.RemoveByID(false, candidatesigncontract.Id);
				}

				foreach (CandidateRecruitmentEmploymentHistoryDto candidateexp in Listcandidatemploymenthistory)
				{
					EmployeeExperience employeexp = new EmployeeExperience();
					employeexp.employee_basic_info_id = newBasicInfo.Id;
					employeexp.company_name = candidateexp.company_name;
					employeexp.address = candidateexp.company_address;
					employeexp.phone = candidateexp.company_phone;
					employeexp.position = candidateexp.position;
					employeexp.start_date = candidateexp.year_from;
					employeexp.end_date = candidateexp.year_to;
					employeexp.remark = candidateexp.main_responsibilities;
					_repoExperience.Create(false, employeexp);
				}

				foreach (CandidateRecruitmentEducationInfoDto candidatedu in ListcandidateEducationInfo)
				{
					Education employeedu = new Education();
					employeedu.employee_basic_info_id = newBasicInfo.Id;
					employeedu.address = candidatedu.location;
					employeedu.start_education = candidatedu.year_from;
					employeedu.end_education = candidatedu.year_to;
					employeedu.institution_id = candidatedu.institution_id;
					employeedu.education_level_id = candidatedu.education_level_id;
					employeedu.majors = candidatedu.major;
					employeedu.qualification = "";
					_repoEducation.Create(false, employeedu);
				}

				if (dto.listDataNewEmployeeChecklist != null)
				{
					var IsEmployeeChecklists = _repo.GetContext().Set<EmployeeChecklist>().Where(row => row.employee_basic_info_id == newBasicInfo.Id).FirstOrDefault();
					if (IsEmployeeChecklists != null)
					{
						var listCheckList = _repo.GetContext().Set<EmployeeChecklist>().Where(row => row.employee_basic_info_id == newBasicInfo.Id).ToList();
						listCheckList.ForEach(x => { x.is_deleted = true;  });
						_repo.GetContext().Set<EmployeeChecklist>().UpdateRange(listCheckList);
					}
					
					foreach (dataNewEmployeeChecklist dataNewEmployeeChecklist in dto.listDataNewEmployeeChecklist)
					{
						string[] arrayEmployeeChecklistCategoryId = dataNewEmployeeChecklist.checklist_category.Split(',');

						foreach (var item in arrayEmployeeChecklistCategoryId)
                        {
							EmployeeChecklist employeecheclist = new EmployeeChecklist();
							employeecheclist.employee_basic_info_id = newBasicInfo.Id;
							employeecheclist.employee_checklist_category_id = item;
							employeecheclist.checklistitem = dataNewEmployeeChecklist.checklistitem;
							employeecheclist.description = dataNewEmployeeChecklist.description;
							employeecheclist.whentoalert = dataNewEmployeeChecklist.whentoalert;
							employeecheclist.Id = Guid.NewGuid().ToString();
							_repoEmployeeChecklist.Create(false, employeecheclist);
						}
					}
				}

				CandidateRecruitment candidatereq = _repoCandidateRecruitment.FindByID(dto.candicate_recruitment_id);
				candidatereq.applicant_move_stage_id = "8";
				candidatereq.comment = dto.comment;
				var IdFileMasterCV = "";
				var IdFileMasterPhoto = "";
				var IdFileMasterPortfolio = "";

				if (!string.IsNullOrEmpty(dto.cv))
				{
					string[] dataCv = dto.cv.Split(new string[] { "::" }, StringSplitOptions.None);
					IdFileMasterCV = dataCv[0];
				}

				if (!string.IsNullOrEmpty(dto.photo))
				{
					string[] dataPhoto = dto.photo.Split(new string[] { "::" }, StringSplitOptions.None);
					IdFileMasterPhoto = dataPhoto[0];
				}

				if (!string.IsNullOrEmpty(dto.portfolio))
				{
					string[] dataPortfolio = dto.portfolio.Split(new string[] { "::" }, StringSplitOptions.None);
					IdFileMasterPortfolio = dataPortfolio[0];
				}

				
				
				candidatereq.cv_file_master_id = IdFileMasterCV;
				candidatereq.photo_file_master_id = IdFileMasterPhoto;
				candidatereq.portofolio_file_master_id = IdFileMasterPortfolio;
				_repoCandidateRecruitment.Update(false, candidatereq);
				var isNewEmployeeExist = _repo.GetContext().Set<EmployeeBasicInfo>().Where(x => x.Id == newBasicInfo.Id).FirstOrDefault();
				
				if (isNewEmployeeExist == null)
				{
					_repo.Create(false, newBasicInfo);
				}
			
				
				_repoCandidateRecruitment.SaveChanges();
				_repo.SaveChanges();
				_repoEmployeeContract.SaveChanges();
				_repoCandidateRecruitmentSignContract.SaveChanges();
				_repoExperience.SaveChanges();
				_repoEducation.SaveChanges();
				_repoEmployeeChecklist.SaveChanges();
				_repoCandidateRecruitment.Dispose();
				_repo.Dispose();
				_repoEmployeeContract.Dispose();
				_repoCandidateRecruitmentSignContract.Dispose();
				_repoExperience.Dispose();
				_repoEducation.Dispose();
				_repoEmployeeChecklist.Dispose();

				resp.Success = true;
				resp.Message = "Sucess";
				resp.Code = "00";
			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message.ToString();
			}
			return resp;
		}

		public GeneralResponse<EmployeeBasicInfoNew> CreateNewEmployeeBasic(EmployeeBasicInfoNew dto)
		{
			GeneralResponse<EmployeeBasicInfoNew> resp = new GeneralResponse<EmployeeBasicInfoNew>();

			try
			{

				EmployeeBasicInfo newBasicInfo = new EmployeeBasicInfo();
				//PropertyMapper.All(dto, newBasicInfo);
				newBasicInfo.first_name = dto.first_name;
				newBasicInfo.last_name = dto.last_name;
				newBasicInfo.nick_name = dto.nick_name;
				newBasicInfo.birth_place = dto.birth_place;
				newBasicInfo.gender = dto.gender;
				newBasicInfo.date_birth = dto.date_birth;  
				newBasicInfo.phone_number = dto.phone_number;
				newBasicInfo.email = dto.email;
				newBasicInfo.mobile_number = dto.mobile_number;
				newBasicInfo.name_employee = dto.first_name + " " + dto.last_name;
				newBasicInfo.nationality_id = dto.nationality_id;
				newBasicInfo.last_education = dto.last_education;
				newBasicInfo.kitas_number = dto.kitas_number;
				newBasicInfo.passport_number = dto.passport_number;
				if (dto.kitas_expiry != null)
				{
					newBasicInfo.kitas_expiry = DateTime.Parse(dto.kitas_expiry, CultureInfo.InvariantCulture);
				}

				if (dto.passport_expiry != null)
				{
					newBasicInfo.passport_expiry = DateTime.Parse(dto.passport_expiry, CultureInfo.InvariantCulture); 
				}
				
				newBasicInfo.religion_id = dto.religion_id;
				newBasicInfo.npwp_number = dto.npwp_number;
				newBasicInfo.married_status_id = dto.married_status_id;
				newBasicInfo.npwp_expiry = dto.npwp_expiry;
				newBasicInfo.blood = dto.blood;
				newBasicInfo.identity_card = dto.identity_card;
				newBasicInfo.kk_no = dto.kk_no;
				newBasicInfo.post_code = dto.post_code;
				newBasicInfo.address_identity_card = dto.address_identity_card;
				newBasicInfo.home_address = dto.home_address;
				newBasicInfo.photo = dto.photo;

				//tab jobrelated
				newBasicInfo.business_unit_job_level_id = dto.business_unit_job_level_id;
				newBasicInfo.legal_entity_id = dto.legal_entity_id;
				newBasicInfo.location_id = dto.location_id;
				if (dto.join_date != null)
				{
					newBasicInfo.join_date = DateTime.Parse(dto.join_date, CultureInfo.InvariantCulture); 
				}
				
				newBasicInfo.employement_status_id = dto.employement_status_id;

				newBasicInfo.ptkp_setting_id = dto.ptkp_setting_id;
				newBasicInfo.payroll_methods = dto.payroll_methods;
				newBasicInfo.bpjs_healthy = dto.bpjs_healthy;
				newBasicInfo.account_no = dto.account_no;
				newBasicInfo.account_name = dto.account_name;
				newBasicInfo.bank_city = dto.bank_city;
				newBasicInfo.bank_address = dto.bank_address;
				newBasicInfo.bpjs_tk = dto.bpjs_tk;
				newBasicInfo.bank_id = dto.bank_id;
				newBasicInfo.bank_branch = dto.bank_branch;

				_repo.Create(false, newBasicInfo);


				if (dto.dataListEmployeBasicFamily != null)
				{
					foreach (FamilyNew employeBasicFamily in dto.dataListEmployeBasicFamily)
					{
						Family newEmployeeBasicFamily = new Family();
						newEmployeeBasicFamily.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeBasicFamily.family_status_id = employeBasicFamily.family_status_id;
						newEmployeeBasicFamily.name = employeBasicFamily.name;
						newEmployeeBasicFamily.phone_no = employeBasicFamily.phone_no;
						newEmployeeBasicFamily.other_no = employeBasicFamily.other_no;
						newEmployeeBasicFamily.address = employeBasicFamily.address;
						newEmployeeBasicFamily.place_birth = employeBasicFamily.place_birth;
						if (employeBasicFamily.birthday != null)
						{
							newEmployeeBasicFamily.birthday = DateTime.Parse(employeBasicFamily.birthday, CultureInfo.InvariantCulture); 
						}
						
						newEmployeeBasicFamily.gender = employeBasicFamily.gender;
						newEmployeeBasicFamily.religion_id = employeBasicFamily.religion_id;
						newEmployeeBasicFamily.education_level_id = employeBasicFamily.education_level_id;
						newEmployeeBasicFamily.institution_id = employeBasicFamily.institution_id;
						newEmployeeBasicFamily.description = employeBasicFamily.description;
						newEmployeeBasicFamily.married_status_id = employeBasicFamily.married_status_id;

						if (!string.IsNullOrEmpty(employeBasicFamily.family_education_institusion_id_other))
						{
							Institution newDataInstitution = new Institution();
							newDataInstitution.institution_name = employeBasicFamily.family_education_institusion_id_other;
							newDataInstitution.institution_address = "not defined";
							newDataInstitution.institution_type = "1";
							newDataInstitution.is_active = true;
							var newIdInstitution = _repoInstitution.Create(newDataInstitution);

							newEmployeeBasicFamily.institution_id = newIdInstitution.Id;
						}

						_repoFamily.Create(false, newEmployeeBasicFamily);
					}
				}

				if (dto.dataListEmployeBasicEmegencyContact != null)
				{
					foreach (EmployeeEmergencyContactNew employeBasicEmegencyContact in dto.dataListEmployeBasicEmegencyContact)
					{
						EmployeeEmergencyContact newEmployeeEmployeeEmergencyContact = new EmployeeEmergencyContact();
						newEmployeeEmployeeEmergencyContact.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeEmployeeEmergencyContact.family_status_id = employeBasicEmegencyContact.family_status_id;
						newEmployeeEmployeeEmergencyContact.full_name = employeBasicEmegencyContact.full_name;
						newEmployeeEmployeeEmergencyContact.phone_no = employeBasicEmegencyContact.phone_no;
						newEmployeeEmployeeEmergencyContact.other_no = employeBasicEmegencyContact.other_no;
						newEmployeeEmployeeEmergencyContact.address = employeBasicEmegencyContact.address;

						_repoEmergency.Create(false, newEmployeeEmployeeEmergencyContact);
					}
				}

				if (dto.dataListEmployeBasicEducationInfo != null)
				{
					foreach (EducationNew employeBasicEducationInfo in dto.dataListEmployeBasicEducationInfo)
					{
						Education newEmployeeEducation = new Education();
						newEmployeeEducation.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeEducation.education_level_id = employeBasicEducationInfo.education_level_id;
						newEmployeeEducation.institution_id = employeBasicEducationInfo.institution_id;
						newEmployeeEducation.majors = employeBasicEducationInfo.majors;
						newEmployeeEducation.start_education = employeBasicEducationInfo.start_education;
						newEmployeeEducation.end_education = employeBasicEducationInfo.end_education;
						newEmployeeEducation.qualification = employeBasicEducationInfo.qualification;
						newEmployeeEducation.address = employeBasicEducationInfo.address;

						if (!string.IsNullOrEmpty(employeBasicEducationInfo.education_institusion_id_other))
						{
							Institution newDataInstitution = new Institution();
							newDataInstitution.institution_name = employeBasicEducationInfo.education_institusion_id_other;
							newDataInstitution.institution_address = "not defined";
							newDataInstitution.institution_type = "1";
							newDataInstitution.is_active = true;
							var newIdInstitution = _repoInstitution.Create(newDataInstitution);

							newEmployeeEducation.institution_id = newIdInstitution.Id;
						}

						_repoEducation.Create(false, newEmployeeEducation);
					}
				}

				if (dto.dataListEmployeBasicEmploymentHistory != null)
				{
					foreach (EmployeeExperienceNew employeBasicEmploymentHistory in dto.dataListEmployeBasicEmploymentHistory)
					{
						EmployeeExperience newEmployeeExperience = new EmployeeExperience();
						newEmployeeExperience.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeExperience.company_name = employeBasicEmploymentHistory.company_name;
						newEmployeeExperience.phone = employeBasicEmploymentHistory.phone;
						newEmployeeExperience.remark = employeBasicEmploymentHistory.remark;
						newEmployeeExperience.address = employeBasicEmploymentHistory.address;
						newEmployeeExperience.start_date = employeBasicEmploymentHistory.start_date;
						newEmployeeExperience.end_date = employeBasicEmploymentHistory.end_date;
						newEmployeeExperience.position = employeBasicEmploymentHistory.position;

						_repoExperience.Create(false, newEmployeeExperience);
					}
				}

				if (dto.dataListEmployeBasicCourseSeminar != null)
				{
					foreach (EmployeeTrainingNew employeBasicCourseSeminar in dto.dataListEmployeBasicCourseSeminar)
					{
						EmployeeTraining newEmployeeTraining = new EmployeeTraining();
						newEmployeeTraining.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeTraining.name_training = employeBasicCourseSeminar.name_training;
						newEmployeeTraining.institution_id = employeBasicCourseSeminar.institution_id;
						newEmployeeTraining.address = employeBasicCourseSeminar.address;
						newEmployeeTraining.start_training = employeBasicCourseSeminar.start_training;
						newEmployeeTraining.end_training = employeBasicCourseSeminar.end_training;
						newEmployeeTraining.no_certification = employeBasicCourseSeminar.no_certification;
						if (employeBasicCourseSeminar.expired_certificate != null)
						{
							newEmployeeTraining.expired_certificate = DateTime.Parse(employeBasicCourseSeminar.expired_certificate, CultureInfo.InvariantCulture);  
						}
						
						newEmployeeTraining.description = employeBasicCourseSeminar.description;
						newEmployeeTraining.filemaster_id = employeBasicCourseSeminar.filemaster_id;

						if (!string.IsNullOrEmpty(employeBasicCourseSeminar.course_institusion_id_other))
						{
							Institution newDataInstitution = new Institution();
							newDataInstitution.institution_name = employeBasicCourseSeminar.course_institusion_id_other;
							newDataInstitution.institution_address = "not defined";
							newDataInstitution.institution_type = "2";
							newDataInstitution.is_active = true;
							var newIdInstitution = _repoInstitution.Create(newDataInstitution);

							newEmployeeTraining.institution_id = newIdInstitution.Id;
						}

						_repoTraining.Create(false, newEmployeeTraining);
					}
				}

				_repo.SaveChanges();
				if (dto.dataListEmployeBasicFamily != null)
				{
					_repoFamily.SaveChanges();
				}

				if (dto.dataListEmployeBasicEducationInfo != null)
				{
					_repoEducation.SaveChanges();
				}
				if (dto.dataListEmployeBasicEmploymentHistory != null)
				{
					_repoExperience.SaveChanges();
				}
				if (dto.dataListEmployeBasicCourseSeminar != null)
				{
					_repoTraining.SaveChanges();
				}
				if (dto.dataListEmployeBasicEmegencyContact != null)
				{
					_repoEmergency.SaveChanges();
				}

				_repo.Dispose();
				if (dto.dataListEmployeBasicFamily != null)
				{
					_repoFamily.Dispose();
				}
				if (dto.dataListEmployeBasicEducationInfo != null)
				{
					_repoEducation.Dispose();
				}
				if (dto.dataListEmployeBasicEmploymentHistory != null)
				{
					_repoExperience.Dispose();
				}
				if (dto.dataListEmployeBasicCourseSeminar != null)
				{
					_repoTraining.Dispose();
				}
				if (dto.dataListEmployeBasicEmegencyContact != null)
				{
					_repoEmergency.Dispose();
				}

				resp.Success = true;
				resp.Message = "Sucess";
				resp.Code = "00";
			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message.ToString();
			}
			return resp;
		}

		public GeneralResponse<EmployeeBasicInfoDto> GetWithDetail(string id)
		{
			GeneralResponse<EmployeeBasicInfoDto> resp = new GeneralResponse<EmployeeBasicInfoDto>();
			try
			{
				var que = from tb1 in _repo.GetContext().Set<EmployeeBasicInfo>()
						  join tbn in _repo.GetContext().Set<Nationality>() on tb1.nationality_id equals tbn.Id
						  into bk1
						  from tbk1 in bk1.DefaultIfEmpty()
						  join tble in _repo.GetContext().Set<EducationLevel>() on tb1.last_education equals tble.Id
						  into bk2
						  from tbk2 in bk2.DefaultIfEmpty()
						  join tbr in _repo.GetContext().Set<Religion>() on tb1.religion_id equals tbr.Id
						  into bk3
						  from tbk3 in bk3.DefaultIfEmpty()
						  join tbms in _repo.GetContext().Set<MarriedStatus>() on tb1.married_status_id equals tbms.Id
						  into bk4
						  from tbk4 in bk4.DefaultIfEmpty()
						  join tbes in _repo.GetContext().Set<EmployementStatus>() on tb1.employement_status_id equals tbes.Id
						  into bk5
						  from tbk5 in bk5.DefaultIfEmpty()
						  join tblei in _repo.GetContext().Set<LegalEntity>() on tb1.legal_entity_id equals tblei.Id
						  into bk6
						  from tbk6 in bk6.DefaultIfEmpty()
						  join tblo in _repo.GetContext().Set<MasterLocation>() on tb1.location_id equals tblo.Id
						  into bk7
						  from tbk7 in bk7.DefaultIfEmpty()
						  join tbl8 in _repo.GetContext().Set<PTKPSetting>() on tb1.ptkp_setting_id equals tbl8.Id
						  into bk8
						  from tbk8 in bk8.DefaultIfEmpty()
						  where tb1.Id == id
						  select new EmployeeBasicInfoDto()
						  {
							  Id = tb1.Id,
							  name_employee = tb1.name_employee,
							  first_name = tb1.first_name,
							  last_name = tb1.last_name,
							  nick_name = tb1.nick_name,
							  birth_place = tb1.birth_place,
							  gender = tb1.gender,
							  date_birth = tb1.date_birth,
							  phone_number = tb1.phone_number,
							  email = tb1.email,
							  mobile_number = tb1.mobile_number,
							  nationality_id = tb1.nationality_id,
							  nationality_name = tbk1.nationality_name,
							  last_education = tb1.last_education,
							  last_education_name = tbk2.education_name,
							  kitas_number = tb1.kitas_number,
							  passport_number = tb1.passport_number,
							  kitas_expiry = tb1.kitas_expiry.ToString(),
							  passport_expiry = tb1.passport_expiry.ToString(),
							  religion_id = tb1.religion_id,
							  religion_name = tbk3.religion_name,
							  npwp_number = tb1.npwp_number,
							  married_status_id = tb1.married_status_id,
							  married_status_name = tbk4.married_name,
							  npwp_expiry = tb1.npwp_expiry,
							  blood = tb1.blood,
							  identity_card = tb1.identity_card,
							  kk_no = tb1.kk_no,
							  post_code = tb1.post_code,
							  address_identity_card = tb1.address_identity_card,
							  home_address = tb1.home_address,
							  photo = tb1.photo,
							  business_unit_job_level_id = tb1.business_unit_job_level_id,
							  legal_entity_id = tb1.legal_entity_id,
							  legal_entity_name = tbk6.legal_entity_name,
							  location_id = tb1.location_id,
							  location_name = tbk7.location_name,
							  join_date = tb1.join_date.ToString(),
							  employement_status_id = tb1.employement_status_id,
							  employement_status_name = tbk5.employment_status_name,
							  ptkp_setting_id = tb1.ptkp_setting_id,
							  payroll_methods = tb1.payroll_methods,
							  bpjs_healthy = tb1.bpjs_healthy,
							  account_no = tb1.account_no,
							  account_name = tb1.account_name,
							  bank_city = tb1.bank_city,
							  bank_address = tb1.bank_address,
							  bpjs_tk = tb1.bpjs_tk,
							  bank_id = tb1.bank_id,
							  bank_branch = tb1.bank_branch,
							  ptkp_setting_name = tbk8.name,
							  listFamily = (from tb2 in _repo.GetContext().Set<Family>()
											join tbfm in _repo.GetContext().Set<FamilyStatus>() on tb2.family_status_id equals tbfm.Id
											into bktbfm
											from tbktbfm in bktbfm.DefaultIfEmpty()
											where tb2.employee_basic_info_id == id
											select new FamilyNew()
											{
												Id = tb2.Id,
												address = tb2.address,
												birthday = tb2.birthday.ToString(),
												description = tb2.description,
												education_level_id = tb2.education_level_id,
												employee_basic_info_id = tb2.employee_basic_info_id,
												family_status_id = tb2.family_status_id,
												family_status_name = tbktbfm.family_status_name,
												gender = tb2.gender,
												institution_id = tb2.institution_id,
												married_status_id = tb2.married_status_id,
												name = tb2.name,
												other_no = tb2.other_no,
												phone_no = tb2.phone_no,
												place_birth = tb2.place_birth,
												religion_id = tb2.religion_id
											}).ToList(),
							  listEducation = (from tb3 in _repo.GetContext().Set<Education>()
											   join tba in _repo.GetContext().Set<EducationLevel>() on tb3.education_level_id equals tba.Id
											   join tbc1 in _repo.GetContext().Set<Institution>() on tb3.institution_id equals tbc1.Id
											   where tb3.employee_basic_info_id == id
											   select new EducationNew()
											   {
												   Id = tb3.Id,
												   employee_basic_info_id = tb3.employee_basic_info_id,
												   address = tb3.address,
												   education_level_id = tb3.education_level_id,
												   institution_id = tb3.institution_id,
												   end_education = tb3.end_education,
												   majors = tb3.majors,
												   qualification = tb3.qualification,
												   start_education = tb3.start_education,
												   education_level_name = tba.education_name,
												   institution_name = tbc1.institution_name

											   }).ToList(),
							  listEmergency = (from tb4 in _repo.GetContext().Set<EmployeeEmergencyContact>()
											   join tbb in _repo.GetContext().Set<FamilyStatus>() on tb4.family_status_id equals tbb.Id
											   where tb4.employee_basic_info_id == id
											   select new EmployeeEmergencyContactNew()
											   {
												   Id = tb4.Id,
												   address = tb4.address,
												   employee_basic_info_id = tb4.employee_basic_info_id,
												   family_status_id = tb4.family_status_id,
												   full_name = tb4.full_name,
												   other_no = tb4.other_no,
												   phone_no = tb4.phone_no,
												   relation_name = tbb.family_status_name
											   }).ToList(),
							  listExperience = (from tb5 in _repo.GetContext().Set<EmployeeExperience>()
												where tb5.employee_basic_info_id == id
												select new EmployeeExperienceNew()
												{
													Id = tb5.Id,
													address = tb5.address,
													company_name = tb5.company_name,
													employee_basic_info_id = tb5.employee_basic_info_id,
													end_date = tb5.end_date,
													phone = tb5.phone,
													position = tb5.position,
													remark = tb5.remark,
													start_date = tb5.start_date
												}).ToList(),
							  listTraining = (from tb6 in _repo.GetContext().Set<EmployeeTraining>()
											  join tbc in _repo.GetContext().Set<Institution>() on tb6.institution_id equals tbc.Id
											  where tb6.employee_basic_info_id == id
											  select new EmployeeTrainingNew()
											  {
												  Id = tb6.Id,
												  address = tb6.address,
												  description = tb6.description,
												  employee_basic_info_id = tb6.employee_basic_info_id,
												  end_training = tb6.end_training,
												  expired_certificate = tb6.expired_certificate.ToString(),
												  filemaster_id = tb6.filemaster_id,
												  institution_id = tb6.institution_id,
												  name_training = tb6.name_training,
												  no_certification = tb6.no_certification,
												  start_training = tb6.start_training,
												  institution_name = tbc.institution_name
											  }).ToList(),
							  bussinesUnitJobLevel = (from tb7 in _repo.GetContext().Set<BusinessUnitJobLevel>()
													  join tgrade in _repo.GetContext().Set<JobGrade>() on tb7.job_grade_id equals tgrade.Id
													  into bkgrade
													  from tbkgrade in bkgrade.DefaultIfEmpty()
													  join tlevel in _repo.GetContext().Set<JobLevel>() on tb7.job_level_id equals tlevel.Id
													  into bklevel
													  from tbklevel in bklevel.DefaultIfEmpty()
													  join ttitle in _repo.GetContext().Set<JobTitle>() on tb7.job_title_id equals ttitle.Id
													  into bktitle
													  from tbktitle in bktitle.DefaultIfEmpty()
													  where tb1.business_unit_job_level_id == tb7.Id
													  select new BussinesUnitJobLevelDto()
													  {
														  Id = tb7.Id,
														  job_grade_id = tb7.job_grade_id,
														  job_grade_name = tbkgrade.grade_name,
														  job_level_id = tb7.job_level_id,
														  job_level_name = tbklevel.job_name,
														  job_title_id = tb7.job_title_id,
														  job_title_name = tbktitle.title_name,
														  parent_id = tb7.parent_id,
														  business_unit_group_id = tb7.business_unit_group_id,
														  business_unit_subgroup_id = tb7.business_unit_subgroup_id,
														  business_unit_departement_id = tb7.business_unit_departement_id,
														  business_unit_division_id = tb7.business_unit_division_id,
														  BusinessUnitGroup = newbug((from tb9 in _repo.GetContext().Set<BusinessUnit>()
																					  where tb7.business_unit_group_id == tb9.Id
																					  select new BusinessUnitGroup
																					  {
																						  id = tb9.Id,
																						  name = tb9.unit_name

																					  }).FirstOrDefault()),
														  BusinessUnitSubgroup = newbugsub((from tb10 in _repo.GetContext().Set<BusinessUnit>()
																							where tb7.business_unit_subgroup_id == tb10.Id
																							select new BusinessUnitSubgroup
																							{
																								id = tb10.Id,
																								name = tb10.unit_name

																							}).FirstOrDefault()),
														  BusinessUnitDivision = newbugDiv((from tb11 in _repo.GetContext().Set<BusinessUnit>()
																							where tb7.business_unit_division_id == tb11.Id
																							select new BusinessUnitDivision
																							{
																								id = tb11.Id,
																								name = tb11.unit_name

																							}).FirstOrDefault()),
														  BusinessUnitDepartement = newbugDepart((from tb12 in _repo.GetContext().Set<BusinessUnit>()
																								  where tb7.business_unit_departement_id == tb12.Id
																								  select new BusinessUnitDepartement
																								  {
																									  id = tb12.Id,
																									  name = tb12.unit_name

																								  }).FirstOrDefault()),
														  ParentBusinessUnit = newBugParent((from tcc in _repo.GetContext().Set<BusinessUnitJobLevel>()
																							 join tc1 in _repo.GetContext().Set<JobTitle>() on tcc.job_title_id equals tc1.Id
																							 where tb7.parent_id == tcc.Id
																							 select new ParentBusinessUnit()
																							 {
																								 id = tcc.Id,
																								 name = tc1.title_name
																							 }).FirstOrDefault())
													  }).FirstOrDefault()
						  };

				resp.Data = que.FirstOrDefault();
				resp.Success = true;
				resp.Message = "Sucess";
				resp.Code = "00";

			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message.ToString();
			}
			return resp;
		}

		public GeneralResponse<EmployeeBasicInfoDto> GetWithEmployeeId(string id)
		{
			GeneralResponse<EmployeeBasicInfoDto> resp = new GeneralResponse<EmployeeBasicInfoDto>();
			try
			{
				var que = from tb1 in _repo.GetContext().Set<EmployeeBasicInfo>()
						  join tbn in _repo.GetContext().Set<Nationality>() on tb1.nationality_id equals tbn.Id
						  into bk1
						  from tbk1 in bk1.DefaultIfEmpty()
						  join tble in _repo.GetContext().Set<EducationLevel>() on tb1.last_education equals tble.Id
						  into bk2
						  from tbk2 in bk2.DefaultIfEmpty()
						  join tbr in _repo.GetContext().Set<Religion>() on tb1.religion_id equals tbr.Id
						  into bk3
						  from tbk3 in bk3.DefaultIfEmpty()
						  join tbms in _repo.GetContext().Set<MarriedStatus>() on tb1.married_status_id equals tbms.Id
						  into bk4
						  from tbk4 in bk4.DefaultIfEmpty()
						  join tbes in _repo.GetContext().Set<EmployementStatus>() on tb1.employement_status_id equals tbes.Id
						  into bk5
						  from tbk5 in bk5.DefaultIfEmpty()
						  join tblei in _repo.GetContext().Set<LegalEntity>() on tb1.legal_entity_id equals tblei.Id
						  into bk6
						  from tbk6 in bk6.DefaultIfEmpty()
						  join tblo in _repo.GetContext().Set<MasterLocation>() on tb1.location_id equals tblo.Id
						  into bk7
						  from tbk7 in bk7.DefaultIfEmpty()
						  where tb1.Id == id
						  select new EmployeeBasicInfoDto()
						  {
							  Id = tb1.Id,
							  name_employee = tb1.name_employee,
							  first_name = tb1.first_name,
							  last_name = tb1.last_name,
							  nick_name = tb1.nick_name,
							  birth_place = tb1.birth_place,
							  gender = tb1.gender,
							  date_birth = tb1.date_birth,
							  phone_number = tb1.phone_number,
							  email = tb1.email,
							  mobile_number = tb1.mobile_number,
							  nationality_id = tb1.nationality_id,
							  nationality_name = tbk1.nationality_name,
							  last_education = tb1.last_education,
							  last_education_name = tbk2.education_name,
							  kitas_number = tb1.kitas_number,
							  passport_number = tb1.passport_number,
							  kitas_expiry = tb1.kitas_expiry.ToString(),
							  passport_expiry = tb1.passport_expiry.ToString(),
							  religion_id = tb1.religion_id,
							  religion_name = tbk3.religion_name,
							  npwp_number = tb1.npwp_number,
							  married_status_id = tb1.married_status_id,
							  married_status_name = tbk4.married_name,
							  npwp_expiry = tb1.npwp_expiry,
							  blood = tb1.blood,
							  identity_card = tb1.identity_card,
							  kk_no = tb1.kk_no,
							  post_code = tb1.post_code,
							  address_identity_card = tb1.address_identity_card,
							  home_address = tb1.home_address,
							  photo = tb1.photo,
							  business_unit_job_level_id = tb1.business_unit_job_level_id,
							  legal_entity_id = tb1.legal_entity_id,
							  legal_entity_name = tbk6.legal_entity_name,
							  location_id = tb1.location_id,
							  location_name = tbk7.location_name,
							  join_date = tb1.join_date.ToString(),
							  employement_status_id = tb1.employement_status_id,
							  employement_status_name = tbk5.employment_status_name,
							  listFamily = (from tb2 in _repo.GetContext().Set<Family>()
											join tbfm in _repo.GetContext().Set<FamilyStatus>() on tb2.family_status_id equals tbfm.Id
											into bktbfm
											from tbktbfm in bktbfm.DefaultIfEmpty()
											where tb2.employee_basic_info_id == id && tb2.is_deleted != true
											select new FamilyNew()
											{
												Id = tb2.Id,
												address = tb2.address,
												birthday = tb2.birthday.ToString(),
												description = tb2.description,
												education_level_id = tb2.education_level_id,
												employee_basic_info_id = tb2.employee_basic_info_id,
												family_status_id = tb2.family_status_id,
												family_status_name = tbktbfm.family_status_name,
												gender = tb2.gender,
												institution_id = tb2.institution_id,
												married_status_id = tb2.married_status_id,
												name = tb2.name,
												other_no = tb2.other_no,
												phone_no = tb2.phone_no,
												place_birth = tb2.place_birth,
												religion_id = tb2.religion_id
											}).ToList(),
							  listEducation = (from tb3 in _repo.GetContext().Set<Education>()
											   join tba in _repo.GetContext().Set<EducationLevel>() on tb3.education_level_id equals tba.Id
											   join tbc1 in _repo.GetContext().Set<Institution>() on tb3.institution_id equals tbc1.Id
											   where tb3.employee_basic_info_id == id && tb3.is_deleted != true
											   select new EducationNew()
											   {
												   Id = tb3.Id,
												   employee_basic_info_id = tb3.employee_basic_info_id,
												   address = tb3.address,
												   education_level_id = tb3.education_level_id,
												   institution_id = tb3.institution_id,
												   end_education = tb3.end_education,
												   majors = tb3.majors,
												   qualification = tb3.qualification,
												   start_education = tb3.start_education,
												   education_level_name = tba.education_name,
												   institution_name = tbc1.institution_name

											   }).ToList(),
							  listEmergency = (from tb4 in _repo.GetContext().Set<EmployeeEmergencyContact>()
											   join tbb in _repo.GetContext().Set<FamilyStatus>() on tb4.family_status_id equals tbb.Id
											   where tb4.employee_basic_info_id == id && tb4.is_deleted != true
											   select new EmployeeEmergencyContactNew()
											   {
												   Id = tb4.Id,
												   address = tb4.address,
												   employee_basic_info_id = tb4.employee_basic_info_id,
												   family_status_id = tb4.family_status_id,
												   full_name = tb4.full_name,
												   other_no = tb4.other_no,
												   phone_no = tb4.phone_no,
												   relation_name = tbb.family_status_name
											   }).ToList(),
							  listExperience = (from tb5 in _repo.GetContext().Set<EmployeeExperience>()
												where tb5.employee_basic_info_id == id && tb5.is_deleted != true
												select new EmployeeExperienceNew()
												{
													Id = tb5.Id,
													address = tb5.address,
													company_name = tb5.company_name,
													employee_basic_info_id = tb5.employee_basic_info_id,
													end_date = tb5.end_date,
													phone = tb5.phone,
													position = tb5.position,
													remark = tb5.remark,
													start_date = tb5.start_date
												}).ToList(),
							  listTraining = (from tb6 in _repo.GetContext().Set<EmployeeTraining>()
											  join tbc in _repo.GetContext().Set<Institution>() on tb6.institution_id equals tbc.Id
											  where tb6.employee_basic_info_id == id && tb6.is_deleted != true
											  select new EmployeeTrainingNew()
											  {
												  Id = tb6.Id,
												  address = tb6.address,
												  description = tb6.description,
												  employee_basic_info_id = tb6.employee_basic_info_id,
												  end_training = tb6.end_training,
												  expired_certificate = tb6.expired_certificate.ToString(),
												  filemaster_id = tb6.filemaster_id,
												  institution_id = tb6.institution_id,
												  name_training = tb6.name_training,
												  no_certification = tb6.no_certification,
												  start_training = tb6.start_training,
												  institution_name = tbc.institution_name
											  }).ToList(),
							  bussinesUnitJobLevel = (from tb7 in _repo.GetContext().Set<BusinessUnitJobLevel>()
													  join tgrade in _repo.GetContext().Set<JobGrade>() on tb7.job_grade_id equals tgrade.Id
													  into bkgrade
													  from tbkgrade in bkgrade.DefaultIfEmpty()
													  join tlevel in _repo.GetContext().Set<JobLevel>() on tb7.job_level_id equals tlevel.Id
													  into bklevel
													  from tbklevel in bklevel.DefaultIfEmpty()
													  join ttitle in _repo.GetContext().Set<JobTitle>() on tb7.job_title_id equals ttitle.Id
													  into bktitle
													  from tbktitle in bktitle.DefaultIfEmpty()
													  where tb1.business_unit_job_level_id == tb7.Id && tb7.is_deleted != true
													  select new BussinesUnitJobLevelDto()
													  {
														  Id = tb7.Id,
														  job_grade_id = tb7.job_grade_id,
														  job_grade_name = tbkgrade.grade_name,
														  job_level_id = tb7.job_level_id,
														  job_level_name = tbklevel.job_name,
														  job_title_id = tb7.job_title_id,
														  job_title_name = tbktitle.title_name,
														  parent_id = tb7.parent_id,
														  business_unit_group_id = tb7.business_unit_group_id,
														  business_unit_subgroup_id = tb7.business_unit_subgroup_id,
														  business_unit_departement_id = tb7.business_unit_departement_id,
														  business_unit_division_id = tb7.business_unit_division_id,
														  BusinessUnitGroup = newbug((from tb9 in _repo.GetContext().Set<BusinessUnit>()
																					  where tb7.business_unit_group_id == tb9.Id
																					  select new BusinessUnitGroup
																					  {
																						  id = tb9.Id,
																						  name = tb9.unit_name

																					  }).FirstOrDefault()),
														  BusinessUnitSubgroup = newbugsub((from tb10 in _repo.GetContext().Set<BusinessUnit>()
																							where tb7.business_unit_subgroup_id == tb10.Id
																							select new BusinessUnitSubgroup
																							{
																								id = tb10.Id,
																								name = tb10.unit_name

																							}).FirstOrDefault()),
														  BusinessUnitDivision = newbugDiv((from tb11 in _repo.GetContext().Set<BusinessUnit>()
																							where tb7.business_unit_division_id == tb11.Id
																							select new BusinessUnitDivision
																							{
																								id = tb11.Id,
																								name = tb11.unit_name

																							}).FirstOrDefault()),
														  BusinessUnitDepartement = newbugDepart((from tb12 in _repo.GetContext().Set<BusinessUnit>()
																								  where tb7.business_unit_departement_id == tb12.Id
																								  select new BusinessUnitDepartement
																								  {
																									  id = tb12.Id,
																									  name = tb12.unit_name

																								  }).FirstOrDefault()),
														  ParentBusinessUnit = newBugParent((from tcc in _repo.GetContext().Set<BusinessUnitJobLevel>()
																							 join tc1 in _repo.GetContext().Set<JobTitle>() on tcc.job_title_id equals tc1.Id
																							 where tb7.parent_id == tcc.Id
																							 select new ParentBusinessUnit()
																							 {
																								 id = tcc.Id,
																								 name = tc1.title_name
																							 }).FirstOrDefault())
													  }).FirstOrDefault()
						  };

				resp.Data = que.FirstOrDefault();
				resp.Success = true;
				resp.Message = "Sucess";
				resp.Code = "00";

			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message.ToString();
			}
			return resp;
		}

		public GeneralResponseList<EmployeeBasicInfoDto> GetWithDivisionId(string DivisionId)
		{
			GeneralResponseList<EmployeeBasicInfoDto> resp = new GeneralResponseList<EmployeeBasicInfoDto>();
			try
			{
				PagingHelper q = new PagingHelper();
				var que = from tb1 in _repo.GetContext().Set<EmployeeBasicInfo>()
						  join tbk in _repo.GetContext().Set<BusinessUnitJobLevel>() on tb1.business_unit_job_level_id equals tbk.Id
						  into bk1
						  from tbk1 in bk1.DefaultIfEmpty()
						  join tb2 in _repo.GetContext().Set<BusinessUnit>() on tbk1.business_unit_division_id equals tb2.Id
						  into bu
						  from tbu in bu.DefaultIfEmpty()
						  join tb4 in _repo.GetContext().Set<JobTitle>() on tbk1.job_title_id equals tb4.Id
						  into ttl
						  from tbttl in ttl.DefaultIfEmpty()
						  join tb5 in _repo.GetContext().Set<JobGrade>() on tbk1.job_grade_id equals tb5.Id
						  into grd
						  from tblgrd in grd.DefaultIfEmpty()
						  join tb6 in _repo.GetContext().Set<EmployementStatus>() on tb1.employement_status_id equals tb6.Id
						  into employee
						  from tbempoyee in employee.DefaultIfEmpty()
						  where tbk1.business_unit_division_id == DivisionId
						  select new EmployeeBasicInfoDto()
						  {
							  Id = tb1.Id,
							  name_employee = tb1.name_employee,
							  division = tbu.unit_name,
							  job_title = tbttl.title_name,
							  grade = tblgrd.grade_name,
							  employee_status = tbempoyee.employment_status_name

						  };
				resp.RecordsTotal = que.Count();

				resp.RecordsFiltered = que.Count();
				resp.Total = resp.RecordsTotal;

				resp.Rows = que.ToList();
				resp.Success = true;

			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message;
			}
			return resp;
		}

		public GeneralResponse<EmployeeBasicInfo> UpdateNewEmployeeBasic(EmployeeBasicInfoNew dto)
		{
			GeneralResponse<EmployeeBasicInfo> resp = new GeneralResponse<EmployeeBasicInfo>();
			try
			{
				EmployeeBasicInfo newBasicInfo = new EmployeeBasicInfo();
				newBasicInfo = _repo.FindByID(dto.Id);
				newBasicInfo.first_name = dto.first_name;
				newBasicInfo.last_name = dto.last_name;
				newBasicInfo.nick_name = dto.nick_name;
				newBasicInfo.birth_place = dto.birth_place;
				newBasicInfo.gender = dto.gender;
				newBasicInfo.date_birth = dto.date_birth; 
				newBasicInfo.phone_number = dto.phone_number;
				newBasicInfo.email = dto.email;
				newBasicInfo.mobile_number = dto.mobile_number;
				newBasicInfo.name_employee = dto.first_name + " " + dto.last_name;
				newBasicInfo.nationality_id = dto.nationality_id;
				newBasicInfo.last_education = dto.last_education;
				newBasicInfo.kitas_number = dto.kitas_number;
				newBasicInfo.passport_number = dto.passport_number;
				newBasicInfo.kitas_expiry = DateTime.Parse(dto.kitas_expiry, CultureInfo.InvariantCulture); 
				newBasicInfo.passport_expiry = DateTime.Parse(dto.passport_expiry, CultureInfo.InvariantCulture);
				newBasicInfo.religion_id = dto.religion_id;
				newBasicInfo.npwp_number = dto.npwp_number;
				newBasicInfo.married_status_id = dto.married_status_id;
				newBasicInfo.npwp_expiry = dto.npwp_expiry;
				newBasicInfo.blood = dto.blood;
				newBasicInfo.identity_card = dto.identity_card;
				newBasicInfo.kk_no = dto.kk_no;
				newBasicInfo.post_code = dto.post_code;
				newBasicInfo.address_identity_card = dto.address_identity_card;
				newBasicInfo.home_address = dto.home_address;
				newBasicInfo.photo = dto.photo;

				//tab jobrelated
				newBasicInfo.business_unit_job_level_id = dto.business_unit_job_level_id;
				newBasicInfo.legal_entity_id = dto.legal_entity_id;
				newBasicInfo.location_id = dto.location_id;
				if (dto.join_date != null)
				{
					newBasicInfo.join_date = DateTime.Parse(dto.join_date, CultureInfo.InvariantCulture); 
				}
				
				newBasicInfo.employement_status_id = dto.employement_status_id;

				newBasicInfo.ptkp_setting_id = dto.ptkp_setting_id;
				newBasicInfo.payroll_methods = dto.payroll_methods;
				newBasicInfo.bpjs_healthy = dto.bpjs_healthy;
				newBasicInfo.account_no = dto.account_no;
				newBasicInfo.account_name = dto.account_name;
				newBasicInfo.bank_city = dto.bank_city;
				newBasicInfo.bank_address = dto.bank_address;
				newBasicInfo.bpjs_tk = dto.bpjs_tk;
				newBasicInfo.bank_id = dto.bank_id;
				newBasicInfo.bank_branch = dto.bank_branch;

				_repo.Update(false, newBasicInfo);

				var que = from tb1 in _repo.GetContext().Set<Family>()
						  where tb1.employee_basic_info_id == dto.Id
						  select new FamilyNew()
						  {
							  Id = tb1.Id,
							  address = tb1.address,
							  birthday = tb1.birthday.ToString(),
							  description = tb1.description,
							  education_level_id = tb1.education_level_id,
							  employee_basic_info_id = tb1.employee_basic_info_id,
							  family_status_id = tb1.family_status_id,
							  gender = tb1.gender,
							  institution_id = tb1.institution_id,
							  married_status_id = tb1.married_status_id,
							  name = tb1.name,
							  other_no = tb1.other_no,
							  phone_no = tb1.phone_no,
							  place_birth = tb1.place_birth,
							  religion_id = tb1.religion_id
						  };

				List<FamilyNew> listFamilyCurrent = que.ToList();
				foreach (FamilyNew employeBasicFamilyCurrent in listFamilyCurrent)
				{
					_repoFamily.RemoveByID(false, employeBasicFamilyCurrent.Id);
				}


				if (dto.dataListEmployeBasicFamily != null)
				{
					foreach (FamilyNew employeBasicFamily in dto.dataListEmployeBasicFamily)
					{
						Family newEmployeeBasicFamily = new Family();
						newEmployeeBasicFamily.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeBasicFamily.family_status_id = employeBasicFamily.family_status_id;
						newEmployeeBasicFamily.name = employeBasicFamily.name;
						newEmployeeBasicFamily.phone_no = employeBasicFamily.phone_no;
						newEmployeeBasicFamily.other_no = employeBasicFamily.other_no;
						newEmployeeBasicFamily.address = employeBasicFamily.address;
						newEmployeeBasicFamily.place_birth = employeBasicFamily.place_birth;
						newEmployeeBasicFamily.birthday = DateTime.Parse(employeBasicFamily.birthday, CultureInfo.InvariantCulture);  
						newEmployeeBasicFamily.gender = employeBasicFamily.gender;
						newEmployeeBasicFamily.religion_id = employeBasicFamily.religion_id;
						newEmployeeBasicFamily.education_level_id = employeBasicFamily.education_level_id;
						newEmployeeBasicFamily.institution_id = employeBasicFamily.institution_id;
						newEmployeeBasicFamily.description = employeBasicFamily.description;
						newEmployeeBasicFamily.married_status_id = employeBasicFamily.married_status_id;

						_repoFamily.Create(false, newEmployeeBasicFamily);
					}
				}

				var que2 = from tb2 in _repo.GetContext().Set<EmployeeEmergencyContact>()
						   where tb2.employee_basic_info_id == dto.Id
						   select new EmployeeEmergencyContactNew()
						   {
							   Id = tb2.Id,
							   address = tb2.address,
							   employee_basic_info_id = tb2.employee_basic_info_id,
							   family_status_id = tb2.family_status_id,
							   full_name = tb2.full_name,
							   other_no = tb2.other_no,
							   phone_no = tb2.phone_no
						   };

				List<EmployeeEmergencyContactNew> listEmergencyCurrent = que2.ToList();
				foreach (EmployeeEmergencyContactNew employeBasicEmergencyCurrent in listEmergencyCurrent)
				{
					_repoEmergency.RemoveByID(false, employeBasicEmergencyCurrent.Id);
				}

				if (dto.dataListEmployeBasicEmegencyContact != null)
				{
					foreach (EmployeeEmergencyContactNew employeBasicEmegencyContact in dto.dataListEmployeBasicEmegencyContact)
					{
						EmployeeEmergencyContact newEmployeeEmployeeEmergencyContact = new EmployeeEmergencyContact();
						newEmployeeEmployeeEmergencyContact.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeEmployeeEmergencyContact.family_status_id = employeBasicEmegencyContact.family_status_id;
						newEmployeeEmployeeEmergencyContact.full_name = employeBasicEmegencyContact.full_name;
						newEmployeeEmployeeEmergencyContact.phone_no = employeBasicEmegencyContact.phone_no;
						newEmployeeEmployeeEmergencyContact.other_no = employeBasicEmegencyContact.other_no;
						newEmployeeEmployeeEmergencyContact.address = employeBasicEmegencyContact.address;

						_repoEmergency.Create(false, newEmployeeEmployeeEmergencyContact);
					}
				}

				var que3 = from tb3 in _repo.GetContext().Set<Education>()
						   where tb3.employee_basic_info_id == dto.Id
						   select new EducationNew()
						   {
							   Id = tb3.Id,
							   address = tb3.address,
							   education_level_id = tb3.education_level_id,
							   employee_basic_info_id = tb3.employee_basic_info_id,
							   end_education = tb3.end_education,
							   institution_id = tb3.institution_id,
							   majors = tb3.majors,
							   qualification = tb3.qualification,
							   start_education = tb3.start_education
						   };

				List<EducationNew> listEducationCurrent = que3.ToList();
				foreach (EducationNew employeBasicEducationCurrent in listEducationCurrent)
				{
					_repoEducation.RemoveByID(false, employeBasicEducationCurrent.Id);
				}

				if (dto.dataListEmployeBasicEducationInfo != null)
				{
					foreach (EducationNew employeBasicEducationInfo in dto.dataListEmployeBasicEducationInfo)
					{
						Education newEmployeeEducation = new Education();
						newEmployeeEducation.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeEducation.education_level_id = employeBasicEducationInfo.education_level_id;
						newEmployeeEducation.institution_id = employeBasicEducationInfo.institution_id;
						newEmployeeEducation.majors = employeBasicEducationInfo.majors;
						newEmployeeEducation.start_education = employeBasicEducationInfo.start_education;
						newEmployeeEducation.end_education = employeBasicEducationInfo.end_education;
						newEmployeeEducation.qualification = employeBasicEducationInfo.qualification;
						newEmployeeEducation.address = employeBasicEducationInfo.address;

						_repoEducation.Create(false, newEmployeeEducation);
					}
				}

				var que4 = from tb4 in _repo.GetContext().Set<EmployeeExperience>()
						   where tb4.employee_basic_info_id == dto.Id
						   select new EmployeeExperienceNew()
						   {
							   Id = tb4.Id,
							   address = tb4.address,
							   company_name = tb4.company_name,
							   employee_basic_info_id = tb4.employee_basic_info_id,
							   end_date = tb4.end_date,
							   phone = tb4.phone,
							   position = tb4.position,
							   remark = tb4.remark,
							   start_date = tb4.start_date
						   };

				List<EmployeeExperienceNew> listExperienceCurrent = que4.ToList();
				foreach (EmployeeExperienceNew employeBasicExperienceCurrent in listExperienceCurrent)
				{
					_repoExperience.RemoveByID(false, employeBasicExperienceCurrent.Id);
				}

				if (dto.dataListEmployeBasicEmploymentHistory != null)
				{
					foreach (EmployeeExperienceNew employeBasicEmploymentHistory in dto.dataListEmployeBasicEmploymentHistory)
					{
						EmployeeExperience newEmployeeExperience = new EmployeeExperience();
						newEmployeeExperience.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeExperience.company_name = employeBasicEmploymentHistory.company_name;
						newEmployeeExperience.phone = employeBasicEmploymentHistory.phone;
						newEmployeeExperience.remark = employeBasicEmploymentHistory.remark;
						newEmployeeExperience.address = employeBasicEmploymentHistory.address;
						newEmployeeExperience.start_date = employeBasicEmploymentHistory.start_date;
						newEmployeeExperience.end_date = employeBasicEmploymentHistory.end_date;
						newEmployeeExperience.position = employeBasicEmploymentHistory.position;

						_repoExperience.Create(false, newEmployeeExperience);
					}
				}


				var que5 = from tb5 in _repo.GetContext().Set<EmployeeTraining>()
						   where tb5.employee_basic_info_id == dto.Id
						   select new EmployeeTrainingNew()
						   {
							   Id = tb5.Id,
							   address = tb5.address,
							   description = tb5.description,
							   employee_basic_info_id = tb5.employee_basic_info_id,
							   end_training = tb5.end_training,
							   expired_certificate = tb5.expired_certificate.ToString(),
							   filemaster_id = tb5.filemaster_id,
							   institution_id = tb5.institution_id,
							   name_training = tb5.name_training,
							   no_certification = tb5.no_certification,
							   start_training = tb5.start_training
						   };

				List<EmployeeTrainingNew> listTrainingCurrent = que5.ToList();
				foreach (EmployeeTrainingNew employeBasicTrainingCurrent in listTrainingCurrent)
				{
					_repoTraining.RemoveByID(false, employeBasicTrainingCurrent.Id);
				}

				if (dto.dataListEmployeBasicCourseSeminar != null)
				{
					foreach (EmployeeTrainingNew employeBasicCourseSeminar in dto.dataListEmployeBasicCourseSeminar)
					{
						EmployeeTraining newEmployeeTraining = new EmployeeTraining();
						newEmployeeTraining.employee_basic_info_id = newBasicInfo.Id;
						newEmployeeTraining.name_training = employeBasicCourseSeminar.name_training;
						newEmployeeTraining.institution_id = employeBasicCourseSeminar.institution_id;
						newEmployeeTraining.address = employeBasicCourseSeminar.address;
						newEmployeeTraining.start_training = employeBasicCourseSeminar.start_training;
						newEmployeeTraining.end_training = employeBasicCourseSeminar.end_training;
						newEmployeeTraining.no_certification = employeBasicCourseSeminar.no_certification;
						newEmployeeTraining.expired_certificate = DateTime.Parse(employeBasicCourseSeminar.expired_certificate, CultureInfo.InvariantCulture); 
						newEmployeeTraining.description = employeBasicCourseSeminar.description;
						newEmployeeTraining.filemaster_id = employeBasicCourseSeminar.filemaster_id;

						_repoTraining.Create(false, newEmployeeTraining);
					}

				}

				_repo.SaveChanges();
				_repoFamily.SaveChanges();
				_repoEducation.SaveChanges();
				_repoExperience.SaveChanges();
				_repoTraining.SaveChanges();
				_repoEmergency.SaveChanges();
				_repo.Dispose();
				_repoFamily.Dispose();
				_repoEducation.Dispose();
				_repoExperience.Dispose();
				_repoTraining.Dispose();
				_repoEmergency.Dispose();

				resp.Data = newBasicInfo;
				resp.Success = true;
				resp.Message = "Sucess";
				resp.Code = "00";
			}
			catch (Exception ex)
			{
				resp.Success = false;
				resp.Message = ex.Message.ToString();
			}
			return resp;
		}


	}

}

