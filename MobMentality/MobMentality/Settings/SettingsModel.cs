using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobMentality.Settings
{
    class SettingsModel
    {
        public List<string> Mobbers { get; set; }
        public Int16 TurnMinutes { get; set; }
        public Int16 BreakMinutes { get; set; }
        public Int16 BreakTurns { get; set; }
    }
}
