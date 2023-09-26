using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Omu.ValueInjecter;
using NLog;
using System.Transactions;
using System.Dynamic;
using System.Linq;
using BusinessLogic.Services.General;
using NPoco;
using DataAccess.PhoenixERP.General;
using CommonTool.QueryBuilder.Model;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.Core
{
    public partial class ApprovalTemplateDetail : ApprovalTemplateDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ApprovalTemplateDetail(DataContext AppUserData) : base(AppUserData)
        {
        }
        
        public override bool SaveEntity(approval_template_detailDTO RecordDto, ref bool isNew)
        {
            bool result = false;
            if (RecordDto == null) return result;
            try
            {
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }
    }
}
