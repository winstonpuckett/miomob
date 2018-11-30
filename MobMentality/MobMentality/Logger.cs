using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobMentality
{
    class Logger
    {
        FileStream loggerStream = new FileStream(DateTime.Now.ToString("yyyy.MM.dd"), FileMode.Create)
    }
}
