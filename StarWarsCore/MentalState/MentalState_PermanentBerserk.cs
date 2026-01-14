using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;

namespace SWCP.Core.MentalState
{
    internal class MentalState_PermanentBerserk : MentalState_Berserk
    {
        public override bool ForceHostileTo(Thing t)
        {
            Pawn p;
            if (t.def.race?.Humanlike != true || t.def.race.IsMechanoid || t.def.race.IsAnomalyEntity)
            {
                return true;

            }
            p = t as Pawn;
            if (p?.genes?.Xenotype.HasModExtension<TurnBerserk_ModExtension>() == true)
            {
                return false;
            }
            return base.ForceHostileTo(t);

        }
    }
    public class TurnBerserk_ModExtension : DefModExtension
    {
    }

    public class PermanentBerserk_ModExtension : DefModExtension
    {
    }
}
