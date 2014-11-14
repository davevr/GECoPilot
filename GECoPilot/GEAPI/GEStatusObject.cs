using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GECoPilot
{
    public class GEStatusObject
    {
        public List<object> authchooseserver { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
        public GEState state { get; set; }
        public int online_users { get; set; }
        public string total_users { get; set; }

        public void Normalize()
        {
            state.planetList = new List<GEPlanet>();
            List<GEPlanet> moonList = new List<GEPlanet>();
            foreach (JToken curObj in state.planets.Children())
            {
                JToken subObj = curObj.First;
                GEPlanet newPlanet = subObj.ToObject<GEPlanet>();

                if (newPlanet.planet_type == "3")
                    moonList.Add(newPlanet);
                else
                    state.planetList.Add(newPlanet);
            }

            if (moonList.Count > 0)
            {
                foreach (GEPlanet curMoon in moonList)
                {
                    state.planetList.Find(planet => planet.moon_id == curMoon.id).moon = curMoon;
                }
            }

            state.planetSummaryList = new List<GEPlanetSummary>();
            List<GEPlanetSummary> moonSummaryList = new List<GEPlanetSummary>();

            foreach (JToken curObj in state.planets_sorted.Children())
            {
                JToken subObj = curObj.First;
                GEPlanetSummary newPlanet = subObj.ToObject<GEPlanetSummary>();

                if (newPlanet.planet_type == "3")
                    moonSummaryList.Add(newPlanet);
                else
                    state.planetSummaryList.Add(newPlanet);
            }

            if (moonSummaryList.Count > 0)
            {
                foreach (GEPlanetSummary curMoon in moonSummaryList)
                {
                    string planetId = state.planetList.Find(planet => planet.moon_id == curMoon.id).id;
                    state.planetSummaryList.Find(planet => planet.id == planetId).moon = curMoon;
                }
            }

            state.fleetList = new List<GEFleet>();
            if (!(state.fleet == null))
            {
                if (state.fleet is JObject)
                {
                    foreach (JToken curObj in state.fleet.Children())
                    {
                        JToken subObj = curObj.First;
                        GEFleet newFleet = subObj.ToObject<GEFleet>();

                        state.fleetList.Add(newFleet);
                    }
                }
            }


        }
    }
}

