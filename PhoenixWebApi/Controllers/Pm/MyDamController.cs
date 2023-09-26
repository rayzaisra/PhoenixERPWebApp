using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Pm.Transaction;

namespace PhoenixWebApi.Controllers.Pm
{

    [Produces("application/json")]
    [Route("api/pm/[controller]")]
    public class MyDamController : Controller
    {

        readonly IMyDamService service;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        private IHostingEnvironment environment;
        public MyDamController(IMyDamService service, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure, IHostingEnvironment environment)
        {
            this.service = service;
            this._repoAzure = repoAzure;
            this.environment = environment;
        }

        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetMyDamWaitingApproval")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMyDamWaitingApproval()
        {
            var result = await service.GetWaitingApproval();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetByFilename/{filename}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByFilename(string filename)
        {
            var result = await service.GetByFilename(filename);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetFolderName")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFolderName()
        {
            var result = await service.GetFolderName();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetFolderById/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFolderById(string Id)
        {
            var result = await service.GetFolderById(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]RequestMyDam model)
        {
            
            await service.AddCustomAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("{Move}")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Move([FromBody]RequestMyDam model)
        {

            var Mv = await service.MoveFile(model);
           
            if (Mv == 0)
            {
                return Ok(new ApiResponse(false, Messages.Fail));
            }
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }


        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]MyDam model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]MyDam model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            string[] data = Id.Split(new string[] { "," }, StringSplitOptions.None);
            List<string> dataClean = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                if (!dataClean.Contains(data[i]))
                {
                    dataClean.Add(data[i]);
                }
            }
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            foreach (var item in dataClean)
            {
                var modelFile = await service.Get(item);
                if (modelFile == null)
                {
                    var modelFolder = await service.GetFolderById(item);
                    var listFileBasedOnPath = await service.GetListFileNameBasedOnPathFolder(modelFolder.folder_path);
                    if (listFileBasedOnPath.Count > 0)
                    {
                        foreach (var itemx in listFileBasedOnPath)
                        {

                            try
                            {
                                await graphClient.Me.Drive.Root.ItemWithPath("/" + itemx.FilePath).Request().DeleteAsync();
                                var pathlocalImagex = Path.Combine(environment.WebRootPath, modelFolder.folder_path + "/");
                                if (System.IO.File.Exists(pathlocalImagex + "\\" + itemx.FileName))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(pathlocalImagex + "\\" + itemx.FileName);
                                    }
                                    catch (Exception ex)
                                    {
                                        //Do something
                                    }
                                }


                            }
                            catch
                            {
                            }
                            await service.DeleteSoftAsync(itemx);
                        }
                    }
                    try
                    {
                        await graphClient.Me.Drive.Root.ItemWithPath("/" + modelFolder.folder_path).Request().DeleteAsync();
                    }
                    catch 
                    {

                    }
                    
                    await service.DeleteFolderPe(item);
                }
                else
                {
                    try
                    {
                        await graphClient.Me.Drive.Root.ItemWithPath("/" + modelFile.FilePath).Request().DeleteAsync();
                    }
                    catch
                    {
                    }
                    var pathlocalImage = Path.Combine(environment.WebRootPath, modelFile.FilePath.Replace(modelFile.FileName, ""));
                    if (System.IO.File.Exists(pathlocalImage + "\\" + modelFile.FileName))
                    {
                        try
                        {
                            System.IO.File.Delete(pathlocalImage + "\\" + modelFile.FileName);
                        }
                        catch (Exception ex)
                        {
                            //Do something
                        }
                    }
                    await service.DeleteSoftAsync(modelFile);
                }
            }
           
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

    }
}