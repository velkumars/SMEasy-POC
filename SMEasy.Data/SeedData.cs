using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Transactions;
 
using Xen.Entity;
using Xen.Helpers;
using System.Linq;
using System.Data.Entity;

namespace SMEasy.Data
{
    internal sealed class SeedData : CreateDatabaseIfNotExists<SMEasyEntities>
    {
        public SeedData()
        {
            //AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SMEasyEntities context)
        {
            using (System.Transactions.TransactionScope scope = new TransactionScope())
            {
 
            }            
        }

       
    }
}
 