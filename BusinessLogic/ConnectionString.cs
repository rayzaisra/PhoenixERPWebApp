using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using CodeMarvel.Infrastructure.Options;

namespace BusinessLogic
{
    public static class ConnectionString
    {
        public static void PhoenixConnectionStringSetup(string connectionString)
        {
            DataAccess.PhoenixERP.PhoenixERPRepo.ConnectionStringBase = connectionString;
        }
    }
}
