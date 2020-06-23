// RealismTest.CharacterExtensions.CharacterExtensions
using Rage;
using Rage.Native;

namespace RealismTest.CharacterExtensions
{

    public static class CharacterExtensions
    {
        public static int GetPlayerMoney(this Ped p)
        {
            uint num = 0u;
            switch (Game.LocalPlayer.Model.Hash)
            {
                case 225514697u:
                    num = NativeFunction.Natives.GET_HASH_KEY<uint>("SP0_TOTAL_CASH");
                    break;
                case 2602752943u:
                    num = NativeFunction.Natives.GET_HASH_KEY<uint>("SP1_TOTAL_CASH");
                    break;
                case 2608926626u:
                    num = NativeFunction.Natives.GET_HASH_KEY<uint>("SP2_TOTAL_CASH");
                    break;
            }
            if (num != 0)
            {
                NativeFunction.Natives.STAT_GET_INT(num, out int result, -1);
                return result;
            }
            return 0;
        }

        public static void SetPlayerMoney(this Ped p, int Money)
        {
            uint num = 0u;
            switch (Game.LocalPlayer.Model.Hash)
            {
                case 225514697u:
                    num = NativeFunction.Natives.GET_HASH_KEY<uint>("SP0_TOTAL_CASH");
                    break;
                case 2602752943u:
                    num = NativeFunction.Natives.GET_HASH_KEY<uint>("SP1_TOTAL_CASH");
                    break;
                case 2608926626u:
                    num = NativeFunction.Natives.GET_HASH_KEY<uint>("SP2_TOTAL_CASH");
                    break;
            }
            if (num != 0)
            {
                NativeFunction.Natives.STAT_SET_INT(num, Money, -1);
                TrunkLoadout.RestockMoney.Text = "You have $" + Money;
                
            }
        }
    }
}