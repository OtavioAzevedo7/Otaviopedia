using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otaviopedia.Models.COVID
{
    public class CasesByCountry
    {
        public string Country { get; set; }
        public string Flag { get; set; }
        public int Cases { get; set; }
        public int TotalDeaths { get; set; }
        public int TodayCases{ get; set; }
        public int TodayDeaths { get; set; }
        public int Population { get; set; }
        public string Continent { get; set; }
        public int OneCasePerPeople { get; set; }
        public int OneDeathPerPeople { get; set; }
        public string SourceAPI { get; set; }
    }
}
