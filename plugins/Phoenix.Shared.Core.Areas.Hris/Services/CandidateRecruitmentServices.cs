using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Entities.Hris;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Linq;



namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface ICandidateRecruitmentServices : IBaseService<CandidateRecruitmentDto, CandidateRecruitment, string>
    {
        GeneralResponseList<CandidateRecruitmentDto> ListAll(CandidateRecruitmentSearch filter);
        GeneralResponse<CandidateRecruitment> CreateDetil(CandidateRecruitmentNew dto);
        GeneralResponse<CandidateRecruitment> DeleteDetil(CandidateRecruitmentDto dto);
        GeneralResponse<CandidateRecruitment> UpdateCustom(CandidateRecruitmentDto dto);
        GeneralResponse<CandidateRecruitment> CheckApprovalCustom(CandidateRecruitmentDto dto);
        GeneralResponse<CandidateRecruitment> UpdateCustomWithContract(CandidateRecruitmentDto dto);
        GeneralResponse<CandidateRecruitmentDto> GetDataDetail(string id);
        string DataFileMaster(string id = null);
    }
    public partial class CandidateRecruitmentServices : BaseService<CandidateRecruitmentDto, CandidateRecruitment, string>, ICandidateRecruitmentServices
    {
        IEFRepository<HiringApplicant, string> _repoHiringApplicant;
        IEFRepository<Filemaster, string> _repoFileMaster;
        IEFRepository<CandidateRecruitmentSignContract, string> _repoCandidateRecruitmentSignContract;
        IEFRepository<JobVacancy, string> _repoJobVacancy;
        public CandidateRecruitmentServices(IEFRepository<CandidateRecruitment, string> repo, IEFRepository<HiringApplicant, string> repoHiringApplicant, IEFRepository<CandidateRecruitmentSignContract, string> repoCandidateRecruitmentSignContract, IEFRepository<JobVacancy, string> repoJobVacancy, IEFRepository<Filemaster, string> repoFileMaster) : base(repo)
        {
            _repo = repo;
            _repoHiringApplicant = repoHiringApplicant;
            _repoCandidateRecruitmentSignContract = repoCandidateRecruitmentSignContract;
            _repoJobVacancy = repoJobVacancy;
            _repoFileMaster = repoFileMaster;
        }

        public GeneralResponse<CandidateRecruitmentDto> GetDataDetail(string id)
        {
            GeneralResponse<CandidateRecruitmentDto> resp = new GeneralResponse<CandidateRecruitmentDto>();
            try
            {
                resp = GetByIDResponseDto(id);
                if (resp != null)
                {
                    if (!string.IsNullOrEmpty(resp.Data.cv_file_master_id))
                    {
                        resp.Data.cv_file = DataFileMaster(resp.Data.cv_file_master_id);
                    }
                    if (!string.IsNullOrEmpty(resp.Data.photo_file_master_id))
                    {
                        resp.Data.photo_file = DataFileMaster(resp.Data.photo_file_master_id);
                    }
                    if (!string.IsNullOrEmpty(resp.Data.portofolio_file_master_id))
                    {
                        resp.Data.portofolio_file = DataFileMaster(resp.Data.portofolio_file_master_id);
                    }

                    var dataContract = _repo.GetContext().Set<CandidateRecruitmentSignContract>().Where(x => x.candidate_recruitment_id == id).OrderByDescending(x=>x.created_on).FirstOrDefault();
                    if (dataContract != null)
                    {
                        resp.Data.contract_number = dataContract.ptpp;
                        resp.Data.contract_date = dataContract.start_date;
                        resp.Data.contractSignDate = dataContract.contractSignDate;


                    }
                   
                    resp.Data.apply_date = resp.Data.created_on;
                    var dataInterview = _repo.GetContext().Set<CandidateRecruitmentInterview>().Where(x => x.candidate_recruitment_id == id && x.subject == "Invite 1st Interview").OrderByDescending(x => x.created_on).FirstOrDefault();
                    if (dataInterview != null)
                    {
                        resp.Data.interview_date = dataInterview.date_interview;
                    }

                    var queryGetJobTitle = (from q in _repo.GetContext().Set<JobVacancy>()
                                            join r in _repo.GetContext().Set<HiringRequest>() on q.hiring_request_id equals r.Id
                                            join s in _repo.GetContext().Set<BusinessUnitJobLevel>() on r.job_title_id equals s.job_title_id
                                            where q.Id == resp.Data.job_vacancy_id && s.job_grade_id == r.job_grade_id && q.is_deleted == false
                                            select new BusinessUnitJobLevel()
                                            {
                                                Id = s.Id

                                            }).FirstOrDefault();
                    if (queryGetJobTitle != null)
                    {
                        resp.Data.businessJobLevelId = queryGetJobTitle.Id;
                    }

                    resp.Success = true;
                    resp.Code = "200";
                }
                else
                {
                    resp.Success = false;
                    resp.Code = "200";
                    resp.Message = "Data not Found";
                }
                
            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "404";
                resp.Message = x.Message.ToString();
            }
            return resp;
        }

        public string DataFileMaster(string id = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(id))
            {
                var dataFile = _repo.GetContext().Set<Filemaster>().Where(x => x.Id == id).FirstOrDefault();
                if (dataFile != null)
                {
                    result = dataFile.Id + "::" + dataFile.name;
                }
            }
            
            return result;
        }

        public GeneralResponse<CandidateRecruitment> CreateDetil(CandidateRecruitmentNew dto)
        {
            GeneralResponse<CandidateRecruitment> resp = new GeneralResponse<CandidateRecruitment>();
            try
            {
                Random rn = new Random();
                CandidateRecruitment candidate = new CandidateRecruitment();
                PropertyMapper.All(dto, candidate);
                candidate.applicant_progress_id = "0";
                candidate.applicant_move_stage_id = "1";
                candidate.code = "APL" + rn.Next(1, 99999);
                candidate.job_vacancy_id = dto.job_vacancy_id;
                string IdFileMasterCV = "";
                string IdFileMasterPhoto = "";
                string IdFileMasterPortfolio = "";
                if (!string.IsNullOrEmpty(dto.cv))
                {
                    string[] dataCv = dto.cv.Split(new string[] { "::" }, StringSplitOptions.None);
                    IdFileMasterCV = dataCv[0];
                    var file = _repoFileMaster.FindByID(IdFileMasterCV);
                    file.record_id = candidate.Id;
                    _repoFileMaster.Update(file);
                }

                if (!string.IsNullOrEmpty(dto.photo))
                {
                    string[] dataPhoto = dto.photo.Split(new string[] { "::" }, StringSplitOptions.None);
                    IdFileMasterPhoto = dataPhoto[0];
                    var file = _repoFileMaster.FindByID(IdFileMasterPhoto);
                    file.record_id = candidate.Id;
                    _repoFileMaster.Update(file);
                }

                if (!string.IsNullOrEmpty(dto.portfolio))
                {
                    string[] dataPortfolio = dto.portfolio.Split(new string[] { "::" }, StringSplitOptions.None);
                    IdFileMasterPortfolio = dataPortfolio[0];
                    var file = _repoFileMaster.FindByID(IdFileMasterPortfolio);
                    file.record_id = candidate.Id;
                    _repoFileMaster.Update(file);
                }

                candidate.cv_file_master_id = IdFileMasterCV;
                candidate.photo_file_master_id = IdFileMasterPhoto;
                candidate.portofolio_file_master_id = IdFileMasterPortfolio;
                _repo.Create(false, candidate);

                HiringApplicant newHiringAplicant = new HiringApplicant();
                newHiringAplicant.job_vacancy_id = dto.job_vacancy_id;
                newHiringAplicant.candidate_recruitment_id = candidate.Id;

                _repoHiringApplicant.Create(false, newHiringAplicant);

                if (!string.IsNullOrEmpty(dto.cv))
                {
                    var file = _repoFileMaster.FindByID(IdFileMasterCV);
                    file.record_id = candidate.Id;
                    _repoFileMaster.Update(file);
                }

                if (!string.IsNullOrEmpty(dto.photo))
                {
                    var file = _repoFileMaster.FindByID(IdFileMasterPhoto);
                    file.record_id = candidate.Id;
                    _repoFileMaster.Update(file);
                }

                if (!string.IsNullOrEmpty(dto.portfolio))
                {
                    var file = _repoFileMaster.FindByID(IdFileMasterPortfolio);
                    file.record_id = candidate.Id;
                    _repoFileMaster.Update(file);
                }


                _repo.SaveChanges();
                _repoHiringApplicant.SaveChanges();
                //_repo.Dispose();
                //_repoHiringApplicant.Dispose();
                _repoFileMaster.SaveChanges();
                //_repoFileMaster.Dispose();
                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";
                resp.Data = candidate;

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<CandidateRecruitment> DeleteDetil(CandidateRecruitmentDto dto)
        {
            GeneralResponse<CandidateRecruitment> resp = new GeneralResponse<CandidateRecruitment>();
            try
            {


                HiringApplicantDto ha = new HiringApplicantDto();
                PagingHelper q = new PagingHelper();
                IQueryable<HiringApplicantDto> que = from tb1 in _repo.GetContext().Set<HiringApplicant>()
                                                     where tb1.job_vacancy_id == dto.job_vacancy_id && tb1.candidate_recruitment_id == dto.Id
                                                     select new HiringApplicantDto()
                                                     {
                                                         Id = tb1.Id,
                                                         candidate_recruitment_id = tb1.candidate_recruitment_id,
                                                         hiring_request_id = tb1.candidate_recruitment_id
                                                     };
                ha = que.FirstOrDefault();
                HiringApplicant hanew = new HiringApplicant();
                hanew = _repoHiringApplicant.FindByID(ha.Id);

                _repo.RemoveByID(false, dto.Id);
                _repoHiringApplicant.RemoveByID(false, hanew.Id);


                _repo.SaveChanges();
                _repoHiringApplicant.SaveChanges();
                _repo.Dispose();
                _repoHiringApplicant.Dispose();
                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponseList<CandidateRecruitmentDto> ListAll(CandidateRecruitmentSearch filter)
        {
            GeneralResponseList<CandidateRecruitmentDto> resp = new GeneralResponseList<CandidateRecruitmentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CandidateRecruitmentDto> que = from thp in _repo.GetContext().Set<HiringApplicant>()
                                                          join tb1 in _repo.GetContext().Set<CandidateRecruitment>() on thp.candidate_recruitment_id equals tb1.Id
                                                          //where tb1.applicant_progress_id == "0"
                                                          select new CandidateRecruitmentDto()
                                                          {
                                                              Id = tb1.Id,
                                                              code = tb1.code,
                                                              achievement = tb1.achievement,
                                                              address = tb1.address,
                                                              alcohol_willing = tb1.alcohol_willing,
                                                              applicant_progress_id = tb1.applicant_progress_id,
                                                              approver_id = tb1.approver_id,
                                                              birth_place = tb1.birth_place,
                                                              certificate = tb1.certificate,
                                                              cigaret_willing = tb1.cigaret_willing,
                                                              city_id = tb1.city_id,
                                                              company_last_address = tb1.company_last_address,
                                                              company_last_from = tb1.company_last_from,
                                                              company_last_name = tb1.company_last_name,
                                                              company_last_phone = tb1.company_last_phone,
                                                              company_last_until = tb1.company_last_until,
                                                              country = tb1.country,
                                                              course_institution = tb1.course_institution,
                                                              course_tittle = tb1.course_tittle,
                                                              course_years = tb1.course_years,
                                                              date_birth = tb1.date_birth,
                                                              education_level_id = tb1.education_level_id,
                                                              last_education_name = (from a in _repo.GetContext().Set<EducationLevel>()
                                                                                     where a.Id == tb1.last_education
                                                                                     select a.education_name).FirstOrDefault(),
                                                              education_level_name = (from a in _repo.GetContext().Set<EducationLevel>()
                                                                                      where a.Id == tb1.education_level_id
                                                                                      select a.education_name).FirstOrDefault(),
                                                              education_year_from = tb1.education_year_from,
                                                              education_year_to = tb1.education_year_to,
                                                              email = tb1.email,
                                                              file_id = tb1.file_id,
                                                              first_name = tb1.first_name,
                                                              gender = tb1.gender,
                                                              identity_card = tb1.identity_card,
                                                              institusion = tb1.institusion,
                                                              interest = tb1.interest,
                                                              job_repository_id = tb1.job_repository_id,
                                                              last_education = tb1.last_education,
                                                              last_name = tb1.last_name,
                                                              last_position = tb1.last_position,
                                                              looking_job = tb1.looking_job,
                                                              major = tb1.major,
                                                              married_status_id = tb1.married_status_id,
                                                              mobile_number = tb1.mobile_number,
                                                              nationality_id = tb1.nationality_id,
                                                              nick_name = tb1.nick_name,
                                                              pay_slip = tb1.pay_slip,
                                                              phone_number = tb1.phone_number,
                                                              portofolio = tb1.portofolio,
                                                              profile_file = tb1.profile_file,
                                                              province = tb1.province,
                                                              relation_people = tb1.relation_people,
                                                              religion_id = tb1.religion_id,
                                                              resume_screening = tb1.resume_screening,
                                                              source_recruitment = tb1.source_recruitment,
                                                              work_number = tb1.work_number,
                                                              zip_code = tb1.zip_code,
                                                              job_vacancy_id = thp.job_vacancy_id,
                                                              source = tb1.source,
                                                              year_experience = tb1.year_experience,
                                                              applicant_move_stage_id = tb1.applicant_move_stage_id,
                                                              //thp = (from b in _repo.GetContext().Set<CandidateRecruitmentSignContract>()
                                                              //       where b.candidate_recruitment_id == tb1.Id
                                                              //       select b.thp).FirstOrDefault(),
                                                              ptpp = (from b in _repo.GetContext().Set<CandidateRecruitmentSignContract>()
                                                                      where b.candidate_recruitment_id == tb1.Id
                                                                      select b.ptpp).FirstOrDefault()

                                                          };

                resp.RecordsTotal = que.Count();


                que = q.filterEquals(que, "applicant_progress_id", filter.Search_applicant_progress_id);
                que = q.filterEquals(que, "applicant_move_stage_id", filter.Search_applicant_move_stage_id);
                que = q.filterEquals(que, "job_vacancy_id", filter.Search_job_vacancy_id);

               if (!string.IsNullOrEmpty(filter.Search_title_name))
                    que = q.filterContains(que, "first_name", filter.Search_title_name);



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

        public GeneralResponse<CandidateRecruitment> UpdateCustom(CandidateRecruitmentDto dto)
        {
            GeneralResponse<CandidateRecruitment> resp = new GeneralResponse<CandidateRecruitment>();
            try
            {
                var message = "";
                var IdFileMasterCV = "";
                var IdFileMasterPhoto = "";
                var IdFileMasterPortfolio = "";
                CandidateRecruitment candidate = _repo.FindByID(dto.Id);
                if (candidate != null)
                {
                    IdFileMasterCV = candidate.cv_file_master_id;
                    IdFileMasterPortfolio = candidate.portofolio_file_master_id;
                    IdFileMasterPhoto = candidate.photo_file_master_id;

                    candidate.applicant_progress_id = dto.applicant_progress_id;
                    if (string.IsNullOrEmpty(dto.applicant_move_stage_id))
                    {
                        dto.applicant_move_stage_id = "1";
                    }
                    candidate.applicant_move_stage_id = dto.applicant_move_stage_id;

                    if (!string.IsNullOrEmpty(dto.cv))
                    {
                        string[] dataCv = dto.cv.Split(new string[] { "::" }, StringSplitOptions.None);
                        IdFileMasterCV = dataCv[0];
                        var file = _repoFileMaster.FindByID(IdFileMasterCV);
                        file.record_id = candidate.Id;
                        _repoFileMaster.Update(file);
                    }
                    if (!string.IsNullOrEmpty(dto.photo))
                    {
                        string[] dataPhoto = dto.photo.Split(new string[] { "::" }, StringSplitOptions.None);
                        IdFileMasterPhoto = dataPhoto[0];
                        var file = _repoFileMaster.FindByID(IdFileMasterPhoto);
                        file.record_id = candidate.Id;
                        _repoFileMaster.Update(file);
                    }
                    if (!string.IsNullOrEmpty(dto.portfolio))
                    {
                        string[] dataPortfolio = dto.portfolio.Split(new string[] { "::" }, StringSplitOptions.None);
                        IdFileMasterPortfolio = dataPortfolio[0];
                        var file = _repoFileMaster.FindByID(IdFileMasterPortfolio);
                        file.record_id = candidate.Id;
                        _repoFileMaster.Update(file);
                    }
                   
                    candidate.cv_file_master_id = IdFileMasterCV;
                    candidate.photo_file_master_id = IdFileMasterPhoto;
                    candidate.portofolio_file_master_id = IdFileMasterPortfolio;
                    message = "Sucess";

                    
                    _repo.Update(candidate);
                    //_repoFileMaster.SaveChanges();
                    //_repoFileMaster.Dispose();
                }
                else
                {
                    message = "Candidate data not found";
                }
               

                resp.Success = true;
                resp.Message = message;
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<CandidateRecruitment> CheckApprovalCustom(CandidateRecruitmentDto dto)
        {
            GeneralResponse<CandidateRecruitment> resp = new GeneralResponse<CandidateRecruitment>();
            try
            {
                IQueryable<TrTemplateApproval> que = from tb1 in _repo.GetContext().Set<TrTemplateApproval>()
                                                     where tb1.RefId == dto.Id
                                                     select new TrTemplateApproval
                                                     {
                                                         Id = tb1.Id,
                                                         RefId = tb1.RefId,
                                                         StatusApproved = tb1.StatusApproved
                                                     };

                TrTemplateApproval Header = que.FirstOrDefault();

                //if (Header.StatusApproved != null)
                //{
                //    if (Header.StatusApproved.Equals("3") || Header.StatusApproved == 3)
                //    {
                //        CandidateRecruitment candidate = _repo.FindByID(dto.Id);
                //        candidate.applicant_progress_id = "4";
                //        candidate.applicant_move_stage_id = "6";

                //        _repo.Update(candidate);
                //    }
                //}


                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<CandidateRecruitment> UpdateCustomWithContract(CandidateRecruitmentDto dto)
        {
            GeneralResponse<CandidateRecruitment> resp = new GeneralResponse<CandidateRecruitment>();
            try
            {

                CandidateRecruitment candidate = _repo.FindByID(dto.Id);
                candidate.applicant_progress_id = dto.applicant_progress_id;
                candidate.applicant_move_stage_id = dto.applicant_move_stage_id;


                _repo.Update(false, candidate);

                CandidateRecruitmentSignContract candidatesigncontract = new CandidateRecruitmentSignContract();
                var csc = _repo.GetContext().Set<CandidateRecruitmentSignContract>().Where(x => x.candidate_recruitment_id == candidate.Id).FirstOrDefault();
                if (csc == null)
                {
                    candidatesigncontract.candidate_recruitment_id = candidate.Id;
                    candidatesigncontract.filemaster_id = dto.filemaster_id;
                    candidatesigncontract.remarks = dto.remarks;
                    candidatesigncontract.start_date = dto.start_date;
                    candidatesigncontract.end_date = dto.end_date;
                    candidatesigncontract.thp = dto.thp;
                    candidatesigncontract.ptpp = dto.ptpp;
                    candidatesigncontract.contracttype = dto.contracttype;
                    candidatesigncontract.contractSignDate = dto.contractSignDate;
                    _repoCandidateRecruitmentSignContract.Create(false, candidatesigncontract);
                }
                else
                {
                    csc.candidate_recruitment_id = candidate.Id;
                    csc.filemaster_id = dto.filemaster_id;
                    csc.remarks = dto.remarks;
                    csc.start_date = dto.start_date;
                    csc.end_date = dto.end_date;
                    csc.thp = dto.thp;
                    csc.ptpp = dto.ptpp;
                    csc.contracttype = dto.contracttype;
                    csc.contractSignDate = dto.contractSignDate;
                    _repoCandidateRecruitmentSignContract.Update(csc);
                }
                

                JobVacancy vacancy = _repoJobVacancy.FindByID(candidate.job_vacancy_id);
                vacancy.posting_status = "5";
                _repoJobVacancy.Update(false, vacancy);


                _repo.SaveChanges();
                _repoCandidateRecruitmentSignContract.SaveChanges();
                _repoJobVacancy.SaveChanges();
                //_repo.Dispose();
                //_repoCandidateRecruitmentSignContract.Dispose();
                //_repoJobVacancy.Dispose();

                resp.Success = true;
                resp.Message = "Sucess";
                resp.Code = "00";

            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }

        public GeneralResponse<CandidateRecruitment> GetDetail(string jobVacancyId)
        {
            GeneralResponse<CandidateRecruitment> resp = new GeneralResponse<CandidateRecruitment>();
          
            try
            {
                var query = (from a in _repo.GetContext().Set<CandidateRecruitment>()
                             where a.job_vacancy_id == jobVacancyId
                             select new CandidateRecruitment()
                             {
                                 job_vacancy_id = a.job_vacancy_id,
                                 achievement = a.achievement,
                                 address = a.address,
                                 alcohol_willing = a.alcohol_willing,
                                 applicant_move_stage_id = a.applicant_move_stage_id,
                                 applicant_progress_id = a.applicant_progress_id,
                                 birth_place = a.birth_place,
                                 certificate = a.certificate,
                                 cigaret_willing = a.cigaret_willing,
                                 city_id = a.city_id,
                                 code = a.code,
                                 company_last_address = a.company_last_address,
                                 company_last_from = a.company_last_from,
                                 company_last_name = a.company_last_name,
                                 company_last_phone = a.company_last_phone,
                                 company_last_until = a.company_last_until,
                                 country = a.country,
                                 course_institution = a.course_institution,
                                 course_tittle = a.course_tittle,
                                 course_years = a.course_years,
                                 date_birth = a.date_birth,
                                 education_level_id = a.education_level_id,
                                 education_year_from = a.education_year_from,
                                 education_year_to = a.education_year_to,
                                 email = a.email,
                                 file_id = a.file_id,
                                 first_name = a.first_name,
                                 gender = a.gender,
                                 Id = a.Id,
                                 identity_card = a.identity_card,
                                 institusion = a.institusion,
                                 interest = a.interest,
                                 job_repository_id = a.job_repository_id,
                                 last_education = a.last_education,
                                 last_name = a.last_name,
                                 last_position = a.last_position,
                                 looking_job = a.looking_job,
                                 major = a.major,
                                 married_status_id = a.married_status_id,
                                 mobile_number = a.mobile_number,
                                 nationality_id = a.nationality_id,
                                 nick_name = a.nick_name,
                                 out_of_town_willing = a.out_of_town_willing,
                                 pay_slip = a.pay_slip,
                                 phone_number = a.phone_number,
                                 portofolio = a.portofolio,
                                 profile_file = a.profile_file,
                                 province = a.province,
                                 relation_people = a.relation_people,
                                 religion_id = a.religion_id,
                                 resume_screening = a.resume_screening,
                                 source = a.source,
                                 source_recruitment = a.source_recruitment,
                                 work_number = a.work_number,
                                 year_experience = a.year_experience,
                                 zip_code = a.zip_code,
                                 portofolio_file_master_id = a.portofolio_file_master_id,
                                 photo_file_master_id = a.photo_file_master_id,
                                 cv_file_master_id = a.cv_file_master_id


                             }).FirstOrDefault();
            }
            catch (Exception x)
            {
                resp.Success = false;
                resp.Code = "500";
                resp.Message = x.Message.ToString();
            }

            return resp;
        }
    }
}

