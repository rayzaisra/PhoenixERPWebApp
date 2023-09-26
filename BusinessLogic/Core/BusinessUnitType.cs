using System;
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

namespace BusinessLogic.Core
{
    public partial class BusinessUnitType : BusinessUnitTypeService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public BusinessUnitType(DataContext AppUserData) : base(AppUserData)
        {
        }



        public override bool SaveEntity(business_unit_typeDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = Context.OrganizationId;
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic Lookup(String Id = null, int page = 1, int length = 100, String optionalFilters = null, String optionalParameters = null, String ParentUnitId = null)
        {
            dynamic result = null;
            try
            {
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";
                    #region Use filters
                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in business_unit_type.DefaultViewColumns)
                        {
                            filters = optionalFilters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                        }
                        filters = filters.Replace(";", "--");
                    }

                    parameters = optionalParameters;
                    int pageDraw = page;
                    int rowPerPage = length;

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var sql = Sql.Builder.Append(business_unit_type.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 AND r.organization_id=@0 ", Context.OrganizationId);

                            if (!String.IsNullOrEmpty(ParentUnitId))
                            {
                                var parent = business_unit.FirstOrDefault(" WHERE id=@0 ", ParentUnitId);
                                if (parent == null)
                                    throw new ArgumentException(" Parent unit not found! ");
                                var parentLevel = business_unit_type.FirstOrDefault(" WHERE id=@0", parent.business_unit_type_id);
                                if (parentLevel == null)
                                    sql.Append(String.Format(" AND r.business_unit_level < {0} ", 0));
                                else
                                    sql.Append(String.Format(" AND r.business_unit_level < {0} ", parentLevel.business_unit_level ?? 0));

                            }

                            if (!String.IsNullOrEmpty(filters))
                            {
                                if (String.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }



                            sql.Append(" ORDER BY r.business_unit_level ");
                            result = db.Page<vw_business_unit_type>(page, rowPerPage, sql);

                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex);
                        }
                        finally
                        {
                            appLogger.Debug(db.LastCommand);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Use Id
                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            int rowPerPage = 10;
                            var sql = Sql.Builder.Append(business_unit_type.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" AND r.organization_id = @0 ", Context.OrganizationId);
                            sql.Append(" ORDER BY r.business_unit_level ");
                            result = db.Page<vw_business_unit_type>(1, rowPerPage, sql);

                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex.ToString());
                        }
                        finally
                        {
                            appLogger.Debug(db.LastCommand);
                        }
                    }
                    #endregion
                }
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