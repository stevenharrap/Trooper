using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Testing.ShopModel.Config
{
    public class RecreateDbConfiguration : DbMigrationsConfiguration<ShopModelDbContext>
    {
        public RecreateDbConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }
    }
}
