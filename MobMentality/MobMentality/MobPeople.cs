using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace MobMentality
{
    class MobPeople
    {
        private ResourceDictionary myAppDictionary;

        #region Properties
        public ObservableCollection<string> ActivePeople { get; set; }
        public ObservableCollection<string> InactivePeople { get; set; }
        public List<string> DriverNavigator => GetDriverNavigator();
        #endregion Properties

        public MobPeople()
        {
            myAppDictionary = Application.Current.Resources;

            ActivePeople = new ObservableCollection<string>();

            InactivePeople = new ObservableCollection<string>();
        }

        #region Add or Remove from Lists
        public void MoveActivePerson(string name, string targetName)
        {
            if (ActivePeople.Contains(name))
            {
                if (ActivePeople.Contains(targetName))
                {
                    ActivePeople.Move(ActivePeople.IndexOf(name), ActivePeople.IndexOf(targetName));
                }
                else if (InactivePeople.Contains(targetName))
                {
                    ActivePeople.Remove(name);
                    InactivePeople.Insert(InactivePeople.IndexOf(targetName), name);
                }
            }
            else if (InactivePeople.Contains(name))
            {
                if (InactivePeople.Contains(targetName))
                {
                    InactivePeople.Move(InactivePeople.IndexOf(name), InactivePeople.IndexOf(targetName));
                }
                else if (ActivePeople.Contains(targetName))
                {
                    InactivePeople.Remove(name);
                    ActivePeople.Insert(ActivePeople.IndexOf(targetName), name);
                }
            }
        }

        public bool AddActivePerson(string name)
        {
            if (
                !ActivePeople.Contains(name) &&
                !InactivePeople.Contains(name) &&
                !String.IsNullOrWhiteSpace(name)
                )
            {
                ActivePeople.Add(name);
                return true;
            }

            return false;
        }

        public void SwitchPersonState(string name)
        {
            if (ActivePeople.Contains(name))
            {
                InactivePeople.Add(name);
                ActivePeople.Remove(name);
            }
            else if (InactivePeople.Contains(name))
            {
                ActivePeople.Add(name);
                InactivePeople.Remove(name);
            }
        }

        public void RemovePerson(string name)
        {
            if (ActivePeople.Contains(name))
            {
                ActivePeople.Remove(name);
            }
            else
            {
                InactivePeople.Remove(name);
            }
        }

        public void RotateToPerson(string name)
        {
            string startingName = ActivePeople[0];
            do
            {
                NextPerson();
            }
            while (startingName != ActivePeople[0] && ActivePeople[0] != name);
        }

        public void NextPerson()
        {
            if (ActivePeople.Count > 0)
            {
                ActivePeople.Move(0, ActivePeople.Count - 1);
            }
        }
        public void LastPerson()
        {
            if (ActivePeople.Count > 0)
            {
                ActivePeople.Move(ActivePeople.Count - 1, 0);
            }
        }

        private List<string> GetDriverNavigator()
        {
            List<string> list = new List<string> { "", "" };

            if (ActivePeople.Count > 0)
            {
                list[0] = ActivePeople[0];
                if (ActivePeople.Count > 1)
                {
                    list[1] = ActivePeople[1];
                }
            }

            return list;
        }

        public void RandomizeActive()
        {
            // Taken from stack overflow
            // https://stackoverflow.com/questions/273313/randomize-a-listt
            Random rng = new Random();
            int n = ActivePeople.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = ActivePeople[k];
                ActivePeople[k] = ActivePeople[n];
                ActivePeople[n] = value;
            }
        }
        #endregion
    }
}
