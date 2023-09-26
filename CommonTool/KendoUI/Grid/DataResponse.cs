using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.KendoUI.Grid
{
    public class DataResponse
    {
        public int RecordsTotal { get; protected set; }
        public object Data { get; protected set; }
        public List<Group> Group { get; protected set; }

        public IDictionary<string, object> AdditionalParameters { get; protected set; }

        protected DataResponse(int totalRecords, List<Group> group, object data)
         : this(totalRecords,group, data, null)
        { }

        protected DataResponse(int totalRecords, List<Group> group, object data, IDictionary<string, object> additionalParameters)
        {
            RecordsTotal = totalRecords;
            Group = group;
            Data = data;
            AdditionalParameters = additionalParameters;
        }

        public static DataResponse Create(DataRequest request, int totalRecords, int totalRecordsFiltered, object data)
        {
            return DataResponse.Create(request, totalRecords, totalRecordsFiltered, data, null);
        }

        public static DataResponse Create(DataRequest request, int totalRecords, int totalRecordsFiltered, object data, IDictionary<string, object> additionalParameters)
        {
            // When request is null, there should be no response (null response).
            if (request == null) return null;

            return new DataResponse(totalRecords,request.group ,data, additionalParameters);
        }
    }
}
