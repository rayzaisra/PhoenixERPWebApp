using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
	/// <summary>
	/// Termination API Endpoint.
	/// </summary>
	[Produces("application/json")]
	[Route("api/EmployeeBasic")]
	public class EmployeeBasicController : Controller
	{
		readonly IEmployeeBasicInfoService service;

		/// <summary>
		/// And endpoint to manage Termination
		/// </summary>
		/// <param name="context">Database context</param>
		public EmployeeBasicController(IEmployeeBasicInfoService service)
		{
			this.service = service;
		}

		/// <summary>
		/// Creates new record.
		/// </summary>
		/// <param name="model">Object parameter to post in json format.</param>
		/// <returns>Json message if the process is successfully done or failed.</returns>
		[HttpPost]
		[ActionRole(ActionMethod.Add)]
		public async Task<IActionResult> Post([FromForm]IFormCollection collection)//([FromBody]EmployeeBasicInfo model)
		{
			if (collection.Count == 0 || !collection.Keys.Contains(nameof(EmployeeBasicInfo)))
			{
				return Ok(new ApiResponse(null, false, "bad request"));
			}

			var model = JsonConvert.DeserializeObject<EmployeeBasicInfo>(collection[nameof(EmployeeBasicInfo)].ToString());

			if (collection.Files.Count() > 0)
			{
				model.PhotoFile = collection.Files[0];
			}
			
			await service.AddAsync(model);
			return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
		}

		/// <summary>
		/// Edits existing record based on primary key id.
		/// </summary>
		/// <param name="model">Object parameter to post in json format.</param>
		/// <returns>Json message if the process is successfully done or failed.</returns>
		[HttpPut]
		[ActionRole(ActionMethod.Edit)]
		public async Task<IActionResult> Put([FromForm]IFormCollection collection)//[FromBody]EmployeeBasicInfo model)
		{
			if (!collection.Keys.Contains(nameof(EmployeeBasicInfo)))
			{
				return Ok(new ApiResponse(null, false, "bad request"));
			}

			var model = JsonConvert.DeserializeObject<EmployeeBasicInfo>(collection[nameof(EmployeeBasicInfo)].ToString());

			if (collection.Files.Count() > 0)
			{
				model.PhotoFile = collection.Files[0];
			}
			await service.EditAsync(model);
			return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
		}


		/// <summary>
		/// Deletes specific record based on given id parameter.
		/// </summary>
		/// <param name="id">Key id parameter.</param>
		/// <returns>Json message if the process is successfully done or failed.</returns>
		[HttpDelete("{id}")]
		[ActionRole(ActionMethod.Delete)]
		public async Task<IActionResult> Delete(string id)
		{
			var model = await service.Get(id);
			await service.DeleteSoftAsync(model);
			return Ok(new ApiResponse(true, Messages.SuccessDelete));
		}

		/// <summary>
		/// Gets all records without parameter as criteria.
		/// </summary>
		/// <returns>List of json object.</returns>
		[HttpGet]
		[ActionRole(ActionMethod.Read)]
		public async Task<IActionResult> Get()
		{
			var result = await service.Get();
			return Ok(new ApiResponse(result));
		}

		/// <summary>
		/// Gets single record based on specific primary key id.
		/// </summary>
		/// <param name="id">Key id parameter.</param>
		/// <returns>A json object with given id.</returns>
		[HttpGet("{id}")]
		[ActionRole(ActionMethod.Read)]
		public async Task<IActionResult> Get(string id)
		{
			var result = await service.Get(id);
			return Ok(new ApiResponse(result));
		}

		/// <summary>
		/// Gets single record based on specific primary key id.
		/// </summary>
		/// <param name="id">Key id parameter.</param>
		/// <returns>A json object with given id.</returns>
		[HttpGet("GetInfoByLogin")]
		[ActionRole(ActionMethod.Read)]
		public async Task<IActionResult> GetInfoByLogin()
		{
			var result = await service.GetInfoByLogin();
			return Ok(new ApiResponse(result));
		}
		
	}
}