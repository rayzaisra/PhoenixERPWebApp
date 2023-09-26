using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Phoenix.ApiExtension.Extensions
{
    public static class ExcelConverter
    {
        public static DataSet ExcelToDataTable(this byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return stream.ExcelToDataSet();
            }
        }

        public static DataSet ExcelToDataSet(this Stream stream)
        {
            IWorkbook workbook = WorkbookFactory.Create(stream);  //IWorkbook xls dan xlsx       
            var ds = new DataSet("Excel Workbook");

            var sheetCount = workbook.NumberOfSheets;
            for (var sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
            {
                var worksheet = workbook.GetSheetAt(sheetIndex);
                //if (worksheet.GetType() == typeof(HSSFSheet))
                //{
                //    var sheet = ((HSSFSheet)worksheet).IsActive;
                //}
                //else
                //{
                //    var sheet = ((XSSFSheet)worksheet).ActiveCell;
                //}

                var table = new DataTable(worksheet.SheetName);
                short? colCount = 0;
                //for (var c = 0; c < worksheet.LastRowNum; c++)
                //{
                //    int x = worksheet.GetRow(c).LastCellNum;
                //    if (x > colCount)
                //        colCount = x;
                //}
                var obj = new object();
                Parallel.For(0, worksheet.LastRowNum, index =>
                {
                    var count = worksheet.GetRow(index)?.LastCellNum;
                    lock (obj)
                    {
                        if (count > colCount)
                            colCount = count;
                    }
                });

                for (int c = 0; c < colCount; c++)
                    table.Columns.Add(GetExcelColumnName(c + 1)/*, typeof(string)*/);

                //IEnumerator rows = worksheet.GetRowEnumerator();
                //while (rows.MoveNext())
                for (var c = 0; c < (worksheet.LastRowNum + 1); c++)
                {
                    //IRow row = null;
                    //if (rows.Current is HSSFRow)
                    //    row = (HSSFRow)rows.Current;
                    //else
                    //    row = (XSSFRow)rows.Current;

                    var dr = table.NewRow();

                    IRow row = worksheet.GetRow(c);

                    if (row == null)
                    {
                        table.Rows.Add(dr);
                        continue;
                    }

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        SetRowValueAsString(dr, i, cell);
                    }

                    table.Rows.Add(dr);
                }

                table.AcceptChanges();
                ds.Tables.Add(table);
            }
            return ds;
        }

        public static List<T> ExcelToEntity<T>(this Stream stream, int sheetIndex = 0, int titleIndex = 0, int dataIndex = 1) where T : class
        {
            List<T> entities = new List<T>();

            IWorkbook workbook = WorkbookFactory.Create(stream);  //IWorkbook xls dan xlsx       

            var worksheet = workbook.GetSheetAt(sheetIndex);

            //var table = new DataTable(worksheet.SheetName);
            var cellTitle = worksheet.GetRow(titleIndex).Cells;

            for (var c = dataIndex; c < (worksheet.LastRowNum + 1); c++)
            {
                T entity = Activator.CreateInstance<T>();

                IRow row = worksheet.GetRow(c);

                if (row == null)
                {
                    entities.Add(entity);
                    continue;
                }

                for (int i = 0; i < cellTitle.Count; i++)
                {
                    ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    entity.SetInstanceField(cellTitle[i].ToSafeString(), GetCellValue(cell));
                }

                entities.Add(entity);
            }

            return entities;
        }

        private static void SetRowValueAsString(DataRow row, int index, ICell cell)
        {
            var cellType = cell.CellType;
            if (cell.CellType == CellType.Formula)
                cellType = cell.CachedFormulaResultType;

            switch (cellType)
            {
                case CellType.Error:
                    row[index] = cell.ErrorCellValue.ToSafeString();
                    break;
                case CellType.Unknown:
                    row[index] = cell.ToSafeString();
                    break;
                case CellType.Blank:
                    row[index] = null;
                    break;
                case CellType.Boolean:
                    row[index] = cell.BooleanCellValue.ToSafeString().ToLower();
                    break;
                case CellType.String:
                    row[index] = cell.StringCellValue.ToSafeString();
                    break;
                case CellType.Numeric:
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                        row[index] = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        row[index] = cell.NumericCellValue.ToSafeString();
                    break;
                default:
                    row[index] = cell.ToSafeString();
                    break;
            }
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        private static string GetInstanceField(object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
            return Convert.ToString(field.GetValue(instance));
        }

        private static object GetCellValue(ICell cell)
        {
            var cellType = cell.CellType;
            if (cell.CellType == CellType.Formula)
                cellType = cell.CachedFormulaResultType;

            switch (cellType)
            {
                case CellType.Error:
                    return cell.ErrorCellValue.ToSafeString();
                case CellType.Unknown:
                    return cell.ToSafeString();
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;//.ToSafeString().ToLower();
                case CellType.String:
                    return cell.StringCellValue;//.ToSafeString();
                case CellType.Numeric:
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                        return cell.DateCellValue;//.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        return cell.NumericCellValue;//.ToSafeString();
                default:
                    return cell.ToSafeString();
            }
        }

        #region Write To Excel By M.Salih
        public static void DataTableToExcel(DataTable data, string filePath)
        {
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            var memoryStream = new MemoryStream();

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                List<String> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (DataRow dsrow in table.Rows)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;
                    foreach (String col in columns)
                    {
                        if (string.IsNullOrEmpty(dsrow[col].ToString()))
                            row.CreateCell(cellIndex).SetCellValue("NULL");
                        else
                            row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                        cellIndex++;
                    }
                    rowIndex++;
                }
                workbook.Write(fs);
            }
        }
        #endregion
    }

}
