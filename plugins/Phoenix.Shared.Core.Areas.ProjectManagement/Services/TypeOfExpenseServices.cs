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
    public partial interface ITypeOfExpenseServices: IBaseService<TypeOfExpenseDto, TypeOfExpense, string>
    {
        GeneralResponseList<TypeOfExpenseDto> ListAll(TypeOfExpenseDto data);
    }

    public partial class TypeOfExpenseServices : BaseService<TypeOfExpenseDto, TypeOfExpense, string>, ITypeOfExpenseServices
    {
        public TypeOfExpenseServices(IEFRepository<TypeOfExpense, string> repo): base(repo)
        {
            _repo = repo;
        }
        
        public GeneralResponseList<TypeOfExpenseDto> ListAll(TypeOfExpenseDto data)
        {
            GeneralResponseList<TypeOfExpenseDto> resp = new GeneralResponseList<TypeOfExpenseDto>();

            try
            {
                PagingHelper q = new PagingHelper();

                IQueryable<TypeOfExpenseDto> que = from tb1 in _repo.GetContext().Set<TypeOfExpense>()
                          select new TypeOfExpenseDto()
                          {
                              id= tb1.Id,
                              name = tb1.name
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
