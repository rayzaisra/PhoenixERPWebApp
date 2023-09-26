using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.Models.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Data
{
    public class ScheduleService
    {
        public string GetSchedule(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    return context.SysParams.Select(x => x.CrontabPayroll).FirstOrDefault();
                }
            }
        }

        public async Task PromotionAsync(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    var promotion = await (from a in context.Promotions
                                           join b in context.TrTemplateApprovals on a.Id equals b.RefId
                                           where !(a.IsSynchronize ?? false) && Convert.ToDateTime(a.EffectiveDate).Date == DateTime.Now.Date && b.StatusApproved == Models.Um.StatusApproved.Approved
                                           select a).AsNoTracking().ToListAsync();
                    if (promotion.Count > 0)
                    {
                        var additionalList = await context.PromotionSubcomponentAdditionals.AsNoTracking().Where(x => promotion.Any(y => y.Id == x.AdditionalPromotionId)).ToListAsync();
                        var deductionList = await context.PromotionSubcomponentDeductions.AsNoTracking().Where(x => promotion.Any(y => y.Id == x.PromotionId)).ToListAsync();
                        var employeeAdditionalList = await context.EmployeeSubcomponentAdditionals.AsNoTracking().Where(x => promotion.Any(y => y.EmployeeBasicInfoId == x.AdditionalEmployeeBasicInfoId)).ToListAsync();
                        var employeeDeductionList = await context.EmployeeSubcomponentDeductions.AsNoTracking().Where(x => promotion.Any(y => y.EmployeeBasicInfoId == x.EmployeeBasicInfoId)).ToListAsync();

                        using (var transaction = await context.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                //proses update salary deduction dan additional employee profile
                                var currentAdditional = additionalList.Where(x => x.Type == Payroll.Type.CurrentSallary);
                                var newAdditional = additionalList.Where(x => x.Type == Payroll.Type.NewSallary);
                                var currentDeduction = deductionList.Where(x => x.Type == Payroll.Type.CurrentSallary);
                                var newDeduction = deductionList.Where(x => x.Type == Payroll.Type.NewSallary);

                                //context.PromotionSubcomponentAdditionals.RemoveRange(currentAdditional.ToArray());
                                //context.PromotionSubcomponentDeductions.RemoveRange(currentDeduction.ToArray());

                                Parallel.ForEach(newAdditional, (item) =>
                                {
                                    //item.Type = Payroll.Type.CurrentSallary;

                                    var employeeAdditional = employeeAdditionalList.Where(x => x.AdditionalEmployeeBasicInfoId == item.AdditionalEmployeeBasicInfoId &&
                                                            x.AdditionalSalaryComponent == item.AdditionalSalaryComponent && x.AdditionalComponentId == item.AdditionalComponentId).FirstOrDefault();

                                    employeeAdditional.AdditionalAmount =  item.AdditionalAmount.ToString();
                                    context.EmployeeSubcomponentAdditionals.Update(employeeAdditional);
                                    //context.PromotionSubcomponentAdditionals.Update(item);
                                });

                                Parallel.ForEach(newDeduction, (item) =>
                                {
                                    //item.Type = Payroll.Type.CurrentSallary;

                                    var employeeDeduction = employeeDeductionList.Where(x => x.EmployeeBasicInfoId == item.EmployeeBasicInfoId &&
                                                            x.SalaryComponent == item.SalaryComponent && x.ComponentId == item.ComponentId).FirstOrDefault();

                                    employeeDeduction.Amount = item.DeductionAmount.ToString();
                                    context.EmployeeSubcomponentDeductions.Update(employeeDeduction);
                                    //context.PromotionSubcomponentDeductions.Update(item);
                                });

                                Parallel.ForEach(promotion, (item) =>
                                {

                                    item.IsSynchronize = true;
                                    context.Promotions.Update(item);
                                });

                                await context.SaveChangesAsync();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception(ex.Message);
                            }
                        }
                    }
                }

            }
        }

        public async Task RotationAsync(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    var rotation = await (from a in context.Rotations
                                          join b in context.TrTemplateApprovals on a.Id equals b.RefId
                                          where !(a.IsSynchronize ?? false) && Convert.ToDateTime(a.EffectiveDate).Date == DateTime.Now.Date && b.StatusApproved == Models.Um.StatusApproved.Approved
                                          select a).AsNoTracking().ToListAsync();
                    if (rotation.Count > 0)
                    {
                        var additionalList = await context.RotationSubcomponentAdditionals.AsNoTracking().Where(x => rotation.Any(y => y.Id == x.AdditionalRotationId)).ToListAsync();
                        var deductionList = await context.RotationSubcomponentDeductions.AsNoTracking().Where(x => rotation.Any(y => y.Id == x.RotationId)).ToListAsync();
                        var employeeAdditionalList = await context.EmployeeSubcomponentAdditionals.AsNoTracking().Where(x => rotation.Any(y => y.EmployeeBasicInfoId == x.AdditionalEmployeeBasicInfoId)).ToListAsync();
                        var employeeDeductionList = await context.EmployeeSubcomponentDeductions.AsNoTracking().Where(x => rotation.Any(y => y.EmployeeBasicInfoId == x.EmployeeBasicInfoId)).ToListAsync();

                        using (var transaction = await context.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                //proses update salary deduction dan additional employee profile
                                var currentAdditional = additionalList.Where(x => x.Type == Payroll.Type.CurrentSallary);
                                var newAdditional = additionalList.Where(x => x.Type == Payroll.Type.NewSallary);
                                var currentDeduction = deductionList.Where(x => x.Type == Payroll.Type.CurrentSallary);
                                var newDeduction = deductionList.Where(x => x.Type == Payroll.Type.NewSallary);

                                //context.RotationSubcomponentAdditionals.RemoveRange(currentAdditional.ToArray());
                                //context.RotationSubcomponentDeductions.RemoveRange(currentDeduction.ToArray());

                                Parallel.ForEach(newAdditional, (item) =>
                                {
                                    //item.Type = Payroll.Type.CurrentSallary;

                                    var employeeAdditional = employeeAdditionalList.Where(x => x.AdditionalEmployeeBasicInfoId == item.AdditionalEmployeeBasicInfoId &&
                                                           x.AdditionalSalaryComponent == item.AdditionalSalaryComponent && x.AdditionalComponentId == item.AdditionalComponentId).FirstOrDefault();

                                    employeeAdditional.AdditionalAmount = item.AdditionalAmount.ToString();
                                    context.EmployeeSubcomponentAdditionals.Update(employeeAdditional);
                                    //context.RotationSubcomponentAdditionals.Update(item);
                                });

                                Parallel.ForEach(newDeduction, (item) =>
                                {
                                    //item.Type = Payroll.Type.CurrentSallary;

                                    var employeeDeduction = employeeDeductionList.Where(x => x.EmployeeBasicInfoId == item.EmployeeBasicInfoId &&
                                                            x.SalaryComponent == item.SalaryComponent && x.ComponentId == item.ComponentId).FirstOrDefault();

                                    employeeDeduction.Amount = item.DeductionAmount.ToString();
                                    context.EmployeeSubcomponentDeductions.Update(employeeDeduction);
                                    //context.RotationSubcomponentDeductions.Update(item);
                                });

                                Parallel.ForEach(rotation, (item) =>
                                {

                                    item.IsSynchronize = true;
                                    context.Rotations.Update(item);
                                });

                                await context.SaveChangesAsync();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception(ex.Message);
                            }

                        }
                    }
                }
            }
        }
    }
}
