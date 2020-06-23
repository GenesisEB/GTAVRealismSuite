// RealismTest.ModHelper
using Newtonsoft.Json;
using Rage;
namespace RealismTest
{
    [JsonObject]
    internal class ModHelper
    {
        [JsonProperty]
        private int ArmorModIndex;

        [JsonProperty]
        private int BonnetModIndex;

        [JsonProperty]
        private int BrakesModIndex;

        [JsonProperty]
        private int ChassisModIndex;

        [JsonProperty]
        private int EngineModIndex;

        [JsonProperty]
        private int ExhaustModIndex;

        [JsonProperty]
        private int FrontBumperModIndex;

        [JsonProperty]
        private int GrillModIndex;

        [JsonProperty]
        private bool HasTurbo;

        [JsonProperty]
        private bool HasXenonHeadlights;

        [JsonProperty]
        private int HornsModIndex;

        [JsonProperty]
        private int LeftFenderModIndex;

        [JsonProperty]
        private int RearBumperModIndex;

        [JsonProperty]
        private int RightFenderModIndex;

        [JsonProperty]
        private int RoofModIndex;

        [JsonProperty]
        private int SkirtModIndex;

        [JsonProperty]
        private int SpoilerModIndex;

        [JsonProperty]
        private int SuspensionModIndex;

        [JsonProperty]
        private int TransmissionModIndex;

        [JsonProperty]
        private VehicleWheelType WheelsType;

        [JsonProperty]
        private int WheelsModIndex;

        [JsonConstructor]
        internal ModHelper()
        {
        }

        internal ModHelper(Vehicle v)
        {
            ArmorModIndex = v.Mods.ArmorModIndex;
            BonnetModIndex = v.Mods.BonnetModIndex;
            BrakesModIndex = v.Mods.BrakesModIndex;
            ChassisModIndex = v.Mods.ChassisModIndex;
            EngineModIndex = v.Mods.EngineModIndex;
            ExhaustModIndex = v.Mods.ExhaustModIndex;
            FrontBumperModIndex = v.Mods.FrontBumperModIndex;
            GrillModIndex = v.Mods.GrillModIndex;
            HasTurbo = v.Mods.HasTurbo;
            HasXenonHeadlights = v.Mods.HasXenonHeadlights;
            HornsModIndex = v.Mods.HornsModIndex;
            LeftFenderModIndex = v.Mods.LeftFenderModIndex;
            RearBumperModIndex = v.Mods.RearBumperModIndex;
            RightFenderModIndex = v.Mods.RightFenderModIndex;
            RoofModIndex = v.Mods.RoofModIndex;
            SkirtModIndex = v.Mods.SkirtModIndex;
            SpoilerModIndex = v.Mods.SpoilerModIndex;
            SuspensionModIndex = v.Mods.SuspensionModIndex;
            TransmissionModIndex = v.Mods.TransmissionModIndex;
            v.Mods.GetWheelMod(out WheelsType, out WheelsModIndex);
        }

        internal void ReplaceMods(Vehicle v)
        {
            v.Mods.ArmorModIndex = ArmorModIndex;
            v.Mods.BonnetModIndex = BonnetModIndex;
            v.Mods.BrakesModIndex = BrakesModIndex;
            v.Mods.ChassisModIndex = ChassisModIndex;
            v.Mods.EngineModIndex = EngineModIndex;
            v.Mods.ExhaustModIndex = ExhaustModIndex;
            v.Mods.FrontBumperModIndex = FrontBumperModIndex;
            v.Mods.GrillModIndex = GrillModIndex;
            v.Mods.HasTurbo = HasTurbo;
            v.Mods.HasXenonHeadlights = HasXenonHeadlights;
            v.Mods.HornsModIndex = HornsModIndex;
            v.Mods.LeftFenderModIndex = LeftFenderModIndex;
            v.Mods.RearBumperModIndex = RearBumperModIndex;
            v.Mods.RightFenderModIndex = RightFenderModIndex;
            v.Mods.RoofModIndex = RoofModIndex;
            v.Mods.SkirtModIndex = SkirtModIndex;
            v.Mods.SpoilerModIndex = SpoilerModIndex;
            v.Mods.SuspensionModIndex = SuspensionModIndex;
            v.Mods.TransmissionModIndex = TransmissionModIndex;
        }
    }
}