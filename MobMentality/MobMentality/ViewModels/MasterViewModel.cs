using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using MobMentality.Annotations;
using MobMentality.Command;
using Newtonsoft.Json;

namespace MobMentality.ViewModels
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        #region Ctor

        public MasterViewModel()
        {
            InitializePeople();
            InitializeTimer();
        }

        #endregion

        #region People

        #region Initialize

        private void InitializePeople()
        {
            ActivePeople = new ObservableCollection<string>();
            InactivePeople = new ObservableCollection<string>();

            ActivePeople.CollectionChanged += ActivePeople_CollectionChanged;

            MovePersonCommand = new RelayCommand<List<string>>((e) => { PeopleValidator.MoveActivePerson(e[0], e[1], ActivePeople, InactivePeople); }); //TODO: This
            AddPersonCommand = new RelayCommand<string>((e) => { PeopleValidator.AddActivePerson(e, ActivePeople, InactivePeople); });
            SwitchPersonStateCommand = new RelayCommand<string>((e) => { PeopleValidator.SwitchPersonState(e, ActivePeople, InactivePeople); });
            RemovePersonCommand = new RelayCommand<string>((e) => { PeopleValidator.RemovePerson(e, ActivePeople, InactivePeople); });
            RotateToPersonCommand = new RelayCommand<string>((e) => { PeopleValidator.RotateToPerson(e, ActivePeople); });
            NextPersonCommand = new RelayCommand<object>((e) => { PeopleValidator.NextPerson(ActivePeople); });
            LastPersonCommand = new RelayCommand<object>((e) => { PeopleValidator.LastPerson(ActivePeople); });
            RandomizeActiveCommand = new RelayCommand<object>((e) => { PeopleValidator.RandomizeActive(ActivePeople); });
        }


        #endregion

        #region Prop

        private ObservableCollection<string> _activePeople = new ObservableCollection<string>();
        public ObservableCollection<string> ActivePeople
        {
            get => _activePeople;
            set
            {
                if (value is null) return;

                _activePeople = value;

                OnPropertyChanged(nameof(ActivePeople));
            }
        }

        private ObservableCollection<string> _inactivePeople = new ObservableCollection<string>();
        public ObservableCollection<string> InactivePeople
        {
            get => _inactivePeople;
            set
            {
                if (value is null) return;

                _inactivePeople = value;
                OnPropertyChanged(nameof(InactivePeople));
            }
        }

        private string _driver;
        public string Driver
        {
            get => _driver;
            set
            {
                if (value is null) return;

                _driver = value;

                OnPropertyChanged(nameof(Driver));
            }
        }

        private string _navigator;
        public string Navigator
        {
            get => _navigator;
            set
            {
                if (value is null) return;

                _navigator = value;

                OnPropertyChanged(nameof(Navigator));
            }
        }

        #endregion

        #region Command

        [JsonIgnore]
        public RelayCommand<List<string>> MovePersonCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<string> AddPersonCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<string> SwitchPersonStateCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<string> RemovePersonCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<string> RotateToPersonCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> NextPersonCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> LastPersonCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> RandomizeActiveCommand { get; set; }

        private void ActivePeople_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Driver = (ActivePeople.Count > 0) ? ActivePeople[0] : "";

            Navigator = (ActivePeople.Count > 1) ? ActivePeople[1] : "";
        }

        #endregion

        #endregion

        #region Timer

        #region Initialize

        private void InitializeTimer()
        {
            Timer.Elapsed += (s, e) => { TimerValidator.TimerElapsed(this); };

            IncrementCommand = new RelayCommand<string>((e) => { TimerValidator.Increment(true, e, this); });
            DecrementCommand = new RelayCommand<string>((e) => { TimerValidator.Increment(false, e, this); });
            StartTimerCommand = new RelayCommand<object>((e) => { TimerValidator.StartTimer(Timer); });
            PauseTimerCommand = new RelayCommand<object>((e) => { TimerValidator.PauseTimer(Timer); });
            ResetTimerCommand = new RelayCommand<object>((e) => { TimerValidator.ResetTimer(this); });
            ResetBreaksCommand = new RelayCommand<object>((e) => { TimerValidator.ResetBreaks(this); });
        }

        #endregion

        #region Prop

        public Timer Timer { get; set; } = new Timer(1000);

        private int _turnsTillBreak;
        public int TurnsTillBreak
        {
            get => _turnsTillBreak;
            set
            {
                _turnsTillBreak = value;
                OnPropertyChanged(nameof(TurnsTillBreak));
            }
        }

        private int _turnMinutes;
        public int TurnMinutes
        {
            get => _turnMinutes;
            set
            {
                _turnMinutes = value;
                OnPropertyChanged(nameof(TurnMinutes));
            }
        }

        private int _breakMinutes;
        public int BreakMinutes
        {
            get => _breakMinutes;
            set
            {
                _breakMinutes = value;
                OnPropertyChanged(nameof(BreakMinutes));
            }
        }

        private int _timeLeft;
        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                OnPropertyChanged(nameof(TimeLeft));
            }
        }

        private int _turnsLeft;
        public int TurnsLeft
        {
            get => _turnsLeft;
            set
            {
                _turnsLeft = value;
                OnPropertyChanged(nameof(TurnsLeft));
            }
        }

        public bool TimeForBreak { get; set; } = false;

        #endregion

        #region Command

        [JsonIgnore]
        public RelayCommand<string> IncrementCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<string> DecrementCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> StartTimerCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> PauseTimerCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> ResetTimerCommand { get; set; }
        [JsonIgnore]
        public RelayCommand<object> ResetBreaksCommand { get; set; }


        public event EventHandler TimerUp;
        public void RaiseTimerUp()
        {
            TimerUp?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #endregion

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
