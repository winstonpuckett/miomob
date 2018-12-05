using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MobMentality
{
    class SettingsExportImport
    {
        public static void Save(SettingsModel model)
        {
            string jsonModel = JsonConvert.SerializeObject(model);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "json (*.json) | *.json | All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveDialog.FileName, jsonModel);
            }
        }
        public static void Load(ResourceDictionary myAppDictionary, MobPeople mobPeople)
        {
            try
            {
                OpenFileDialog loadDialog = new OpenFileDialog();

                if (loadDialog.ShowDialog() == true)
                {
                    string path = loadDialog.FileName;

                    string content = File.ReadAllText(path);

                    SettingsModel model = JsonConvert.DeserializeObject<SettingsModel>(content);

                    myAppDictionary["TurnMinutes"] = model.TurnMinutes;
                    myAppDictionary["BreakMinutes"] = model.BreakMinutes;
                    myAppDictionary["TurnsTillBreak"] = model.BreakTurns;
                    mobPeople.ActivePeople.Clear();
                    foreach (string name in model.Mobbers)
                    {
                        mobPeople.AddActivePerson(name);
                    }
                }

            }
            catch(Exception e)
            {
                MessageBox.Show("File wasn't compatible");
            }
        }
    }
}
