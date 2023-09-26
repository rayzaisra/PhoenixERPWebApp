using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extenso;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Phoenix.ApiExtension;
using Phoenix.ApiExtension.Helpers;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Um;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Responses;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/odrive")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class OneDriveOfficeController : Controller
    {
        private IHostingEnvironment environment;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        private readonly IMappingFolderOneDriveService _repoMapping;
        private FileConfig fileConfig;
        public OneDriveOfficeController(IHostingEnvironment environment, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure, IMappingFolderOneDriveService repoMapping, IOptions<FileConfig> FileConfig)
        {
            this.environment = environment;
            this._repoAzure = repoAzure;
            _repoMapping = repoMapping;
            fileConfig = FileConfig.Value;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getroot")]
        [Route("getroot/{folder?}")]
        public async Task<IActionResult> GetRootFile(string folder = "")
        {
            if (!string.IsNullOrEmpty(folder))
                folder = folder.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId,model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
            var groupid = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetGroupAccess(HttpContext);
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var listItemId = await _repoMapping.Get(userId,groupid);
            if (string.IsNullOrEmpty(folder))
            {
                var driveItem = await graphClient.Me.Drive.Root.Children.Request().GetAsync();

                var listItem = new List<VmDriveItem>();
                foreach (var item in driveItem)
                {
                    foreach (var x in listItemId)
                    {
                        if (x.ItemId == item.Id) {
                            var vm = new VmDriveItem()
                            {
                                File = item.File,
                                Folder = item.Folder,
                                Name = item.Name,
                                LastModifiedDateTime = item.LastModifiedDateTime,
                                LastModifiedBy = item.LastModifiedBy.User.DisplayName,
                                ChildCount = item.Folder.ChildCount,
                                Size = item.Size
                            };
                            listItem.Add(vm);
                        }
                    }
                }
                return Ok(listItem.GroupBy(x=>x.Name).Select(grp => grp.First()));
            }
            else {
                var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + folder).Children.Request().GetAsync();
                var listItem = new List<VmDriveItem>();
                foreach (var item in driveItem)
                {
                    foreach (var x in listItemId)
                    {
                        if (x.ItemId == item.Id)
                        {
                            var vm = new VmDriveItem()
                            {
                                File = item.File,
                                Folder = item.Folder,
                                Name = item.Name,
                                LastModifiedDateTime = item.LastModifiedDateTime,
                                LastModifiedBy = item.LastModifiedBy.User.DisplayName,
                                ChildCount = item.Folder.ChildCount,
                                Size = item.Size
                            };
                            listItem.Add(vm);
                        }
                    }
                }
                return Ok(listItem.GroupBy(x => x.Name).Select(grp => grp.First()));
            }
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getrootpe")]
        [Route("getrootpe/{folder}")]
        public async Task<IActionResult> getrootpe(string folder = "")
        {
            if (!string.IsNullOrEmpty(folder))
               folder = folder.Replace('*', '/');

            string[] data = folder.Split(new string[] { "::" }, StringSplitOptions.None);
            var path = data[0];
            var jobid = data[1];
        
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            
            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var fullpath = pathRelative + "\\fileOneDrive\\";

            var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + path).Children.Request().GetAsync();

            var listItem = new List<VmDriveItem>();
            foreach (var item in driveItem)
            {
                var userUpload = await _repoMapping.GetUserUploadFIle(jobid, item.Name);
                var vm = new VmDriveItem();
                vm.File = item.File;
                vm.Folder = item.Folder;
                vm.Name = item.Name;
                vm.LastModifiedDateTime = item.LastModifiedDateTime;
                vm.WebUrl = fullpath + item.Name; 
                if (!string.IsNullOrEmpty(userUpload.Id))
                {
                    vm.LastModifiedBy = userUpload.AliasName;
                }
                else
                {
                    vm.LastModifiedBy = "";
                }

                vm.Size = item.Size;
                
                if (item.Folder != null)
                {
                    vm.ChildCount = item.Folder.ChildCount;
                }
                else
                {
                    vm.ChildCount = 0;
                }

                listItem.Add(vm);
            }
            return Ok(listItem.GroupBy(x => x.Name).Select(grp => grp.First()));
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("getrootmydrive")]
        [Route("getrootmydrive/{folder}")]
        public async Task<IActionResult> getrootmydrive(string folder = "")
        {
            if (!string.IsNullOrEmpty(folder))
                folder = folder.Replace('*', '/');
            string[] data = folder.Split(new string[] { "::" }, StringSplitOptions.None);
            var folderName = data[0];
            var shareToId = data[1];

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var fullpath = pathRelative + "\\fileOneDrive\\";

            try
            {
                var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + folderName).Children.Request().GetAsync();

                var listItem = new List<VmDriveItem>();
                foreach (var item in driveItem)
                {
                    var myDrive = await _repoMapping.GetDataShareMyDrive(shareToId);

                    var vmx = new VmDriveItem();
                    vmx.File = item.File;
                    vmx.Folder = item.Folder;
                    vmx.Name = item.Name;
                    vmx.LastModifiedDateTime = item.LastModifiedDateTime;
                    vmx.WebUrl = fullpath + item.Name;
                    string[] arrFol = folderName.Split(new string[] { "/" }, StringSplitOptions.None);
                    vmx.LastModifiedBy = arrFol[1];

                    vmx.Size = item.Size;

                    if (item.Folder != null)
                    {
                        vmx.ChildCount = item.Folder.ChildCount;
                    }
                    else
                    {
                        vmx.ChildCount = 0;
                    }

                    listItem.Add(vmx);


                    foreach (var itemDrive in myDrive)
                    {
                        if (item.Name.Contains(".*"))
                        {
                            if (item.Name == itemDrive.FileName)
                            {
                                var vm = new VmDriveItem();
                                vm.File = item.File;
                                vm.Folder = item.Folder;
                                vm.Name = item.Name;
                                vm.LastModifiedDateTime = item.LastModifiedDateTime;
                                vm.JobId = itemDrive.JobId;
                                if (!string.IsNullOrEmpty(itemDrive.Id))
                                {
                                    vm.LastModifiedBy = itemDrive.AliasName;
                                }
                                else
                                {
                                    vm.LastModifiedBy = "";
                                }

                                vm.Size = item.Size;

                                if (item.Folder != null)
                                {
                                    vm.ChildCount = item.Folder.ChildCount;
                                }
                                else
                                {
                                    vm.ChildCount = 0;
                                }

                                listItem.Add(vm);
                            }

                        }
                        else
                        {
                            var vm = new VmDriveItem();
                            vm.File = item.File;
                            vm.Folder = item.Folder;
                            vm.Name = item.Name;
                            vm.LastModifiedDateTime = item.LastModifiedDateTime;
                            vm.JobId = itemDrive.JobId;
                            if (!string.IsNullOrEmpty(itemDrive.Id))
                            {
                                vm.LastModifiedBy = itemDrive.AliasName;
                            }
                            else
                            {
                                vm.LastModifiedBy = "";
                            }

                            vm.Size = item.Size;

                            if (item.Folder != null)
                            {
                                vm.ChildCount = item.Folder.ChildCount;
                            }
                            else
                            {
                                vm.ChildCount = 0;
                            }

                            listItem.Add(vm);
                        }



                    }

                }
                return Ok(listItem.GroupBy(x => x.Name).Select(grp => grp.First()));
            }
            catch
            {
                string[] data2 = folderName.Split(new string[] { "/" }, StringSplitOptions.None);
                string folderName2 = data2[1];
                var driveItem = new DriveItem
                {
                    Name = folderName2,
                    Folder = new Folder
                    {
                    },
                    AdditionalData = new Dictionary<string, object>()
            {
                {"@microsoft.graph.conflictBehavior","fail"}
            }
                };
                driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/USER" ).Children.Request().AddAsync(driveItem);
                var listItem = new List<VmDriveItem>();
                return Ok(listItem.GroupBy(x => x.Name).Select(grp => grp.First()));
            }
          
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("download/{path?}")]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> Download(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
                path = path.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            var stream = await graphClient.DownloadFile("/" + path, false);
            var arr = path.Split('/');
            var filename = arr[arr.Length - 1];
            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;

            var pathDownload = Path.Combine(environment.WebRootPath, "fileOneDrive");


            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                FileStream file = new FileStream(pathDownload + "\\" + filename, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();
            }
            var fullpath = pathRelative + "\\fileOneDrive\\" + filename;
            return Ok(new { path = fullpath });
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("downloadPe/{path?}")]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> DownloadPe(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
                path = path.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            var stream = await graphClient.DownloadFile("/" + path, false);
            var arr = path.Split('/');
            var filename = arr[arr.Length - 1];
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var result = ms.ToArray();
                return Ok(new DownloadFileViewModel { File = result, FileName = filename, MimeType = "" });
            }
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("uploadtodrivePe")]
        public async Task<IActionResult> UploadToDrivePe([FromForm]VmUploadDrive data)
        {
            if (!string.IsNullOrEmpty(data.Path))
                data.Path = data.Path.Replace('*', '/');

            if (!string.IsNullOrEmpty(data.PathFullWithFileName))
                data.PathFullWithFileName = data.PathFullWithFileName.Replace('*', '/');
            string extension = Path.GetExtension(data.File.FileName);
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            var file = data.File.OpenReadStream();
            
            var upload = await graphClient.UploadLargeFile(file, data.PathFullWithFileName, false);

            if (!string.IsNullOrEmpty(data.FileAliasName))
            {
                var update = new DriveItem
                {
                    Name = data.FileAliasName + extension
                };

                await graphClient.Me.Drive.Root.ItemWithPath(data.PathFullWithFileName)
                    .Request()
                    .UpdateAsync(update);
            }
            var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + data.Path).Children.Request().GetAsync();

            var listItem = new List<VmDriveItem>();
           
            PeFileDto peFileData = new PeFileDto();
            peFileData.file_name = data.FileAliasName + extension;
            peFileData.tags = "dam upload";
            peFileData.file_size = data.File.Length;
            peFileData.file_path = data.PathFullWithFileName;
            peFileData.mimetype = GetMimeTypeByWindowsRegistry(peFileData.file_name);
            peFileData.parent_id = data.JobId;
            peFileData.upload_date = DateTime.Now;
            peFileData.file_type = extension;

            var saveFile = await _repoMapping.InsertUploadFilePe(peFileData);

            foreach (var item in driveItem)
            {
                var userUpload = await _repoMapping.GetUserUploadFIle(data.JobId, item.Name);
                var vm = new VmDriveItem();
                vm.File = item.File;
                vm.Folder = item.Folder;
                vm.Name = item.Name;
                vm.LastModifiedDateTime = item.LastModifiedDateTime;
                if (userUpload != null)
                {
                    vm.LastModifiedBy = userUpload.AliasName;
                }
                else
                {
                    vm.LastModifiedBy = "";
                }
               
                vm.Size = item.Size;
                if (item.Folder != null)
                {
                    vm.ChildCount = item.Folder.ChildCount;
                }
                else
                {
                    vm.ChildCount = 0;
                }

                listItem.Add(vm);
            }
            return Ok(listItem.GroupBy(x => x.Name).Select(grp => grp.First()));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetTrainingSchedule")]
        public async Task<IActionResult> GetTrainingSchedule()
        {
            var resp = await _repoMapping.GetTrainingSchedule();
            return Ok(resp);
        }


        [AllowAnonymousRole]
        [HttpPost]
        [Route("uploadtodrive")]
        public async Task<IActionResult> UploadToDrive([FromForm]VmUploadDrive data)
        {
            if (!string.IsNullOrEmpty(data.Path))
                data.Path = data.Path.Replace('*', '/');

            if (!string.IsNullOrEmpty(data.PathFullWithFileName))
                data.PathFullWithFileName = data.PathFullWithFileName.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            var file = data.File.OpenReadStream();
            var upload = await graphClient.UploadLargeFile(file, data.PathFullWithFileName, false);

            if (string.IsNullOrEmpty(data.Path))
                return Ok(await graphClient.Me.Drive.Root.Children.Request().GetAsync());
            else
                return Ok(await graphClient.Me.Drive.Root.ItemWithPath("/" + data.Path).Children.Request().GetAsync());

            //return Ok("data success");
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("uploadProfile")]
        public async Task<IActionResult> UploadProfile([FromForm]VmUploadDrive data)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            var file = data.File.OpenReadStream();
            var ext = System.IO.Path.GetExtension(data.File.FileName);
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            data.PathFullWithFileName = data.PathFullWithFileName + userId + ext;
            await graphClient.UploadLargeFile(file, data.PathFullWithFileName, false);

            //save to local...
            var pathlocalImage = Path.Combine(environment.WebRootPath, "ImageProfile");
            if (!System.IO.Directory.Exists(pathlocalImage))
            {
                System.IO.Directory.CreateDirectory(pathlocalImage);
            }

            var uploads = Path.Combine(environment.WebRootPath, "ImageProfile");
            var flname = userId + ext;
            var fPath = Path.Combine(uploads, flname);
            if (System.IO.File.Exists(fPath))
            {
                System.IO.File.Delete(fPath);
            }

            using (var fileStream = new FileStream(Path.Combine(uploads, flname), FileMode.Create))
            {
                await data.File.CopyToAsync(fileStream).ConfigureAwait(false);
                await fileStream.FlushAsync().ConfigureAwait(false);
            }
            //end off save to local...

            return Ok("upload success");
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetImageProfile")]
        public async Task<IActionResult> GetImageProfile()
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
            var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/IMAGESPROFILE").Children.Request().GetAsync();
            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var fullpath = pathRelative + "\\ImageProfile\\" ;

            var vm = new VmDriveProfile();
            if (driveItem.Count > 0 || driveItem != null)
            {
                foreach (var item in driveItem.Where(x => x.Name.Contains(empId)).OrderBy(x => x.LastModifiedDateTime))
                {
                    if (item.Name.Contains(empId))
                    {
                        vm.Id = item.Id;
                        vm.WebUrl = fullpath + item.Name;
                        vm.FName = item.Name;
                       
                    }
                }
            }
            if (!string.IsNullOrEmpty(vm.WebUrl)) {
                var pathlocalImage = Path.Combine(environment.WebRootPath, "ImageProfile");
                if (!System.IO.Directory.Exists(pathlocalImage))
                {
                    System.IO.Directory.CreateDirectory(pathlocalImage);
                }

                if (!System.IO.File.Exists(pathlocalImage + "\\" + vm.FName))
                {
                    var stream = await graphClient.DownloadFile("/IMAGESPROFILE/" + vm.FName, false);
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        FileStream file = new FileStream(pathlocalImage + "\\" + vm.FName, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(file);
                        file.Close();
                        ms.Close();
                    }
                }

            }


            return Ok(vm);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetUploadedFiles")]
        public async Task<IActionResult> GetUploadedFiles()
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
            return Ok(await graphClient.Me.Drive.Root.ItemWithPath("/UploadedFiles").Children.Request().GetAsync());
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("createfolder")]
        public async Task<IActionResult> CreateFolder([FromBody]VmCreateFolder data)
        {
            if (!ModelState.IsValid) {
                return await Task.FromResult(BadRequest("Model Not Valid"));
            }

            if (!string.IsNullOrEmpty(data.Path))
                data.Path = data.Path.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            var isroot = string.IsNullOrEmpty(data.Path) ? true : false;
            if (!string.IsNullOrEmpty(data.Id))
            {
                var parentEnt = "pm.job_detail";
                var userid = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
                if (data.Id.ToLower() == "mydam")
                {
                    parentEnt = "pm.mydam";
                }
                var CreateFolderToDb = await _repoMapping.CreateFolderToDB(data.Id, data.Path + "/" + data.FolderName, parentEnt, userid);
            }
           
            await MsGraphExtension.CreateFolder(graphClient, data.FolderName, data.Path, isroot);
            //var urlSearch = "Documents/" + data.FolderName;
            //if (!isroot) urlSearch = data.Path + "/" + data.FolderName;

            //var search = await graphClient.Me.Drive.Search("{"+data.FolderName+"}").Request().GetAsync();
            //var driveItemx = search.Where(x=>x.WebUrl.Contains(urlSearch)).FirstOrDefault();
            //if (driveItemx != null) {
            //    var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            //    var modelMap = new TrMappingFolderOneDrive();
            //    modelMap.Id = Guid.NewGuid().ToString();
            //    modelMap.ItemId = driveItemx.Id;
            //    modelMap.RefId = userId;
            //    modelMap.RefType = RefTypeMapping.User;
            //    modelMap.CreatedOn = DateTime.Now;
            //    modelMap.IsActive = true;
            //    await _repoMapping.AddAsync(modelMap);
            //}
            //add mapping user to drive..
            return Ok(new Phoenix.Data.RestApi.ApiResponse(true, Messages.SuccessAdd));

            //jika sudah berhasil di buat...maka di add user ke table mapping folder one drive....di Map FOlder ONe Drive..
            //next codingan.....

            //if (string.IsNullOrEmpty(data.Path))
            //    return Ok(await graphClient.Me.Drive.Root.Children.Request().GetAsync());
            //else
            //    return Ok(await graphClient.Me.Drive.Root.ItemWithPath("/" + data.Path).Children.Request().GetAsync());

        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("deletePe")]
        public async Task<IActionResult> deletePe([FromBody]VmCreateFolder data)
        {
            if (!ModelState.IsValid)
            {
                return await Task.FromResult(BadRequest("Model Not Valid"));
            }

            if (!string.IsNullOrEmpty(data.Path))
                data.Path = data.Path.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            await graphClient.Me.Drive.Root.ItemWithPath("/"+data.Path).Request().DeleteAsync();
            try
            {
                var modelDelete = new PeFileDto()
                {
                    file_path = data.Path
                };
                var saveFile = await _repoMapping.DeleteFilePe(modelDelete);
            }
            catch 
            {

            }
           
            return Ok(new Phoenix.Data.RestApi.ApiResponse(true, Messages.SuccessAdd));


        }

        private string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "application/unknown";
            string ext = (fileNameOrExtension.Contains(".")) ? Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }


        static string ListOneDriveContents(List<DriveItem> contents)
        {
            if (contents == null || contents.Count == 0)
            {
                return "No content found";
            }

            var str = new StringBuilder();
            foreach (var item in contents)
            {
                if (item.Folder != null)
                {
                    str.AppendLine($"'{item.Name}' is a folder");
                }
                else if (item.File != null)
                {
                    str.AppendLine($"'{item.Name}' is a file with size {item.Size}");
                }
                else if (item.Audio != null)
                {
                    str.AppendLine($"'{item.Audio.Title}' is an audio file with size {item.Size}");
                }
                else
                {
                    str.AppendLine($"Generic drive item found with name {item.Name}");
                }
            }

            return str.ToString();
        }

        static async Task GetAllChildren(IDriveRequestBuilder drive, List<DriveItem> result, IDriveItemRequestBuilder root = null)
        {
            root = root ?? drive.Root;
            var items = await root.Children.Request().GetAsync();
            result.AddRange(items.ToList());
            foreach (var item in items)
            {
                if (item.Folder != null && item.Folder.ChildCount > 0)
                {
                    await GetAllChildren(drive, result, drive.Items[item.Id]);
                }
            }
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetImageEmployeeProfile")]
        public async Task<IActionResult> GetImageEmployeeProfile(string id)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
            var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            var driveItem = graphClient.Me.Drive.Root.ItemWithPath("/HRIS").Children.Request().Filter(id);
            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var fullpath = pathRelative + "\\HRIS\\";

            var vm = new VmDriveInventory();
            vm.WebUrl = fullpath + driveItem.QueryOptions[0].Value;
            vm.FName = driveItem.QueryOptions[0].Value;
            if (!string.IsNullOrEmpty(vm.WebUrl))
            {
                var pathlocalImage = Path.Combine(environment.WebRootPath, "HRIS");
                if (!System.IO.Directory.Exists(pathlocalImage))
                {
                    System.IO.Directory.CreateDirectory(pathlocalImage);
                }

                if (!System.IO.File.Exists(pathlocalImage + "\\" + vm.FName))
                {
                    try
                    {
                        var stream = await graphClient.DownloadFile("/HRIS/" + vm.FName, false);
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            FileStream file = new FileStream(pathlocalImage + "\\" + vm.FName, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(file);
                            file.Close();
                            ms.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error = " + ex.Message.ToString() + "Stack Trace = " + ex.StackTrace.ToString());
                    }

                }
            }
            return Ok(vm);
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetImageInventory")]
        public async Task<IActionResult> GetImageInventory(string id)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
            var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            var driveItem =   graphClient.Me.Drive.Root.ItemWithPath("/ASSETPHOENIX").Children.Request().Filter(id);
            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var fullpath = pathRelative + "\\ImageInventory\\";

            //var aaa = ListOneDriveContents(driveItem.ToList());
            //var bbb =  GetAllChildren(graphClient.Me.Drive, allItems);

            
            var vm = new VmDriveInventory();
            vm.WebUrl = fullpath + driveItem.QueryOptions[0].Value;
            vm.FName = driveItem.QueryOptions[0].Value;
            //if ( driveItem != null)
            //{
            //    foreach (var item in driveItem)
            //    {
            //        if (item.Name.Contains(id))
            //        {
            //            vm.Id = item.Id;
            //            vm.WebUrl = fullpath + item.Name;
            //            vm.FName = item.Name;
            //        }else
            //        {
            //            vm.Id = item.Id;
            //            vm.WebUrl = fullpath + id;
            //            vm.FName = id;
            //        }
            //    }
            //}
            if (!string.IsNullOrEmpty(vm.WebUrl))
            {
                var pathlocalImage = Path.Combine(environment.WebRootPath, "ImageInventory");
                if (!System.IO.Directory.Exists(pathlocalImage))
                {
                    System.IO.Directory.CreateDirectory(pathlocalImage);
                }

                if (!System.IO.File.Exists(pathlocalImage + "\\" + vm.FName))
                {
                    try
                    {
                        var stream = await graphClient.DownloadFile("/ASSETPHOENIX/" + vm.FName, false);
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            FileStream file = new FileStream(pathlocalImage + "\\" + vm.FName, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(file);
                            file.Close();
                            ms.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error = " + ex.Message.ToString() + "Stack Trace = " + ex.StackTrace.ToString());
                    }
                   
                }
            }

            return Ok( vm);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetImageDAM")]
        public async Task<IActionResult> GetImageDAM(string filepath)
        {
            try
            {
                var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
                var filename = Path.GetFileName(filepath);
                string lastDirectory = new DirectoryInfo(filepath).Parent.Name;
                var model = _repoAzure.GetOnlyOne().Result;
                var graph = new MsGraph(model.ClientId, model.TenantId);
                var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

                //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
                var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
                //var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/PROJECTMANAGEMENT/DAM/" + filename).Children.Request().GetAsync();
                IDriveItemChildrenCollectionPage driveItem;
                var pathLocalImageDirectory = "";
                var streamDirectory = "";
                if (lastDirectory == "DAM")
                {
                    driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/PROJECTMANAGEMENT/DAM").Children.Request().GetAsync();
                    pathLocalImageDirectory = "PROJECTMANAGEMENT/DAM/";
                    streamDirectory = "/PROJECTMANAGEMENT/DAM/";
                }
                else
                {
                    driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/PROJECTMANAGEMENT/DAM"+"/"+ lastDirectory).Children.Request().GetAsync();
                    pathLocalImageDirectory = "PROJECTMANAGEMENT/DAM/"+ lastDirectory+"/";
                    streamDirectory = "/PROJECTMANAGEMENT/DAM/" + lastDirectory + "/";
                }
                

                var fullpath = pathRelative + "\\fileOneDrive\\";

                var drive = driveItem.OrderBy(x => x.LastModifiedDateTime);

                var vm = new VmDriveInventory();

                if (driveItem.Count > 0 || driveItem != null)
                {
                    foreach (var item in drive)
                    {
                        if (item.Name.Contains(filename))
                        {
                            vm.Id = item.Id;
                            vm.WebUrl = fullpath + item.Name;
                            vm.FName = item.Name;
                        }

                    }
                }

                if (!string.IsNullOrEmpty(vm.WebUrl))
                {
                    var pathlocalImage = Path.Combine(environment.WebRootPath, pathLocalImageDirectory);
                    if (!System.IO.Directory.Exists(pathlocalImage))
                    {
                        System.IO.Directory.CreateDirectory(pathlocalImage);
                    }

                    if (!System.IO.File.Exists(pathlocalImage + "\\" + vm.FName))
                    {
                        var stream = await graphClient.DownloadFile(streamDirectory + vm.FName, false);
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            FileStream file = new FileStream(pathlocalImage + "\\" + vm.FName, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(file);
                            file.Close();
                            ms.Close();
                        }
                    }
                }

                return Ok(vm);
            }
            catch 
            {
                var vm = new VmDriveInventory();
                return Ok(vm); 
            }
            
        }


        #region MYDAM

        [AllowAnonymousRole]
        [HttpPost]
        [Route("uploadtodrivemydam")]
        public async Task<IActionResult> UploadToDriveMyDam([FromForm]VmUploadDrive data)
        {
            if (!string.IsNullOrEmpty(data.Path))
                data.Path = data.Path.Replace('*', '/');

            if (!string.IsNullOrEmpty(data.PathFullWithFileName))
                data.PathFullWithFileName = data.PathFullWithFileName.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            var file = data.File.OpenReadStream();
            var upload = await graphClient.UploadLargeFile(file, data.PathFullWithFileName, false);
            string extension = Path.GetExtension(data.File.FileName);
            if (!string.IsNullOrEmpty(data.FileAliasName))
            {
                var update = new DriveItem
                {
                    Name = data.FileAliasName + extension
                };

                await graphClient.Me.Drive.Root.ItemWithPath(data.PathFullWithFileName)
                    .Request()
                    .UpdateAsync(update);
            }

            var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + data.Path).Children.Request().GetAsync();

            var listItem = new List<VmDriveItem>();
            string oldFileName = Path.GetFileName(data.PathFullWithFileName);
            MyDamFileDto myDamFileData = new MyDamFileDto();
            myDamFileData.file_name = data.FileAliasName + Path.GetExtension(data.File.FileName).ToLower();
            myDamFileData.tags = "dam upload";
            myDamFileData.file_size = data.File.Length;
            myDamFileData.file_path = data.PathFullWithFileName.Replace(oldFileName, data.FileAliasName + Path.GetExtension(data.File.FileName).ToLower());
            myDamFileData.mimetype = GetMimeTypeByWindowsRegistry(data.File.FileName);
            myDamFileData.upload_date = DateTime.Now;
            myDamFileData.created_by = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            myDamFileData.file_type = extension;
            myDamFileData.parent_id = data.JobId;
           
            var saveFile = await _repoMapping.InsertUploadFileMyDam(myDamFileData);
           
            return Ok(saveFile);
        }
        #endregion


    }


    public class VmCreateFolder
    {
        public string Path { get; set; }
        [Required]
        public string FolderName { get; set; }
        public string Id { get; set; }
    }
    public class VmDriveProfile
    {
        public string Id { get; set; }
        public string WebUrl { get; set; }
        public string FName { get; set; }
    }
    public class VmDriveItem
    {
        public Microsoft.Graph.File File { get; set; }
        public Microsoft.Graph.Folder Folder { get; set; }
        public string Name { get; set; }
        public System.DateTimeOffset? LastModifiedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public int? ChildCount { get; set; }
        public long? Size { get; set; }
        public string JobId { get; set; }
        public string WebUrl { get; set; }
    }
    public class VmUploadDrive
    {
        public string Path { get; set; }
        public string PathFullWithFileName { get; set; }
        public IFormFile File { get; set; }
        public bool IsBig { get; set; }
        public string JobId { get; set; }
        public string FileAliasName { get; set; }

    }

    public class DownloadFileViewModel
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public string MimeType { get; set; }
    }

    public class VmDriveInventory
    {
        public string Id { get; set; }
        public string WebUrl { get; set; }
        public string FName { get; set; }
    }

}