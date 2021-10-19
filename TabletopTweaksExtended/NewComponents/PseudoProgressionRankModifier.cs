using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaksExtended.NewComponents
{
    public abstract class PseudoProgressionRankModifier : UnitFactComponentDelegate
    {

        public BlueprintFeatureReference Key;

        public abstract double GetModifier(UnitDescriptor d);


    }
}
