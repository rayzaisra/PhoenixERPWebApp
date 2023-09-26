using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonTool.QueryBuilderGeneral;
using DataAccess.PhoenixERP.Finance;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using static CommonTool.QueryBuilderGeneral.QueryBuilderFilter;
using DataAccess.PhoenixERP.General;
using System.Reflection;


namespace PhoenixWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    [Phoenix.Data.Attributes.AllowAnonymousRole]
    public class ValuesController : ControllerBase
    {
        
        protected NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();
        // GET api/values
        [HttpGet]
        public ActionResult<IActionResult> Get()
        {
            IDictionary<string, object> paa = new Dictionary<string, object>();
            paa.Add("Query", "TEST");

            return Ok(paa);
        }

        [HttpPost]
        [Route("kendoTest")]
        public ActionResult<IActionResult> KendoTEST(CommonTool.KendoUI.Grid.DataRequest request)
        {
            appLogger.Debug("KENDO TEST");
            appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(request)); 
            
            var response = CommonTool.KendoUI.Grid.DataResponse.Create(request, 0, 0, new List<vw_application_userDTO>());
            return Ok(response);
        }

        [HttpGet]
        [Route("ngetest")]
        public async Task<IActionResult>  GetQueryBuilderFilterOption()
        {

            var saveResult = await Task.Run<QueryBuilderSettings>(() =>
            {
                string QBID;
                string QBLabel;
                string propertyInputType;
                QueryBuilderDataType QBDataType;
                List<QueryBuilderFilterOperators> QBFilterOperators;
                string propertyType;
                QueryBuilderInputType QBInputType;
                //List<object> dropdownValues = new List<object>();
                Dictionary<string, string> dropdownValues = new Dictionary<string, string>();

                QueryBuilderSettings settings = new QueryBuilderSettings();

                // plugins
                //settings.plugins.Add("bt-tooltip-errors");
                //settings.plugins.Add("not-group");
                //settings.plugins.Add("sortable");

                // filters

                Type recordType = typeof(cityDTO);
                foreach (var pi in recordType.GetProperties())
                {
                    try
                    {
                       
                        if (pi != null)
                        {
                            if ((pi.Name != "id") && (pi.Name != "created_on")) continue;

                            Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                            
                            QBID = pi.Name;
                            QBLabel = pi.Name;
                            propertyInputType = t.Name;
                            QBDataType = QueryBuilderFilter.GetQueryBuilderDataType(propertyInputType);
                            propertyType = t.Name;
                            QBInputType = QueryBuilderFilter.GetQueryBuilderInputType(propertyType);
                            QBFilterOperators = QueryBuilderFilter.GetQueryBuilderFilterOperator(QBInputType);
                            dropdownValues = null;
                            appLogger.Debug($"QBID : {QBID}");
                            appLogger.Debug($"propertyInputType : {propertyInputType}");
                            appLogger.Debug($"QBDataType : {QBDataType}");
                            appLogger.Debug($"QBInputType : {QBInputType}");
                            settings.filters.Add(new QueryBuilderFilter(QBID, QBLabel, QBDataType, QBFilterOperators, QBInputType, dropdownValues));
                        }
                        



                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                    }

                }
                //foreach (string propertyName in PropertyNames)
                //{
                //    QBID = propertyName;
                //    QBLabel = PropertyTitle[propertyName];
                //    propertyInputType = GetPropertyInputType(propertyName);
                //    QBDataType = QueryBuilderFilter.GetQueryBuilderDataType(propertyInputType);
                //    propertyType = GetPropertyType(propertyName);
                //    QBInputType = QueryBuilderFilter.GetQueryBuilderInputType(propertyType);
                //    QBFilterOperators = QueryBuilderFilter.GetQueryBuilderFilterOperator(QBInputType);

                //    dropdownValues = GetDropdownDictionary(propertyName);

                //    settings.filters.Add(new QueryBuilderFilter(QBID, QBLabel, QBDataType, QBFilterOperators, QBInputType, dropdownValues));
                //    //dropdownValues =  new List<object>();
                //    dropdownValues = new Dictionary<string, string>(); //Clear the Dictionary or it will add up all dropdown from different properties
                //}

                return settings;

            });
            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }


        [HttpPost]
        [Route("listdatatablesbinder")]
        public void ListDataTablesBinder(IDataTablesRequest dtRequest)
        {
            //var test = DataTableRequest.Draw;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
