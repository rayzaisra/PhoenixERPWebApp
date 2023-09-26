using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Entities.Hris;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IJobVacancyJobSourceServices : IBaseService<JobVacancyJobSourceDto, JobVacancyJobSouce, string>
    {
       
    }
    public partial class JobVacancyJobSourceServices : BaseService<JobVacancyJobSourceDto, JobVacancyJobSouce, string>, IJobVacancyJobSourceServices
    {
        public JobVacancyJobSourceServices(IEFRepository<JobVacancyJobSouce, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        
    }
}

