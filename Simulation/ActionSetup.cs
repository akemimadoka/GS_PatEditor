﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    static class ActionSetup
    {
        private static void RunEffects(this IEnumerable<Pat.Effect> effectList, Simulation.Actor actor)
        {
            foreach (var e in effectList)
            {
                e.Run(actor);
            }
        }

        public static void SetupActorForAction(Actor actor, Pat.Action action)
        {
            action.InitEffects.RunEffects(actor);
            actor.UpdateLabel = action.UpdateEffects.RunEffects;
            actor.EndKeyFrameLabel = action.KeyFrameEffects.Select(list => (ActorLabel)list.RunEffects).ToArray();
        }
    }
}