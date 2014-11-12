using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;


namespace GECoPilot
{
    public delegate void string_callback(String theResult);
    public delegate void bool_callback(bool theResult);


    public class ServerResponse
    {
        public List<object> authlogin { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
    }

    public class ServerInfo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string start { get; set; }
        public string restart { get; set; }
        public string username { get; set; }
        public int open { get; set; }
    }

    public class ServerInfoSet
    {
        public ServerInfo srv1 { get; set; }
        public ServerInfo srv2 { get; set; }
        public ServerInfo srv3 { get; set; }
    }

    public class ServersList
    {
        public string default_server { get; set; }
        public ServerInfoSet list { get; set; }
    }

    public class Authservers
    {
        public ServersList servers_list { get; set; }
    }

    public class ServerListRequest
    {
        public Authservers authservers { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
    }


    public class GEServer
    {
        private  string _token = null;
        private  bool _isSignedIn = false;
        private  string url = "http://ge.seazonegames.com/";
        private CookieContainer cookie = null;
        private bool _serverSelected = false;
        private State _serverState = null;

        private static GEServer _singleton = null;

        public bool IsServerSelected
        {
            get { return _serverSelected; }
        }

        public State ServerState
        {
            get { return _serverState; }
        }


        public static GEServer Instance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new GEServer();
                    _singleton.InitClient();
                }
                return _singleton;
            }
        }

        private void InitClient()
        {
            cookie = new CookieContainer();
        }

        private void MakeAPICall(string paramStr, string_callback callback)
        {
            string fullURL = "api.php?" + paramStr;
            if (!String.IsNullOrEmpty(_token))
                fullURL += "&token=" + _token;

            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var client = new System.Net.Http.HttpClient(handler);
            client.BaseAddress = new Uri(url);
            client.PostAsync(fullURL, null).ContinueWith((theTask) =>
                {
                    HttpResponseMessage resp = theTask.Result;
                    if (callback != null)
                    {
                        resp.Content.ReadAsStringAsync().ContinueWith((strTask) => 
                            {
                                string theResult = strTask.Result;
                                callback(theResult);
                            });
                    }
                });
        }

        public void Login(string username, string password, string_callback callback)
        {
            string queryString = "";
            queryString += "object=auth";
            queryString += "&action=login";
            queryString += "&email=" + System.Net.WebUtility.UrlEncode(username);
            queryString += "&password=" + password;


            MakeAPICall(queryString, (content) =>
                {
                    ServerResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerResponse>(content);
                    if (!String.IsNullOrEmpty(response.token))
                    {
                        _isSignedIn = true;
                        _token = response.token;
                        callback("");
                    }
                    else
                    {
                        callback("error");
                    }
                });
        }

        public bool IsSignedIn
        {
            get { return _isSignedIn; }
        }

        public delegate void ServerInfoSet_callback(ServerInfoSet theResult);

        public void GetServerList(ServerInfoSet_callback callback)
        {
            string queryString = "";
            queryString += "object=auth";
            queryString += "&action=servers";

            MakeAPICall(queryString, (content) =>
                {
                    ServerListRequest response = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerListRequest>(content);
                    if (callback != null)
                        callback(response.authservers.servers_list.list);
                });
        }

        //public delegate void ServerInfoSet_callback(ServerInfoSet theResult);

        public void SetServer(string serverName, string_callback callback)
        {
            string queryString = "";
            queryString += "object=auth";
            queryString += "&action=chooseserver";
            queryString += "&server=" + serverName;

            MakeAPICall(queryString, (content) =>
                {
                    try 
                    {
                        GEStatusObject response = Newtonsoft.Json.JsonConvert.DeserializeObject<GEStatusObject>(content);
                        response.Normalize();
                        _serverState = response.state;
                        if (callback != null)
                            callback("ok");
                    }
                    catch (Exception exp)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
        }


    }

    public class GEUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public string federated_id { get; set; }
        public string planet_id { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string current_planet { get; set; }
        public string rank { get; set; }
        public string points { get; set; }
        public string fights { get; set; }
        public string fights_win { get; set; }
        public string fights_loot { get; set; }
        public string fights_lost { get; set; }
        public string avatar { get; set; }
        public string ally_id { get; set; }
        public string ally_name { get; set; }
        public string ally_tag { get; set; }
        public string ally_rank { get; set; }
        public string ally_stat { get; set; }
        public string ally_joined { get; set; }
        public string ally_apply_id { get; set; }
        public string ally_apply_name { get; set; }
        public string ally_apply_tag { get; set; }
        public string ally_apply_text { get; set; }
        public string description { get; set; }
        public string has_mail { get; set; }
        public string send_spys { get; set; }
        public string chat_color { get; set; }
        public string vacation { get; set; }
        public string b_tech_planet { get; set; }
        public string espionage_tech { get; set; }
        public string computer_tech { get; set; }
        public string weapons_tech { get; set; }
        public string shielding_tech { get; set; }
        public string armour_tech { get; set; }
        public string energy_tech { get; set; }
        public string hyperspace_tech { get; set; }
        public string combustion_drive_tech { get; set; }
        public string impulse_drive_tech { get; set; }
        public string hyperspace_drive_tech { get; set; }
        public string laser_tech { get; set; }
        public string ion_tech { get; set; }
        public string plasma_tech { get; set; }
        public string intergalactic_research_tech { get; set; }
        public string astrophysics_tech { get; set; }
        public string graviton_tech { get; set; }
        public string phalanx_planet_id { get; set; }
        public string off_geologist { get; set; }
        public string off_scientist { get; set; }
        public string off_engineer { get; set; }
        public string off_general { get; set; }
        public string off_admiral { get; set; }
        public string off_energy { get; set; }
        public string off_storer { get; set; }
        public string off_spy { get; set; }
        public string off_exspy { get; set; }
        public string starter { get; set; }
        public string ban_account { get; set; }
        public string ban_account_reason { get; set; }
        public string ban_forum { get; set; }
        public string ban_forum_reason { get; set; }
        public string ban_chat { get; set; }
        public string ban_chat_reason { get; set; }
        public string show_in_stat { get; set; }
        public string wheel_time { get; set; }
        public string email_registration { get; set; }
        public string email_current { get; set; }
        public string level { get; set; }
        public int MAX_BUILDING_QUEUE_SIZE { get; set; }
        public int FLEET_TO_DEBRIES { get; set; }
        public int resource_multiplier { get; set; }
        public int ENERGY_MULTIPLIER { get; set; }
        public int game_speed { get; set; }
        public int fleet_speed { get; set; }
        public int attack_enabled { get; set; }
    }



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
    }

    public class GEFleet
    {
        public string fleet_id { get; set; }
        public string fleet_owner { get; set; }
        public string fleet_mission { get; set; }
        public string fleet_amount { get; set; }
        public string fleet_array { get; set; }
        public string fleet_start_time { get; set; }
        public string fleet_start_galaxy { get; set; }
        public string fleet_start_system { get; set; }
        public string fleet_start_planet { get; set; }
        public string fleet_start_type { get; set; }
        public string fleet_start_username { get; set; }
        public string fleet_start_planet_name { get; set; }
        public string fleet_start_planet_id { get; set; }
        public string fleet_end_time { get; set; }
        public string fleet_end_stay { get; set; }
        public string fleet_end_galaxy { get; set; }
        public string fleet_end_system { get; set; }
        public string fleet_end_planet { get; set; }
        public string fleet_end_type { get; set; }
        public string fleet_end_username { get; set; }
        public string fleet_end_planet_name { get; set; }
        public string fleet_end_planet_id { get; set; }
        public string fleet_target_obj { get; set; }
        public string fleet_resource_metal { get; set; }
        public string fleet_resource_crystal { get; set; }
        public string fleet_resource_deuterium { get; set; }
        public string fleet_resource_darkmatter { get; set; }
        public string fleet_target_owner { get; set; }
        public string fleet_group { get; set; }
        public string fleet_mess { get; set; }
        public string start_time { get; set; }
        public int fleet_time_total { get; set; }
        public int fleet_time_percent { get; set; }
        public int fleet_time_left { get; set; }
        public string fleet_start_time_real { get; set; }
        public string fleet_end_time_real { get; set; }
        public string start_time_real { get; set; }
        public FleetTypes fleet_types { get; set; }
    }

    public class FleetTypes
    {
        public string large_cargo { get; set; }
        public string small_cargo { get; set; }
    }


    public class GEPlanetSummary
    {
        public string id { get; set; }
        public string name { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string planet_type { get; set; }
    }


    public class State
    {
        public GEUser user { get; set; }
        public JObject planets { get; set; }
        public JObject planets_sorted { get; set; }
        public List<GEPlanet> planetList { get; set; }
        public List<GEPlanetSummary> planetSummaryList { get; set; }
        public List<GEFleet> fleetList { get; set; }
        public JObject fleet { get; set; }
        public string global_notification { get; set; }
        public string global_notification_type { get; set; }
    }

    public class GEStatusObject
    {
        public List<object> authchooseserver { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
        public State state { get; set; }
        public int online_users { get; set; }
        public string total_users { get; set; }

        public void Normalize()
        {
            // to do:  convert the JObject members into actual objects.
            state.planetList = new List<GEPlanet>();
            foreach (JToken curObj in state.planets.Children())
            {
                JToken subObj = curObj.First;
                GEPlanet newPlanet = subObj.ToObject<GEPlanet>();

                state.planetList.Add(newPlanet);
            }

            state.planetSummaryList = new List<GEPlanetSummary>();
            foreach (JToken curObj in state.planets_sorted.Children())
            {
                JToken subObj = curObj.First;
                GEPlanetSummary newPlanet = subObj.ToObject<GEPlanetSummary>();

                state.planetSummaryList.Add(newPlanet);
            }

            state.fleetList = new List<GEFleet>();
            if (!(state.fleet == null))
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

