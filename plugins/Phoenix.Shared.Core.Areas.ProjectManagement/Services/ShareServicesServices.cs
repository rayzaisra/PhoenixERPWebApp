using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
   public partial interface IShareServicesServices : IBaseService<ShareServicesDto, ShareServices, string>
    {
        GeneralResponseList<ShareServicesDto> ListAll(ShareServicesDto data);
    }

    public partial class ShareServicesServices : BaseService<ShareServicesDto, ShareServices, string>, IShareServicesServices
    {
        public ShareServicesServices(IEFRepository<ShareServices, string> repo) : base(repo)
        {
            _repo = repo;
        }

        public GeneralResponseList<ShareServicesDto> ListAll(ShareServicesDto data)
        {
            GeneralResponseList<ShareServicesDto> resp = new GeneralResponseList<ShareServicesDto>();

            try
            {
                var que = from tb1 in _repo.GetContext().Set<ShareServices>()
                          select new ShareServicesDto()
                          {
                              id = tb1.Id,
                              shareservice_name = tb1.shareservice_name
                          };

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

    }

}
