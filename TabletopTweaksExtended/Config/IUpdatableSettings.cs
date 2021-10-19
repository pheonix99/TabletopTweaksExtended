using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaksExtended.Config
{
    public interface IUpdatableSettings
    {
        void OverrideSettings(IUpdatableSettings userSettings);
    }
}
