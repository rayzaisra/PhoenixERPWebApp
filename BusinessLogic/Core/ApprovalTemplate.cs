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
using Newtonsoft.Json;
using CommonTool.JEasyUI.DataGrid;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Core
{
    public partial class ApprovalTemplate : ApprovalTemplateService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        public ApprovalTemplate(DataContext AppUserData) : base(AppUserData)
        {
            var temp = Context.Approval.GetTemplate<DataAccess.PhoenixERP.Finance.request_for_quotation>();
        }

        

        public bool SaveApproval(approval_template_setupDTO recordDTO, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {

                        var template = new approval_templateDTO();
                        template.InjectFrom(recordDTO);

                        if (recordDTO.templates == null || template == null) throw new Exception("Please fill approver detail");
                        if (string.IsNullOrEmpty(template.id))
                        {
                            template.organization_id = Context.OrganizationId;
                        }

                        result = Svc.SaveEntity(template, ref isNew);
                        if (!result) throw new Exception("Aborted on saving template");

                        #region clear existing 
                        if (!isNew)
                        {
                            db.Delete<approval_template_detail>(" WHERE approval_template_id=@0 ", template.id);
                        }
                        #endregion
                        for (var index = 0; index < recordDTO.templates.Count(); index++)
                        {
                            var record = new approval_template_detail();
                            record.InjectFrom(recordDTO.templates[index]);
                            record.approval_template_id = template.id;
                            record.approval_level = index;
                            //is_primary sementara di tampung di is_auto_approved, jangan lupa di buatkan colomn is_primary untuk tahu siapa yg pertamakali wajib jadi approver 
                            record.is_primary = true;
                            result &= Context.SaveEntity<approval_template_detail>(record, true);

                            if (recordDTO.templates[index].Backup != null && recordDTO.templates[index].Backup.Count > 0)
                            {
                                var backup = new approval_template_detail();
                                backup.InjectFrom(recordDTO.templates[index].Backup.FirstOrDefault());
                                backup.approval_template_id = template.id;
                                backup.is_primary = false;
                                backup.approval_level = index;
                                result &= Context.SaveEntity<approval_template_detail>(backup, true);
                            }
                        }


                        if (result)
                        {
                            scope.Complete();
                        }

                    }
                    catch (Exception ex)
                    {
                        result = false;
                        appLogger.Error(ex);
                    }
                }
            }
            return result;
        }

        public List<Iapproval_template> GetTemplates(string template_id)
        {
            List<Iapproval_template> result = new List<Iapproval_template>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = Sql.Builder.Append(approval_template_detail.DefaultView);
                    qry.Append(" WHERE r.approval_template_id=@0 ", template_id);
                    qry.Append("ORDER BY r.approval_level asc ");
                    var temp = db.Fetch<vw_approval_template_detail>(qry);
                    var tempPrimary = temp.Where(o => o.is_primary == true);

                    foreach (var t in tempPrimary)
                    {
                        var tempBackup = (from b in temp
                                          where b.is_primary == false && b.approval_level == t.approval_level
                                          select new Iapproval_template
                                          {
                                              approval_level = b.approval_level,
                                              approver_id = b.approver_id,
                                              app_fullname = b.app_fullname,
                                              is_primary = false
                                          }).ToList();

                        result.Add(new Iapproval_template()
                        {
                            approval_level = t.approval_level,
                            approver_id = t.approver_id,
                            app_fullname = t.app_fullname,
                            Backup = tempBackup,
                            is_primary = true

                        });
                    }
                    if (result.Count > 0)
                    {
                        result = result.OrderBy(x => x.approval_level).ToList();
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result;
        }

        public List<vw_approval_template_detailDTO> GetTemplatesDatasource(string template_id)
        {
            List<vw_approval_template_detailDTO> result = new List<vw_approval_template_detailDTO>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = Sql.Builder.Append(approval_template_detail.DefaultView);
                    qry.Append(" WHERE r.approval_template_id=@0 ", template_id);
                    qry.Append("ORDER BY r.approval_level asc ");
                    var data = db.Fetch<vw_approval_template_detail>(qry);
                    result = data.Select(x => new vw_approval_template_detailDTO().InjectFrom(x)).Cast<vw_approval_template_detailDTO>().ToList();
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result;
        }

        public List<approval_template_connector> GetTemplatesConnector(string template_id)
        {
            List<approval_template_connector> result = new List<approval_template_connector>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = Sql.Builder.Append(approval_template_detail.DefaultView);
                    qry.Append(" WHERE r.approval_template_id=@0 ", template_id);
                    qry.Append("ORDER BY r.approval_level asc ");
                    var temp = db.Fetch<vw_approval_template_detail>(qry);
                    var tempPrimary = temp.Where(o => o.is_primary == true).ToList();

                    for (var index = 0; index < tempPrimary.Count-1; index++)
                    {
                       
                        result.Add(new approval_template_connector()
                        {
                            From = tempPrimary[index].id,
                            To = tempPrimary[index + 1].id,
                            Label = "Next"
                        });

                    }

                    for (var index = 0; index < tempPrimary.Count; index++)
                    {
                        var tempBackup = (from b in temp
                                          where b.is_primary == false && b.approval_level == tempPrimary[index].approval_level
                                          select new approval_template_connector()
                                          {
                                              From = tempPrimary[index].id,
                                              To = b.id,
                                              Label = "Secondary"
                                          }).ToList();

                        if (tempBackup != null)
                        {
                            result.AddRange(tempBackup);
                        }

                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result;
        }


        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String OptionalFilters = "",
                  String OptionalParameters = "", String OptionalQueryBuilder = "")
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.organization_id ='{0}' ", Context.OrganizationId);
                result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
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
