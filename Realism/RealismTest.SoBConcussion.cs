// RealismTest.SoBConcussion
using Rage;
using Rage.Native;
using RealismTest;

namespace RealismTest
{

    internal class SoBConcussion
    {
        internal static void ProcessConcusions(Ped PlayerPed)
        {
            string text = "move_m @gangster@a";
            if (PlayerPed.Health - 100 > 97 || BleedSystem.CurrentBlood == 2000f)
            {
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 1.1f);
            }
            else if ((PlayerPed.Health - 100 <= 97 && PlayerPed.Health - 100 >= 92) || BleedSystem.CurrentBlood < 1900f)
            {
                text = "move_m@gangster@a";
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 1.05f);
                //Game.DisplaySubtitle("I feel warmed up.");
            }
            else if ((PlayerPed.Health - 100 <= 91 && PlayerPed.Health - 100 >= 82) || BleedSystem.CurrentBlood < 1700f)
            {
                text = "move_m@hurry_butch@c";
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.93f);
                //Game.DisplaySubtitle("I'm breathing hard.");
            }
            else if ((PlayerPed.Health - 100 <= 81 && PlayerPed.Health - 100 >= 66) || BleedSystem.CurrentBlood < 1500f)
            {
                text = "move_m@casual@c";
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.91f);
                //Game.DisplaySubtitle("I'm struggling for breath and my guard is down.");
            }
            else if ((PlayerPed.Health - 100 <= 65 && PlayerPed.Health - 100 >= 61) || BleedSystem.CurrentBlood < 1340f)
            {
                text = "move_m@hobo@b";
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.91f);
                //Game.DisplaySubtitle("My breath is ragged and I am at my limit.");
                NativeFunction.Natives.SET_PLAYER_SPRINT(PlayerPed, false);
            }
            else if ((PlayerPed.Health - 100 < 60 && PlayerPed.Health - 100 >= 51) || BleedSystem.CurrentBlood < 1200f)
            {
                text = "move_m@hobo@b";
                NativeFunction.Natives.SET_PLAYER_SPRINT(PlayerPed, false);
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.94f);
                NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(0.5f);
                //Game.DisplaySubtitle("I'm in a fair bit of pain.");
            }
            else if ((PlayerPed.Health - 100 < 51 && PlayerPed.Health - 100 >= 38) || BleedSystem.CurrentBlood < 1000f)
            {
                text = "move_m@hobo@b";
                NativeFunction.Natives.SET_PLAYER_SPRINT(PlayerPed, false);
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.94f);
                NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(0.3f);
                //Game.DisplaySubtitle("It's painful to move.");
            }
            else if ((PlayerPed.Health - 100 < 38 && PlayerPed.Health - 100 >= 31) || BleedSystem.CurrentBlood < 800f)
            {
                text = "move_m@hobo@b";
                NativeFunction.Natives.SET_PLAYER_SPRINT(PlayerPed, false);
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.87f);
                NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(0.2f);
                //Game.DisplaySubtitle("I'm disoriented.");
            }
            else if ((PlayerPed.Health - 100 < 30 && PlayerPed.Health - 100 >= 18) || BleedSystem.CurrentBlood < 600f)
            {
                text = "move_m@drunk@moderatedrunk";
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.9f);
                NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(0.1f);
                NativeFunction.Natives.SET_PLAYER_SPRINT(PlayerPed, false);
                Game.DisplaySubtitle("I feel myself slipping deeper into shock.");
            }
            else if (PlayerPed.Health - 100 < 18 || BleedSystem.CurrentBlood < 200f)
            {
                text = "move_m@drunk@verydrunk";
                NativeFunction.Natives.SET_PLAYER_SPRINT(PlayerPed, false);
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE(PlayerPed, 0.84f);
                NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(0f);
                Game.DisplaySubtitle("My vision is growing dim.");
            }
            if (PlayerPed.IsAlive && text != "")
            {
                if ((!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(text)))
                {
                    NativeFunction.Natives.REQUEST_ANIM_SET(text);
                }
                else
                {
                    NativeFunction.Natives.SET_PED_MOVEMENT_CLIPSET(PlayerPed, text, 1f);
                }
            }
        }
    }
}