using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Contexts
{
    public partial class EFContext : DbContext
    {
        public EFContext() : base()
        {

        }
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {

        }
    }
}
