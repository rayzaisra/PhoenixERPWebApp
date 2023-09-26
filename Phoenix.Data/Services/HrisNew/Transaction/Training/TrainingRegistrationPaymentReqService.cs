using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Phoenix.Data.Attributes;
using System.Linq.Dynamic.Core;
using Microsoft.Graph;
using Phoenix.Data.Services.Um;

namespace Phoenix.Data
{

	public interface ITrainingRegistrationPaymentReqService : IDataServiceHris<TrainingRequisition>
	{
		Task<List<TrainingRequisition>> GetTrainingRequestList();
		Task<List<TrainingRegpaymentDetail>> GetTrainingRegPaymentDetailById(string Id);
		Task<int> AddAsync(TrainingRegPayment TrRegPayment, bool isEdit);
	}
	public class TrainingRegistrationPaymentReqService : ITrainingRegistrationPaymentReqService
	{
		readonly DataContext context;
		readonly GlobalFunctionApproval globalFcApproval;
		readonly IManageMenuService menuService;
		string idTemplate = "";

		/// <summary>
		/// And endpoint to manage  TrainingRegistrationPaymentReqService
		/// </summary>
		/// <param name="context">Database context</param>
		public TrainingRegistrationPaymentReqService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService)
		{
			this.context = context;
			this.globalFcApproval = globalFcApproval;
			this.menuService = menuService;
		}

		public async Task<List<TrainingRegpaymentDetail>> GetTrainingRegPaymentDetailById(string Id)
		{
			var trn = await (from trpd in context.TrainingRegpaymentDetail
							 where trpd.IsDeleted.Equals(false) && trpd.RegPaymentId == Id
							 select new TrainingRegpaymentDetail
							 {
								 Id = trpd.Id,
								 StartDate = trpd.StartDate,
								 EndDate = trpd.EndDate,
								 StatusRequest = trpd.StatusRequest,
								 Remarks = trpd.Remarks,
								 MasterTrainingRequestId = trpd.MasterTrainingRequestId,
								 MasterTrainingRequestName = context.MasterTrainingRequests.Where(x => x.Id == trpd.MasterTrainingRequestId).FirstOrDefault().RequestName,
								 RegPaymentId = trpd.RegPaymentId,
							 }).ToListAsync();
			return trn;
		}


		public string GetTrainingRequetName(string MasterTrainingRequestId)
		{
			string result = context.MasterTrainingRequests.Where(x => x.Id == MasterTrainingRequestId).FirstOrDefault().RequestName;

			return result;
		}
		public async Task<List<TrainingRequisition>> GetTrainingRequestList()
		{
			var trn = await (from tb in context.TrainingRequisition
							 join tr in context.Training on tb.TrainingId equals tr.Id
							 join eb in context.EmployeeBasicInfos on tb.RequesterEmployeeBasicInfoId equals eb.Id
							 join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id into buj
							 from bulj in buj.DefaultIfEmpty()
								 //join trg in context.TrainingRegPayment on tr.Id equals trg.TrainingId into trgm
								 //from trg in trgm.DefaultIfEmpty()
							 where tb.IsDeleted.Equals(false) && tb.Status == "3" || tb.Status == "1" || tb.Status == "5"
							 select new TrainingRequisition
							 {
								 Id = tb.Id,
								 TrainingName = tr.Name,
								 TrainingType = tr.TrainingType,
								 BusinessUnitId = tb.BusinessUnitId,
								 BusinessUnitName = bulj.UnitName,
								 TrainingId = tb.TrainingId,
								 Code = tb.Code,
								 AddRecomendation = tb.AddRecomendation,
								 Notes = tb.Notes,
								 ParticipantList = context.TrainingRequisitionDetail.Where(x => x.IsDeleted.Equals(false) && x.TrainingRequisitionId == tb.Id).ToList(),
								 Status = tb.Status,
								 Venue = tb.Venue,
								 PurposeEndDate = tb.PurposeEndDate,
								 PurposeStartDate = tb.PurposeStartDate,
								 RequesterEmployeeBasicInfoId = tb.RequesterEmployeeBasicInfoId,
								 RequesterEmployeeBasicInfoName = eb.NameEmployee
							 }).ToListAsync();
			return trn;
		}

		public static int IncrementedNextSequence(int increment)
		{
			return getthenumber() + increment;
		}

		public static int getthenumber()
		{
			return 000;
		}

		public async Task<int> AddAsync(TrainingRegPayment TrRegPayment, bool isEdit)
		{
			using (var transaction = await context.Database.BeginTransactionAsync())
			{
				try
				{
					var isExist = context.TrainingRegPayment.Any(x => x.Id == TrRegPayment.Id);
					if (isExist)
					{
						if (TrRegPayment.trainingRegPaymentDeail.Count > 0)
						{
							List<TrainingRegpaymentDetail> igrd = new List<TrainingRegpaymentDetail>();
							var id = await TransID.GetTransId(context, Code.TrainingRegPayment, DateTime.Today);
							int counter = 1;
							
							foreach (TrainingRegpaymentDetail data in TrRegPayment.trainingRegPaymentDeail)
							{
								var isExistDetail = context.TrainingRegpaymentDetail.Any(x => x.Id == data.Id);
								if (isExistDetail == true)
								{
									igrd.Add(data);
									TrainingRegpaymentDetail[] igrdArray = igrd.ToArray();
									context.PhoenixEditRange(igrdArray);
								}else
								{
									data.Id = id + String.Format("{0:D3}", IncrementedNextSequence(counter));
									data.RegPaymentId = id;
									igrd.Add(data);
									TrainingRegpaymentDetail[] igrdArray = igrd.ToArray();
									await context.PhoenixAddRangeAsync(igrdArray);
									counter++;
								}
							}
						}
						TrainingRequisition trainingRequisition = context.TrainingRequisition.Where(x => x.Code == TrRegPayment.Code).FirstOrDefault();

						if (TrRegPayment.Action == "Save")
						{
							trainingRequisition.Status = "1";
						}
						if (TrRegPayment.Action == "Submit")
						{
							trainingRequisition.Status = "5";
						}
						context.PhoenixEdit(trainingRequisition);
					}
					else
					{
						var id = await TransID.GetTransId(context, Code.TrainingRegPayment, DateTime.Today);
						TrRegPayment.Id = id;
						await context.PhoenixAddAsync(TrRegPayment);

						if (TrRegPayment.trainingRegPaymentDeail.Count > 0)
						{
							List<TrainingRegpaymentDetail> igrd = new List<TrainingRegpaymentDetail>();
							int counter = 1;
							foreach (TrainingRegpaymentDetail data in TrRegPayment.trainingRegPaymentDeail)
							{
								data.Id = id + String.Format("{0:D3}", IncrementedNextSequence(counter));
								data.RegPaymentId = id;
								igrd.Add(data);
								counter++;
							}

							TrainingRegpaymentDetail[] igrdArray = igrd.ToArray();
							await context.PhoenixAddRangeAsync(igrdArray);
						}
						TrainingRequisition trainingRequisition = context.TrainingRequisition.Where(x => x.Code == TrRegPayment.Code).FirstOrDefault();

						if (TrRegPayment.Action == "Save")
						{
							trainingRequisition.Status = "1";
						}
						if (TrRegPayment.Action == "Submit")
						{
							trainingRequisition.Status = "5";
						}

						context.PhoenixEdit(trainingRequisition);
					}
					var save = await context.SaveChangesAsync();
					transaction.Commit();
					return save;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw new Exception(ex.Message.ToString());
				}
			}
		}


		public Task<int> AddAsync(TrainingRequisition entity)
		{
			throw new NotImplementedException();
		}

		public Task<int> ApproveAsync(TrainingRequisition entity)
		{
			throw new NotImplementedException();
		}

		public Task<int> DeleteSoftAsync(TrainingRequisition entity)
		{
			throw new NotImplementedException();
		}

		public async Task<List<TrainingRequisition>> Get()
		{
			return await context.TrainingRequisition.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
		}

		public async Task<TrainingRequisition> Get(string Id)
		{
			var query = await (from a in context.TrainingRequisition
							   join t in context.TrTemplateApprovals on a.Id equals t.RefId into tta
							   from ttalj in tta.DefaultIfEmpty()
							   join trg in context.TrainingRegPayment on a.TrainingId equals trg.TrainingId into trgm
							   from trg in trgm.DefaultIfEmpty()
							   join trgc in context.TrainingRegPayment on a.RequesterEmployeeBasicInfoId equals trgc.EmployeeBasicInfoId into trgcm
							   from trgc in trgm.DefaultIfEmpty()

							   where a.IsDeleted.Equals(false) && a.Id == Id
							   select new TrainingRequisition()
							   {
								   Id = a.Id,
								   AddRecomendation = a.AddRecomendation,
								   ApprovedBy = a.ApprovedBy,
								   ApprovedOn = a.ApprovedOn,
								   BusinessUnitDepartementId = a.BusinessUnitDepartementId,
								   BusinessUnitDivisionId = a.BusinessUnitDivisionId,
								   BusinessUnitId = a.BusinessUnitId,
								   BusinessUnitName = a.BusinessUnitName,
								   Code = a.Code,
								   CreatedBy = a.CreatedBy,
								   CreatedOn = a.CreatedOn,
								   DeletedBy = a.DeletedBy,
								   DeletedOn = a.DeletedOn,
								   IsActive = a.IsActive,
								   IsDefault = a.IsDefault,
								   IsDeleted = a.IsDeleted,
								   IsLocked = a.IsLocked,
								   ModifiedBy = a.ModifiedBy,
								   ModifiedOn = a.ModifiedOn,
								   Notes = a.Notes,
								   OwnerId = a.OwnerId,
								   ParticipantList = a.ParticipantList,
								   PurposeEndDate = a.PurposeEndDate,
								   PurposeStartDate = a.PurposeStartDate,
								   RemarkRejected = a.RemarkRejected,
								   RequesterEmployeeBasicInfoId = a.RequesterEmployeeBasicInfoId,
								   RequesterEmployeeBasicInfoName = a.RequesterEmployeeBasicInfoName,
								   Status = a.Status,
								   RegPaymentId = trg.Id,
								   TrainingId = a.TrainingId,
								   TrainingName = a.TrainingName,
								   TrainingType = a.TrainingType,
								   Venue = a.Venue,
								   ApprovalId = ttalj.Id
							   }).FirstOrDefaultAsync();

			return query;
		}

		public Task<int> EditAsync(TrainingRequisition entity)
		{
			throw new NotImplementedException();
		}
	}
}
