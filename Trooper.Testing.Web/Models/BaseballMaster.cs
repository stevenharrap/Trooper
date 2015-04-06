using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Trooper.Testing.Web.Models
{
    public class BaseballMasterMap : CsvClassMap<BaseballMaster>
    {
        public BaseballMasterMap()
        {
            Map(m => m.Debut).ConvertUsing<DateTime?>(row => {
                var value = row.GetField("debut");
                DateTime result;
                if (DateTime.TryParseExact(value, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    return result;
                }

                return null;
            });

            Map(m => m.FinalGame).ConvertUsing<DateTime?>(row =>
            {
                var value = row.GetField("finalGame");
                DateTime result;
                if (DateTime.TryParseExact(value, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    return result;
                }

                return null;
            });
        }
    }

    public class BaseballMaster
    {
        public string PlayerID { get; set; }

        public int? BirthYear { get; set; }

        public int? BirthMonth { get; set; }

        public int? BirthDay { get; set; }

        public string BirthCountry { get; set; }

        public string BirthState { get; set; }

        public string BirthCity { get; set; }

        public int? DeathYear { get; set; }

        public int? DeathMonth { get; set; }

        public int? DeathDay { get; set; }

        public string DeathCountry { get; set; }

        public string DeathState { get; set; }
                
        public string DeathCity { get; set; }
        
        public string NameFirst { get; set; }

        public string NameLast { get; set; }
        
        public string NameGiven { get; set; }

        public int? Weight { get; set; }

        public int? Height { get; set; }

        public string Bats { get; set; }

        public string Throws { get; set; }

        public DateTime? Debut { get; set; }

        public DateTime? FinalGame { get; set; }

        public string RetroID { get; set; }

        public string BbrefID { get; set; }
    }
}