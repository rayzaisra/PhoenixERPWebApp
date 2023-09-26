using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IMappingFolderOneDriveService : IDataService<TrMappingFolderOneDrive>
    {
        Task<int> DeleteAllMappingByItemId(string itemId);
        Task<int> DeleteByItemIdAndRefId(string itemId, string refId);
        Task<List<VmMapFodAccess>> GetByItemId(string itemid);
        Task<List<VmFodUserList>> GetUserList(string itemid, string key);
        Task<List<VmFodGroupList>> GetGroupList(string itemid, string key);
        Task<List<TrMappingFolderOneDrive>> Get(string userId, string groupId);
        Task<int> InsertUploadFilePe(PeFileDto data);
        Task<int> DeleteFilePe(PeFileDto data);
        Task<int> InsertUploadFileMyDam(MyDamFileDto data);
        Task<VmFodUserList> GetUserUploadFIle(string itemid, string key);
        Task<bool> CreateFolderToDB(string key, string itemid, string parent_entity, string userid);
        Task<List<VmFileFodUserList>> GetDataShareMyDrive(string shareid);
        Task<List<TrainingSchedule>> GetTrainingSchedule();
    }

}
