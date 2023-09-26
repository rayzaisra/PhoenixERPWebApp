using Phoenix.Shared.Core.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Shared.Core.Entities
{
    public class BaseEntity<PK> 
    {
        //[Key]
        public PK Id { get; set; }

    }
}
