// RealismTest.BleedData
using Rage;
using RealismTest;

namespace RealismTest
{

    internal class BleedData
    {
        public PedBoneId Bone;

        public float DamageDealt;

        public uint EndTime;

        public uint DamageToDeal;

        public uint Start;

        public BleedData(PedBoneId bone, uint end, uint damage)
        {
            Bone = bone;
            EndTime = end;
            DamageDealt = 0f;
            DamageToDeal = damage;
            Start = Game.GameTime;
        }

        public void CalculateAndDoDamage()
        {
            if (Game.GameTime > EndTime)
            {
                BleedSystem.BleedsLateRemove.Add(this);
                return;
            }

            //Get Game Ticks Since Start
            uint num = Game.GameTime - Start;

            //Get Game Ticks Until End
            uint num2 = EndTime - Start;

            //If Processing Required
            if (num >= 1 && num2 >= 1)
            {
                // Damage This Tick is equal to ((TotalDamage / (End / Start)) - Damage Already Done)
                // (End / Start)
                float num3 = (DamageToDeal / (num2 / (float)num)) - DamageDealt;
                if (!(num3 < 1f))
                {
                    DamageDealt += num3;
                    BleedSystem.CurrentBlood -= num3;
                    BleedSystem.CurrentBlood = MathHelper.Clamp(BleedSystem.CurrentBlood, 0f, 2000f);
                }
            }
        }
    }
}