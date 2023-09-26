using Phoenix.Shared.Core.Areas.General.Dtos;
using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Phoenix.Shared.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Entities;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data;
using Phoenix.Shared.Core.Areas.General.Dtos.Config;
using System.Data.SqlClient;
using System.Data;

namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface IFileServices
    {
        Task<GeneralResponse<FileMasterDto>> Upload(DamUpload files);
        Task<GeneralResponse<FileMasterDto>> UploadOneDrive(DamUpload files, MsGraphSetting modelAzure); 
        Task<GeneralResponse<FileMasterDto>> UploadOneDrivePeFolder(UploadFileDto files, MsGraphSetting modelAzure);
        Filemaster getfile(string id);
        Task<DownloadFileViewModel> Download(string id, MsGraphSetting modelAzure);
        Task<DownloadFileViewModel> DownloadPe(string id, MsGraphSetting modelAzure);
        Task<Filemaster> GetDataFile(string id);
        GeneralResponseList<FileMasterDto> GetFilemasterByRecordId(string recordId);
        GeneralResponseList<PeFileDto> GetFileByRecordId(string recordId);
    }
    public partial class FileServices : IFileServices
    {
        readonly DataContext context;
        private readonly FileConfig _fileconfig;
        private IEFRepository<Filemaster, string> _repo;
        private IEFRepository<PeFile, string> _repoFile;
        private IEFRepository<PeFileFolder, string> _repoFileFolder;
        const int twidth = 150;
        const int theight = 150;
        public FileServices(IOptions<FileConfig> settings, IEFRepository<Filemaster, string> repo, IEFRepository<PeFile, string> repoFile, IEFRepository<PeFileFolder, string> repoFileFolder, DataContext context)
        {
            _fileconfig = settings.Value;
            _repo = repo;
            _repoFile = repoFile;
            _repoFileFolder = repoFileFolder;
            this.context = context;
        }
        private bool ThumbnailCallback()
        {
            return false;
        }
        //private void createthumbnail(string inputPath)
        //{
        //    using (var ResourceImage = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
        //    {
        //        Image ReducedImage;

        //        Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

        //        ReducedImage = ResourceImage.GetThumbnailImage(twidth, theight, callb, IntPtr.Zero);
        //        ReducedImage.Save("E:\\temp\\filephoenix\\thumb.jpg");
        //    }

        //}
        public Filemaster getfile(string id)
        {
            return _repo.FindByID(id);
        }

        public PeFile getfilePe(string id)
        {
            return _repoFile.FindByID(id);
        }

        public async Task<GeneralResponse<FileMasterDto>> Upload(DamUpload files)
        {
            GeneralResponse<FileMasterDto> response = new GeneralResponse<FileMasterDto>();
            long size = files.file.Length;

            // full path to file in temp location
            var filePath = _fileconfig.Path;
            try
            {
                string fullPath = filePath + files.file.FileName;
                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);
                string newFullPath = fullPath;
                string newfilename = files.file.FileName;
                string mime = MimeTypes.GetMimeType(newfilename);
                int count = 0;
                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    newfilename = tempFileName + extension;
                    newFullPath = Path.Combine(filePath, tempFileName + extension);
                }

                Filemaster filemaster = new Filemaster();
                filemaster.filepath = filePath;
                filemaster.mimetype = mime;
                filemaster.size = size;
                filemaster.tags = files.tags;
                filemaster.name = newfilename;
                _repo.Create(false, filemaster);


                if (size > 0)
                {
                    using (var stream = new FileStream(newFullPath, FileMode.Create))
                    {
                        await files.file.CopyToAsync(stream);
                    }
                }
                //createthumbnail(newFullPath);

                _repo.SaveChanges();
                _repo.Dispose();

                response.Data = new FileMasterDto();
                response.Data.id = filemaster.Id.ToString();
                response.Data.name = filemaster.name;
                response.Data.tags = filemaster.tags;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            ;
            return response;
        }

        public async Task<GeneralResponse<FileMasterDto>> UploadOneDrivePeFolder(UploadFileDto files, MsGraphSetting modelAzure)
        {
            GeneralResponse<FileMasterDto> response = new GeneralResponse<FileMasterDto>();
            long size = files.file.Length;

            // full path to file in temp location
            if (string.IsNullOrEmpty(files.folderpath))
            {
                files.folderpath = (from a in _repoFileFolder.GetContext().Set<PeFileFolder>()
                                    where a.parent_id == files.uploadid
                                    select a.folder_path).FirstOrDefault();
            }

            var filePath = files.folderpath;
            try
            {
                string fileNameOnly = Path.GetFileNameWithoutExtension(files.name);
                string extension = Path.GetExtension(files.file.FileName);
                string pathFile = filePath + "/"+fileNameOnly + extension;

                PeFile peFileData = new PeFile();
                peFileData.file_name = files.name + extension;
                peFileData.tags = files.tags;
                peFileData.file_size = files.file.Length;
                peFileData.file_path = pathFile;
                peFileData.mimetype = GetMimeTypeByWindowsRegistry(peFileData.file_name);
                peFileData.parent_id = files.uploadid;
                peFileData.upload_date = DateTime.Now;
                peFileData.is_default = true;
                peFileData.is_active = true;
                peFileData.is_deleted = false;
                peFileData.file_type = extension;
                _repoFile.Create(false, peFileData);
                _repoFile.SaveChanges();
                _repoFile.Dispose();
                var streamFile = files.file.OpenReadStream();
                var graph = new MsGraph(modelAzure.clientId, modelAzure.tenantId);
                var authResult = await graph.GetAuthentication(modelAzure.office365Username, modelAzure.office365Password);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

                var sizes = (streamFile.Length / 1024) / 1024;
                if (sizes > 3)
                {
                    var item = await graphClient.UploadLargeFile(streamFile, pathFile, false);

                }
                else
                {
                    var item = await graphClient.UploadSmallFile(streamFile, pathFile, false);

                }

                if (!string.IsNullOrEmpty(files.name))
                {
                    var update = new Microsoft.Graph.DriveItem
                    {
                        Name = peFileData.file_name
                    };

                    await graphClient.Me.Drive.Root.ItemWithPath(pathFile)
                        .Request()
                        .UpdateAsync(update);
                }

                FileMasterDto dataDto = new FileMasterDto()
                {
                    id = peFileData.Id,
                    name = peFileData.file_name,
                    tags = peFileData.tags
                };
                response.Data = dataDto;
                response.Success = true;
                response.Message = "Success Upload File";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
        public async Task<GeneralResponse<FileMasterDto>> UploadOneDrive(DamUpload files, MsGraphSetting modelAzure)
        {
            GeneralResponse<FileMasterDto> response = new GeneralResponse<FileMasterDto>();
            long size = files.file.Length;
            var applicationEntityId = "";
            var recordid = "";
            // full path to file in temp location
            var filePath = _fileconfig.Path;
            try
            {
                string fileNameOnly = Path.GetFileNameWithoutExtension(files.file.FileName);
                string extension = Path.GetExtension(files.file.FileName);
                string pathFile = filePath + files.name + extension;

                Filemaster filemaster = new Filemaster();
                filemaster.tags = files.tags;
                filemaster.name = files.name + extension;
                filemaster.size = files.file.Length;
                filemaster.filepath = filePath;
                filemaster.mimetype = GetMimeTypeByWindowsRegistry(filemaster.name);
                filemaster.record_id = files.damUploadId;
                if (files.application_entity_id != null)
                {
                    try
                    {
                        SqlParameter[] parameter = {
                                new SqlParameter("@id",files.application_entity_id) };
                        DataTable dt = await context.ExecuteStoredProcedure("dbo.uspGetApplicationEntityId", parameter);
                        applicationEntityId = dt.Rows[0]["id"].ToString();
                        filemaster.application_entity_id = applicationEntityId;
                    }
                    catch 
                    {
                        applicationEntityId = "";
                    }
                  
                }
				//if (files.record_id != null)
				if (!string.IsNullOrEmpty(files.record_id))
				{
                    recordid = files.record_id;
                    filemaster.record_id = recordid;
                }

                var streamFile = files.file.OpenReadStream();
                var graph = new MsGraph(modelAzure.clientId,modelAzure.tenantId);
                var authResult = await graph.GetAuthentication(modelAzure.office365Username, modelAzure.office365Password);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                
                var sizes = (streamFile.Length / 1024) / 1024;
                if (sizes > 3)
                {
                    var item = await graphClient.UploadLargeFile(streamFile, pathFile, false);
                    
                }
                else
                {
                    var item = await graphClient.UploadSmallFile(streamFile, pathFile , false);

                }
               
                _repo.Create(false, filemaster);
                _repo.SaveChanges();
                _repo.Dispose();


                response.Data = new FileMasterDto();
                response.Data.id = filemaster.Id.ToString();
                response.Data.name = filemaster.name;
                response.Data.tags = filemaster.tags;
                response.Success = true;
                response.Message = "Success Upload File";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        private string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "application/unknown";
            string ext = (fileNameOrExtension.Contains(".")) ? Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public async Task<DownloadFileViewModel> Download(string id,MsGraphSetting modelAzure)
        {
            var data = getfile(id);
            if (data != null)
            {
                string path = $"{data.filepath}/{data.name}";
                //path = "/root/Notebooks
               
                var graph = new MsGraph(modelAzure.clientId, modelAzure.tenantId);
                var authResult = await graph.GetAuthentication(modelAzure.office365Username, modelAzure.office365Password);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                using (var stream = await graphClient.DownloadFile(path, false))
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var result = ms.ToArray();
                    return new DownloadFileViewModel { File = result, FileName = data.name, MimeType = data.mimetype };
                }
            }

            return new DownloadFileViewModel { File = null, FileName = data.name, MimeType = data.mimetype };
        }

        public async Task<DownloadFileViewModel> DownloadPe(string id, MsGraphSetting modelAzure)
        {
            var data = getfilePe(id);
            string path = "";
            if (data != null)
            {
                if (data.file_path.Contains(data.file_type))
                {
                    path = $"{data.file_path}";
                }
                else
                {
                    path = $"{data.file_path}/{data.file_name}";
                }
                
                //path = "/root/Notebooks

                var graph = new MsGraph(modelAzure.clientId, modelAzure.tenantId);
                var authResult = await graph.GetAuthentication(modelAzure.office365Username, modelAzure.office365Password);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                using (var stream = await graphClient.DownloadFile(path, false))
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var result = ms.ToArray();
                    return new DownloadFileViewModel { File = result, FileName = data.file_name, MimeType = data.file_type };
                }
            }

            return new DownloadFileViewModel { File = null, FileName = data.file_name, MimeType = data.file_type };
        }

        public async Task<Filemaster> GetDataFile(string id)
        {
            var data = getfile(id);
            return data;
        }

        public GeneralResponseList<FileMasterDto> GetFilemasterByRecordId(string recordId)
        {
            GeneralResponseList<FileMasterDto> resp = new GeneralResponseList<FileMasterDto>();
            List<FileMasterDto> query = new List<FileMasterDto>();
            try
            {
                if (recordId != null)
                {
                    query = (from q in _repo.GetContext().Set<Filemaster>()
                             where q.record_id == recordId
                             select new FileMasterDto()
                             {
                                 id = q.Id,
                                 name = q.name,
                                 tags = q.tags
                             }).ToList();
                }
               
                resp.Rows = query;
                resp.Success = true;
                resp.Code = "200";
                resp.Total = query.Count();
            }
            catch (Exception ex)
            {
                resp.Rows = null;
                resp.Success = false;
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Total = 0;
            }
            return resp;
            

        }

        public GeneralResponseList<PeFileDto> GetFileByRecordId(string recordId)
        {
            GeneralResponseList<PeFileDto> resp = new GeneralResponseList<PeFileDto>();
            List<PeFileDto> query = new List<PeFileDto>();
            try
            {
                if (recordId != null)
                {
                    query = (from q in _repo.GetContext().Set<PeFile>()
                             join r in _repo.GetContext().Set<Phoenix.Shared.Core.Entities.Task>() on q.parent_id equals r.job_number
                             where r.Id == recordId
                             select new PeFileDto()
                             {
                                 Id = q.Id,
                                 file_path = q.file_path,
                                 file_name = q.file_name,
                                 upload_date = q.upload_date,
                                 tags = q.tags
                             }).ToList();
                }

                resp.Rows = query;
                resp.Success = true;
                resp.Code = "200";
                resp.Total = query.Count();
            }
            catch (Exception ex)
            {
                resp.Rows = null;
                resp.Success = false;
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Total = 0;
            }
            return resp;


        }

    }
}
