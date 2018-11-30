using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MobMentality
{
    class MobTimer
    {
        #region Properties
        #region Session Settings

        private ResourceDictionary myAppDictionary;
        #endregion

        #region Counters
        private Timer myTimer;
        public bool IsRunning { get; set; }
        #endregion
        #endregion

        public MobTimer()
        {
            myAppDictionary = Application.Current.Resources;

            myTimer = new Timer(1000);

            ResetTimer(false);

            myTimer.Elapsed += MyTimer_Elapsed;
        }

        #region Working with timer
        public void StartTimer()
        {
            myTimer.Start();
            myAppDictionary["TimeCounterFriendly"] = ConvertTime((Int16)(myAppDictionary["TimeCounter"]));
            IsRunning = true;
        }

        public void PauseTimer()
        {
            myTimer.Stop();
            IsRunning = false;
        }

        public void ResetTimer(bool timeForBreak)
        {
            myTimer.Stop();

            Int16 time = 0;
            if (timeForBreak)
            {
                time = (Int16)((Int16)myAppDictionary["BreakMinutes"] * 60);
                myAppDictionary["BreakCounter"] = myAppDictionary["TurnsTillBreak"];
            }
            else
            {
                time = (Int16)((Int16)myAppDictionary["TurnMinutes"] * 60);
            }

            myAppDictionary["TimeCounter"] = time;
        }
        #endregion

        public static string ConvertTime(Int16 seconds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(seconds / 60);
            stringBuilder.Append(":");
            stringBuilder.Append((seconds % 60).ToString("D2"));

            return stringBuilder.ToString();
        }

        #region Events
        public class TimerUpEventArgs : EventArgs
        {
            public TimerUpEventArgs(bool timeForBreak)
            {
                TimeForBreak = timeForBreak;
            }

            public readonly bool TimeForBreak;
        }

        public event EventHandler<TimerUpEventArgs> TimerUpEvent;

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeCounter = (Int16)myAppDictionary["TimeCounter"];
            var breakCounter = (Int16)myAppDictionary["BreakCounter"];

            if ((timeCounter - 1) < 0)
            {
                bool timeForBreak;
                PauseTimer();

                if ((breakCounter - 1) <= 0)
                {
                    timeForBreak = true;
                }
                else
                {
                    myAppDictionary["BreakCounter"] = --breakCounter;


                    timeForBreak = false;
                }
                //ResetTimer(false);
                RaiseTimerUp(new TimerUpEventArgs(timeForBreak));
            }
            else
            {
                myAppDictionary["TimeCounter"] = --timeCounter;
                myAppDictionary["TimeCounterFriendly"] = ConvertTime(timeCounter);
            }
        }

        protected virtual void RaiseTimerUp(TimerUpEventArgs e)
        {
            TimerUpEvent?.Invoke(this, e);
        }
        #endregion
    }
}
