// RealismTest.TrunkLoadout
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RealismTest.CharacterExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RealismTest
{

    internal class TrunkLoadout
    {
        internal static UIMenu TrunkMenu;

        internal static UIMenuItem RestockTrunk;

        internal static UIMenuItem Bandages;

        internal static UIMenuItem HealthPack;

        internal static UIMenuItem Armor;

        internal static UIMenuItem StoreGun;

        internal static Dictionary<uint, short> StoredGunAssetNames = new Dictionary<uint, short>();

        internal static UIMenu TrunkRestockMenu;

        internal static UIMenuItem BandagesR;

        internal static UIMenuItem RestockMoney;

        internal static UIMenuItem HealthPackR;

        internal static UIMenuItem ArmorR;

        internal static UIMenuItem Back;

        internal static int NumBandages;

        internal static int NumHealthPacks;

        internal static int NumArmor;

        internal static void MenuInit()
        {
            NumBandages = Config.CurrentConfig.TrunkBandages;
            NumHealthPacks = Config.CurrentConfig.TrunkHealth;
            NumArmor = Config.CurrentConfig.TrunkArmor;
            TrunkMenu = new UIMenu("Trunk", "It lets you store stuff.");
            TrunkRestockMenu = new UIMenu("Trunk (Restocking)", "What're ya buying?");
            RestockTrunk = new UIMenuItem("Restock", "Lets you restock the trunk.");
            Back = new UIMenuItem("Go Back", "Ends the restocking and reopens the trunk.");
            Bandages = new UIMenuItem("Bandages x " + NumBandages, "Will stop 1 bleed.");
            RestockMoney = new UIMenuItem("You have $" + Game.LocalPlayer.Character.GetPlayerMoney());
            BandagesR = new UIMenuItem("Bandages x " + NumBandages + " - $500", "Some expensive damn bandages");
            if (NumBandages == 0)
            {
                Bandages.Enabled = false;
            }
            HealthPack = new UIMenuItem("Health Packs x " + NumHealthPacks, "Will stop all bleeding, heal you, and restore blood.");
            HealthPackR = new UIMenuItem("Health Packs x " + NumHealthPacks + " - $5000", "Classic Capitalism...");
            if (NumHealthPacks == 0)
            {
                HealthPack.Enabled = false;
            }
            Armor = new UIMenuItem("Armor Packs x " + NumArmor, "Adds 20 armor points.");
            ArmorR = new UIMenuItem("Armor Packs x " + NumArmor + " - $1000", "20 armor for $1000... Fuck.");
            if (NumArmor == 0)
            {
                Armor.Enabled = false;
            }
            StoreGun = new UIMenuItem("Store Gun", "Allows you to put any weapon in your trunk.");
            StoreGun.Activated += DoStoreGun;
            RestockTrunk.Activated += OpenRestock;
            Armor.Activated += ApplyArmor;
            Bandages.Activated += ApplyBandage;
            HealthPack.Activated += ApplyHealthPack;
            BandagesR.Activated += BuyItem;
            ArmorR.Activated += BuyItem;
            HealthPackR.Activated += BuyItem;
            Back.Activated += GoBack;
            TrunkRestockMenu.AddItem(RestockMoney);
            RestockMoney.Enabled = false;
            TrunkRestockMenu.AddItem(BandagesR);
            TrunkRestockMenu.AddItem(ArmorR);
            TrunkRestockMenu.AddItem(HealthPackR);
            TrunkRestockMenu.AddItem(Back);
            TrunkMenu.AddItem(RestockTrunk);
            TrunkMenu.AddItem(Bandages);
            TrunkMenu.AddItem(Armor);
            TrunkMenu.AddItem(HealthPack);
            TrunkMenu.AddItem(StoreGun);
            TrunkMenu.RefreshIndex();
            TrunkMenu.OnMenuClose += MenuClose;
            TrunkRestockMenu.OnMenuClose += MenuClose;
        }

        internal static void PersonalVehicleCheck()
        {
            if (TaskSystem.SavedVehicle != null && EntryPoint.PlayerPed && TaskSystem.SavedVehicle.ActualVehicle && (!EntryPoint.PlayerPed.CurrentVehicle || !(TaskSystem.SavedVehicle.ActualVehicle == EntryPoint.PlayerPed.CurrentVehicle)) && EntryPoint.PlayerPed.DistanceTo(TaskSystem.SavedVehicle.ActualVehicle.GetOffsetPositionFront(0f - (TaskSystem.SavedVehicle.ActualVehicle.Length + 0.05f) / 2f)) < 1f && TaskSystem.SavedVehicle.ActualVehicle.GetDoors().Length > 2)
            {
                Game.DisplayHelp("Press E to open your trunk!");
                if (Game.IsKeyDown(Keys.E))
                {
                    OpenTrunk(TaskSystem.SavedVehicle.ActualVehicle);
                }
                GameFiber.WaitUntil(() => !Game.IsKeyDown(Keys.E));
            }
        }

        internal static void OpenTrunk(Vehicle vehicle)
        {
            VehicleDoor[] doors = vehicle.GetDoors();
            if (doors[doors.Length - 1].IsValid())
            {
                doors[doors.Length - 1].Open(instantly: false);
                TrunkMenu.Visible = true;
                NativeFunction.Natives.DISPLAY_CASH(true);
                return;
            }
            Game.DisplayNotification("This vehicle has no trunk!");
            VehicleDoor[] array = doors;
            foreach (VehicleDoor vehicleDoor in array)
            {
                if (vehicleDoor.IsValid())
                {
                    vehicleDoor.Open(instantly: false);
                }
                Game.Console.Print(vehicleDoor.GetHashCode().ToString());
            }
        }

        internal static void OpenRestock(UIMenu sender, UIMenuItem selectedItem)
        {
            TrunkMenu.Visible = false;
            TrunkRestockMenu.Visible = true;
        }

        internal static void GoBack(UIMenu sender, UIMenuItem selectedItem)
        {
            TrunkMenu.Visible = true;
            TrunkRestockMenu.Visible = false;
        }

        internal static void DoStoreGun(UIMenu sender, UIMenuItem selectedItem)
        {
            if (EntryPoint.PlayerPed && AttachmentSystem.EquippedWeapon && AttachmentSystem.EquippedWeapon.Asset.IsValid)
            {
                WeaponDescriptor equippedWeapon = EntryPoint.PlayerPed.Inventory.EquippedWeapon;
                if (!StoredGunAssetNames.ContainsKey((uint)EntryPoint.PlayerPed.Inventory.EquippedWeapon.Hash))
                {
                    StoredGunAssetNames.Add((uint)EntryPoint.PlayerPed.Inventory.EquippedWeapon.Hash, AttachmentSystem.EquippedWeaponDescriptor.Ammo);
                    UIMenuItem uIMenuItem = new UIMenuItem(EntryPoint.PlayerPed.Inventory.EquippedWeapon.Hash.ToString(), "Get this weapon from your trunk.");
                    WeaponDescriptor changeTo = EntryPoint.PlayerPed.Inventory.Weapons.FirstOrDefault(d => d.Hash != equippedWeapon.Hash);
                    if(changeTo != null)
                        EntryPoint.PlayerPed.Inventory.EquippedWeapon = changeTo;
                    EntryPoint.PlayerPed.Inventory.Weapons.Remove(equippedWeapon);
                    uIMenuItem.Activated += GetGun;
                    TrunkMenu.AddItem(uIMenuItem);
                    TrunkMenu.RefreshIndex();
                }
            }
        }

        internal static void GetGun(UIMenu sender, UIMenuItem selectedItem)
        {
            uint num = 0u;
            foreach (KeyValuePair<uint, short> keyValuePair in StoredGunAssetNames)
            {
                if (selectedItem.Text == ((WeaponHash)keyValuePair.Key).ToString())
                {
                    Game.LocalPlayer.Character.Inventory.GiveNewWeapon((WeaponHash)keyValuePair.Key, keyValuePair.Value, equipNow: true);
                    num = keyValuePair.Key;
                    TrunkMenu.RemoveItemAt(TrunkMenu.MenuItems.FindIndex((UIMenuItem i) => i.Text == ((WeaponHash)keyValuePair.Key).ToString()));
                    TrunkMenu.RefreshIndex();
                }
            }
            if (num != 0 && StoredGunAssetNames.ContainsKey(num))
            {
                StoredGunAssetNames.Remove(num);
            }
        }

        internal static void ApplyArmor(UIMenu sender, UIMenuItem selectedItem)
        {
            if (NumArmor < 1 || EntryPoint.PlayerPed.Armor >= 100)
            {
                Game.DisplayNotification("You don't have any armor or your armor is full!");
                return;
            }
            NumArmor--;
            if (NumArmor == 0)
            {
                Armor.Enabled = false;
            }
            Armor.Text = "Armor Packs x " + NumArmor;
            ArmorR.Text = "Armor Packs x " + NumArmor + " - $1000";
            EntryPoint.PlayerPed.Armor += 20;
            int currentSelection = TrunkMenu.CurrentSelection;
            TrunkMenu.RefreshIndex();
            TrunkRestockMenu.RefreshIndex();
            TrunkMenu.CurrentSelection = currentSelection;
            Config.CurrentConfig.SaveConfig();
        }

        internal static void ApplyBandage(UIMenu sender, UIMenuItem selectedItem)
        {
            if (NumBandages < 1 || BleedSystem.Bleeds.Count == 0)
            {
                Game.DisplayNotification("You don't have any bandages or your not bleeding!");
                return;
            }
            if (BleedSystem.Bleeds.Count > 0)
            {
                NumBandages--;
                Bandages.Text = "Bandages x " + NumBandages;
                BandagesR.Text = "Bandages x " + NumBandages + " - $500";
                BleedSystem.Bleeds.Remove(BleedSystem.Bleeds.Last());
            }
            int currentSelection = TrunkMenu.CurrentSelection;
            TrunkMenu.RefreshIndex();
            TrunkRestockMenu.RefreshIndex();
            TrunkMenu.CurrentSelection = currentSelection;
            Config.CurrentConfig.SaveConfig();
        }

        internal static void ApplyHealthPack(UIMenu sender, UIMenuItem selectedItem)
        {
            if (NumHealthPacks < 1)
            {
                Game.DisplayNotification("You don't have any Health Packs!");
                return;
            }
            if (BleedSystem.Bleeds.Count > 0)
            {
                BleedSystem.Bleeds.Clear();
            }
            EntryPoint.PlayerPed.Health = EntryPoint.PlayerPed.MaxHealth;
            BleedSystem.CurrentBlood = MathHelper.Clamp(BleedSystem.CurrentBlood + 1000f, 0f, 2000f);
            NumHealthPacks--;
            HealthPack.Text = "Health Packs x " + NumHealthPacks;
            HealthPackR.Text = "Health Packs x " + NumHealthPacks + " - $5000";
            int currentSelection = TrunkMenu.CurrentSelection;
            TrunkMenu.RefreshIndex();
            TrunkRestockMenu.RefreshIndex();
            TrunkMenu.CurrentSelection = currentSelection;
            Config.CurrentConfig.SaveConfig();
        }

        internal static void BuyItem(UIMenu sender, UIMenuItem selectedItem)
        {
            int num = Game.LocalPlayer.Character.GetPlayerMoney();
            Game.Console.Print(num.ToString());
            if (selectedItem.Text.Contains("Bandage"))
            {
                if (num - 500 >= 0)
                {
                    num -= 500;
                    Bandages.Enabled = true;
                    NumBandages++;
                    Bandages.Text = "Bandages x " + NumBandages;
                    BandagesR.Text = "Bandages x " + NumBandages + " - $500";
                    
                    int currentSelection = TrunkRestockMenu.CurrentSelection;
                    Game.LocalPlayer.Character.SetPlayerMoney(num);
                    TrunkMenu.RefreshIndex();
                    TrunkRestockMenu.RefreshIndex();
                    TrunkRestockMenu.CurrentSelection = currentSelection;
                }
                else
                {
                    Game.DisplayNotification("Not enough money to finish this transaction!");
                }
            }
            if (selectedItem.Text.Contains("Health"))
            {
                if (num - 5000 >= 0)
                {
                    num -= 5000;
                    NumHealthPacks++;
                    HealthPack.Text = "Health Packs x " + NumHealthPacks;
                    HealthPackR.Text = "Health Packs x " + NumHealthPacks + " - $5000";
                    
                    HealthPack.Enabled = true;
                    int currentSelection2 = TrunkRestockMenu.CurrentSelection;
                    Game.LocalPlayer.Character.SetPlayerMoney(num);
                    TrunkMenu.RefreshIndex();
                    TrunkRestockMenu.RefreshIndex();
                    TrunkRestockMenu.CurrentSelection = currentSelection2;
                }
                else
                {
                    Game.DisplayNotification("Not enough money to finish this transaction!");
                }
            }
            if (selectedItem.Text.Contains("Armor"))
            {
                if (num - 1000 >= 0)
                {
                    num -= 1000;
                    NumArmor++;
                    Armor.Text = "Armor Packs x " + NumArmor;
                    ArmorR.Text = "Armor Packs x " + NumArmor + " - $1000";
                    Armor.Enabled = true;
                    int currentSelection3 = TrunkRestockMenu.CurrentSelection;
                    Game.LocalPlayer.Character.SetPlayerMoney(num);
                    TrunkMenu.RefreshIndex();
                    TrunkRestockMenu.RefreshIndex();
                    TrunkRestockMenu.CurrentSelection = currentSelection3;
                }
                else
                {
                    Game.DisplayNotification("Not enough money to finish this transaction!");
                }
            }
            
            Config.CurrentConfig.SaveConfig();
        }

        internal static void MenuClose(UIMenu sender)
        {
            if (TrunkRestockMenu.Visible)
            {
                return;
            }
            Game.Console.Print("Closing door");
            NativeFunction.Natives.DISPLAY_CASH(false);
            Vehicle actualVehicle = TaskSystem.SavedVehicle.ActualVehicle;
            if (actualVehicle)
            {
                if (actualVehicle.Doors[5].IsValid())
                {
                    actualVehicle.Doors[5].Close(instantly: false);
                }
                else
                {
                    Game.DisplayNotification("This vehicle has no trunk! WTF?");
                }
            }
        }
    }
}