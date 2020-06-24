// RealismTest.RealismMenu
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RealismTest.CharacterExtensions;
using RealismTest;
using System;
using System.Linq;
using System.Windows.Forms;

namespace RealismTest
{
    
    internal class RealismMenu
    {
        internal static int CurrentGarage = 0;

        internal static UIMenu RealismMenuMain;

        internal static UIMenu RealismMenuMechanic;

        internal static UIMenu RealismMenuKeyBinds;

        internal static UIMenu RealismMenuToggle;

        internal static UIMenu GarageMenu;

        internal static UIMenuItem TogglesMenuSwitch;

        internal static UIMenuItem KeybindMenuSwitch;

        internal static UIMenuItem MechanicMenuSwitch;

        internal static UIMenuItem BuyGarage;

        internal static UIMenuItem SaveCar;

        internal static UIMenuItem FetchCar;

        internal static UIMenuItem MenuKeyBeingChanged = null;

        internal static UIMenuItem DropKeyWeapon;

        internal static UIMenuItem GetKeyCar;

        internal static Keys WeaponDropKey = Keys.D9;

        internal static Keys GetCarKey = Keys.NumPad0;

        internal static string OldKeyString = "";

        internal static UIMenuCheckboxItem ToggleHandDamage;

        internal static UIMenuCheckboxItem ToggleKnockOuts;

        internal static UIMenuCheckboxItem ToggleConcussion;

        internal static UIMenuCheckboxItem ToggleMechanic;

        internal static UIMenuCheckboxItem ToggleAttachment;

        internal static UIMenuCheckboxItem ToggleBleeds;

        internal static UIMenuCheckboxItem ToggleDogs;

        internal static bool isHandDamageOn = true;

        internal static bool isKnockoutsOn = true;

        internal static bool isConcussionOn = true;

        internal static bool isMechanicOn = false;

        internal static bool isAttachmentOn = true;

        internal static bool isBleedingOn = true;

        internal static bool isDogsOn = true;

        internal static void ChangeKey(UIMenu sender, UIMenuItem selectedItem)
        {
            if (EntryPoint.ChangingKey && MenuKeyBeingChanged != null)
            {
                Keys keys = Game.GetKeyboardState().PressedKeys.FirstOrDefault();
                if (keys != default(Keys))
                {
                    Game.DisplayNotification("Key set to " + keys.ToString());

                    if (MenuKeyBeingChanged == DropKeyWeapon)
                    {
                        MenuKeyBeingChanged.Text = OldKeyString.Replace(WeaponDropKey.ToString(), keys.ToString());
                        WeaponDropKey = keys;
                    }
                    if (MenuKeyBeingChanged == GetKeyCar)
                    {
                        MenuKeyBeingChanged.Text = OldKeyString.Replace(GetCarKey.ToString(), keys.ToString());
                        GetCarKey = keys;
                    }
                    RealismMenuKeyBinds.RefreshIndex();
                    RealismMenuKeyBinds.Visible = true;
                    EntryPoint.ChangingKey = false;
                    MenuKeyBeingChanged = null;
                }
            }
            else if (MenuKeyBeingChanged == null && selectedItem != null)
            {
                MenuKeyBeingChanged = selectedItem;
                OldKeyString = MenuKeyBeingChanged.Text;
                RealismMenuKeyBinds.Visible = false;
                EntryPoint.ChangingKey = true;
                GameFiber.Sleep(500);
                Game.DisplayNotification("Press a keyboard button");
            }
        }

        private static void SaveCarMenuEvent(UIMenu sender, UIMenuItem selectedItem)
        {
            if (EntryPoint.PlayerPed.CurrentVehicle)
            {
                TaskSystem.SaveCar(EntryPoint.PlayerPed.CurrentVehicle);
                TaskSystem.SavedVehicle.SaveVehicle();
                if (!RealismMenuMain.MenuItems.Contains(FetchCar))
                {
                    RealismMenuMain.AddItem(FetchCar);
                }
                else
                {
                    FetchCar.Enabled = true;
                }
                RealismMenuMain.RefreshIndex();
                RealismMenuMain.Visible = false;
                Game.DisplayNotification("Registered your car with the mechanic successfully.");
            }
            else
            {
                Game.DisplayNotification("<b><span color='red'>Error: You are not in a vehicle!</span></b>");
            }
        }

        private static void GetCarMenuEvent(UIMenu sender, UIMenuItem selectedItem)
        {
            if (TaskSystem.SavedVehicle == null)
            {
                Game.DisplayNotification("Error: Vehicle couldn't be recovered!");
            }
            else if (TaskSystem.SavedVehicle.ActualVehicle && EntryPoint.PlayerPed.IsInVehicle(TaskSystem.SavedVehicle.ActualVehicle, atGetIn: false))
            {
                Game.DisplayNotification("Error: The mechanic can't bring you the vehicle as you are currently in it.");
            }
            else
            {
                TaskSystem.GetCar(TaskSystem.SavedVehicle);
            }
        }

        private static void OpenToggleMenu(UIMenu sender, UIMenuItem selectedItem)
        {
            RealismMenuMain.Visible = false;
            RealismMenuToggle.Visible = true;
        }

        private static void OpenMechanicMenu(UIMenu sender, UIMenuItem selectedItem)
        {
            RealismMenuMain.Visible = false;
            RealismMenuMechanic.Visible = true;
        }

        private static void OpenKeybindMenu(UIMenu sender, UIMenuItem selectedItem)
        {
            RealismMenuKeyBinds.Visible = true;
            RealismMenuMain.Visible = false;
        }

        internal static void UpdateMenu(UIMenuCheckboxItem sender, bool Checked)
        {
            SaveCar.Enabled = Checked;
            RealismMenuMain.RefreshIndex();
        }

        private static void UpdateToggle(UIMenuCheckboxItem sender, bool Checked)
        {
            if (sender == ToggleAttachment)
            {
                isAttachmentOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isAttachmentOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            else if (sender == ToggleConcussion)
            {
                isConcussionOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isConcussionOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            else if (sender == ToggleHandDamage)
            {
                isHandDamageOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isHandDamageOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            else if (sender == ToggleKnockOuts)
            {
                isKnockoutsOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isKnockoutsOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            else if (sender == ToggleBleeds)
            {
                isBleedingOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isBleedingOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            if (sender == ToggleMechanic)
            {
                isMechanicOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isMechanicOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            if (sender == ToggleDogs)
            {
                isDogsOn = Checked;
                if (Config.CurrentConfig != null)
                {
                    Config.CurrentConfig.isDogsOn = Checked;
                    Config.CurrentConfig.SaveConfig();
                }
            }
            RealismMenuMain.RefreshIndex();
        }

        internal static void MenuInit()
        {
            //Menus
            RealismMenuMain = new UIMenu("Realism Menu", "Script Version 1.02.15");
            RealismMenuMechanic = new UIMenu("Mechanic Menu", "Realism");
            RealismMenuKeyBinds = new UIMenu("Keybinds", "Realism");
            RealismMenuToggle = new UIMenu("Feature Toggle", "Realism");
            GarageMenu = new UIMenu("Garage", "Realism");

            //Main
            SaveCar = new UIMenuItem("Register Car", "Save the car you are currently in.");
            FetchCar = new UIMenuItem("Recall Last Car", "Have the mechanic bring you the last vehicle you requested.");

            //Keybind
            DropKeyWeapon = new UIMenuItem("Weapon Drop: " + WeaponDropKey.ToString());
            GetKeyCar = new UIMenuItem("Quick Call Mechanic: " + GetCarKey.ToString());

            //Toggles
            ToggleHandDamage = new UIMenuCheckboxItem("Hand Damage", isHandDamageOn, "Allows the dropping of weapons by anyone when hit in the hands.");
            ToggleKnockOuts = new UIMenuCheckboxItem("Player Knockout", isKnockoutsOn, "Allows the Player to stumble and fall when hit in the head.");
            ToggleConcussion = new UIMenuCheckboxItem("SoB's Concussion Script", isConcussionOn, "Modifies the Player based on the amount of damage it has taken. (ex. Hurt Animations)");
            ToggleAttachment = new UIMenuCheckboxItem("Attachment Control", isAttachmentOn, "Allows the Player pick up weapons as they were dropped. (Doesn't work with some other mods. Can break sniper scope mods)");
            ToggleMechanic = new UIMenuCheckboxItem("Mechanic", isMechanicOn, "Allows the Player to save a car, have the mechanic retrieve it, and gives the car a working trunk.");
            ToggleBleeds = new UIMenuCheckboxItem("Bleeding", isBleedingOn, "Gives the player a blood level and the ability to bleed.");
            ToggleDogs = new UIMenuCheckboxItem("Dogs", isDogsOn, "Allows the cops to bring dogs into the chase. Who let the dogs out?");

            //Main 2
            TogglesMenuSwitch = new UIMenuItem("Toggle Scripts", "Allows you to change the running scripts");
            KeybindMenuSwitch = new UIMenuItem("Change Keybinds", "Allows you to change the scripts keybinds");
            MechanicMenuSwitch = new UIMenuItem("Garages", "Allows you to store multiple cars");

            //Garage
            BuyGarage = new UIMenuItem("Buy a Garage", "You need one of these to store cars in...");

            //Events
            SaveCar.Activated += SaveCarMenuEvent;
            FetchCar.Activated += GetCarMenuEvent;

            ToggleMechanic.CheckboxEvent += UpdateMenu;
            ToggleMechanic.CheckboxEvent += UpdateToggle;
            ToggleKnockOuts.CheckboxEvent += UpdateToggle;
            ToggleHandDamage.CheckboxEvent += UpdateToggle;
            ToggleConcussion.CheckboxEvent += UpdateToggle;
            ToggleAttachment.CheckboxEvent += UpdateToggle;
            ToggleBleeds.CheckboxEvent += UpdateToggle;
            ToggleDogs.CheckboxEvent += UpdateToggle;

            DropKeyWeapon.Activated += ChangeKey;
            GetKeyCar.Activated += ChangeKey;

            BuyGarage.Activated += BuyGarageEvent;

            TogglesMenuSwitch.Activated += OpenToggleMenu;
            KeybindMenuSwitch.Activated += OpenKeybindMenu;
            MechanicMenuSwitch.Activated += OpenMechanicMenu;

            RealismMenuMain.AddItem(SaveCar);
            RealismMenuMain.AddItem(TogglesMenuSwitch);
            RealismMenuMain.AddItem(KeybindMenuSwitch);
            RealismMenuMain.AddItem(MechanicMenuSwitch);
            RealismMenuMain.RefreshIndex();

            RealismMenuKeyBinds.AddItem(DropKeyWeapon);
            RealismMenuKeyBinds.AddItem(GetKeyCar);
            RealismMenuKeyBinds.RefreshIndex();

            RealismMenuToggle.AddItem(ToggleBleeds);
            RealismMenuToggle.AddItem(ToggleDogs);
            RealismMenuToggle.AddItem(ToggleHandDamage);
            RealismMenuToggle.AddItem(ToggleKnockOuts);
            RealismMenuToggle.AddItem(ToggleConcussion);
            RealismMenuToggle.AddItem(ToggleAttachment);
            RealismMenuToggle.AddItem(ToggleMechanic);
            RealismMenuToggle.RefreshIndex();

            RealismMenuMechanic.AddItem(BuyGarage);
            RealismMenuMechanic.RefreshIndex();

            UpdateMenu(null, isMechanicOn);
        }

        private static void BuyGarageEvent(UIMenu sender, UIMenuItem selectedItem)
        {
            int num = Game.LocalPlayer.Character.GetPlayerMoney();
            if (num > 50000)
            {
                num -= 50000;
                Garage g = new Garage(Garage.Garages.Count);
                Garage.Garages.Add(g);
                UIMenuItem uIMenuItem = new UIMenuItem("Garage #" + g.ID, "Look inside your garage...");
                uIMenuItem.Activated += OpenGarage;
                RealismMenuMechanic.AddItem(uIMenuItem);
                RealismMenuMechanic.RefreshIndex();
                Garage.SaveGarages();
                Game.LocalPlayer.Character.SetPlayerMoney(num);
            }
            else
            {
                Game.DisplayNotification("Not enough money to finish this transaction!");
            }
        }

        internal static void GarageTest()
        {
            Garage g = new Garage(Garage.Garages.Count);
            Garage.Garages.Add(g);
            UIMenuItem uIMenuItem = new UIMenuItem("Garage #" + g.ID, "Look inside your garage...");
            uIMenuItem.Activated += OpenGarage;
            RealismMenuMechanic.AddItem(uIMenuItem);
            RealismMenuMechanic.RefreshIndex();
        }

        internal static void OpenGarage(UIMenu sender, UIMenuItem selectedItem)
        {
            GarageMenu.Clear();
            if (selectedItem == null)
            {

            }
            else
            {
                CurrentGarage = int.Parse(selectedItem.Text.Split("#"[0])[1].Trim());
            }
            RealismMenuMain.Visible = false;
            RealismMenuMechanic.Visible = false;
            Garage thisGarage = Garage.Garages.First((g) => g.ID == CurrentGarage);
            
            for(int i = 0; i < thisGarage.GarageInventory.Length; i++)
            {
                PersonalVehicle p = thisGarage.GarageInventory[i];
                if (p != null)
                {
                    UIMenuItem uIMenuItem = new UIMenuItem(p.ModelName + " #" + i, "A car! Hold shift to overwrite!");
                    uIMenuItem.Activated += GetCarFromGarage;
                    GarageMenu.AddItem(uIMenuItem);
                }
                else
                {
                    UIMenuItem uIMenuItem = new UIMenuItem("Register Car in slot #" + i, "An empty space!");
                    uIMenuItem.Activated +=  SaveCarToGarage; 
                    GarageMenu.AddItem(uIMenuItem);
                }
            }
            GarageMenu.RefreshIndex();
            GarageMenu.Visible = true;
        }

        
        private static void SaveCarToGarage(UIMenu sender, UIMenuItem selectedItem)
        {
            if (EntryPoint.PlayerPed.CurrentVehicle)
            {
                int Slot = int.Parse(selectedItem.Text.Split("#"[0])[1].Trim());
                Game.Console.Print("i " + Slot.ToString() + " Length is " + Garage.Garages.First((g) => g.ID == CurrentGarage).GarageInventory.Length);
                if (EntryPoint.PlayerPed.CurrentVehicle.IsValid())
                {
                    Garage.Garages.First((g) => g.ID == CurrentGarage).GarageInventory[Slot] = new PersonalVehicle(EntryPoint.PlayerPed.CurrentVehicle);
                }
                Garage.SaveGarages();
                OpenGarage(sender, null);
            }
            else
            {
                Game.DisplayNotification("You must be in a car to register it to the garage.");
            }
        }

        private static void GetCarFromGarage(UIMenu sender, UIMenuItem selectedItem)
        {
            if (Game.IsShiftKeyDownRightNow)
                SaveCarToGarage(sender, selectedItem);
            else
            {
                int Slot = int.Parse(selectedItem.Text.Split("#"[0])[1].Trim());
                if (Garage.Garages.First((g) => g.ID == CurrentGarage).GarageInventory[Slot] != null)
                    TaskSystem.GetCar(Garage.Garages.First((g) => g.ID == CurrentGarage).GarageInventory[Slot]);
            }
        }
    }
}