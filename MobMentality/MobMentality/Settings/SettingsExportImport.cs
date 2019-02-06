using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using MobMentality.ViewModels;
using Newtonsoft.Json;

namespace MobMentality.Settings
{
    class SettingsExportImport
    {
        public static void Save(object model)
        {
            string jsonModel = JsonConvert.SerializeObject(model);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "json (*.json) | *.json | All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveDialog.FileName, jsonModel);
            }
        }
        public static void Load()
        {
            try
            {
                OpenFileDialog loadDialog = new OpenFileDialog();

                if (loadDialog.ShowDialog() == true)
                {
                    string path = loadDialog.FileName;

                    string content = File.ReadAllText(path);

                    Application.Current.MainWindow.DataContext = JsonConvert.DeserializeObject<MasterViewModel>(content);
                }

            }
            catch(Exception)
            {
                MessageBox.Show("File wasn't compatible");
            }
        }
    }
}
