using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GECoPilot
{
    public class GEState
    {
        public GEUser user { get; set; }
        public JObject planets { get; set; }
        public JObject planets_sorted { get; set; }
        public List<GEPlanet> planetList { get; set; }
        public List<GEPlanetSummary> planetSummaryList { get; set; }
        public List<GEFleet> fleetList { get; set; }
        public JToken fleet { get; set; }
        public string global_notification { get; set; }
        public string global_notification_type { get; set; }

        public string SummaryText
        {
            get
            {
                string summary = "Your planets have a total of ";
                summary += TotalMetal.ToString("#,#") + " metal, ";
                summary += TotalCrystal.ToString("#,#") + " crystal, and ";
                summary += TotalDeuterium.ToString("#,#") + " deuterium.  \n";
                summary += "They are generating ";
                summary += HourlyMetal.ToString("#,#") + " metal, ";
                summary += HourlyCrystal.ToString("#,#") + " crystal, and ";
                summary += HourlyDeuterium.ToString("#,#") + " deuterium per hour.";
                return summary;
            }
        }

        public int TotalMetal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.metal;
                }

                return (int)total;
            }
        }

        public int TotalCrystal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.crystal;
                }

                return (int)total;
            }
        }

        public int TotalDeuterium
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.deuterium;
                }

                return (int)total;
            }
        }

        public int HourlyMetal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.metal_perhour;
                }

                return (int)total;
            }
        }

        public int HourlyCrystal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.crystal_perhour;
                }

                return (int)total;
            }
        }

        public int HourlyDeuterium
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.deuterium_perhour;
                }

                return (int)total;
            }
        }
    }
}

