using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.Models.Um;
using Phoenix.ApiExtension.Helpers;

namespace Phoenix.Data
{
    public interface IFileService
    {
        Task<DownloadFileViewModel> Download(string id);
    }

    public class FileService : IFileService
    {
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage MasterDivisionSelfservice
        /// </summary>
        /// <param name="context">Database context</param>
        public FileService(DataContext context)
        {
            this.context = context;
        }

        public async Task<string> Upload(IFormFile file, string id, string location="")
        {
            string configPath = string.Empty;
            if (location == "HRIS")
                configPath = DataConfiguration.Configuration.FileConfig.PathEmployee;
            else
                configPath = DataConfiguration.Configuration.FileConfig.Path;
            var master = new Filemaster();

            if (file != null)
            {
                var pathFileName = $@"{configPath}/{file.FileName}";
                if (string.IsNullOrEmpty(id))
                    master.Id = Guid.NewGuid().ToString();
                else
                    master.Id = id;
                master.IsActive = true;
                master.Name = file.FileName;
                master.Size = Convert.ToInt32(file.Length);
                master.Filepath = configPath;
                master.Mimetype = GetMimeTypeByWindowsRegistry(file.FileName);


                var vmAz = GetOnlyOne().Result;
                var graph = new MsGraph(vmAz.ClientId, vmAz.TenantId);
                var authResult = await graph.GetAuthentication(vmAz.EmailAccount, vmAz.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                var streamFile = file.OpenReadStream();
                var size = (streamFile.Length / 1024) / 1024;
                if (size > 3)
                {
                    var item = await graphClient.UploadLargeFile(streamFile, pathFileName, false);
                }
                else
                {
                    var item = await graphClient.UploadSmallFile(streamFile, pathFileName, false);
                }

                if (string.IsNullOrEmpty(id))
                    context.PhoenixAdd(master);
                else
                    context.PhoenixEdit(master);

                await context.SaveChangesAsync();

            }

            return master.Id;
        }

        private string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "application/unknown";
            string ext = (fileNameOrExtension.Contains(".")) ? Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public async Task<DownloadFileViewModel> Download(string id)
        {
            var data = context.Filemasters.Where(x => x.Id == id).FirstOrDefault();
            if (data != null)
            {
                string path = $"{data.Filepath}/{data.Name}";
                //path = "/root/Notebooks
                var vmAz = GetOnlyOne().Result;
                var graph = new MsGraph(vmAz.ClientId, vmAz.TenantId);
                var authResult = await graph.GetAuthentication(vmAz.EmailAccount, vmAz.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                using (var stream = await graphClient.DownloadFile(path, false))
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var result = ms.ToArray();
                    return new DownloadFileViewModel { File = result, FileName = data.Name };
                }
            }

            return new DownloadFileViewModel { File = null, FileName = data.Name };
        }

        private async Task<AzurePortalSetup> GetOnlyOne()
        {
            var data = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
                              select new AzurePortalSetup
                              {
                                  Id = x.Id,
                                  TenantId = x.TenantId,
                                  ClientId = x.ClientId,
                                  EmailAccount = x.EmailAccount,
                                  SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                              }).Take(1).FirstOrDefaultAsync();
            return data;
        }
    }
    public class DownloadFileViewModel
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public string MimeType { get; set; }
    }
}
