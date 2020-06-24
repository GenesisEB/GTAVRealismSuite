// RealismTest.Knockouts
using Rage;
using Rage.Native;
using RealismTest;

namespace RealismTest
{
    internal class Knockouts
    {
        internal static void ProcessKnockouts(Ped PlayerPed)
        {
            if (PlayerPed.CanRagdoll && !PlayerPed.IsGettingIntoVehicle)
            {
                Game.Console.Print("You just got knocked the fuck out man!");
                if (PlayerPed.Health - 100 < 65)
                {
                    NativeFunction.Natives.SET_PED_TO_RAGDOLL_WITH_FALL(PlayerPed, 20000, 1000, 1, -PlayerPed.ForwardVector, 1f, 0f, 0f, 0f, 0f, 0f);
                }
                else if (EntryPoint.r.Next(0, 5) == 1)
                {
                    NativeFunction.Natives.SET_PED_TO_RAGDOLL_WITH_FALL(PlayerPed, 100, 1000, 3, -PlayerPed.ForwardVector, 1f, 0f, 0f, 0f, 0f, 0f);
                }
            }
        }
    }
}