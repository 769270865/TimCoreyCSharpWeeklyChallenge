using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Model
{
    public interface ITimeProvider
    {
        DateTime CurrrentDateTime { get;  }
        Time CurrentTime { get; }
    }
}
