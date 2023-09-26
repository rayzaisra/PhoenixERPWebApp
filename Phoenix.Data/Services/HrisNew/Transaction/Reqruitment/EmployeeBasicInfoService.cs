using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Services.Um;

namespace Phoenix.Data
{
    public interface IEmployeeBasicInfoService : IDataServiceHris<EmployeeBasicInfo>
    {
        Task<EmployeeBasicInfo> GetInfoByLogin();
        //Task<List<Rotation>> GetApprovalList();
        //Task<List<RotationSubcomponentAdditional>> GetAdditionalList(string employeeId, string rotationId);
    }

    public class EmployeeBasicInfoService : IEmployeeBasicInfoService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        private readonly IManageUserAppService userApp;
        readonly FileService uploadFile;

        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Rotation
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeBasicInfoService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, IManageUserAppService userApp, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.userApp = userApp;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(EmployeeBasicInfo entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(EmployeeBasicInfo entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(EmployeeBasicInfo entity, bool isEdit)
        {
            int result = 0;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var file1 = await uploadFile.Upload(entity.PhotoFile, null, "HRIS");
                    if (!string.IsNullOrEmpty(file1))
                    {
                        entity.Photo = file1;
                    }

                    if (string.IsNullOrEmpty(entity.MasterWorkingTimeId))
                    {
                        var workingtimedefault = await context.MasterWorkingTimes.AsNoTracking().Where(x => x.IsDeleted != true && x.IsDefault == true).FirstOrDefaultAsync();
                        if (workingtimedefault != null)
                        {
                            entity.MasterWorkingTimeId = workingtimedefault.Id;
                        }
                    }

                    if (isEdit)
                    {
                        entity.ModifiedBy = context.GetEmployeeId();
                        entity.ModifiedOn = DateTime.Now;
                        entity.IsDeleted = false;
                        entity.NameEmployee = string.Format("{0} {1}", entity.FirstName, entity.LastName);

                        if (entity.Families != null)
                        {
                            if (entity.Families.Count > 0)
                            {
                                foreach (var data in entity.Families)
                                {
                                    var IsExistDataFamily = context.Familys.Any(row => row.Id == data.Id);
                                    if (IsExistDataFamily)
                                    {
                                        var dataFamily = context.Familys.Where(row => row.Id == data.Id).FirstOrDefault();
                                        dataFamily.ReligionId = data.fReligion;
                                        dataFamily.Address = data.fAddress;
                                        dataFamily.Birthday = data.Birthday;
                                        dataFamily.EmployeeBasicInfoId = entity.Id;
                                        dataFamily.EducationLevelId = data.fEducationLevelId;
                                        dataFamily.FamilyStatusId = data.FamilyStatusId;
                                        dataFamily.Gender = data.Gender;
                                        dataFamily.Description = data.Description;
                                        dataFamily.Name = data.fName;
                                        dataFamily.InstitutionId = data.fInstitutionId;
                                        dataFamily.MarriedStatusId = data.fMarriedStatusId;
                                        dataFamily.OtherNo = data.fOtherNo;
                                        dataFamily.PlaceBirth = data.PlaceBirth;
                                        dataFamily.PlaceBirth = data.PlaceBirth;
                                        dataFamily.Id = data.Id;
                                        context.Familys.Update(dataFamily);
                                    }
                                    else
                                    {
                                        var dataFamily = context.Familys.Where(row => row.Id != data.Id).FirstOrDefault();
                                        dataFamily.ReligionId = data.fReligion;
                                        dataFamily.Address = data.fAddress;
                                        dataFamily.Birthday = data.Birthday;
                                        dataFamily.EmployeeBasicInfoId = entity.Id;
                                        dataFamily.EducationLevelId = data.fEducationLevelId;
                                        dataFamily.FamilyStatusId = data.FamilyStatusId;
                                        dataFamily.Gender = data.Gender;
                                        dataFamily.Description = data.Description;
                                        dataFamily.Name = data.fName;
                                        dataFamily.InstitutionId = data.fInstitutionId;
                                        dataFamily.MarriedStatusId = data.fMarriedStatusId;
                                        dataFamily.OtherNo = data.fOtherNo;
                                        dataFamily.PlaceBirth = data.PlaceBirth;
                                        dataFamily.PlaceBirth = data.PlaceBirth;
                                        dataFamily.Id = Guid.NewGuid().ToString();
                                        context.Familys.Add(dataFamily);
                                    }
                                }
                            }
                        }

                        if (entity.EmployeeEmergencyContacts != null)
                        {
                            if (entity.EmployeeEmergencyContacts.Count > 0)
                            {
                                foreach (var data in entity.EmployeeEmergencyContacts)
                                {
                                    var IsExistDataEmployeeEmergencyContacts = context.EmployeeEmergencyContacts.Any(row => row.Id == data.Id);
                                    if (IsExistDataEmployeeEmergencyContacts)
                                    {
                                        var dataEmployeeEmergencyContacts = context.EmployeeEmergencyContacts.Where(row => row.Id == data.Id).FirstOrDefault();
                                        dataEmployeeEmergencyContacts.Address = data.EmergencyAddress;
                                        dataEmployeeEmergencyContacts.EmployeeBasicInfoId = entity.Id;
                                        dataEmployeeEmergencyContacts.FamilyStatusId = data.EmergencyFamilyStatusId;
                                        dataEmployeeEmergencyContacts.FullName = data.EmergencyFullName;
                                        dataEmployeeEmergencyContacts.OtherNo = data.EmergencyOtherNo;
                                        dataEmployeeEmergencyContacts.PhoneNo = data.EmergencyPhoneNo;
                                        dataEmployeeEmergencyContacts.IsDeleted = false;
                                        dataEmployeeEmergencyContacts.Id = data.Id;
                                        context.EmployeeEmergencyContacts.Update(dataEmployeeEmergencyContacts);
                                    }
                                    else
                                    {
                                        var dataEmployeeEmergencyContacts = context.EmployeeEmergencyContacts.Where(row => row.Id != data.Id).FirstOrDefault();
                                        dataEmployeeEmergencyContacts.Address = data.EmergencyAddress;
                                        dataEmployeeEmergencyContacts.EmployeeBasicInfoId = entity.Id;
                                        dataEmployeeEmergencyContacts.FamilyStatusId = data.EmergencyFamilyStatusId;
                                        dataEmployeeEmergencyContacts.FullName = data.EmergencyFullName;
                                        dataEmployeeEmergencyContacts.OtherNo = data.EmergencyOtherNo;
                                        dataEmployeeEmergencyContacts.PhoneNo = data.EmergencyPhoneNo;
                                        dataEmployeeEmergencyContacts.IsDeleted = false;
                                        dataEmployeeEmergencyContacts.Id = Guid.NewGuid().ToString();
                                        context.EmployeeEmergencyContacts.Add(dataEmployeeEmergencyContacts);
                                    }
                                }
                            }
                        }

                        if (entity.Educations != null)
                        {
                            if (entity.Educations.Count > 0)
                            {
                                foreach (var data in entity.Educations)
                                {
                                    var IsExistDataEducation = context.Educations.Any(row => row.Id == data.Id);
                                    if (IsExistDataEducation)
                                    {
                                        var dataEducation = context.Educations.Where(row => row.Id == data.Id).FirstOrDefault();
                                        dataEducation.Address = data.EAddress;
                                        dataEducation.EducationLevelId = data.EEducationLevelId;
                                        dataEducation.EmployeeBasicInfoId = entity.Id;
                                        dataEducation.StartEducation = data.EStartEducation;
                                        dataEducation.EndEducation = data.EEndEducation;
                                        dataEducation.InstitutionId = data.EInstitutionId;
                                        dataEducation.Majors = data.EMajors;
                                        dataEducation.Qualification = data.EQualification;
                                        dataEducation.OtherInstitution = data.OtherInstitution;
                                        dataEducation.Id = data.Id;
                                        dataEducation.IsDeleted = false;
                                        context.Educations.Update(dataEducation);
                                    }
                                    else
                                    {
                                        var dataEducation = context.Educations.Where(row => row.Id != data.Id).FirstOrDefault();
                                        dataEducation.Address = data.EAddress;
                                        dataEducation.EducationLevelId = data.EEducationLevelId;
                                        dataEducation.EmployeeBasicInfoId = entity.Id;
                                        dataEducation.StartEducation = data.EStartEducation;
                                        dataEducation.EndEducation = data.EEndEducation;
                                        dataEducation.InstitutionId = data.EInstitutionId;
                                        dataEducation.Majors = data.EMajors;
                                        dataEducation.Qualification = data.EQualification;
                                        dataEducation.OtherInstitution = data.OtherInstitution;
                                        dataEducation.IsDeleted = false;
                                        dataEducation.Id = Guid.NewGuid().ToString();
                                        context.Educations.Add(dataEducation);
                                    }

                                }
                            }
                        }

                        if (entity.EmployeeTrainings != null)
                        {
                            if (entity.EmployeeTrainings.Count > 0)
                            {
                                foreach (var data in entity.EmployeeTrainings)
                                {
                                    var IsExistDataEmployeeTrainings = context.EmployeeTrainings.Any(row => row.Id == data.Id);
                                    if (IsExistDataEmployeeTrainings)
                                    {
                                        var dataEmployeeTrainings = context.EmployeeTrainings.Where(row => row.Id == data.Id).FirstOrDefault();
                                        dataEmployeeTrainings.Address = data.EmployeeTrainingAddress;
                                        dataEmployeeTrainings.Description = data.EmployeeTrainingDescription;
                                        dataEmployeeTrainings.EmployeeBasicInfoId = entity.Id;
                                        dataEmployeeTrainings.InstitutionId = data.EmployeeTrainingInstitutionId;
                                        dataEmployeeTrainings.StartTraining = data.StartTraining;
                                        dataEmployeeTrainings.EndTraining = data.EndTraining;
                                        dataEmployeeTrainings.NameTraining = data.NameTraining;
                                        dataEmployeeTrainings.NoCertification = data.NoCertification;
                                        dataEmployeeTrainings.ExpiredCertificate = data.ExpiredCertificate;
                                        dataEmployeeTrainings.IsDeleted = false;
                                        dataEmployeeTrainings.Id = data.Id;
                                        context.EmployeeTrainings.Update(dataEmployeeTrainings);
                                    }
                                    else
                                    {
                                        var dataEmployeeTrainings = context.EmployeeTrainings.Where(row => row.Id == data.Id).FirstOrDefault();
                                        dataEmployeeTrainings.Address = data.EmployeeTrainingAddress;
                                        dataEmployeeTrainings.Description = data.EmployeeTrainingDescription;
                                        dataEmployeeTrainings.EmployeeBasicInfoId = entity.Id;
                                        dataEmployeeTrainings.InstitutionId = data.EmployeeTrainingInstitutionId;
                                        dataEmployeeTrainings.StartTraining = data.StartTraining;
                                        dataEmployeeTrainings.EndTraining = data.EndTraining;
                                        dataEmployeeTrainings.NameTraining = data.NameTraining;
                                        dataEmployeeTrainings.NoCertification = data.NoCertification;
                                        dataEmployeeTrainings.ExpiredCertificate = data.ExpiredCertificate;
                                        dataEmployeeTrainings.IsDeleted = false;
                                        dataEmployeeTrainings.Id = Guid.NewGuid().ToString();
                                        context.EmployeeTrainings.Add(dataEmployeeTrainings);
                                    }
                                }
                            }
                        }


                        if (entity.EmployeeExperiences != null)
                        {
                            if (entity.EmployeeExperiences.Count > 0)
                            {
                                foreach (var data in entity.EmployeeExperiences)
                                {
                                    var IsExistDataEmployeeExperiences = context.EmployeeExperiences.Any(row => row.Id == data.Id);
                                    if (IsExistDataEmployeeExperiences)
                                    {
                                        var dataEmployeeExperiences = context.EmployeeExperiences.Where(row => row.Id == data.Id).FirstOrDefault();
                                        dataEmployeeExperiences.Address = data.EmplyeeExperienceAddress;
                                        dataEmployeeExperiences.EmployeeBasicInfoId =entity.Id;
                                        dataEmployeeExperiences.CompanyName = data.CompanyName;
                                        dataEmployeeExperiences.StartDate = data.EmplyeeExperienceStartDate;
                                        dataEmployeeExperiences.EndDate = data.EmplyeeExperienceEndDate;
                                        dataEmployeeExperiences.Phone = data.EmplyeeExperiencePhone;
                                        dataEmployeeExperiences.Position = data.Position;
                                        dataEmployeeExperiences.Remark = data.Remark;
                                        dataEmployeeExperiences.Id = data.Id;
                                        dataEmployeeExperiences.IsDeleted = false;
                                        context.EmployeeExperiences.Update(dataEmployeeExperiences);
                                    }
                                    else
                                    {
                                        var dataEmployeeExperiences = context.EmployeeExperiences.Where(row => row.Id != data.Id).FirstOrDefault();
                                        dataEmployeeExperiences.Address = data.EmplyeeExperienceAddress;
                                        dataEmployeeExperiences.EmployeeBasicInfoId = entity.Id;
                                        dataEmployeeExperiences.CompanyName = data.CompanyName;
                                        dataEmployeeExperiences.StartDate = data.EmplyeeExperienceStartDate;
                                        dataEmployeeExperiences.EndDate = data.EmplyeeExperienceEndDate;
                                        dataEmployeeExperiences.Phone = data.EmplyeeExperiencePhone;
                                        dataEmployeeExperiences.Position = data.Position;
                                        dataEmployeeExperiences.Remark = data.Remark;
                                        dataEmployeeExperiences.Id = Guid.NewGuid().ToString();
                                        dataEmployeeExperiences.IsDeleted = false;
                                        context.EmployeeExperiences.Add(dataEmployeeExperiences);
                                    }

                                }
                            }
                        }

                        if (entity.EmployeeChecklists != null)
                        {
                            if (entity.EmployeeChecklists.Count > 0)
                            {
                                var IsEmployeeChecklists = context.EmployeeChecklists.Where(row => row.EmployeeBasicInfoId == entity.Id).FirstOrDefault();
                                if (IsEmployeeChecklists != null)
                                {
                                    var listCheckList = context.EmployeeChecklists.Where(row => row.EmployeeBasicInfoId == entity.Id).ToList();
                                    listCheckList.ForEach(x => { x.IsDeleted = true; x.DeletedBy = context.GetEmployeeId(); x.DeletedOn = DateTime.Now; });
                                    context.EmployeeChecklists.UpdateRange(listCheckList);
                                }

                                foreach (var data in entity.EmployeeChecklists)
                                {
                                    string[] arrayEmployeeChecklistCategoryId = data.EchkEmployeeChecklistCategoryId.Split(',');
                                    if (arrayEmployeeChecklistCategoryId.Length > 1)
                                    {
                                        foreach (var item in arrayEmployeeChecklistCategoryId)
                                        {
                                            EmployeeChecklist dt = new EmployeeChecklist();
                                            dt.Checklistitem = data.EchkChecklistitem;
                                            dt.Description = data.EchkDescription;
                                            dt.EmployeeBasicInfoId = entity.Id;
                                            dt.EmployeeChecklistCategoryId = item;
                                            dt.Whentoalert = data.EchkWhentoalert;
                                            dt.IsDeleted = false;
                                            dt.Id = Guid.NewGuid().ToString();
                                            context.EmployeeChecklists.Add(dt);
                                        }
                                    }
                                    else
                                    {
                                        var IsExistDataEmployeeChecklists = context.EmployeeChecklists.Any(row => row.Id == data.Id);

                                        if (IsExistDataEmployeeChecklists)
                                        {

                                            foreach (var item in arrayEmployeeChecklistCategoryId)
                                            {
                                                var dataEmployeeChecklists = context.EmployeeChecklists.Where(row => row.Id == data.Id).FirstOrDefault();
                                                dataEmployeeChecklists.Checklistitem = data.EchkChecklistitem;
                                                dataEmployeeChecklists.Description = data.EchkDescription;
                                                dataEmployeeChecklists.EmployeeBasicInfoId = entity.Id;
                                                dataEmployeeChecklists.EmployeeChecklistCategoryId = item;
                                                dataEmployeeChecklists.Whentoalert = data.EchkWhentoalert;
                                                dataEmployeeChecklists.IsDeleted = false;
                                                dataEmployeeChecklists.Id = data.Id;
                                                context.EmployeeChecklists.Update(dataEmployeeChecklists);
                                            }

                                        }
                                        else
                                        {
                                            foreach (var item in arrayEmployeeChecklistCategoryId)
                                            {
                                                var dataEmployeeChecklists = context.EmployeeChecklists.Where(row => row.Id != data.Id).FirstOrDefault();
                                                dataEmployeeChecklists.Checklistitem = data.EchkChecklistitem;
                                                dataEmployeeChecklists.Description = data.EchkDescription;
                                                dataEmployeeChecklists.EmployeeBasicInfoId = entity.Id;
                                                dataEmployeeChecklists.EmployeeChecklistCategoryId = item;
                                                dataEmployeeChecklists.Whentoalert = data.EchkWhentoalert;
                                                dataEmployeeChecklists.IsDeleted = false;
                                                dataEmployeeChecklists.Id = Guid.NewGuid().ToString();
                                                context.EmployeeChecklists.Add(dataEmployeeChecklists);
                                            }

                                        }
                                    }

                                   
                                }
                            }
                        }

                        #region Comment
                        //var familitExist = context.Familys.Where(x => x.EmployeeBasicInfoId == entity.Id).ToList();
                        //context.Familys.RemoveRange(familitExist.ToArray());

                        //var emergencyContacttExist = context.EmployeeEmergencyContacts.Where(x => x.EmployeeBasicInfoId == entity.Id).ToList();
                        //context.EmployeeEmergencyContacts.RemoveRange(emergencyContacttExist.ToArray());

                        //var educationExist = context.Educations.Where(x => x.EmployeeBasicInfoId == entity.Id).ToList();
                        //context.Educations.RemoveRange(educationExist.ToArray());

                        //var empTrainingExist = context.EmployeeTrainings.Where(x => x.EmployeeBasicInfoId == entity.Id).ToList();
                        //context.EmployeeTrainings.RemoveRange(empTrainingExist.ToArray());

                        //var empExperienceExist = context.EmployeeExperiences.Where(x => x.EmployeeBasicInfoId == entity.Id).ToList();
                        //context.EmployeeExperiences.RemoveRange(empExperienceExist.ToArray());

                        //var empChecklist = context.EmployeeChecklists.Where(x => x.EmployeeBasicInfoId == entity.Id).ToList();
                        //context.EmployeeChecklists.RemoveRange(empChecklist.ToArray());
                        #endregion

                        context.SaveChanges();

                        TmUserApp uApp = await userApp.GetByEmployeeBasicInfoId(entity.Id);
                        if (uApp != null)
                        {
                            if (string.Equals(uApp.Email, entity.Email))
                            {
                                uApp.Email = entity.Email;
                                await userApp.EditAsync(uApp);
                            }
                        }

                        context.EmployeeBasicInfos.Update(entity);
                        result = await context.SaveChangesAsync();
                    }
                    else
                    {
                        entity.Id = DateTime.Now.Ticks.ToString();
                        entity.CreatedBy = context.GetEmployeeId();
                        entity.CreatedOn = DateTime.Now;
                        entity.ModifiedBy = context.GetEmployeeId();
                        entity.ModifiedOn = DateTime.Now;
                        entity.IsDeleted = false;
                        entity.NameEmployee = string.Format("{0} {1}", entity.FirstName, entity.LastName);
                        await context.EmployeeBasicInfos.AddAsync(entity);

                        if (entity.Families != null)
                        {
                            if (entity.Families.Count > 0)
                            {
                                var families = entity.Families.Select(x => new Family
                                {
                                    Address = x.fAddress,
                                    Birthday = x.Birthday,
                                    EmployeeBasicInfoId = entity.Id,
                                    EducationLevelId = x.fEducationLevelId,
                                    FamilyStatusId = x.FamilyStatusId,
                                    Gender = x.fGender,
                                    Description = x.Description,
                                    Name = x.fName,
                                    InstitutionId = x.fInstitutionId,
                                    MarriedStatusId = x.fMarriedStatusId,
                                    OtherNo = x.fOtherNo,
                                    PhoneNo = x.fPhoneNo,
                                    PlaceBirth = x.PlaceBirth,
                                    ReligionId = x.fReligionId,
                                    Id = Guid.NewGuid().ToString(),
                                    IsDeleted = false
                                }).ToList();

                                context.Familys.AddRange(families.ToArray());
                            }
                        }

                        if (entity.EmployeeEmergencyContacts != null)
                        {
                            if (entity.EmployeeEmergencyContacts.Count > 0)
                            {
                                var emergencyContact = entity.EmployeeEmergencyContacts.Select(x => new EmployeeEmergencyContact
                                {
                                    Address = x.EmergencyAddress,
                                    EmployeeBasicInfoId = entity.Id,
                                    Id = Guid.NewGuid().ToString(),
                                    FamilyStatusId = x.EmergencyFamilyStatusId,
                                    FullName = x.EmergencyFullName,
                                    OtherNo = x.EmergencyOtherNo,
                                    PhoneNo = x.EmergencyPhoneNo,
                                    IsDeleted = false
                                }).ToList();

                                context.EmployeeEmergencyContacts.AddRange(emergencyContact.ToArray());
                            }
                        }

                        if (entity.Educations != null)
                        {
                            if (entity.Educations.Count > 0)
                            {
                                var education = entity.Educations.Select(x => new Education
                                {
                                    Address = x.EAddress,
                                    EducationLevelId = x.EEducationLevelId,
                                    EmployeeBasicInfoId = entity.Id,
                                    Id = Guid.NewGuid().ToString(),
                                    EndEducation = x.EEndEducation,
                                    StartEducation = x.EStartEducation,
                                    InstitutionId = x.EInstitutionId,
                                    Majors = x.EMajors,
                                    Qualification = x.EQualification,
                                    OtherInstitution = x.OtherInstitution,
                                    IsDeleted = false
                                }).ToList();

                                context.Educations.AddRange(education.ToArray());
                            }
                        }

                        if (entity.EmployeeTrainings != null)
                        {
                            if (entity.EmployeeTrainings.Count > 0)
                            {
                                var course = entity.EmployeeTrainings.Select(x => new EmployeeTraining
                                {
                                    Address = x.EmployeeTrainingAddress,
                                    Description = x.EmployeeTrainingDescription,
                                    EmployeeBasicInfoId = entity.Id,
                                    InstitutionId = x.EmployeeTrainingInstitutionId,
                                    StartTraining = x.StartTraining,
                                    EndTraining = x.EndTraining,
                                    NameTraining = x.NameTraining,
                                    NoCertification = x.NoCertification,
                                    ExpiredCertificate = x.ExpiredCertificate,
                                    IsDeleted = false,
                                    Id = Guid.NewGuid().ToString()
                                }).ToList();

                                context.EmployeeTrainings.AddRange(course.ToArray());
                            }
                        }

                        if (entity.EmployeeExperiences != null)
                        {
                            if (entity.EmployeeExperiences.Count > 0)
                            {
                                var empExperiences = entity.EmployeeExperiences.Select(x => new EmployeeExperience
                                {
                                    Address = x.EmplyeeExperienceAddress,
                                    EmployeeBasicInfoId = entity.Id,
                                    IsDeleted = false,
                                    CompanyName = x.CompanyName,
                                    Id = Guid.NewGuid().ToString(),
                                    StartDate = x.EmplyeeExperienceStartDate,
                                    EndDate = x.EmplyeeExperienceEndDate,
                                    Phone = x.EmplyeeExperiencePhone,
                                    Position = x.Position,
                                    Remark = x.Remark
                                }).ToList();

                                context.EmployeeExperiences.AddRange(empExperiences.ToArray());
                            }
                        }

                        if (entity.EmployeeChecklists != null)
                        {
                            if (entity.EmployeeChecklists.Count > 0)
                            {
                                foreach (var item in entity.EmployeeChecklists)
                                {
                                    string[] arrayEmployeeChecklistCategoryId = item.EchkEmployeeChecklistCategoryId.Split(',');
                                    foreach (var data in arrayEmployeeChecklistCategoryId)
                                    {
                                        var empChecklist = entity.EmployeeChecklists.Select(x => new EmployeeChecklist
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            IsDeleted = false,
                                            EmployeeBasicInfoId = entity.Id,
                                            Checklistitem = x.EchkChecklistitem,
                                            Description = x.EchkDescription,
                                            EmployeeChecklistCategoryId = data,
                                            Whentoalert = x.EchkWhentoalert
                                        }).ToList();
                                        context.EmployeeChecklists.AddRange(empChecklist.ToArray());
                                    }
                                }
                               

                               
                            }
                        }
                        //await context.SaveChangesAsync();
                    }

                    result = await context.SaveChangesAsync();

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> ApproveAsync(EmployeeBasicInfo entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    //var save = await globalFcApproval.UnsafeProcessApproval(entity.Id, entity.Status, entity.RemarkRejected, entity);
                    //var approval = context.TrTemplateApprovals.Where(x => x.RefId == entity.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    //if (approval != null)
                    //{
                    //    if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                    //    {
                    //    }
                    //}

                    //save = await context.SaveChangesAsync();
                    transaction.Commit();
                    //return save;
                    return 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> DeleteSoftAsync(EmployeeBasicInfo entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<EmployeeBasicInfo>> Get()
        {
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            var employeeId = context.GetEmployeeId();

            var result = await (from b in context.EmployeeBasicInfos
                                where b.IsDeleted != true
                                select new EmployeeBasicInfo
                                {
                                    Id = b.Id,
                                    NameEmployee = b.NameEmployee,
                                    JobTitle = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.TitleName).FirstOrDefault(),
                                    JobGrade = b.BusinessUnitJobLevelId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == b.BusinessUnitJobLevelId
                                                                           select s.GradeName).FirstOrDefault(),
                                    BusinessUnitName = b.BusinessUnitJobLevelId == null ? "" :
                                                                        (from r in context.BusinessUnitJobLevels
                                                                         join s in context.BusinessUnits on r.BusinessUnitDivisionId equals s.Id
                                                                         join t in context.BusinessUnitTypes on s.BusinessUnitTypeId equals t.Id
                                                                         where t.BusinessUnitLevel == BusinessUnitLevelCode.Division && r.Id == b.BusinessUnitJobLevelId
                                                                         select s.UnitName).FirstOrDefault(),
                                    EmployeeStatus = b.EmployementStatusId == null ? "" : (from r in context.EmployementStatuss
                                                                                           where r.Id == b.EmployementStatusId
                                                                                           select r.EmploymentStatusName).FirstOrDefault(),
                                    EmployeeActiveStatus = (b.IsTerminated == null || b.IsTerminated == false) ? "Active" : "Not Active"
                                }).OrderBy(x => x.NameEmployee).ToListAsync();


            return result;
        }

        public async Task<EmployeeBasicInfo> Get(string Id)
        {
            var result = await (from a in context.EmployeeBasicInfos
                                where a.IsDeleted != true && a.Id == Id
                                select new EmployeeBasicInfo
                                {
                                    Id = a.Id,
                                    EmployeeCode = a.EmployeeCode,
                                    NameEmployee = a.NameEmployee,
                                    KitasNumber = a.KitasNumber,
                                    KitasExpiry = a.KitasExpiry,
                                    ReligionId = a.ReligionId,
                                    PassportNumber = a.PassportNumber,
                                    PassportExpiry = a.PassportExpiry,
                                    NpwpNumber = a.NpwpNumber,
                                    NpwpExpiry = a.NpwpExpiry,
                                    Blood = a.Blood,
                                    IdentityCard = a.IdentityCard,
                                    KkNo = a.KkNo,
                                    AddressIdentityCard = a.AddressIdentityCard,
                                    Photo = a.Photo,
                                    HomeAddress = a.HomeAddress,
                                    JoinDate = a.JoinDate,
                                    BpjsHealthy = a.BpjsHealthy,
                                    PayrollMethods = a.PayrollMethods,
                                    CentralResourceId = a.CentralResourceId,
                                    StatusEmployeeId = a.StatusEmployeeId,
                                    BusinessUnitId = a.BusinessUnitId,
                                    BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                    EmployementStatusId = a.EmployementStatusId,
                                    CityId = a.CityId,
                                    AccountNo = a.AccountNo,
                                    BpjsTk = a.BpjsTk,
                                    MarriedStatusId = a.MarriedStatusId,
                                    LastEducation = a.LastEducation,
                                    Probation = a.Probation,
                                    Approval = a.Approval,
                                    ProcessSalary = a.ProcessSalary,
                                    JobGradeId = a.JobGradeId,
                                    TypeTax = a.TypeTax,
                                    WarningDate = a.WarningDate,
                                    WorkingStatus = a.WorkingStatus,
                                    DateTermination = a.DateTermination,
                                    TerminationReason = a.TerminationReason,
                                    FlagDiageo = a.FlagDiageo,
                                    PersonalFiles = a.PersonalFiles,
                                    FirstName = a.FirstName,
                                    LastName = a.LastName,
                                    NickName = a.NickName,
                                    Gender = a.Gender,
                                    Email = a.Email,
                                    BirthPlace = a.BirthPlace,
                                    DateBirth = a.DateBirth,
                                    MobileNumber = a.MobileNumber,
                                    Portfolio = a.Portfolio,
                                    PaySlip = a.PaySlip,
                                    LocationId = a.LocationId,
                                    JobTitleId = a.JobTitleId,
                                    BankId = a.BankId,
                                    BankBranch = a.BankBranch,
                                    BankAddress = a.BankAddress,
                                    AccountName = a.AccountName,
                                    BankCity = a.BankCity,
                                    IdentityAddress = a.IdentityAddress,
                                    PostCode = a.PostCode,
                                    NationalityId = a.NationalityId,
                                    PhoneNumber = a.PhoneNumber,
                                    LegalEntityId = a.LegalEntityId,
                                    PtkpSettingId = a.PtkpSettingId,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    ReportsTo = a.ReportsTo,
                                    PositionId = a.PositionId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    BusinessUnitJobLevelId = a.BusinessUnitJobLevelId,
                                    EmployementStatusEndDate = a.EmployementStatusEndDate,
                                    BusinessUnitName = a.BusinessUnitName,
                                    JobTitle = a.JobTitleId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == a.JobTitleId
                                                                           select s.TitleName).FirstOrDefault(),
                                    JobGrade = a.JobGradeId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == a.JobGradeId
                                                                           select s.GradeName).FirstOrDefault(),
                                    EmployeeStatus = context.EmployementStatuss.Where(x => x.Id == a.EmployeeStatus).Select(x => x.EmploymentStatusName).FirstOrDefault(),
                                    IsTerminated = a.IsTerminated
                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                result.LastEducationLabel = await context.EducationLevels.Where(x => x.Id == result.LastEducation).Select(x => x.EducationName).FirstOrDefaultAsync();
                result.MarriageLabel = await context.MarriedStatuss.Where(x => x.Id == result.MarriedStatusId).Select(x => x.MarriedName).FirstOrDefaultAsync();
                result.NationalityLabel = await context.Nationalitys.Where(x => x.Id == result.NationalityId).Select(x => x.NationalityName).FirstOrDefaultAsync();
                result.ReligionLabel = await context.Religions.Where(x => x.Id == result.ReligionId).Select(x => x.ReligionName).FirstOrDefaultAsync();
                result.LocationLabel = await context.Locations.Where(x => x.Id == result.LocationId).Select(x => x.LocationName).FirstOrDefaultAsync();
                result.LegalEntityLabel = await context.LegalEntity.Where(x => x.Id == result.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                result.GradeLabel = await context.JobGrades.Where(x => x.Id == result.JobGradeId).Select(x => x.GradeName).FirstOrDefaultAsync();
                result.JobTitleLabel = await context.JobTitles.Where(x => x.Id == result.JobTitleId).Select(x => x.TitleName).FirstOrDefaultAsync();
                var rpt = await context.BusinessUnitJobLevels.Where(x => x.Id == result.ReportsTo).Select(x => x.JobTitleId).FirstOrDefaultAsync();
                result.ReportToLabel = await context.JobTitles.Where(x => x.Id == rpt).Select(x => x.TitleName).FirstOrDefaultAsync();
                result.EmploymentStatusLabel = await context.EmployementStatuss.Where(x => x.Id == result.EmployementStatusId).Select(x => x.EmploymentStatusName).FirstOrDefaultAsync();
            }

            //if (result == null)
            //{
            //	//where !(a.IsDeleted ?? false) && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
            //}
            return await GetFile(result);
        }

        private async Task<EmployeeBasicInfo> GetFile(EmployeeBasicInfo data)
        {
            if (data.Photo != null)
            {
                var a = await context.Filemasters.Where(x => x.Id == data.Photo).FirstOrDefaultAsync();
                if (a != null) data.PhotoFileName = a.Name;
            }

            return data;
        }

        public async Task<EmployeeBasicInfo> GetInfoByLogin()
        {
            var employeeId = context.GetEmployeeId();
            var result = await (from a in context.EmployeeBasicInfos
                                where a.IsDeleted != true && a.Id == employeeId
                                select new EmployeeBasicInfo
                                {
                                    Id = a.Id,
                                    EmployeeCode = a.EmployeeCode,
                                    NameEmployee = a.NameEmployee,
                                    KitasNumber = a.KitasNumber,
                                    KitasExpiry = a.KitasExpiry,
                                    ReligionId = a.ReligionId,
                                    PassportNumber = a.PassportNumber,
                                    PassportExpiry = a.PassportExpiry,
                                    NpwpNumber = a.NpwpNumber,
                                    NpwpExpiry = a.NpwpExpiry,
                                    Blood = a.Blood,
                                    IdentityCard = a.IdentityCard,
                                    KkNo = a.KkNo,
                                    AddressIdentityCard = a.AddressIdentityCard,
                                    Photo = a.Photo,
                                    HomeAddress = a.HomeAddress,
                                    JoinDate = a.JoinDate,
                                    BpjsHealthy = a.BpjsHealthy,
                                    PayrollMethods = a.PayrollMethods,
                                    CentralResourceId = a.CentralResourceId,
                                    StatusEmployeeId = a.StatusEmployeeId,
                                    BusinessUnitId = a.BusinessUnitId,
                                    BusinessUnitDepartementId = a.BusinessUnitDepartementId,
                                    EmployementStatusId = a.EmployementStatusId,
                                    CityId = a.CityId,
                                    AccountNo = a.AccountNo,
                                    BpjsTk = a.BpjsTk,
                                    MarriedStatusId = a.MarriedStatusId,
                                    LastEducation = a.LastEducation,
                                    Probation = a.Probation,
                                    Approval = a.Approval,
                                    ProcessSalary = a.ProcessSalary,
                                    JobGradeId = a.JobGradeId,
                                    TypeTax = a.TypeTax,
                                    WarningDate = a.WarningDate,
                                    WorkingStatus = a.WorkingStatus,
                                    DateTermination = a.DateTermination,
                                    TerminationReason = a.TerminationReason,
                                    FlagDiageo = a.FlagDiageo,
                                    PersonalFiles = a.PersonalFiles,
                                    FirstName = a.FirstName,
                                    LastName = a.LastName,
                                    NickName = a.NickName,
                                    Gender = a.Gender,
                                    Email = a.Email,
                                    BirthPlace = a.BirthPlace,
                                    DateBirth = a.DateBirth,
                                    MobileNumber = a.MobileNumber,
                                    Portfolio = a.Portfolio,
                                    PaySlip = a.PaySlip,
                                    LocationId = a.LocationId,
                                    JobTitleId = a.JobTitleId,
                                    BankId = a.BankId,
                                    BankBranch = a.BankBranch,
                                    BankAddress = a.BankAddress,
                                    AccountName = a.AccountName,
                                    BankCity = a.BankCity,
                                    IdentityAddress = a.IdentityAddress,
                                    PostCode = a.PostCode,
                                    NationalityId = a.NationalityId,
                                    PhoneNumber = a.PhoneNumber,
                                    LegalEntityId = a.LegalEntityId,
                                    PtkpSettingId = a.PtkpSettingId,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    ReportsTo = a.ReportsTo,
                                    PositionId = a.PositionId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    BusinessUnitJobLevelId = a.BusinessUnitJobLevelId,
                                    EmployementStatusEndDate = a.EmployementStatusEndDate,
                                    BusinessUnitName = a.BusinessUnitName,
                                    JobTitle = a.JobTitleId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                           where r.Id == a.JobTitleId
                                                                           select s.TitleName).FirstOrDefault(),
                                    JobGrade = a.JobGradeId == null ? "" :
                                                                          (from r in context.BusinessUnitJobLevels
                                                                           join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                           where r.Id == a.JobGradeId
                                                                           select s.GradeName).FirstOrDefault(),
                                    EmployeeStatus = context.EmployementStatuss.Where(x => x.Id == a.EmployeeStatus).Select(x => x.EmploymentStatusName).FirstOrDefault()
                                }).FirstOrDefaultAsync();

            return await GetFile(result);
        }
    }
}