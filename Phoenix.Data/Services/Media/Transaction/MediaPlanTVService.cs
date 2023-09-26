using Phoenix.Data.Models.Media.Transaction.MediaPlanTV;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;

namespace Phoenix.Data.Services.Media.Transaction
{
    public interface IMediaPlanTVService : IDataService<TrMediaPlanTV>
    {
        Task<List<TrMediaPlanTV>> GetMediaPlanTVs();
        Task<DataSet> ReadExcel(IFormFile file);
        Task<List<TrMediaPlanTVDetail>> GetMediaPlanTVDetails(string mediaPlanTvId);
        Task<TrMediaPlanTV> AddDetailAsync(TrMediaPlanTV entity, IFormFile file, DataSet data);
        Task<TrMediaPlanTV> UpdateDetailAsync(TrMediaPlanTV entity, IFormFile file, DataSet data);
        Task<string> GetMediaPlanTvId(string lastCode);
        Task<TrMediaFileMaster> GetMediaPlanTvFileTemplateId();
    }

    public class MediaPlanTVService : IMediaPlanTVService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        public MediaPlanTVService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(TrMediaPlanTV entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<TrMediaPlanTV> AddDetailAsync(TrMediaPlanTV mediaPlan, IFormFile file, DataSet data)
        {
            TrMediaFileMaster FileMaster = new TrMediaFileMaster();
            var table = data.Tables["TV"];

            #region UploadFileMaster
            //Upload ke FileMaster
            var fileId = await uploadFile.Upload(file, null);
            if (!string.IsNullOrEmpty(fileId))
            {
                FileMaster.FileMasterId = fileId;
            }

            context.PhoenixAdd(FileMaster);
            #endregion

            #region Insert Media Plan Header
            //Insert Media Plan TV Header

            #region Create Media Plan No
            //Membuat media plan no
            var periodCampaign = table.Rows[9][4].ToString().Replace(':', ' ').Replace(" ", string.Empty);
            var periodCampaignStart = periodCampaign.Split('/')[0];
            var periodCampaignEnd = periodCampaign.Split('/')[1];
            var periodSplit = periodCampaign.Split('/');
            var firstPeriod = periodSplit[0].Split('-');
            var secondPeriod = periodSplit[1].Split('-');
            string firstYear = firstPeriod[1].Substring(2, 2);
            string secondYear = secondPeriod[1].Substring(2, 2);
            string lastCode = firstYear == secondYear ? $"{firstPeriod[0]}-{secondPeriod[0]}/{firstYear}" :
                $"{firstPeriod[0]}-{secondPeriod[0]}/{firstYear}-{secondYear}";
            mediaPlan.MediaPlanNo = await TransID.GetTransIdMediaPlanTV(context, Code.MediaPlanTV, DateTime.Today, lastCode);
            #endregion

            //Targer audience
            mediaPlan.TargetAudience = table.Rows[7][4].ToString().Replace(':', ' ').Replace(" ", string.Empty);

            //Version & Duration
            var versionDuration = table.Rows[6][4].ToString().Replace(':', ' ').Replace(" ", string.Empty).Split('/');
            mediaPlan.Version = versionDuration[0];
            mediaPlan.Duration = versionDuration[1];

            //Client
            mediaPlan.ClientId = table.Rows[4][4].ToString() == string.Empty ? null :
                context.Companys.Any(m => m.CompanyName == table.Rows[4][4].ToString() && (m.IsClient ?? false)) ?
                context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[4][4].ToString() && (m.IsClient ?? false)).Id : null;

            //Brand
            mediaPlan.BrandId = table.Rows[5][4].ToString() == string.Empty ? null :
                context.CompanyBrands.Any(m => m.BrandName == table.Rows[5][4].ToString()) ?
                context.CompanyBrands.FirstOrDefault(m => m.BrandName == table.Rows[5][4].ToString()).Id : null;

            //Revision No
            mediaPlan.RevisionNo = table.Rows[11][4].ToString().Replace(':', ' ').Replace(" ", string.Empty);

            //Perion Campaign
            mediaPlan.PeriodCampaignStart = new DateTime(Convert.ToInt32(firstPeriod[1]), Convert.ToInt32(firstPeriod[0]), 1);
            mediaPlan.PeriodCampaignEnd = new DateTime(Convert.ToInt32(secondPeriod[1]), Convert.ToInt32(secondPeriod[0]), 1);

            //Date Prepared
            var datePrepare = table.Rows[10][4].ToString().Replace(':', ' ').Replace(" ", string.Empty).Split('/');
            mediaPlan.DatePrepared = new DateTime(Convert.ToInt32(datePrepare[2]), Convert.ToInt32(datePrepare[1]), Convert.ToInt32(datePrepare[0]));

            mediaPlan.TmMediaPlanStatusId = MediaPlanStatusName.Draft;
            mediaPlan.IsActive = true;
            mediaPlan.TotalNetCost = 0;

            context.PhoenixAdd(mediaPlan);

            mediaPlan.EmployeeBasicInfoId = mediaPlan.CreatedBy;
            #endregion

            #region Input Media Plan TV Detail

            #region Prime Time
            for (int i = 19; i < 46; i++)
            {
                if (table.Rows[i][3].ToString() != string.Empty)
                {
                    TrMediaPlanTVDetail detail = new TrMediaPlanTVDetail();
                    detail.TrMediaPlanId = mediaPlan.Id;

                    //Program Type
                    detail.TmProgramTypeId = table.Rows[18][3].ToString() == string.Empty ? null :
                        context.TmProgramTypes.Any(m => m.ProgramTypeName == table.Rows[18][3].ToString()) ?
                        context.TmProgramTypes.FirstOrDefault(m => m.ProgramTypeName == table.Rows[18][3].ToString()).Id : null;

                    //Media plan type
                    detail.TmMediaTypeId = table.Rows[16][2].ToString() == string.Empty ? null :
                        context.MediaTypes.Any(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())) ?
                        context.MediaTypes.FirstOrDefault(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())).Id : null;

                    //Programe Name
                    detail.ProgramName = table.Rows[i][3].ToString() == string.Empty ? null : table.Rows[i][3].ToString();

                    //Programe Category Type
                    detail.TmProgramCategoryTypeId = table.Rows[i][4].ToString() == string.Empty ? null :
                        context.TmProgramCategoryTypes.Any(m => m.ProgramCategoryName == table.Rows[i][4].ToString()) ?
                        context.TmProgramCategoryTypes.FirstOrDefault(m => m.ProgramCategoryName == table.Rows[i][4].ToString()).Id : null;

                    //Vendor
                    detail.VendorId = table.Rows[17][3].ToString() == string.Empty ? null :
                        context.Companys.Any(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)) ?
                        context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)).Id : null;

                    //Day
                    detail.DayStart = table.Rows[i][5].ToString() == string.Empty ? null : table.Rows[i][5].ToString();
                    detail.DayEnd = table.Rows[i][6].ToString() == string.Empty ? null : table.Rows[i][6].ToString();

                    //Time
                    detail.TimeStart = table.Rows[i][7].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][7].ToString());
                    detail.TimeEnd = table.Rows[i][8].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][8].ToString());

                    //Duration
                    detail.Duration = table.Rows[i][9].ToString() == string.Empty ? null : table.Rows[i][9].ToString();

                    //Currency
                    detail.Currency = table.Rows[i][10].ToString() == string.Empty ? null : table.Rows[i][10].ToString();

                    //Base Rate 30
                    detail.BaseRate30 = table.Rows[i][11].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][11]);

                    //Userd Rate
                    detail.UsedRate = table.Rows[i][12].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][12]);

                    //Program Position
                    detail.TmProgramPositionId = table.Rows[i][13].ToString() == string.Empty ? null :
                        context.TmProgramPositions.Any(m => m.ProgramPositionName == table.Rows[i][13].ToString()) ?
                        context.TmProgramPositions.FirstOrDefault(m => m.ProgramPositionName == table.Rows[i][13].ToString()).Id : null;

                    //Surcharge 
                    detail.Surcharge = table.Rows[i][14].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][14]);

                    //Gross Rate
                    detail.GrossRate = table.Rows[i][15].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][15]);

                    //Disc
                    detail.Disc = table.Rows[i][16].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][16].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Net Rate
                    detail.NetRate = table.Rows[i][17].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][17]);

                    //Bvc
                    detail.Bvc = table.Rows[i][18].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][18].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Bonus
                    detail.Bonus = table.Rows[i][19].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][19]);

                    //Estimate Tvr
                    detail.EstimateTvr = table.Rows[i][20].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][20].ToString());

                    //Tvr
                    detail.Tvr = table.Rows[i][21].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][21].ToString());

                    //Index
                    detail.Index = table.Rows[i][22].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][22]);

                    //Cprp
                    detail.Cprp = table.Rows[i][23].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][23]);

                    //Total Spot
                    detail.TotalSpot = table.Rows[i][444].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][444]);

                    //Total Tarps
                    detail.TotalTarps = table.Rows[i][445].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][445]);

                    //Gross Comited Bonus
                    detail.GrossComitedBonus = table.Rows[i][446].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][446]);

                    //Gross Taken Bonus
                    detail.GrossTakenBonus = table.Rows[i][447].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][447]);

                    //Remaining Bonus
                    detail.RemaniningBonus = table.Rows[i][448].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][448]);

                    //Total Gross Value 
                    detail.TotalGrossValue = table.Rows[i][449].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][449]);

                    //Total New Cost
                    detail.TotalNetCost = table.Rows[i][450].ToString() == string.Empty ? 0 :
                        Convert.ToDecimal(table.Rows[i][450]);

                    mediaPlan.TotalNetCost += detail.TotalNetCost;
                    context.PhoenixAdd(detail);

                    #region Prime Time Schedule
                    for (int j = 24; j < 444; j++)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty && table.Rows[i][j].ToString() != "0")
                        {
                            TrMediaPlanTVDetailSchedule schedule = new TrMediaPlanTVDetailSchedule();

                            //Media Plan Detail
                            schedule.TrMediaPlanDetailId = detail.Id;

                            //Tanggal
                            schedule.Tanggal = table.Rows[16][j].ToString();

                            //Bulan
                            schedule.Bulan = j >= 24 && j <= 58 ? table.Rows[14][24].ToString() :
                                j >= 59 && j <= 93 ? table.Rows[14][59].ToString() :
                                j >= 94 && j <= 128 ? table.Rows[14][94].ToString() :
                                j >= 129 && j <= 163 ? table.Rows[14][129].ToString() :
                                j >= 164 && j <= 198 ? table.Rows[14][164].ToString() :
                                j >= 199 && j <= 233 ? table.Rows[14][199].ToString() :
                                j >= 234 && j <= 268 ? table.Rows[14][234].ToString() :
                                j >= 269 && j <= 303 ? table.Rows[14][269].ToString() :
                                j >= 304 && j <= 338 ? table.Rows[14][304].ToString() :
                                j >= 339 && j <= 373 ? table.Rows[14][339].ToString() :
                                j >= 374 && j <= 408 ? table.Rows[14][374].ToString() :
                                table.Rows[14][409].ToString();

                            //Tahun
                            schedule.Tahun = firstPeriod[1];

                            //Spot Value
                            schedule.SpotValue = table.Rows[i][j].ToString();

                            context.PhoenixAdd(schedule);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Regular Time
            for (int i = 47; i < 68; i++)
            {
                if (table.Rows[i][3].ToString() != string.Empty)
                {
                    TrMediaPlanTVDetail detail = new TrMediaPlanTVDetail();
                    detail.TrMediaPlanId = mediaPlan.Id;

                    //Program Type
                    detail.TmProgramTypeId = table.Rows[18][3].ToString() == string.Empty ? null :
                        context.TmProgramTypes.Any(m => m.ProgramTypeName == table.Rows[18][3].ToString()) ?
                        context.TmProgramTypes.FirstOrDefault(m => m.ProgramTypeName == table.Rows[18][3].ToString()).Id : null;

                    //Media plan type
                    detail.TmMediaTypeId = table.Rows[16][2].ToString() == string.Empty ? null :
                        context.MediaTypes.Any(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())) ?
                        context.MediaTypes.FirstOrDefault(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())).Id : null;

                    //Programe Name
                    detail.ProgramName = table.Rows[i][3].ToString() == string.Empty ? null : table.Rows[i][3].ToString();

                    //Programe Category Type
                    detail.TmProgramCategoryTypeId = table.Rows[i][4].ToString() == string.Empty ? null :
                        context.TmProgramCategoryTypes.Any(m => m.ProgramCategoryName == table.Rows[i][4].ToString()) ?
                        context.TmProgramCategoryTypes.FirstOrDefault(m => m.ProgramCategoryName == table.Rows[i][4].ToString()).Id : null;

                    //Vendor
                    detail.VendorId = table.Rows[17][3].ToString() == string.Empty ? null :
                        context.Companys.Any(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)) ?
                        context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)).Id : null;

                    //Day
                    detail.DayStart = table.Rows[i][5].ToString() == string.Empty ? null : table.Rows[i][5].ToString();
                    detail.DayEnd = table.Rows[i][6].ToString() == string.Empty ? null : table.Rows[i][6].ToString();

                    //Time
                    detail.TimeStart = table.Rows[i][7].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][7].ToString());
                    detail.TimeEnd = table.Rows[i][8].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][8].ToString());

                    //Duration
                    detail.Duration = table.Rows[i][9].ToString() == string.Empty ? null : table.Rows[i][9].ToString();

                    //Currency
                    detail.Currency = table.Rows[i][10].ToString() == string.Empty ? null : table.Rows[i][10].ToString();

                    //Base Rate 30
                    detail.BaseRate30 = table.Rows[i][11].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][11]);

                    //Userd Rate
                    detail.UsedRate = table.Rows[i][12].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][12]);

                    //Program Position
                    detail.TmProgramPositionId = table.Rows[i][13].ToString() == string.Empty ? null :
                        context.TmProgramPositions.Any(m => m.ProgramPositionName == table.Rows[i][13].ToString()) ?
                        context.TmProgramPositions.FirstOrDefault(m => m.ProgramPositionName == table.Rows[i][13].ToString()).Id : null;

                    //Surcharge 
                    detail.Surcharge = table.Rows[i][14].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][14]);

                    //Gross Rate
                    detail.GrossRate = table.Rows[i][15].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][15]);

                    //Disc
                    detail.Disc = table.Rows[i][16].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][16].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Net Rate
                    detail.NetRate = table.Rows[i][17].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][17]);

                    //Bvc
                    detail.Bvc = table.Rows[i][18].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][18].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Bonus
                    detail.Bonus = table.Rows[i][19].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][19]);

                    //Estimate Tvr
                    detail.EstimateTvr = table.Rows[i][20].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][20].ToString());

                    //Tvr
                    detail.Tvr = table.Rows[i][21].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][21].ToString());

                    //Index
                    detail.Index = table.Rows[i][22].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][22]);

                    //Cprp
                    detail.Cprp = table.Rows[i][23].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][23]);

                    //Total Spot
                    detail.TotalSpot = table.Rows[i][444].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][444]);

                    //Total Tarps
                    detail.TotalTarps = table.Rows[i][445].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][445]);

                    //Gross Comited Bonus
                    detail.GrossComitedBonus = table.Rows[i][446].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][446]);

                    //Gross Taken Bonus
                    detail.GrossTakenBonus = table.Rows[i][447].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][447]);

                    //Remaining Bonus
                    detail.RemaniningBonus = table.Rows[i][448].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][448]);

                    //Total Gross Value 
                    detail.TotalGrossValue = table.Rows[i][449].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][449]);

                    //Total New Cost
                    detail.TotalNetCost = table.Rows[i][450].ToString() == string.Empty ? 0 :
                        Convert.ToDecimal(table.Rows[i][450]);

                    mediaPlan.TotalNetCost += detail.TotalNetCost;
                    context.PhoenixAdd(detail);

                    #region Regular Time Schedule
                    for (int j = 24; j < 444; j++)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty && table.Rows[i][j].ToString() != "0")
                        {
                            TrMediaPlanTVDetailSchedule schedule = new TrMediaPlanTVDetailSchedule();

                            //Media Plan Detail
                            schedule.TrMediaPlanDetailId = detail.Id;

                            //Tanggal
                            schedule.Tanggal = table.Rows[16][j].ToString();

                            //Bulan
                            schedule.Bulan = j >= 24 && j <= 58 ? table.Rows[14][24].ToString() :
                                j >= 59 && j <= 93 ? table.Rows[14][59].ToString() :
                                j >= 94 && j <= 128 ? table.Rows[14][94].ToString() :
                                j >= 129 && j <= 163 ? table.Rows[14][129].ToString() :
                                j >= 164 && j <= 198 ? table.Rows[14][164].ToString() :
                                j >= 199 && j <= 233 ? table.Rows[14][199].ToString() :
                                j >= 234 && j <= 268 ? table.Rows[14][234].ToString() :
                                j >= 269 && j <= 303 ? table.Rows[14][269].ToString() :
                                j >= 304 && j <= 338 ? table.Rows[14][304].ToString() :
                                j >= 339 && j <= 373 ? table.Rows[14][339].ToString() :
                                j >= 374 && j <= 408 ? table.Rows[14][374].ToString() :
                                table.Rows[14][409].ToString();

                            //Tahun
                            schedule.Tahun = firstPeriod[1];

                            //Spot Value
                            schedule.SpotValue = table.Rows[i][j].ToString();

                            context.PhoenixAdd(schedule);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Bonus Time
            for (int i = 70; i < 86; i++)
            {
                if (table.Rows[i][3].ToString() != string.Empty)
                {
                    TrMediaPlanTVDetail detail = new TrMediaPlanTVDetail();
                    detail.TrMediaPlanId = mediaPlan.Id;

                    //Program Type
                    detail.TmProgramTypeId = table.Rows[18][3].ToString() == string.Empty ? null :
                        context.TmProgramTypes.Any(m => m.ProgramTypeName == table.Rows[18][3].ToString()) ?
                        context.TmProgramTypes.FirstOrDefault(m => m.ProgramTypeName == table.Rows[18][3].ToString()).Id : null;

                    //Media plan type
                    detail.TmMediaTypeId = table.Rows[16][2].ToString() == string.Empty ? null :
                        context.MediaTypes.Any(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())) ?
                        context.MediaTypes.FirstOrDefault(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())).Id : null;

                    //Programe Name
                    detail.ProgramName = table.Rows[i][3].ToString() == string.Empty ? null : table.Rows[i][3].ToString();

                    //Programe Category Type
                    detail.TmProgramCategoryTypeId = table.Rows[i][4].ToString() == string.Empty ? null :
                        context.TmProgramCategoryTypes.Any(m => m.ProgramCategoryName == table.Rows[i][4].ToString()) ?
                        context.TmProgramCategoryTypes.FirstOrDefault(m => m.ProgramCategoryName == table.Rows[i][4].ToString()).Id : null;

                    //Vendor
                    detail.VendorId = table.Rows[17][3].ToString() == string.Empty ? null :
                        context.Companys.Any(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)) ?
                        context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)).Id : null;

                    //Day
                    detail.DayStart = table.Rows[i][5].ToString() == string.Empty ? null : table.Rows[i][5].ToString();
                    detail.DayEnd = table.Rows[i][6].ToString() == string.Empty ? null : table.Rows[i][6].ToString();

                    //Time
                    detail.TimeStart = table.Rows[i][7].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][7].ToString());
                    detail.TimeEnd = table.Rows[i][8].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][8].ToString());

                    //Duration
                    detail.Duration = table.Rows[i][9].ToString() == string.Empty ? null : table.Rows[i][9].ToString();

                    //Currency
                    detail.Currency = table.Rows[i][10].ToString() == string.Empty ? null : table.Rows[i][10].ToString();

                    //Base Rate 30
                    detail.BaseRate30 = table.Rows[i][11].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][11]);

                    //Userd Rate
                    detail.UsedRate = table.Rows[i][12].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][12]);

                    //Program Position
                    detail.TmProgramPositionId = table.Rows[i][13].ToString() == string.Empty ? null :
                        context.TmProgramPositions.Any(m => m.ProgramPositionName == table.Rows[i][13].ToString()) ?
                        context.TmProgramPositions.FirstOrDefault(m => m.ProgramPositionName == table.Rows[i][13].ToString()).Id : null;

                    //Surcharge 
                    detail.Surcharge = table.Rows[i][14].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][14]);

                    //Gross Rate
                    detail.GrossRate = table.Rows[i][15].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][15]);

                    //Disc
                    detail.Disc = table.Rows[i][16].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][16].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Net Rate
                    detail.NetRate = table.Rows[i][17].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][17]);

                    //Bvc
                    detail.Bvc = table.Rows[i][18].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][18].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Bonus
                    detail.Bonus = table.Rows[i][19].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][19]);

                    //Estimate Tvr
                    detail.EstimateTvr = table.Rows[i][20].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][20].ToString());

                    //Tvr
                    detail.Tvr = table.Rows[i][21].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][21].ToString());

                    //Index
                    detail.Index = table.Rows[i][22].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][22]);

                    //Cprp
                    detail.Cprp = table.Rows[i][23].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][23]);

                    //Total Spot
                    detail.TotalSpot = table.Rows[i][444].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][444]);

                    //Total Tarps
                    detail.TotalTarps = table.Rows[i][445].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][445]);

                    //Gross Comited Bonus
                    detail.GrossComitedBonus = table.Rows[i][446].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][446]);

                    //Gross Taken Bonus
                    detail.GrossTakenBonus = table.Rows[i][447].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][447]);

                    //Remaining Bonus
                    detail.RemaniningBonus = table.Rows[i][448].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][448]);

                    //Total Gross Value 
                    detail.TotalGrossValue = table.Rows[i][449].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][449]);

                    //Total New Cost
                    detail.TotalNetCost = table.Rows[i][450].ToString() == string.Empty ? 0 :
                        Convert.ToDecimal(table.Rows[i][450]);

                    mediaPlan.TotalNetCost += detail.TotalNetCost;
                    context.PhoenixAdd(detail);

                    #region Bonus Time Schedule
                    for (int j = 24; j < 444; j++)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty && table.Rows[i][j].ToString() != "0")
                        {
                            TrMediaPlanTVDetailSchedule schedule = new TrMediaPlanTVDetailSchedule();

                            //Media Plan Detail
                            schedule.TrMediaPlanDetailId = detail.Id;

                            //Tanggal
                            schedule.Tanggal = table.Rows[16][j].ToString();

                            //Bulan
                            schedule.Bulan = j >= 24 && j <= 58 ? table.Rows[14][24].ToString() :
                                j >= 59 && j <= 93 ? table.Rows[14][59].ToString() :
                                j >= 94 && j <= 128 ? table.Rows[14][94].ToString() :
                                j >= 129 && j <= 163 ? table.Rows[14][129].ToString() :
                                j >= 164 && j <= 198 ? table.Rows[14][164].ToString() :
                                j >= 199 && j <= 233 ? table.Rows[14][199].ToString() :
                                j >= 234 && j <= 268 ? table.Rows[14][234].ToString() :
                                j >= 269 && j <= 303 ? table.Rows[14][269].ToString() :
                                j >= 304 && j <= 338 ? table.Rows[14][304].ToString() :
                                j >= 339 && j <= 373 ? table.Rows[14][339].ToString() :
                                j >= 374 && j <= 408 ? table.Rows[14][374].ToString() :
                                table.Rows[14][409].ToString();

                            //Tahun
                            schedule.Tahun = firstPeriod[1];

                            //Spot Value
                            schedule.SpotValue = table.Rows[i][j].ToString();

                            context.PhoenixAdd(schedule);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #endregion
            context.SaveChanges();

            FileMaster.TrMediaPlanTvId = mediaPlan.Id;

            context.PhoenixEdit(FileMaster);
            context.PhoenixEdit(mediaPlan);

            await context.SaveChangesAsync();

            return mediaPlan;
        }

        public async Task<TrMediaPlanTV> UpdateDetailAsync(TrMediaPlanTV mediaPlan, IFormFile file, DataSet data)
        {
            TrMediaFileMaster FileMaster = new TrMediaFileMaster();
            var table = data.Tables["TV"];

            #region UploadFileMaster
            //Upload ke FileMaster
            var fileId = await uploadFile.Upload(file, null);
            if (!string.IsNullOrEmpty(fileId))
            {
                FileMaster.FileMasterId = fileId;
                FileMaster.TrMediaPlanTvId = mediaPlan.Id;
            }

            context.PhoenixAdd(FileMaster);
            #endregion

            #region Input Media Plan TV Detail

            #region Prime Time
            for (int i = 19; i < 46; i++)
            {
                if (table.Rows[i][3].ToString() != string.Empty)
                {
                    TrMediaPlanTVDetail detail = new TrMediaPlanTVDetail();
                    detail.TrMediaPlanId = mediaPlan.Id;

                    //Program Type
                    detail.TmProgramTypeId = table.Rows[18][3].ToString() == string.Empty ? null :
                        context.TmProgramTypes.Any(m => m.ProgramTypeName == table.Rows[18][3].ToString()) ?
                        context.TmProgramTypes.FirstOrDefault(m => m.ProgramTypeName == table.Rows[18][3].ToString()).Id : null;

                    //Media plan type
                    detail.TmMediaTypeId = table.Rows[16][2].ToString() == string.Empty ? null :
                        context.MediaTypes.Any(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())) ?
                        context.MediaTypes.FirstOrDefault(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())).Id : null;

                    //Programe Name
                    detail.ProgramName = table.Rows[i][3].ToString() == string.Empty ? null : table.Rows[i][3].ToString();

                    //Programe Category Type
                    detail.TmProgramCategoryTypeId = table.Rows[i][4].ToString() == string.Empty ? null :
                        context.TmProgramCategoryTypes.Any(m => m.ProgramCategoryName == table.Rows[i][4].ToString()) ?
                        context.TmProgramCategoryTypes.FirstOrDefault(m => m.ProgramCategoryName == table.Rows[i][4].ToString()).Id : null;

                    //Vendor
                    detail.VendorId = table.Rows[17][3].ToString() == string.Empty ? null :
                        context.Companys.Any(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)) ?
                        context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)).Id : null;

                    //Day
                    detail.DayStart = table.Rows[i][5].ToString() == string.Empty ? null : table.Rows[i][5].ToString();
                    detail.DayEnd = table.Rows[i][6].ToString() == string.Empty ? null : table.Rows[i][6].ToString();

                    //Time
                    detail.TimeStart = table.Rows[i][7].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][7].ToString());
                    detail.TimeEnd = table.Rows[i][8].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][8].ToString());

                    //Duration
                    detail.Duration = table.Rows[i][9].ToString() == string.Empty ? null : table.Rows[i][9].ToString();

                    //Currency
                    detail.Currency = table.Rows[i][10].ToString() == string.Empty ? null : table.Rows[i][10].ToString();

                    //Base Rate 30
                    detail.BaseRate30 = table.Rows[i][11].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][11]);

                    //Userd Rate
                    detail.UsedRate = table.Rows[i][12].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][12]);

                    //Program Position
                    detail.TmProgramPositionId = table.Rows[i][13].ToString() == string.Empty ? null :
                        context.TmProgramPositions.Any(m => m.ProgramPositionName == table.Rows[i][13].ToString()) ?
                        context.TmProgramPositions.FirstOrDefault(m => m.ProgramPositionName == table.Rows[i][13].ToString()).Id : null;

                    //Surcharge 
                    detail.Surcharge = table.Rows[i][14].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][14]);

                    //Gross Rate
                    detail.GrossRate = table.Rows[i][15].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][15]);

                    //Disc
                    detail.Disc = table.Rows[i][16].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][16].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Net Rate
                    detail.NetRate = table.Rows[i][17].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][17]);

                    //Bvc
                    detail.Bvc = table.Rows[i][18].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][18].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Bonus
                    detail.Bonus = table.Rows[i][19].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][19]);

                    //Estimate Tvr
                    detail.EstimateTvr = table.Rows[i][20].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][20].ToString());

                    //Tvr
                    detail.Tvr = table.Rows[i][21].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][21].ToString());

                    //Index
                    detail.Index = table.Rows[i][22].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][22]);

                    //Cprp
                    detail.Cprp = table.Rows[i][23].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][23]);

                    //Total Spot
                    detail.TotalSpot = table.Rows[i][444].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][444]);

                    //Total Tarps
                    detail.TotalTarps = table.Rows[i][445].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][445]);

                    //Gross Comited Bonus
                    detail.GrossComitedBonus = table.Rows[i][446].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][446]);

                    //Gross Taken Bonus
                    detail.GrossTakenBonus = table.Rows[i][447].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][447]);

                    //Remaining Bonus
                    detail.RemaniningBonus = table.Rows[i][448].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][448]);

                    //Total Gross Value 
                    detail.TotalGrossValue = table.Rows[i][449].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][449]);

                    //Total New Cost
                    detail.TotalNetCost = table.Rows[i][450].ToString() == string.Empty ? 0 :
                        Convert.ToDecimal(table.Rows[i][450]);

                    mediaPlan.TotalNetCost += detail.TotalNetCost;
                    context.PhoenixAdd(detail);

                    #region Prime Time Schedule
                    for (int j = 24; j < 444; j++)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty && table.Rows[i][j].ToString() != "0")
                        {
                            TrMediaPlanTVDetailSchedule schedule = new TrMediaPlanTVDetailSchedule();

                            //Media Plan Detail
                            schedule.TrMediaPlanDetailId = detail.Id;

                            //Tanggal
                            schedule.Tanggal = table.Rows[16][j].ToString();

                            //Bulan
                            schedule.Bulan = j >= 24 && j <= 58 ? table.Rows[14][24].ToString() :
                                j >= 59 && j <= 93 ? table.Rows[14][59].ToString() :
                                j >= 94 && j <= 128 ? table.Rows[14][94].ToString() :
                                j >= 129 && j <= 163 ? table.Rows[14][129].ToString() :
                                j >= 164 && j <= 198 ? table.Rows[14][164].ToString() :
                                j >= 199 && j <= 233 ? table.Rows[14][199].ToString() :
                                j >= 234 && j <= 268 ? table.Rows[14][234].ToString() :
                                j >= 269 && j <= 303 ? table.Rows[14][269].ToString() :
                                j >= 304 && j <= 338 ? table.Rows[14][304].ToString() :
                                j >= 339 && j <= 373 ? table.Rows[14][339].ToString() :
                                j >= 374 && j <= 408 ? table.Rows[14][374].ToString() :
                                table.Rows[14][409].ToString();

                            //Tahun
                            schedule.Tahun = mediaPlan.PeriodCampaignStart.GetValueOrDefault().Year.ToString();

                            //Spot Value
                            schedule.SpotValue = table.Rows[i][j].ToString();

                            context.PhoenixAdd(schedule);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Regular Time
            for (int i = 47; i < 68; i++)
            {
                if (table.Rows[i][3].ToString() != string.Empty)
                {
                    TrMediaPlanTVDetail detail = new TrMediaPlanTVDetail();
                    detail.TrMediaPlanId = mediaPlan.Id;

                    //Program Type
                    detail.TmProgramTypeId = table.Rows[18][3].ToString() == string.Empty ? null :
                        context.TmProgramTypes.Any(m => m.ProgramTypeName == table.Rows[18][3].ToString()) ?
                        context.TmProgramTypes.FirstOrDefault(m => m.ProgramTypeName == table.Rows[18][3].ToString()).Id : null;

                    //Media plan type
                    detail.TmMediaTypeId = table.Rows[16][2].ToString() == string.Empty ? null :
                        context.MediaTypes.Any(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())) ?
                        context.MediaTypes.FirstOrDefault(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())).Id : null;

                    //Programe Name
                    detail.ProgramName = table.Rows[i][3].ToString() == string.Empty ? null : table.Rows[i][3].ToString();

                    //Programe Category Type
                    detail.TmProgramCategoryTypeId = table.Rows[i][4].ToString() == string.Empty ? null :
                        context.TmProgramCategoryTypes.Any(m => m.ProgramCategoryName == table.Rows[i][4].ToString()) ?
                        context.TmProgramCategoryTypes.FirstOrDefault(m => m.ProgramCategoryName == table.Rows[i][4].ToString()).Id : null;

                    //Vendor
                    detail.VendorId = table.Rows[17][3].ToString() == string.Empty ? null :
                        context.Companys.Any(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)) ?
                        context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)).Id : null;

                    //Day
                    detail.DayStart = table.Rows[i][5].ToString() == string.Empty ? null : table.Rows[i][5].ToString();
                    detail.DayEnd = table.Rows[i][6].ToString() == string.Empty ? null : table.Rows[i][6].ToString();

                    //Time
                    detail.TimeStart = table.Rows[i][7].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][7].ToString());
                    detail.TimeEnd = table.Rows[i][8].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][8].ToString());

                    //Duration
                    detail.Duration = table.Rows[i][9].ToString() == string.Empty ? null : table.Rows[i][9].ToString();

                    //Currency
                    detail.Currency = table.Rows[i][10].ToString() == string.Empty ? null : table.Rows[i][10].ToString();

                    //Base Rate 30
                    detail.BaseRate30 = table.Rows[i][11].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][11]);

                    //Userd Rate
                    detail.UsedRate = table.Rows[i][12].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][12]);

                    //Program Position
                    detail.TmProgramPositionId = table.Rows[i][13].ToString() == string.Empty ? null :
                        context.TmProgramPositions.Any(m => m.ProgramPositionName == table.Rows[i][13].ToString()) ?
                        context.TmProgramPositions.FirstOrDefault(m => m.ProgramPositionName == table.Rows[i][13].ToString()).Id : null;

                    //Surcharge 
                    detail.Surcharge = table.Rows[i][14].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][14]);

                    //Gross Rate
                    detail.GrossRate = table.Rows[i][15].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][15]);

                    //Disc
                    detail.Disc = table.Rows[i][16].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][16].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Net Rate
                    detail.NetRate = table.Rows[i][17].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][17]);

                    //Bvc
                    detail.Bvc = table.Rows[i][18].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][18].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Bonus
                    detail.Bonus = table.Rows[i][19].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][19]);

                    //Estimate Tvr
                    detail.EstimateTvr = table.Rows[i][20].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][20].ToString());

                    //Tvr
                    detail.Tvr = table.Rows[i][21].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][21].ToString());

                    //Index
                    detail.Index = table.Rows[i][22].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][22]);

                    //Cprp
                    detail.Cprp = table.Rows[i][23].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][23]);

                    //Total Spot
                    detail.TotalSpot = table.Rows[i][444].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][444]);

                    //Total Tarps
                    detail.TotalTarps = table.Rows[i][445].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][445]);

                    //Gross Comited Bonus
                    detail.GrossComitedBonus = table.Rows[i][446].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][446]);

                    //Gross Taken Bonus
                    detail.GrossTakenBonus = table.Rows[i][447].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][447]);

                    //Remaining Bonus
                    detail.RemaniningBonus = table.Rows[i][448].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][448]);

                    //Total Gross Value 
                    detail.TotalGrossValue = table.Rows[i][449].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][449]);

                    //Total New Cost
                    detail.TotalNetCost = table.Rows[i][450].ToString() == string.Empty ? 0 :
                        Convert.ToDecimal(table.Rows[i][450]);

                    mediaPlan.TotalNetCost += detail.TotalNetCost;
                    context.PhoenixAdd(detail);

                    #region Regular Time Schedule
                    for (int j = 24; j < 444; j++)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty && table.Rows[i][j].ToString() != "0")
                        {
                            TrMediaPlanTVDetailSchedule schedule = new TrMediaPlanTVDetailSchedule();

                            //Media Plan Detail
                            schedule.TrMediaPlanDetailId = detail.Id;

                            //Tanggal
                            schedule.Tanggal = table.Rows[16][j].ToString();

                            //Bulan
                            schedule.Bulan = j >= 24 && j <= 58 ? table.Rows[14][24].ToString() :
                                j >= 59 && j <= 93 ? table.Rows[14][59].ToString() :
                                j >= 94 && j <= 128 ? table.Rows[14][94].ToString() :
                                j >= 129 && j <= 163 ? table.Rows[14][129].ToString() :
                                j >= 164 && j <= 198 ? table.Rows[14][164].ToString() :
                                j >= 199 && j <= 233 ? table.Rows[14][199].ToString() :
                                j >= 234 && j <= 268 ? table.Rows[14][234].ToString() :
                                j >= 269 && j <= 303 ? table.Rows[14][269].ToString() :
                                j >= 304 && j <= 338 ? table.Rows[14][304].ToString() :
                                j >= 339 && j <= 373 ? table.Rows[14][339].ToString() :
                                j >= 374 && j <= 408 ? table.Rows[14][374].ToString() :
                                table.Rows[14][409].ToString();

                            //Tahun
                            schedule.Tahun = schedule.Tahun = mediaPlan.PeriodCampaignStart.GetValueOrDefault().Year.ToString();

                            //Spot Value
                            schedule.SpotValue = table.Rows[i][j].ToString();

                            context.PhoenixAdd(schedule);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Bonus Time
            for (int i = 70; i < 86; i++)
            {
                if (table.Rows[i][3].ToString() != string.Empty)
                {
                    TrMediaPlanTVDetail detail = new TrMediaPlanTVDetail();
                    detail.TrMediaPlanId = mediaPlan.Id;

                    //Program Type
                    detail.TmProgramTypeId = table.Rows[18][3].ToString() == string.Empty ? null :
                        context.TmProgramTypes.Any(m => m.ProgramTypeName == table.Rows[18][3].ToString()) ?
                        context.TmProgramTypes.FirstOrDefault(m => m.ProgramTypeName == table.Rows[18][3].ToString()).Id : null;

                    //Media plan type
                    detail.TmMediaTypeId = table.Rows[16][2].ToString() == string.Empty ? null :
                        context.MediaTypes.Any(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())) ?
                        context.MediaTypes.FirstOrDefault(m => m.MediaTypeName.Contains(table.Rows[16][2].ToString())).Id : null;

                    //Programe Name
                    detail.ProgramName = table.Rows[i][3].ToString() == string.Empty ? null : table.Rows[i][3].ToString();

                    //Programe Category Type
                    detail.TmProgramCategoryTypeId = table.Rows[i][4].ToString() == string.Empty ? null :
                        context.TmProgramCategoryTypes.Any(m => m.ProgramCategoryName == table.Rows[i][4].ToString()) ?
                        context.TmProgramCategoryTypes.FirstOrDefault(m => m.ProgramCategoryName == table.Rows[i][4].ToString()).Id : null;

                    //Vendor
                    detail.VendorId = table.Rows[17][3].ToString() == string.Empty ? null :
                        context.Companys.Any(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)) ?
                        context.Companys.FirstOrDefault(m => m.CompanyName == table.Rows[17][3].ToString() && (m.IsVendor ?? false)).Id : null;

                    //Day
                    detail.DayStart = table.Rows[i][5].ToString() == string.Empty ? null : table.Rows[i][5].ToString();
                    detail.DayEnd = table.Rows[i][6].ToString() == string.Empty ? null : table.Rows[i][6].ToString();

                    //Time
                    detail.TimeStart = table.Rows[i][7].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][7].ToString());
                    detail.TimeEnd = table.Rows[i][8].ToString() == string.Empty ? (DateTime?)null :
                        Convert.ToDateTime(table.Rows[i][8].ToString());

                    //Duration
                    detail.Duration = table.Rows[i][9].ToString() == string.Empty ? null : table.Rows[i][9].ToString();

                    //Currency
                    detail.Currency = table.Rows[i][10].ToString() == string.Empty ? null : table.Rows[i][10].ToString();

                    //Base Rate 30
                    detail.BaseRate30 = table.Rows[i][11].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][11]);

                    //Userd Rate
                    detail.UsedRate = table.Rows[i][12].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][12]);

                    //Program Position
                    detail.TmProgramPositionId = table.Rows[i][13].ToString() == string.Empty ? null :
                        context.TmProgramPositions.Any(m => m.ProgramPositionName == table.Rows[i][13].ToString()) ?
                        context.TmProgramPositions.FirstOrDefault(m => m.ProgramPositionName == table.Rows[i][13].ToString()).Id : null;

                    //Surcharge 
                    detail.Surcharge = table.Rows[i][14].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][14]);

                    //Gross Rate
                    detail.GrossRate = table.Rows[i][15].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][15]);

                    //Disc
                    detail.Disc = table.Rows[i][16].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][16].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Net Rate
                    detail.NetRate = table.Rows[i][17].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][17]);

                    //Bvc
                    detail.Bvc = table.Rows[i][18].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][18].ToString().Replace('%', ' ').Replace(" ", string.Empty));

                    //Bonus
                    detail.Bonus = table.Rows[i][19].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][19]);

                    //Estimate Tvr
                    detail.EstimateTvr = table.Rows[i][20].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][20].ToString());

                    //Tvr
                    detail.Tvr = table.Rows[i][21].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][21].ToString());

                    //Index
                    detail.Index = table.Rows[i][22].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][22]);

                    //Cprp
                    detail.Cprp = table.Rows[i][23].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][23]);

                    //Total Spot
                    detail.TotalSpot = table.Rows[i][444].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][444]);

                    //Total Tarps
                    detail.TotalTarps = table.Rows[i][445].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][445]);

                    //Gross Comited Bonus
                    detail.GrossComitedBonus = table.Rows[i][446].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][446]);

                    //Gross Taken Bonus
                    detail.GrossTakenBonus = table.Rows[i][447].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][447]);

                    //Remaining Bonus
                    detail.RemaniningBonus = table.Rows[i][448].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][448]);

                    //Total Gross Value 
                    detail.TotalGrossValue = table.Rows[i][449].ToString() == string.Empty ? (decimal?)null :
                        Convert.ToDecimal(table.Rows[i][449]);

                    //Total New Cost
                    detail.TotalNetCost = table.Rows[i][450].ToString() == string.Empty ? 0 :
                        Convert.ToDecimal(table.Rows[i][450]);

                    mediaPlan.TotalNetCost += detail.TotalNetCost;
                    context.PhoenixAdd(detail);

                    #region Bonus Time Schedule
                    for (int j = 24; j < 444; j++)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty && table.Rows[i][j].ToString() != "0")
                        {
                            TrMediaPlanTVDetailSchedule schedule = new TrMediaPlanTVDetailSchedule();

                            //Media Plan Detail
                            schedule.TrMediaPlanDetailId = detail.Id;

                            //Tanggal
                            schedule.Tanggal = table.Rows[16][j].ToString();

                            //Bulan
                            schedule.Bulan = j >= 24 && j <= 58 ? table.Rows[14][24].ToString() :
                                j >= 59 && j <= 93 ? table.Rows[14][59].ToString() :
                                j >= 94 && j <= 128 ? table.Rows[14][94].ToString() :
                                j >= 129 && j <= 163 ? table.Rows[14][129].ToString() :
                                j >= 164 && j <= 198 ? table.Rows[14][164].ToString() :
                                j >= 199 && j <= 233 ? table.Rows[14][199].ToString() :
                                j >= 234 && j <= 268 ? table.Rows[14][234].ToString() :
                                j >= 269 && j <= 303 ? table.Rows[14][269].ToString() :
                                j >= 304 && j <= 338 ? table.Rows[14][304].ToString() :
                                j >= 339 && j <= 373 ? table.Rows[14][339].ToString() :
                                j >= 374 && j <= 408 ? table.Rows[14][374].ToString() :
                                table.Rows[14][409].ToString();

                            //Tahun
                            schedule.Tahun = schedule.Tahun = mediaPlan.PeriodCampaignStart.GetValueOrDefault().Year.ToString();

                            //Spot Value
                            schedule.SpotValue = table.Rows[i][j].ToString();

                            context.PhoenixAdd(schedule);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #endregion

            context.PhoenixEdit(mediaPlan);

            await context.SaveChangesAsync();
            return mediaPlan;
        }

        public Task<int> AddRangeAsync(params TrMediaPlanTV[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TrMediaPlanTV entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TrMediaPlanTV[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrMediaPlanTV entity)
        {
            context.PhoenixDelete(entity);
            var details = context.TrMediaPlanTVDetails.Where(m => m.TrMediaPlanId == entity.Id);
            if (details.Any())
            {
                foreach (var detail in details)
                {
                    context.PhoenixDelete(detail);
                    var schedules = context.TrMediaPlanTVDetailSchedules.Where(m => m.TrMediaPlanDetailId == detail.Id);
                    if (schedules.Any())
                    {
                        foreach (var schedule in schedules)
                        {
                            context.PhoenixDelete(schedule);
                        }
                    }
                }
            }
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TrMediaPlanTV[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TrMediaPlanTV entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TrMediaPlanTV entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TrMediaPlanTV>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TrMediaPlanTV> Get(string Id) => await context.TrMediaPlanTVs.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        Task<List<TrMediaPlanTV>> IDataService<TrMediaPlanTV>.Get()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrMediaPlanTV>> GetMediaPlanTVs()
        {
            var data = await (from tv in context.TrMediaPlanTVs
                              join emp in context.EmployeeBasicInfos on tv.EmployeeBasicInfoId equals emp.Id
                              join job in context.JobTitles on emp.JobTitleId equals job.Id into j
                              from job in j.DefaultIfEmpty()
                              join stat in context.TmMediaPlanStatuses on tv.TmMediaPlanStatusId equals stat.Id
                              where !tv.IsDeleted.GetValueOrDefault() && tv.TmMediaPlanStatusId == MediaPlanStatusName.Draft
                              orderby tv.CreatedOn
                              select new TrMediaPlanTV
                              {
                                  Id = tv.Id,
                                  MediaPlanNo = tv.MediaPlanNo,
                                  Version = tv.Version,
                                  Duration = tv.Duration,
                                  TargetAudience = tv.TargetAudience,
                                  MediaTypeId = tv.MediaTypeId,
                                  PeriodCampaignStart = tv.PeriodCampaignStart,
                                  PeriodCampaignEnd = tv.PeriodCampaignEnd,
                                  DatePrepared = tv.DatePrepared,
                                  RevisionNo = tv.RevisionNo,
                                  TotalNetCost = tv.TotalNetCost,
                                  AgencyServiceFee = tv.AgencyServiceFee,
                                  AgencyServiceFeePercent = tv.AgencyServiceFeePercent,
                                  TaxBased = tv.TaxBased,
                                  Vat = tv.Vat,
                                  GrandTotal = tv.GrandTotal,
                                  TmMediaPlanRequestTypeId = tv.TmMediaPlanRequestTypeId,
                                  TrMediaFileMasterId = tv.TrMediaFileMasterId,
                                  TmMediaPlanStatusId = tv.TmMediaPlanStatusId,
                                  EmployeeBasicInfoId = tv.EmployeeBasicInfoId,
                                  CreatedBy = tv.CreatedBy,
                                  CreatedOn = tv.CreatedOn,
                                  ModifiedBy = tv.ModifiedBy,
                                  ModifiedOn = tv.ModifiedOn,
                                  ApprovedBy = tv.ApprovedBy,
                                  ApprovedOn = tv.ApprovedOn,
                                  IsActive = tv.IsActive,
                                  IsLocked = tv.IsLocked,
                                  IsDefault = tv.IsDefault,
                                  IsDeleted = tv.IsDeleted,
                                  OwnerId = tv.OwnerId,
                                  DeletedBy = tv.DeletedBy,
                                  DeletedOn = tv.DeletedOn,
                                  RequesterName = emp.NameEmployee,
                                  JobTitleName = job.TitleName,
                                  MediaPlanStatusName = stat.MediaPlanStatusName
                              }).ToListAsync();

            return data;
        }

        public async Task<List<TrMediaPlanTVDetail>> GetMediaPlanTVDetails(string mediaPlanTvId)
        {
            var data = await (from tvDetail in context.TrMediaPlanTVDetails
                              join tv in context.TrMediaPlanTVs on tvDetail.TrMediaPlanId equals tv.Id
                              join type in context.TmProgramTypes on tvDetail.TmProgramTypeId equals type.Id into ty
                              from types in ty.DefaultIfEmpty()
                              join categoryType in context.TmProgramCategoryTypes on tvDetail.TmProgramCategoryTypeId equals categoryType.Id into ct
                              from category in ct.DefaultIfEmpty()
                              join pos in context.TmProgramPositions on tvDetail.TmProgramPositionId equals pos.Id into ps
                              from position in ps.DefaultIfEmpty()
                              where tv.Id == mediaPlanTvId
                              select new TrMediaPlanTVDetail
                              {
                                  Id = tvDetail.Id,
                                  TrMediaPlanId = tvDetail.TrMediaPlanId,
                                  TmProgramCategoryTypeId = tvDetail.TmProgramCategoryTypeId,
                                  TmMediaTypeId = tvDetail.TmMediaTypeId,
                                  VendorId = tvDetail.VendorId,
                                  TmProgramPositionId = tvDetail.TmProgramPositionId,
                                  TmProgramTypeId = tvDetail.TmProgramTypeId,
                                  ProgramName = tvDetail.ProgramName,
                                  DayStart = tvDetail.DayStart,
                                  DayEnd = tvDetail.DayEnd,
                                  TimeStart = tvDetail.TimeStart,
                                  TimeEnd = tvDetail.TimeEnd,
                                  Duration = tvDetail.Duration,
                                  Currency = tvDetail.Currency,
                                  BaseRate30 = tvDetail.BaseRate30,
                                  UsedRate = tvDetail.UsedRate,
                                  Surcharge = tvDetail.Surcharge,
                                  GrossRate = tvDetail.GrossRate,
                                  Disc = tvDetail.Disc,
                                  Bvc = tvDetail.Bvc,
                                  Bonus = tvDetail.Bonus,
                                  EstimateTvr = tvDetail.EstimateTvr,
                                  Tvr = tvDetail.Tvr,
                                  Index = tvDetail.Index,
                                  Cprp = tvDetail.Cprp,
                                  TotalSpot = tvDetail.TotalSpot,
                                  TotalTarps = tvDetail.TotalTarps,
                                  GrossComitedBonus = tvDetail.GrossComitedBonus,
                                  GrossTakenBonus = tvDetail.GrossTakenBonus,
                                  RemaniningBonus = tvDetail.RemaniningBonus,
                                  TotalGrossValue = tvDetail.TotalGrossValue,
                                  TotalNetCost = tvDetail.TotalNetCost,
                                  CreatedBy = tvDetail.CreatedBy,
                                  CreatedOn = tvDetail.CreatedOn,
                                  ModifiedBy = tvDetail.ModifiedBy,
                                  ModifiedOn = tvDetail.ModifiedOn,
                                  ApprovedBy = tvDetail.ApprovedBy,
                                  ApprovedOn = tvDetail.ApprovedOn,
                                  IsActive = tvDetail.IsActive,
                                  IsLocked = tvDetail.IsLocked,
                                  IsDefault = tvDetail.IsDefault,
                                  IsDeleted = tvDetail.IsDeleted,
                                  OwnerId = tvDetail.OwnerId,
                                  DeletedBy = tvDetail.DeletedBy,
                                  DeletedOn = tvDetail.DeletedOn,
                                  BroadcastTypeName = types.ProgramTypeName,
                                  TypeProgramName = category.ProgramCategoryName,
                                  PositionName = position.ProgramPositionName
                              }).ToListAsync();

            return data;
        }

        public async Task<DataSet> ReadExcel(IFormFile file)
        {
            using (var reader = file.OpenReadStream())
            {
                var dataSet = ExcelConverter.ExcelToDataSet(reader);
                return dataSet;
            }
        }

        public async Task<string> GetMediaPlanTvId(string periodCampaign)
        {
            var periodSplit = periodCampaign.Split('/');
            var firstPeriod = periodSplit[0].Split('-');
            var secondPeriod = periodSplit[1].Split('-');
            string firstYear = firstPeriod[1].Substring(2, 2);
            string secondYear = secondPeriod[1].Substring(2, 2);
            string lastCode = firstYear == secondYear ? $"{firstPeriod[0]}-{secondPeriod[0]}/{firstYear}" :
                $"{firstPeriod[0]}-{secondPeriod[0]}/{firstYear}-{secondYear}";
            var planTvId = await TransID.GetTransIdMediaPlanTV(context, Code.MediaPlanTV, DateTime.Today, lastCode);
            return planTvId;
        }

        public async Task<TrMediaFileMaster> GetMediaPlanTvFileTemplateId()
        {
            var template = new TrMediaFileMaster();
            var templateFile = context.TrMediaFileMasters.Where(m => m.TrMediaPlanTvId == null && !m.IsDeleted.GetValueOrDefault()).OrderByDescending(m => m.CreatedOn);
            if (templateFile.Any())
            {
                template = await templateFile.FirstOrDefaultAsync();
            }
            return template;
        }
    }
}
