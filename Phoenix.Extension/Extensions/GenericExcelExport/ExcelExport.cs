using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phoenix.ApiExtension.Extensions.GenericExcelExport
{
    public interface IAbstractDataExport
    {
        HttpResponseMessage Export(List<object> exportData, string fileName, string sheetName);
        DataSet ReadExcel(string path);
    }

    public abstract class AbstractDataExport : IAbstractDataExport
    {
        protected string _sheetName;
        protected string _fileName;
        protected List<string> _headers;
        protected List<string> _type;
        protected IWorkbook _workbook;
        protected ISheet _sheet;
        private const string DefaultSheetName = "Sheet1";

        public HttpResponseMessage Export
              (List<object> exportData, string fileName, string sheetName = DefaultSheetName)
        {
            _fileName = fileName;
            _sheetName = sheetName;

            _workbook = new XSSFWorkbook(); //Creating New Excel object
            _sheet = _workbook.CreateSheet(_sheetName); //Creating New Excel Sheet object

            var headerStyle = _workbook.CreateCellStyle(); //Formatting
            var headerFont = _workbook.CreateFont();
            headerFont.IsBold = true;
            //headerStyle.SetFont(headerFont);

            WriteData(exportData); //your list object to NPOI excel conversion happens here

            //Header
            var header = _sheet.CreateRow(0);
            for (var i = 0; i < _headers.Count; i++)
            {
                var cell = header.CreateCell(i);
                cell.SetCellValue(_headers[i]);
                //cell.CellStyle = headerStyle;
            }

            for (var i = 0; i < _headers.Count; i++)
            {
                _sheet.AutoSizeColumn(i);
            }

            using (var memoryStream = new MemoryStream()) //creating memoryStream
            {
                _workbook.Write(memoryStream);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(memoryStream.ToArray())
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue
                       ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue
                //       ("application/octet-stream");

                response.Content.Headers.ContentDisposition =
                       new ContentDispositionHeaderValue("attachment")
                       {
                           FileName = $"{_fileName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"
                       };

                return response;
            }
        }

        //Generic Definition to handle all types of List
        public abstract void WriteData(List<object> exportData);

        public DataSet ReadExcel(string path)
        {
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //var enviroment = System.Environment.CurrentDirectory;
            //string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
            string fullPath = Path.Combine(path);
            var physicalFile = new FileInfo(fullPath);
            using (var reader = physicalFile.OpenRead())
            {
                var dataSet = ExcelConverter.ExcelToDataSet(reader);
                var table = dataSet.Tables["Campaign Detail"];
                return dataSet;
            }
        }
        
        private void GenerateExcelByData(DataTable table)
        {
            table.Rows[1][1] = ": MEDIKON PRIMA LABORATORIES";
            table.Rows[2][1] = ": VERILE";
            table.Rows[3][1] = ": THE DANCE";
            table.Rows[4][1] = ": MARCH 2018";
            table.Rows[5][1] = ": 014.001/TV/VR/0318";
            table.Rows[6][1] = ": 06 APRIL 2018";
            table.Rows[1][3] = ": 0270/OPT/NPP/04-05/17";
            table.Rows[2][3] = ": 0270/OPT/NPP/04-05/17";

            table.Rows[10][0] = "GLOBAL TV";
            table.Rows[10][1] = "15";
            table.Rows[10][2] = "BIG MOVIES SIANG - FAMILY";
            table.Rows[10][3] = "TVC";
            table.Rows[10][4] = "3";
            table.Rows[10][5] = "SAT";
            table.Rows[10][6] = "14.00";
            table.Rows[10][7] = "15.31";
            table.Rows[10][8] = "1";
            table.Rows[10][9] = "21000";
            table.Rows[10][9] = "";

            table.Rows[11][2] = "30";
            table.Rows[11][8] = "30";
            table.Rows[11][9] = "525000000";

            table.Rows[12][2] = "30";
            table.Rows[12][9] = "112500000";
        }
    }
    public class AbstractDataExportBridge : AbstractDataExport
    {
        public AbstractDataExportBridge()
        {
            _headers = new List<string>();
            _type = new List<string>();
        }

        public override void WriteData(List<object> exportData)
        {
            //PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(object));

            DataTable table = new DataTable();

            //foreach (PropertyDescriptor prop in properties)
            //{
            //    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            //    _type.Add(type.Name);
            //    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
            //                      prop.PropertyType);
            //    string name = Regex.Replace(prop.Name, "([A-Z])", " $1").Trim(); //space separated 
            //                                                                     //name by caps for header
            //    _headers.Add(name);
            //}

            foreach (var obj in JObject.Parse(exportData[0].ToSafeString()))
            {
                _type.Add(obj.Value.ToString().GetType().Name);
                table.Columns.Add(obj.Key, obj.Value.ToString().GetType());
                string name = Regex.Replace(obj.Key, "([A-Z])", " $1").Trim(); //space separated 
                                                                                //name by caps for header
                _headers.Add(name);
            }

            foreach (object item in exportData)
            {
                var jObject = JObject.Parse(item.ToSafeString());
                DataRow row = table.NewRow();
                foreach (var obj in jObject)
                {
                    row[obj.Key] = obj.Value.ToString();
                }
                table.Rows.Add(row);
            }

            IRow sheetRow = null;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                sheetRow = _sheet.CreateRow(i + 1);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    ICell Row1 = sheetRow.CreateCell(j);

                    string type = _type[j].ToLower();
                    var currentCellValue = table.Rows[i][j];

                    if (currentCellValue != null &&
                        !string.IsNullOrEmpty(Convert.ToString(currentCellValue)))
                    {
                        if (type == "string")
                        {
                            Row1.SetCellValue(Convert.ToString(currentCellValue));
                        }
                        else if (type == "int32")
                        {
                            Row1.SetCellValue(Convert.ToInt32(currentCellValue));
                        }
                        else if (type == "double")
                        {
                            Row1.SetCellValue(Convert.ToDouble(currentCellValue));
                        }
                    }
                    else
                    {
                        Row1.SetCellValue(string.Empty);
                    }
                }
            }
        }
    }
}
