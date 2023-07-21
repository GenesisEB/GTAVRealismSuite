// RealismTest.Config
using Newtonsoft.Json;
using Rage;
using RealismTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RealismTest
{
    [JsonObject]
    internal class Config
    {
        internal static Config CurrentConfig;

        internal static bool Excepted = false;

        [JsonProperty]
        internal bool isHandDamageOn = true;

        [JsonProperty]
        internal bool isKnockoutsOn = true;

        [JsonProperty]
        internal bool isConcussionOn = true;

        [JsonProperty]
        internal bool isMechanicOn = true;

        [JsonProperty]
        internal bool isAttachmentOn = true;

        [JsonProperty]
        internal bool isBleedingOn = true;

        [JsonProperty]
        internal bool isDogsOn = true;

        [JsonProperty]
        internal bool isPBlipsOn = true;

        [JsonProperty]
        internal bool isECOn = true;

        [JsonProperty]
        internal bool isKMOn = true;

        [JsonProperty]
        internal Keys MenuKey = Keys.F3;

        [JsonProperty]
        internal Keys CarKey = Keys.NumPad0;

        [JsonProperty]
        internal Keys DropKey = Keys.D9;

        [JsonProperty]
        internal int TrunkBandages = 0;

        [JsonProperty]
        internal int TrunkArmor = 0;

        [JsonProperty]
        internal int TrunkHealth = 0;

        [JsonProperty]
        internal Dictionary<uint, short> StoredGunAssetNames;

        [JsonProperty]
        internal HashSet<uint> OwnedHashes;

        [JsonProperty]
        internal Dictionary<string, int> WeaponTints;

        [JsonConstructor]
        public Config()
        {
        }

        internal void SaveConfig()
        {
            TrunkArmor = TrunkLoadout.NumArmor;
            TrunkBandages = TrunkLoadout.NumBandages;
            TrunkHealth = TrunkLoadout.NumHealthPacks;
            StoredGunAssetNames = TrunkLoadout.StoredGunAssetNames;
            OwnedHashes = AttachmentSystem.OwnedHashes;
            WeaponTints = AttachmentSystem.WeaponTints;
            string text = "";
            text = JsonConvert.SerializeObject(this);
            StreamWriter streamWriter = new StreamWriter(".\\Plugins\\Realism\\config.ini", append: false);
            streamWriter.WriteLine(text);
            streamWriter.Close();
        }

        internal static void LoadConfig()
        {

            if (File.Exists(".\\Plugins\\Realism\\config.ini"))
            {
                StreamReader streamReader = new StreamReader(".\\Plugins\\Realism\\config.ini");
                string currentLine = streamReader.ReadLine();
                try
                {
                    JsonConvert.DeserializeObject<Config>(currentLine);
                }
                catch {
                    Excepted = true;
                }
                if(!Excepted)
                    CurrentConfig = JsonConvert.DeserializeObject<Config>(currentLine);
                streamReader.Close();
                
            }
            if(CurrentConfig == null)
            {
                Game.DisplayNotification("Config was out of date, corrupted, or missing. New config created!");
                CurrentConfig = new Config();
                CurrentConfig.SaveConfig();
            }
            RealismMenu.isAttachmentOn = CurrentConfig.isAttachmentOn;
            RealismMenu.isBleedingOn = CurrentConfig.isBleedingOn;
            RealismMenu.isConcussionOn = CurrentConfig.isConcussionOn;
            RealismMenu.isHandDamageOn = CurrentConfig.isHandDamageOn;
            RealismMenu.isKnockoutsOn = CurrentConfig.isKnockoutsOn;
            RealismMenu.isMechanicOn = CurrentConfig.isMechanicOn;
            RealismMenu.isDogsOn = CurrentConfig.isDogsOn;
            RealismMenu.isPBlipsOn = CurrentConfig.isPBlipsOn;
            RealismMenu.isECOn = CurrentConfig.isECOn;
            RealismMenu.isKMOn = CurrentConfig.isKMOn;
            RealismMenu.ToggleAttachment.Checked = CurrentConfig.isAttachmentOn;
            RealismMenu.ToggleBleeds.Checked = CurrentConfig.isBleedingOn;
            RealismMenu.ToggleConcussion.Checked = CurrentConfig.isConcussionOn;
            RealismMenu.ToggleHandDamage.Checked = CurrentConfig.isHandDamageOn;
            RealismMenu.ToggleKnockOuts.Checked = CurrentConfig.isKnockoutsOn;
            RealismMenu.ToggleMechanic.Checked = CurrentConfig.isMechanicOn;
            RealismMenu.ToggleDogs.Checked = CurrentConfig.isDogsOn;
            RealismMenu.TogglePoliceBlips.Checked = CurrentConfig.isPBlipsOn;
            RealismMenu.ToggleEngineController.Checked = CurrentConfig.isECOn;
            RealismMenu.ToggleKeyboardMode.Checked = CurrentConfig.isKMOn;
            RealismMenu.UpdateMenu(null, RealismMenu.isMechanicOn);
            RealismMenu.RealismMenuToggle.RefreshIndex();
            TrunkLoadout.NumArmor = CurrentConfig.TrunkArmor;
            TrunkLoadout.NumBandages = CurrentConfig.TrunkBandages;
            TrunkLoadout.NumHealthPacks = CurrentConfig.TrunkHealth;
            RealismMenu.GetCarKey = CurrentConfig.CarKey;
            RealismMenu.GetMenuKey = CurrentConfig.MenuKey;
            RealismMenu.WeaponDropKey = CurrentConfig.DropKey;
            if (CurrentConfig.StoredGunAssetNames != null)
                TrunkLoadout.StoredGunAssetNames = CurrentConfig.StoredGunAssetNames;
            if (CurrentConfig.OwnedHashes != null)
                AttachmentSystem.OwnedHashes = CurrentConfig.OwnedHashes;
            if (CurrentConfig.WeaponTints != null)
                AttachmentSystem.WeaponTints = CurrentConfig.WeaponTints;

        }
    }
}