using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Services;
using CommonTool.KendoUI.Grid;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;

namespace BusinessLogic.Services.General
{
    public partial class ApplicationUserService
    {
        public string GetImageProfile(string Id, bool Thumbnail, ref string mimeType)
        {
            try
            {
                var record = application_user.FirstOrDefault(" WHERE id=@0", Id);
                if (record == null) return string.Empty;
                mimeType = (string.IsNullOrEmpty(record.mime_type) ? "image/jpeg" : record.mime_type);
                return (Thumbnail == true ? record.thumbnail_base64 : record.photo_base64);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
        }

        public virtual DataResponse GetListKendoGrid(DataRequest DTRequest)
        {
            DataResponse result = null;

            try
            {
                String OptionalFilters = string.Empty;
                String OptionalParameters = string.Empty;
                String OptionalQueryBuilder = string.Empty;
                appLogger.Debug("Horeee");
                OptionalFilters = " r.organization_id=@0" ;
                OptionalParameters = Context.OrganizationId;
                result = Context.GetListKendoUIGrid<vw_application_user>(DTRequest, OptionalFilters, OptionalParameters, OptionalQueryBuilder);
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