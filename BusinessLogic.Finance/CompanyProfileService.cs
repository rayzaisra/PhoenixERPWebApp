using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP;
using NPoco;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Linq;

namespace BusinessLogic.Finance
{
    public partial class Company : CompanyService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Company(DataContext AppUserData) : base(AppUserData)
        {

        }

        public DataTablesResponseNetCore GetListDataTablesVendor(IDataTablesRequest DTRequest)
        {
            DataTablesResponseNetCore result = null;
            try
            {
                var OptionalFilters = " COALESCE(r.is_vendor,0) = @0 ";
                var OptionalParameters = "1";
                result = Context.GetListDataTablesMarvelBinder<vw_company>(DTRequest, OptionalFilters, OptionalParameters);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataTablesResponseNetCore GetListDataTablesClient(IDataTablesRequest DTRequest)
        {
            DataTablesResponseNetCore result = null;
            try
            {
                var OptionalFilters = " COALESCE(r.is_client,0) = @0 ";
                var OptionalParameters = "1";
                result = Context.GetListDataTablesMarvelBinder<vw_company>(DTRequest, OptionalFilters, OptionalParameters);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }


        public bool SaveEntity(CompanyDetailsDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var scope = new TransactionScope())
            {
                try
                {
                    var _company = RecordDto.company;
                    var _companyAddress = RecordDto.company_address;
                    var _companyBank = RecordDto.company_bank;
                    var _companyContact = RecordDto.company_contact;

                    if (_company == null) return result;

                    _company.organization_id = Context.OrganizationId;
                    result = Svc.SaveEntity(_company, ref isNew);
                    // check Company has saved
                    if (result)
                    {
                        //Save Address Collection
                        var _companyAddressService = new CompanyAddressService(Context);
                        var _companyBankService = new CompanyBankService(Context);
                        var _companyContactService = new CompanyContactService(Context);
                        //var _companyContactService = new CompanyContactService(Context);
                        foreach (var record in _companyAddress)
                        {
                            if (record == null) continue;

                            var isNewAddress = false;
                            record.company_id = _company.id;
                            result &= _companyAddressService.Svc.SaveEntity(record, ref isNewAddress);
                        }

                        // check All Address has saved
                        if (result)
                        {
                            foreach (var record in _companyBank)
                            {
                                if (record == null) continue;

                                var isNewBank = false;
                                record.company_id = _company.id;
                                result &= _companyBankService.Svc.SaveEntity(record, ref isNewBank);
                            }
                        }

                        // check All Bank has saved
                        if (result)
                        {
                            foreach (var record in _companyContact)
                            {
                                if (record == null) continue;

                                var isNewBank = false;
                                record.company_id = _company.id;
                                result &= _companyContactService.Svc.SaveEntity(record, ref isNewBank);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
                finally
                {
                    if (result)
                    {
                        scope.Complete();
                    }
                }
            }

            return result;
        }

        public dynamic Lookup(string Id = null, int Page = 1, int Length = 100,
           string OptionalFilters = null, string OptionalParameters = null)
        {
            dynamic result = null;

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(OptionalFilters))
                    {
                        foreach (var col in company.DefaultViewColumns)
                        {
                            filters = OptionalFilters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                        }
                        filters = filters.Replace(";", "--");
                    }

                    parameters = OptionalParameters;
                    int pageDraw = Page;
                    int rowPerPage = Length;

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var sql = Sql.Builder.Append(company.DefaultView);
                            //sql.Append(" WHERE r.is_active = 1 AND r.organization_id=@0 ",Context.OrganizationId);
                            sql.Append(" WHERE r.is_active = 1 ");
                            if (!string.IsNullOrEmpty(filters))
                            {
                                if (string.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(string.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(string.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.company_name ");
                            result = db.Page<vw_company>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(company.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                           // sql.Append(" AND r.organization_id = @0 ", Context.OrganizationId);
                            
                            sql.Append(" ORDER BY r.company_name ");

                            result = db.Page<vw_company>(1, rowPerPage, sql);
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

        public new vw_CompanyDetailsDTO Details(string Id)
        {
            vw_CompanyDetailsDTO result = new vw_CompanyDetailsDTO();

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    
                    var sql = Sql.Builder.Append(company.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_company>(sql);
                    
                    if (view != null)
                    {
                        result.company = (vw_companyDTO)((new vw_companyDTO()).InjectFrom(view));

                        sql = Sql.Builder.Append(company_address.DefaultView);
                        sql.Append(" WHERE r.is_active = 1 ");
                        sql.Append(" AND r.company_id = @0 ", Id);

                        var viewAddress = db.Fetch<vw_company_address>(sql);
                        appLogger.Debug(db.LastCommand);
                        if (view != null)
                        {
                            //result.company_address = (List<vw_company_addressDTO>)((new List<vw_company_addressDTO>()).InjectFrom(viewAddress));
                            result.company_address = viewAddress
                                .Select(x => new vw_company_addressDTO().InjectFrom(x)).Cast<vw_company_addressDTO>().ToList();
                        }

                        sql = Sql.Builder.Append(company_bank.DefaultView);
                        sql.Append(" WHERE r.is_active = 1 ");
                        sql.Append(" AND r.company_id = @0 ", Id);

                        var viewBank = db.Fetch<vw_company_bank>(sql);
                        if (view != null)
                        {
                            //result.company_bank = (List<vw_company_bankDTO>)((new List<vw_company_bankDTO>()).InjectFrom(viewBank));
                            result.company_bank = viewBank
                                .Select(x => new vw_company_bankDTO().InjectFrom(x)).Cast<vw_company_bankDTO>().ToList();
                        }

                        sql = Sql.Builder.Append(company_contact.DefaultView);
                        sql.Append(" WHERE r.is_active = 1 ");
                        sql.Append(" AND r.company_id = @0 ", Id);

                        var viewContact = db.Fetch<vw_company_contact>(sql);
                        if (view != null)
                        {
                            //result.company_contact = (List<vw_company_contactDTO>)((new List<vw_company_contactDTO>()).InjectFrom(viewContact));
                            result.company_contact = viewContact
                                .Select(x => new vw_company_contactDTO().InjectFrom(x)).Cast<vw_company_contactDTO>().ToList();
                        }
                    }
                    else
                    {
                        result = null;
                    }                                       
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(company.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_company>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.company_name ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_company>(pageDraw, rowPerPage, sql);
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
            }
            return result;
        }
    }
}