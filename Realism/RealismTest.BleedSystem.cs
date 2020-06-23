// RealismTest.BleedSystem
using Rage;
using Rage.Native;
using RealismTest;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RealismTest
{

    internal class BleedSystem
    {
        public enum DecalTypes
        {
            splatters_blood = 1010,
            splatters_blood_dir = 1015,
            splatters_blood_mist = 1017,
            splatters_mud = 1020,
            splatters_paint = 1030,
            splatters_water = 1040,
            splatters_water_hydrant = 1050,
            splatters_blood2 = 1110,
            weapImpact_metal = 4010,
            weapImpact_concrete = 4020,
            weapImpact_mattress = 4030,
            weapImpact_mud = 4032,
            weapImpact_wood = 4050,
            weapImpact_sand = 4053,
            weapImpact_cardboard = 4040,
            weapImpact_melee_glass = 4100,
            weapImpact_glass_blood = 4102,
            weapImpact_glass_blood2 = 4104,
            weapImpact_shotgun_paper = 4200,
            weapImpact_shotgun_mattress = 4201,
            weapImpact_shotgun_metal = 4202,
            weapImpact_shotgun_wood = 4203,
            weapImpact_shotgun_dirt = 4204,
            weapImpact_shotgun_tvscreen = 4205,
            weapImpact_shotgun_tvscreen2 = 4206,
            weapImpact_shotgun_tvscreen3 = 4207,
            weapImpact_melee_concrete = 4310,
            weapImpact_melee_wood = 4312,
            weapImpact_melee_metal = 4314,
            burn1 = 4421,
            burn2 = 4422,
            burn3 = 4423,
            burn4 = 4424,
            burn5 = 4425,
            bang_concrete_bang = 5000,
            bang_concrete_bang2 = 5001,
            bang_bullet_bang = 5002,
            bang_bullet_bang2 = 5004,
            bang_glass = 5031,
            bang_glass2 = 5032,
            solidPool_water = 9000,
            solidPool_blood = 9001,
            solidPool_oil = 9002,
            solidPool_petrol = 9003,
            solidPool_mud = 9004,
            porousPool_water = 9005,
            porousPool_blood = 9006,
            porousPool_oil = 9007,
            porousPool_petrol = 9008,
            porousPool_mud = 9009,
            porousPool_water_ped_drip = 9010,
            liquidTrail_water = 9050
        }

        internal const float MAXBLOOD = 2000f;

        internal const float FONTSIZE = 20f;

        internal static float CurrentBlood = 1000f;

        internal static HashSet<BleedData> Bleeds = new HashSet<BleedData>();

        internal static HashSet<BleedData> BleedsLateRemove = new HashSet<BleedData>();

        internal static Texture BloodTexture;

        internal static Texture BloodBarTexture;

        internal static Texture TunnelTexture;

        internal static PointF DrawAnchor;

        internal static PointF DrawAnchor2;

        internal static PointF DrawAnchor3;

        internal static float DrawOffset;

        internal static float BloodBarWidth;

        internal static uint TimeSinceHeal;

        internal static Dictionary<Model, float> BloodStorage = new Dictionary<Model, float>();

        internal static Model LastModel;

        internal static bool MissionActiveLastFrame = false;

        internal static int BloodTimer = 1000;

        internal static void ProcessBleeds(Ped PlayerPed)
        {
            if (Game.IsMissionActive != MissionActiveLastFrame)
            {
                CurrentBlood = 2000f;
                Bleeds.Clear();
            }
            MissionActiveLastFrame = Game.IsMissionActive;
            if (PlayerPed.Model != LastModel)
            {
                if (BloodStorage.ContainsKey(LastModel))
                {
                    BloodStorage[LastModel] = CurrentBlood;
                }
                else
                {
                    BloodStorage.Add(LastModel, CurrentBlood);
                }
                Bleeds.Clear();
                if (BloodStorage.ContainsKey(PlayerPed.Model))
                {
                    CurrentBlood = BloodStorage[PlayerPed.Model];
                }
                else
                {
                    BloodStorage.Add(PlayerPed.Model, 2000f);
                }
                LastModel = PlayerPed.Model;
            }
            dynamic val = NativeFunction.Natives.IS_SPECIAL_ABILITY_ACTIVE<bool>(Game.LocalPlayer);
            if (val || Game.LocalPlayer.IsInvincible)
            {
                return;
            }
            if (CurrentBlood < 1f || PlayerPed.IsDead)
            {
                if(CurrentBlood < 1f)
                    Game.DisplayNotification("You bled to death!");
                CurrentBlood = 2000f;
                BloodStorage[PlayerPed.Model] = CurrentBlood;
                if (PlayerPed.IsAlive)
                {
                    PlayerPed.Kill();
                }
                TrunkLoadout.TrunkMenu.Visible = false;
                TrunkLoadout.TrunkRestockMenu.Visible = false;
                RealismMenu.GarageMenu.Visible = false;
                RealismMenu.RealismMenuMain.Visible = false;
                RealismMenu.RealismMenuKeyBinds.Visible = false;
                RealismMenu.RealismMenuToggle.Visible = false;
                RealismMenu.RealismMenuMechanic.Visible = false;
                Bleeds.Clear();
            }
            else
            {
                if (Bleeds.Count < 1)
                {
                    return;
                }
                BleedData[] array = Bleeds.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].CalculateAndDoDamage();
                }
                BloodStorage[PlayerPed.Model] = CurrentBlood;
                foreach (BleedData item in BleedsLateRemove)
                {
                    Bleeds.Remove(item);
                }
                BleedsLateRemove.Clear();
                if (Bleeds.Count >= 1)
                {
                    if (EntryPoint.r.Next(1001) == 44)
                    {
                        AddDecal(PlayerPed.Position, DecalTypes.splatters_blood, 2f, 2f, 0.15f, 0f, 0f, 0.95f, 120f);
                    }
                    if (EntryPoint.r.Next(1001) == 782)
                    {
                        AddDecal(PlayerPed.Position, DecalTypes.splatters_blood_dir, 2f, 2f, 0.1f, 0f, 0f, 0.95f, 120f);
                    }
                    if (EntryPoint.r.Next(1001) == 133)
                    {
                        AddDecal(PlayerPed.Position, DecalTypes.splatters_blood_mist, 2f, 2f, 0.1f, 0f, 0f, 0.95f, 120f);
                    }
                }
            }
        }

        internal static void ProcessHealing(Ped PlayerPed)
        {
            if (CurrentBlood > 1000f && TimeSinceHeal + BloodTimer < Game.GameTime)
            {
                CurrentBlood += 2f;
                if (PlayerPed.Health - 1 < PlayerPed.MaxHealth)
                {
                    PlayerPed.Health++;
                }
                CurrentBlood = MathHelper.Clamp(CurrentBlood, 0f, 2000f);
                TimeSinceHeal = Game.GameTime;
            }
            else if (TimeSinceHeal + 1000 < Game.GameTime)
            {
                CurrentBlood += 1f;
                TimeSinceHeal = Game.GameTime;
            }
            if (PlayerPed.MaxHealth < PlayerPed.Health + 10)
            {
                BloodTimer = 333;
            }
            else
            {
                BloodTimer = 1000;
            }
        }

        internal static void ProcessDamage(Ped PlayerPed)
        {
            if (!EntryPoint.HasBeenDamaged)
            {
                return;
            }
            switch (PlayerPed.LastDamageBone)
            {
                case PedBoneId.LeftThumb1:
                case PedBoneId.LeftThumb2:
                case PedBoneId.LeftRingFinger1:
                case PedBoneId.LeftRingFinger2:
                case PedBoneId.LeftPinky1:
                case PedBoneId.LeftPinky2:
                case PedBoneId.LeftIndexFinger1:
                case PedBoneId.LeftIndexFinger2:
                case PedBoneId.LeftMiddleFinger1:
                case PedBoneId.LeftMiddleFinger2:
                case PedBoneId.LeftThumb0:
                case PedBoneId.LeftIndexFinger0:
                case PedBoneId.LeftMiddleFinger0:
                case PedBoneId.LeftRingFinger0:
                case PedBoneId.LeftPinky0:
                case PedBoneId.RightThumb0:
                case PedBoneId.RightIndexFinger0:
                case PedBoneId.RightMiddleFinger0:
                case PedBoneId.RightRingFinger0:
                case PedBoneId.RightPinky0:
                case PedBoneId.RightThumb1:
                case PedBoneId.RightThumb2:
                case PedBoneId.RightRingFinger1:
                case PedBoneId.RightRingFinger2:
                case PedBoneId.RightPinky1:
                case PedBoneId.RightPinky2:
                case PedBoneId.RightIndexFinger1:
                case PedBoneId.RightIndexFinger2:
                case PedBoneId.RightMiddleFinger1:
                case PedBoneId.RightMiddleFinger2:
                    {
                        if (RealismMenu.isHandDamageOn)
                        {
                            WeaponDrop.ProcessHandDamagePlayer(PlayerPed);

                        }
                        IEnumerable<BleedData> source2 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source2.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 5000, 50u));
                        }
                        else
                        {
                            BleedData bleedData2 = source2.First();
                            bleedData2.EndTime += 5000u;
                            bleedData2.DamageDealt = 0f;
                            bleedData2.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.RightClavicle:
                case PedBoneId.LeftClavicle:
                    {
                        IEnumerable<BleedData> source7 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source7.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 30000, 100u));
                        }
                        else
                        {
                            BleedData bleedData7 = source7.First();
                            bleedData7.EndTime += 30000u;
                            bleedData7.DamageDealt = 0f;
                            bleedData7.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.LeftFoot:
                case PedBoneId.RightPhFoot:
                case PedBoneId.RightFoot:
                case PedBoneId.LeftPhFoot:
                    {
                        IEnumerable<BleedData> source8 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source8.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 10000, 75u));
                        }
                        else
                        {
                            BleedData bleedData8 = source8.First();
                            bleedData8.EndTime += 10000u;
                            bleedData8.DamageDealt = 0f;
                            bleedData8.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.LeftHand:
                case PedBoneId.RightPhHand:
                case PedBoneId.RightHand:
                case PedBoneId.LeftPhHand:
                    {
                        if (RealismMenu.isHandDamageOn)
                        {
                            WeaponDrop.ProcessHandDamagePlayer(PlayerPed);
                        }
                        IEnumerable<BleedData> source6 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source6.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 10000, 100u));
                        }
                        else
                        {
                            BleedData bleedData6 = source6.First();
                            bleedData6.EndTime += 10000u;
                            bleedData6.DamageDealt = 0f;
                            bleedData6.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.Spine:
                case PedBoneId.Spine1:
                case PedBoneId.Spine2:
                case PedBoneId.Spine3:
                case PedBoneId.SpineRoot:
                    {
                        IEnumerable<BleedData> source3 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source3.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 20000, 250u));
                        }
                        else
                        {
                            BleedData bleedData3 = source3.First();
                            bleedData3.EndTime += 20000u;
                            bleedData3.DamageDealt = 0f;
                            bleedData3.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.Head:
                    {
                        if (RealismMenu.isKnockoutsOn)
                        {
                            Knockouts.ProcessKnockouts(PlayerPed);
                        }
                        IEnumerable<BleedData> source4 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source4.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 20000, 350u));
                        }
                        else
                        {
                            BleedData bleedData4 = source4.First();
                            bleedData4.EndTime += 20000u;
                            bleedData4.DamageDealt = 0f;
                            bleedData4.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.Neck:
                    {
                        IEnumerable<BleedData> source5 = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source5.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 30000, 500u));
                        }
                        else
                        {
                            BleedData bleedData5 = source5.First();
                            bleedData5.EndTime += 20000u;
                            bleedData5.DamageDealt = 0f;
                            bleedData5.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
                case PedBoneId.Pelvis:
                case PedBoneId.RightForearm:
                case PedBoneId.RightCalf:
                case PedBoneId.RightUpperArm:
                case PedBoneId.LeftUpperArm:
                case PedBoneId.RightThigh:
                case PedBoneId.LeftThigh:
                case PedBoneId.LeftForeArm:
                case PedBoneId.LeftCalf:
                    {
                        IEnumerable<BleedData> source = Bleeds.Where((BleedData b) => b.Bone == PlayerPed.LastDamageBone);
                        if (source.Count() < 1)
                        {
                            Bleeds.Add(new BleedData(PlayerPed.LastDamageBone, Game.GameTime + 7000, 250u));
                        }
                        else
                        {
                            BleedData bleedData = source.First();
                            bleedData.EndTime += 7500u;
                            bleedData.DamageDealt = 0f;
                            bleedData.Start = Game.GameTime;
                        }
                        EntryPoint.HasBeenDamaged = false;
                        break;
                    }
            }
        }

        internal static void BloodInit()
        {
            BloodTexture = Game.CreateTextureFromFile(".\\Plugins\\Realism\\blood.png");
            BloodBarTexture = Game.CreateTextureFromFile(".\\Plugins\\Realism\\blood_bar.png");
            TunnelTexture = Game.CreateTextureFromFile(".\\Plugins\\Realism\\tunnel_vision.png");
            DrawOffset = Game.Resolution.Width - Rage.Graphics.MeasureText("Blood:", "Chalet", 20f).Width - 130f - 40f;
            DrawAnchor = new PointF(Game.Resolution.Width - 10f - Rage.Graphics.MeasureText("Blood", "Chalet", 20f).Width, 110f - Rage.Graphics.MeasureText("Blood:", "Chalet", 20f).Height);
            DrawAnchor2 = new PointF(Game.Resolution.Width - 10f - Rage.Graphics.MeasureText("Blood", "Chalet", 20f).Width - 80f - 5f - 40f, 110f - Rage.Graphics.MeasureText("Blood:", "Chalet", 20f).Height);
            DrawAnchor3 = new PointF(Game.Resolution.Width - 10f - Rage.Graphics.MeasureText("### Bleeding Wounds", "Chalet", 20f).Width, 130f);
            BloodBarWidth = 128f * (CurrentBlood / 2000f);
            TimeSinceHeal = Game.GameTime;
        }

        private static void AddDecal(Vector3 pos, DecalTypes decalType, float width = 1f, float height = 1f, float rCoef = 0.1f, float gCoef = 0.1f, float bCoef = 0.1f, float opacity = 1f, float timeout = 20f)
        {
            NativeFunction.Natives.ADD_DECAL<int>((int)decalType, pos.X, pos.Y, GetGroundZ(pos), 0, 0, -1f, 0, 1f, 0, width, height, rCoef, gCoef, bCoef, opacity, timeout, 0, 0, 0);
        }

        private static float GetGroundZ(Vector3 pos)
        {
            NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(pos.X, pos.Y, pos.Z, out float result, false);
            return result;
        }
    }
}