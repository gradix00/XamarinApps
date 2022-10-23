using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TestBDonline.Scripts
{
    public static class GeneralSettings
    {
        private static readonly string email = "email";
        private static readonly string pwd = "password";
        private static readonly string dataSave = "dataSave";
        private static readonly string defaultValue = string.Empty;
        
        public static string LastEmail
        {
            get { return CrossSettings.Current.GetValueOrDefault(email, defaultValue); }
            set { CrossSettings.Current.AddOrUpdateValue(email, value); }
        }

        public static string LastPassword
        {
            get {  return CrossSettings.Current.GetValueOrDefault(pwd, defaultValue); }
            set { CrossSettings.Current.AddOrUpdateValue(pwd, value); }
        }

        public static bool DataSave
        {
            get { return CrossSettings.Current.GetValueOrDefault(dataSave, false); }
            set { CrossSettings.Current.AddOrUpdateValue(dataSave, value); }
        }
    }
}
