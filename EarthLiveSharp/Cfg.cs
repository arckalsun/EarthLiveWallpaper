﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace EarthLiveSharp
{
    public static class Cfg
    {
        public static string version;
        public static string image_folder;
        public static int interval;
        public static bool autostart;
        public static bool showcoastline;
        public static int size;
        public static int zoom;
        public static string image_source;
        public static string cloud_name;
        public static int source_selection;
        public static void Load()
        {
            try
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                AppSettingsSection app = config.AppSettings;
                version = app.Settings["version"].Value;
                image_folder = app.Settings["image_folder"].Value;
                interval = Convert.ToInt32(app.Settings["interval"].Value);
                autostart = Convert.ToBoolean(app.Settings["autostart"].Value);
                showcoastline = Convert.ToBoolean(app.Settings["showcoastline"].Value);
                size = Convert.ToInt32(app.Settings["size"].Value);
                zoom = Convert.ToInt32(app.Settings["zoom"].Value);
                image_source = app.Settings["image_source"].Value;
                cloud_name = app.Settings["cloud_name"].Value;
                source_selection = Convert.ToInt16(app.Settings["source_selection"].Value);
                return;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                MessageBox.Show("Configure error!");
                throw (e);
            }
        }
        public static void Save()
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            app.Settings["image_folder"].Value = image_folder;
            app.Settings["interval"].Value = interval.ToString();
            app.Settings["autostart"].Value = autostart.ToString();
            app.Settings["showcoastline"].Value = autostart.ToString();
            app.Settings["size"].Value = size.ToString();
            app.Settings["cloud_name"].Value = cloud_name;
            app.Settings["source_selection"].Value = source_selection.ToString();
            app.Settings["image_source"].Value = image_source;
            app.Settings["zoom"].Value = zoom.ToString();
            config.Save();
            return;
        }
    }
}
