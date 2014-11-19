using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;



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
        private bool _serverSelected = false;
        private GEState _serverState = null;
        private System.Net.Http.HttpClient client = null;

        private static GEServer _singleton = null;

        public bool IsServerSelected
        {
            get { return _serverSelected; }
        }

        public GEState ServerState
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
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.CookieContainer = new CookieContainer();

            client = new System.Net.Http.HttpClient(handler);
            client.BaseAddress = new Uri(url);
        }

        private void MakeAPICall(string paramStr, string_callback callback)
        {
            string fullURL = "api.php?" + paramStr;
            if (!String.IsNullOrEmpty(_token))
                fullURL += "&token=" + _token;

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

            try
            {
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
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
            }
            
        }

        public void Refresh(string_callback callback)
        {
            string queryString = "";
            queryString += "live=1";

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
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
        }


    }


}

