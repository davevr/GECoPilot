﻿using System;

namespace GECoPilot
{
    public class GEPlanet
    {
        public string id { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string clan_id { get; set; }
        public string created { get; set; }
        public int last_update { get; set; }
        public string last_activity_update { get; set; }
        public string planet_type { get; set; }
        public string moon_id { get; set; }
        public string b_building { get; set; }
        public string b_building_id { get; set; }
        public string b_tech_started { get; set; }
        public string b_tech { get; set; }
        public string b_tech_id { get; set; }
        public int b_shipyard { get; set; }
        public string b_shipyard_id { get; set; }
        public string image { get; set; }
        public string diameter { get; set; }
        public string field_current { get; set; }
        public string field_max { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }
        public double metal { get; set; }
        public int metal_perhour { get; set; }
        public int metal_max { get; set; }
        public double crystal { get; set; }
        public int crystal_perhour { get; set; }
        public int crystal_max { get; set; }
        public double deuterium { get; set; }
        public int deuterium_perhour { get; set; }
        public int deuterium_max { get; set; }
        public int energy_used { get; set; }
        public int energy_max { get; set; }
        public string debries_crystal { get; set; }
        public string debries_metal { get; set; }
        public string metal_mine { get; set; }
        public string crystal_mine { get; set; }
        public string deuterium_synthesizer { get; set; }
        public string solar_plant { get; set; }
        public string fusion_reactor { get; set; }
        public string robotics_factory { get; set; }
        public string nanite_factory { get; set; }
        public string shipyard { get; set; }
        public string metal_storage { get; set; }
        public string crystal_storage { get; set; }
        public string deuterium_storage { get; set; }
        public string research_lab { get; set; }
        public string terraformer { get; set; }
        public string moon_base { get; set; }
        public string sensor_phalanx { get; set; }
        public string jump_gate { get; set; }
        public string missile_silo { get; set; }
        public string small_cargo { get; set; }
        public string large_cargo { get; set; }
        public string light_fighter { get; set; }
        public string heavy_fighter { get; set; }
        public string cruiser { get; set; }
        public string battleship { get; set; }
        public string colony_ship { get; set; }
        public string recycler { get; set; }
        public string espionage_probe { get; set; }
        public string bomber { get; set; }
        public string solar_satellite { get; set; }
        public string destroyer { get; set; }
        public string deathstar { get; set; }
        public string battlecruiser { get; set; }
        public string rocket_launcher { get; set; }
        public string light_laser { get; set; }
        public string heavy_laser { get; set; }
        public string gauss_cannon { get; set; }
        public string ion_cannon { get; set; }
        public string plasma_cannon { get; set; }
        public string small_shield_dome { get; set; }
        public string large_shield_dome { get; set; }
        public string anti_ballistic_missiles { get; set; }
        public string interplanetary_missiles { get; set; }
        public string metal_mine_percent { get; set; }
        public string crystal_mine_percent { get; set; }
        public string deuterium_synthesizer_percent { get; set; }
        public string solar_plant_percent { get; set; }
        public string fusion_reactor_percent { get; set; }
        public string solar_satellite_percent { get; set; }
        public string last_jump_time { get; set; }
        public int moved { get; set; }
        public string show_in_scoreboard { get; set; }
        public int BASE_STORAGE_SIZE { get; set; }
        public int METAL_BASIC_INCOME { get; set; }
        public int CRYSTAL_BASIC_INCOME { get; set; }
        public int DEUTERIUM_BASIC_INCOME { get; set; }
        public int ENERGY_BASIC_INCOME { get; set; }
        public GEPlanet moon { get; set; }
    }


    public class GEPlanetSummary
    {
        public string id { get; set; }
        public string name { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string planet_type { get; set; }
        public GEPlanetSummary moon {get; set;}
    }


}
