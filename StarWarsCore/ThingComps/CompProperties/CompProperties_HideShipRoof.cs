using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCP.Core.ThingComps
{
    internal class CompProperties_HideShipRoof : CompProperties
    {
        public float layer;
        public CompProperties_HideShipRoof()
        {
            compClass = typeof(CompHideShipRoof);
        }
    }
}
