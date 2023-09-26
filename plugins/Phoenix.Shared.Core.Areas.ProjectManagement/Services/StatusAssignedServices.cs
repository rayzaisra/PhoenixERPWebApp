using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Libraries.SnowFlake;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IStatusAssignedServices : IBaseService<StatusAssignedDto, StatusAssigned, string>
    {
        GeneralResponseList<StatusAssignedDto> ListAll(StatusAssignedSearch filter);
        #region AddonService
        GeneralResponse<SetStatusDetailDto> GetDetailSetStatusByJobNumber(string JobNumber);
        GeneralResponse<SetStatusResponseDto> SubmitSetStatus(SetStatusParameterDto parameter);
        #endregion

    }
    public partial class StatusAssignedServices : BaseService<StatusAssignedDto, StatusAssigned, string>, IStatusAssignedServices
    {
        string userid = "";
        IEFRepository<Dam, string> _repoDam;
        IEFRepository<JobDetail, string> _repoJobDetail;

        public StatusAssignedServices(IEFRepository<StatusAssigned, string> repo,
            IEFRepository<Dam, string> repoDam,
            IEFRepository<JobDetail, string> repoJobDetail
            ) : base(repo)
        {
            _repo = repo;
            userid = Helpers.Glosing.Instance.Username;
            _repoDam = repoDam;
            _repoJobDetail = repoJobDetail;
        }

        public GeneralResponseList<StatusAssignedDto> ListAll(StatusAssignedSearch filter)
        {

            GeneralResponseList<StatusAssignedDto> resp = new GeneralResponseList<StatusAssignedDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<StatusAssignedDto> que = from tb1 in _repo.GetContext().Set<StatusAssigned>()
                                                    select new StatusAssignedDto()
                                                    {
                                                        Id = tb1.Id,
                                                        status_assigned = tb1.status_assigned,
                                                    };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "status_assigned", filter.Search_status_assigned);



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

        #region AddonService
        public GeneralResponse<SetStatusDetailDto> GetDetailSetStatusByJobNumber(string JobNumber)
        {
            GeneralResponse<SetStatusDetailDto> resp = new GeneralResponse<SetStatusDetailDto>();
            try
            {
                var query = (from tb1 in _repo.GetContext().Set<JobDetail>()
                             join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                             join tb4 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb4.Id
                             join tb5 in _repo.GetContext().Set<Company>() on tb4.company_id equals tb5.Id
                             where tb1.job_number == JobNumber
                             select new SetStatusDetailDto()
                             {
                                 CampaignName = tb2.campaign_name,
                                 ClientName = tb5.company_name,
                                 JobName = tb1.job_name,
                                 JobNumber = tb1.job_number,
                                 Id = tb1.Id
                             }).FirstOrDefault();

                var queryListFile = from tb1 in _repo.GetContext().Set<Dam>()
                                    join tb2 in _repo.GetContext().Set<DamType>() on tb1.dam_type_id equals tb2.Id
                                    join tb3 in _repo.GetContext().Set<Task>() on tb1.task_id equals tb3.Id
                                    where tb3.job_number == JobNumber
                                    select new SetStatusFileDto()
                                    {
                                        DamId = tb1.Id,
                                        DamName = tb1.dam_type_id,
                                        FileId = tb1.file_id,
                                        DamTypeId = tb2.Id,
                                        DamTypeName = tb2.name
                                    };

                var queryResult = new SetStatusDetailDto()
                {
                    CampaignName = query.CampaignName,
                    ClientName = query.ClientName,
                    JobName = query.JobName,
                    JobNumber = query.JobNumber,
                    Id = query.Id,
                    ListFileDAM = queryListFile.ToList()
                };

                resp.Data = queryResult;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<SetStatusResponseDto> SubmitSetStatus(SetStatusParameterDto parameter)
        {
            GeneralResponse<SetStatusResponseDto> resp = new GeneralResponse<SetStatusResponseDto>();

            try
            {
                for (int i = 0; i < parameter.ListFileDAM.Count(); i++)
                {
                    var DataDam = new Dam()
                    {
                        //dam = parameter.ListFileDAM[i].DamName,
                        dam_type_id = parameter.ListFileDAM[i].DamTypeId,
                        file_id = parameter.ListFileDAM[i].FileId,
                        job_detail_id = parameter.JobNumber
                    };

                    _repoDam.Create(DataDam);
                }


                var JobDetail = _repo.GetContext().Set<JobDetail>().Where(x => x.job_number == parameter.JobNumber).FirstOrDefault();

                if (JobDetail != null)
                {
                    JobDetail.job_status_id = parameter.SetStatusId;
                }

                _repoDam.SaveChanges();
                _repoJobDetail.SaveChanges();

                var response = new SetStatusResponseDto()
                {
                    CampaignName = parameter.CampaignName,
                    ClientName = parameter.ClientName,
                    Id = parameter.Id,
                    JobName = parameter.JobName,
                    JobNumber = parameter.JobNumber,
                    UpdateDataResult = true,
                    ListFileDAM = parameter.ListFileDAM
                };

                resp.Data = response;
                resp.Success = true;
                resp.Message = "Sukses";
            }
            catch (Exception ex)
            {
                var response = new SetStatusResponseDto()
                {
                    CampaignName = parameter.CampaignName,
                    ClientName = parameter.ClientName,
                    Id = parameter.Id,
                    JobName = parameter.JobName,
                    JobNumber = parameter.JobNumber,
                    UpdateDataResult = false,
                    ListFileDAM = parameter.ListFileDAM
                };
                resp.Data = response;
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }



        #endregion
    }
}

