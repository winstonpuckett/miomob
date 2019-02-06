using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MobMentality.ViewModels
{
    public static class PeopleValidator
    {
        public static void MoveActivePerson(string name, string targetName, ObservableCollection<string> activePeople, ObservableCollection<string> inactivePeople)
        {
            if (activePeople.Contains(name))
            {
                if (activePeople.Contains(targetName))
                {
                    activePeople.Move(activePeople.IndexOf(name), activePeople.IndexOf(targetName));
                }
                else if (inactivePeople.Contains(targetName))
                {
                    activePeople.Remove(name);
                    inactivePeople.Insert(inactivePeople.IndexOf(targetName), name);
                }
            }
            else if (inactivePeople.Contains(name))
            {
                if (inactivePeople.Contains(targetName))
                {
                    inactivePeople.Move(inactivePeople.IndexOf(name), inactivePeople.IndexOf(targetName));
                }
                else if (activePeople.Contains(targetName))
                {
                    inactivePeople.Remove(name);
                    activePeople.Insert(activePeople.IndexOf(targetName), name);
                }
            }
        }

        public static bool AddActivePerson(string name, ObservableCollection<string> activePeople, ObservableCollection<string> inactivePeople)
        {
            if (
                !activePeople.Contains(name) &&
                !inactivePeople.Contains(name) &&
                !string.IsNullOrWhiteSpace(name)
                )
            {
                activePeople.Add(name);
                return true;
            }

            return false;
        }

        public static void SwitchPersonState(string name, ObservableCollection<string> activePeople, ObservableCollection<string> inactivePeople)
        {
            if (activePeople.Contains(name))
            {
                inactivePeople.Add(name);
                activePeople.Remove(name);
            }
            else if (inactivePeople.Contains(name))
            {
                activePeople.Add(name);
                inactivePeople.Remove(name);
            }
        }

        public static void RemovePerson(string name, ObservableCollection<string> activePeople, ObservableCollection<string> inactivePeople)
        {
            if (activePeople.Contains(name))
            {
                SwitchPersonState(name, activePeople, inactivePeople);
            }
            else
            {
                inactivePeople.Remove(name);
            }
        }

        public static void RotateToPerson(string name, ObservableCollection<string> activePeople)
        {
            string startingName = activePeople[0];

            do
            {
                NextPerson(activePeople);
            }
            while (startingName != activePeople[0] && activePeople[0] != name);
        }

        public static void NextPerson(ObservableCollection<string> activePeople)
        {
            if (activePeople.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    activePeople.Move(0, activePeople.Count - 1);
                });
            }
        }

        public static void LastPerson(ObservableCollection<string> activePeople)
        {
            if (activePeople.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    activePeople.Move(activePeople.Count - 1, 0);
                });
            }
        }

        public static void RandomizeActive(ObservableCollection<string> activePeople)
        {
            // Taken from stack overflow
            // https://stackoverflow.com/questions/273313/randomize-a-listt
            Random rng = new Random();
            int n = activePeople.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = activePeople[k];
                activePeople[k] = activePeople[n];
                activePeople[n] = value;
            }
        }
    }
}
