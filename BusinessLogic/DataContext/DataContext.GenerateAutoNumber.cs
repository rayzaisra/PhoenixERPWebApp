using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PhoenixERP;
using DataAccess;
using System.Dynamic;
using Newtonsoft.Json;
using DataAccess.PhoenixERP.General;
using NPoco;
using CommonTool.Helper;

namespace BusinessLogic
{
    public partial class DataContext
    {
        public string GenerateAutoNumber(string SequenceName, string Prefix, 
            int NumberLength = 6, int SuffixLength = 6, char PaddingChar = '0')
        {
            string result = null;            

            using(var db = new PhoenixERPRepo())
            {
                try
                {
                    // Check sequence
                    var sql = Sql.Builder.Append(@"SELECT COALESCE(COUNT(*), 0) FROM sys.sequences");
                    sql.Append("WHERE object_id = OBJECT_ID(@0)", SequenceName);
                    var n = db.ExecuteScalar<int>(sql);
                    if(n > 0)
                    {
                        sql = Sql.Builder.Append(string.Format("SELECT NEXT VALUE FOR {0}", SequenceName));
                        var val = db.ExecuteScalar<long>(sql);

                        int numberLength = NumberLength < 1 ? 1 : NumberLength;
                        long maxVal = (long)Math.Pow(10, numberLength);
                        val = val % maxVal;

                        result = "";
                        if (!string.IsNullOrEmpty(Prefix))
                        {
                            result += Prefix + "-";
                        }

                        if(numberLength > 0)
                        {
                            result += string.Format("{0}", val.ToString().PadLeft(numberLength, PaddingChar));
                        }
                        else
                        {
                            result += string.Format("{0}", val.ToString());
                        }

                        if (SuffixLength > 0)
                        {
                            result += string.Format("-{0}", RandomBase21.Generate(SuffixLength));
                        }
                    }
                    else
                    {
                        appLogger.Error("Sequence {0} does not exist", SequenceName);                        
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex.ToString);                    
                }
            }

            return result;
        }
    }
}
