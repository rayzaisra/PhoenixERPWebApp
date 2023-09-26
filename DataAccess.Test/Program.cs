using System;
using System.Collections.Generic;
using System.Collections;
using DataAccess.PhoenixERP;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.PhoenixERP.General;

namespace DataAccess.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var services = new ServiceCollection();

            ////BEGIN::MASTERNPOCO
            using (var dbContext = new PhoenixERPRepo())
            {
                var data = dbContext.FirstOrDefault<application_user>(" WHERE app_username=@0", "admin_codemarvel");
                using (var scope = dbContext.GetTransaction())
                {
                    try
                    {
                        var record = new application_user();
                        record.id = Guid.NewGuid().ToString("D");
                        record.business_unit_id = data.business_unit_id;
                        record.app_username = "admin_baru";
                        record.app_password = data.app_password;
                        record.app_fullname = "Administrator Poco netcore";
                        dbContext.Insert(record);
                    }
                    catch (Exception)
                    {

                    }
                }

                Console.WriteLine(JsonConvert.SerializeObject(data));
            }

            using (var dbContext = new PhoenixERPRepo())
            {
                var data = dbContext.Fetch<application_user>(" WHERE is_default=@0", false);
                Console.WriteLine(JsonConvert.SerializeObject(data.Select(o => o.app_fullname)));
            }

            //END::MASTERNPOCO

            ////BEGIN::NPOCO

            ////var record = application_user.FirstOrDefault("WHERE app_username=@0","admin_codemarvel");
            ////Console.WriteLine(record.app_fullname);
            //using (var dbContext = new MarvelNPoco())
            //{
            //    var data = dbContext.Database.Fetch<application_user>(" WHERE 1=1");
            //    Console.WriteLine(JsonConvert.SerializeObject(data));
            //}

            //using (var dbContext = new MarvelNPoco())
            //{
            //    var data = dbContext.Database.Fetch<application_user>(" WHERE is_default=@0",false);
            //    Console.WriteLine(JsonConvert.SerializeObject(data.Select(o=>o.app_fullname)));
            //}

            ////END::NPOCO

            //var dbContext = new ERPPhoenixDB();
            //var data = dbContext.db.FirstOrDefaultAsync<application_user>(" WHERE app_username=@0", "admin_codemarvel").Result;
            //using (var scope = dbContext.db.GetTransactionAsync())
            //{

            //    var record = new application_user();
            //    record.id = Guid.NewGuid().ToString("D");
            //    record.organization_id = data.organization_id;
            //    record.app_username = "admin_baru";
            //    record.app_password = data.app_password;
            //    record.app_fullname = "Administrator Poco netcore";
            //    dbContext.db.InsertAsync(record);

            //}

            //using (var db = new DatabaseMarvel())
            //{

            //    using (var scope = db.GetTransactionAsync())
            //    {
            //        var data = db.FirstOrDefaultAsync<application_user>(" WHERE app_username=@0", "admin_codemarvel").Result;
            //        var record = new application_user();
            //        record.id = Guid.NewGuid().ToString("D");
            //        record.organization_id = data.organization_id;
            //        record.app_username = "admin_baru";
            //        record.app_password = data.app_password;
            //        record.app_fullname = "Administrator Poco netcore";
            //        var ss = db.InsertAsync(record).Result;

            //    }
            //    var result = db.FetchAsync<application_user>(" WHERE is_default=@0", true).Result;

            //}

            //var result = dbContext.Repo.InsertAsync(record).Result;sss            
            //getUser(true);
            //getUser(false);
            var r = getUser().Result;
            Console.Read();
        }

        static async Task<string> getUser()
        {
            string r = "asd";
            using (var dbContext = new PhoenixERPRepo())
            {
                var data = await dbContext.FetchAsync<application_user>(" WHERE 1=1");
                Console.WriteLine(JsonConvert.SerializeObject(data.Select(o => o.app_fullname)));
            }
            return r;
        }
    }
}