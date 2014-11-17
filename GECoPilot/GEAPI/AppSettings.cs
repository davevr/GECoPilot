using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using Newtonsoft.Json;

namespace GECoPilot
{
    class AppSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Universe { get; set; }

        private static AppSettings _instance = null;



        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }

        public static async Task SaveSettingsAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("GESettings", CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting);
            string jsonString = JsonConvert.SerializeObject(Instance);

            await file.WriteAllTextAsync(jsonString);

        }

        public static async Task LoadSettingsAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("GESettings", CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);
            string jsonString = await file.ReadAllTextAsync();
            try
            {
                _instance = JsonConvert.DeserializeObject<AppSettings>(jsonString);
            }
            catch (Exception)
            {
                _instance = new AppSettings();
            }
        }

        public static void SaveSettings()
        {
            Task waitTask = Task.Run( async () => { await SaveSettingsAsync(); });
            waitTask.Wait();

        }

        public static void LoadSettings()
        {
            Task waitTask = Task.Run( async () => { await LoadSettingsAsync(); });
            waitTask.Wait();
        }

        public void Clear()
        {
            Username = "";
            Password = "";
            Universe = "";
        }
    }


}
