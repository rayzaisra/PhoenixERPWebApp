using System;

namespace DataAccess
{
  
    public interface IBaseRecord
    {
        //string _id { get; set; }

        string id { get; set; }


        string created_by { get; set; }

        DateTime? created_on { get; set; }

        string modified_by { get; set; }

        DateTime? modified_on { get; set; }

        string approved_by { get; set; }

        DateTime? approved_on { get; set; }

        string deleted_by { get; set; }

        DateTime? deleted_on { get; set; }

        bool? is_active { get; set; }

        bool? is_locked { get; set; }

        bool? is_default { get; set; }

        bool? is_deleted { get; set; }

        string owner_id { get; set; }

    }
}
