using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MobMentality.ViewModels
{
    public static class TimerValidator
    {

        public static void Increment(bool positive, string prop, MasterViewModel model)
        {
            int num = 0;
            if (positive)
            {
                num = 1;
            }
            else
            {
                num = -1;
            }

            switch (prop)
            {
                case nameof(model.TurnMinutes):
                    model.TurnMinutes += num;
                    break;
                case nameof(model.BreakMinutes):
                    model.BreakMinutes += num;
                    break;
                case nameof(model.TurnsTillBreak):
                    model.TurnsTillBreak += num;
                    break;
            }
        }

        public static void StartTimer(Timer timer)
        {
            timer.Start();
        }

        public static void PauseTimer(Timer timer)
        {
            timer.Stop();
        }

        public static void ResetTimer(MasterViewModel model)
        {
            model.Timer.Stop();

            int time = 0;
            if (model.TimeForBreak)
            {
                time = model.BreakMinutes * 60;

                ResetBreaks(model);
            }
            else
            {
                time = model.TurnMinutes * 60;
            }

            model.TimeLeft = time;
        }

        public static void ResetBreaks(MasterViewModel model)
        {
            model.TurnsLeft = model.TurnsTillBreak + 1;
            model.TimeForBreak = false;
        }

        public static string ConvertTime(int seconds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(seconds / 60);
            stringBuilder.Append(":");
            stringBuilder.Append((seconds % 60).ToString("D2"));

            return stringBuilder.ToString();
        }

        public static void TimerElapsed(MasterViewModel m)
        {
            // TODO: Inline timerUp and negate
            bool timerUp = (m.TimeLeft - 1) < 0;

            if (!timerUp)
            {
                --m.TimeLeft;
            }
            else
            {
                PauseTimer(m.Timer);

                if ((m.TurnsLeft - 1) <= 0)
                {
                    m.TimeForBreak = true;
                }
                else
                {
                    --m.TurnsLeft;

                    m.TimeForBreak = false;
                }

                m.NextPersonCommand.Execute(null);

                m.RaiseTimerUp();
            }
        }
    }
}
