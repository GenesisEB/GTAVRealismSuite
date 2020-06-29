[assembly: Rage.Attributes.Plugin("Realism Script Suite", Description = "Designed specifically for the GTA 5 Realism Project", Author = "Genesis")]



namespace RealismTest
{
    using Rage;
    using Rage.Native;
    using RAGENativeUI;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal static class EntryPoint
    {
        internal static int LastHealth = 0;

        internal static bool HasBeenDamaged = false;

        internal static Ped PlayerPed = Game.LocalPlayer.Character;

        internal static Random r = new Random();

        internal static bool ChangingKey;

        internal static bool IsRampagePatchOn = false;

        internal static bool DrawMarker = false;

        internal static Vector3 MarkerVector;
        private static float LastBlood;

        private static void Main()
        {
            new KeyboardState();
            Game.ShowPoliceBlipsOnRadar = false;
            Game.Console.Print("Test Loaded");
            RealismMenu.MenuInit();
            
            Config.LoadConfig();
            TrunkLoadout.MenuInit();
            BleedSystem.BloodInit();
            GameFiber.WaitUntil(() => !Game.IsLoading);
            PersonalVehicle.LoadVehicle();
            Garage.LoadGarages();
            TaskSystem.TaskInit();
            Game.FrameRender += MenuRender;
            Game.RawFrameRender += Game_FrameRender;
            while (true)
            {
                UpdateStaticVariables();
                RegisterInputs();
                
                if (ChangingKey)
                {
                    RealismMenu.ChangeKey(null, null);
                }
                
                if (AttachmentSystem.EquippedWeapon && AttachmentSystem.EquippedWeaponDescriptor != null && PlayerPed && AttachmentSystem.LastWeapon != AttachmentSystem.EquippedWeapon && RealismMenu.isAttachmentOn)
                {
                    AttachmentSystem.EquipAttachments(PlayerPed);
                    AttachmentSystem.ProcessAttachments();
                    AttachmentSystem.EquippedWeapon.CanBeDamaged = true;
                }
                AttachmentSystem.LastWeapon = AttachmentSystem.EquippedWeapon;
                
                if (PlayerPed)
                {
                    if (HasBeenDamaged)
                    {
                        if (RealismMenu.isConcussionOn)
                        {
                            
                            SoBConcussion.ProcessConcusions(PlayerPed);
                        }
                        BleedSystem.ProcessDamage(PlayerPed);
                        WeaponDrop.ProcessHandDamageNearby(PlayerPed);
                        PlayerPed.ClearLastDamageBone();
                        
                    }
                    if (RealismMenu.isBleedingOn)
                    {
                        BleedSystem.ProcessBleeds(PlayerPed);
                        BleedSystem.ProcessHealing(PlayerPed);
                        
                    }
                    if(RealismMenu.isDogsOn)
                        CheckDogs();
                }
                
                TrunkLoadout.PersonalVehicleCheck();
                
                UpdateUI();
                
                if (PlayerPed && PlayerPed.CurrentVehicle)
                {
                    float fps = 1f / Game.FrameTime;
                    if (fps == 0f) fps = 1;
                    float downforcePercent = PlayerPed.CurrentVehicle.Speed / 240;
                    PlayerPed.CurrentVehicle.ApplyForce(new Vector3(0f, 0f, -(downforcePercent / fps)), new Vector3(0f, 0f, 0f), isForceRelative: true, isOffsetRelative: true);
                    

                }
                //CheckCollision();
                RampagePatch();
                GameFiber.Yield();
            }
        }
        
        internal static void RampagePatch()
        {
            
            bool ramptrigger = false;
            foreach(string script in Game.GetRunningScripts())
            {
                if (script.Contains("rampage"))
                {
                    ramptrigger = true;
                }
            }
            if (ramptrigger && RealismMenu.isAttachmentOn)
            {
                RealismMenu.isAttachmentOn = false;
                IsRampagePatchOn = true;
            }
            else
            {
                if (IsRampagePatchOn)
                {
                    RealismMenu.isAttachmentOn = true;
                    IsRampagePatchOn = false;
                }
            }
        }

        private static void UpdateUI()
        {
            BleedSystem.BloodBarWidth = 128f * (BleedSystem.CurrentBlood / 2000f);

        }

        private static void RegisterInputs()
        {
            
            RealismMenu.RealismMenuMain.ProcessControl();
            RealismMenu.RealismMenuKeyBinds.ProcessControl();
            RealismMenu.RealismMenuToggle.ProcessControl();
            RealismMenu.RealismMenuMechanic.ProcessControl();
            RealismMenu.GarageMenu.ProcessControl();
            TrunkLoadout.TrunkMenu.ProcessControl();
            TrunkLoadout.TrunkRestockMenu.ProcessControl();
            if (Game.IsKeyDown(RealismMenu.WeaponDropKey) && !(AttachmentSystem.EquippedWeapon == null))
            {
                AttachmentSystem.ProcessAttachments();
                AttachmentSystem.EquippedWeaponDescriptor.DropToGround();
                Game.DisplayNotification("Dropped weapon!");
            }
            if (Game.IsKeyDown(RealismMenu.GetCarKey) && RealismMenu.isMechanicOn)
                TaskSystem.GetCar(TaskSystem.SavedVehicle);
            Game.IsKeyDown(Keys.L);
            if (Game.IsKeyDown(RealismMenu.GetMenuKey))
            {
                RealismMenu.RealismMenuMain.Visible = !RealismMenu.RealismMenuMain.Visible;
                RealismMenu.RealismMenuKeyBinds.Visible = false;
                RealismMenu.RealismMenuToggle.Visible = false;
                RealismMenu.RealismMenuMechanic.Visible = false;
                RealismMenu.GarageMenu.Visible = false;
                TrunkLoadout.TrunkMenu.Visible = false;
                TrunkLoadout.TrunkRestockMenu.Visible = false;
            }
            
        }

        private static void CheckCollision()
        {
            Vehicle ActualVehicle = TaskSystem.SavedVehicle.ActualVehicle;
            if (TaskSystem.SavedVehicle.ActualVehicle)
            {
                ActualVehicle.GetLastCollision(out Vector3 pos, out Vector3 norm, out string mat);
                ActualVehicle.IsDeformationEnabled = true;
                if (pos != default(Vector3) && pos != null)
                    ActualVehicle.Deform(ActualVehicle.GetPositionOffset(pos), 10, 10);
            }
        }

        private static void UpdateStaticVariables()
        {
            if (Game.LocalPlayer.Character)
            {
                PlayerPed = Game.LocalPlayer.Character;
            }
            if (PlayerPed.Inventory.EquippedWeaponObject)
            {
                AttachmentSystem.EquippedWeapon = PlayerPed.Inventory.EquippedWeaponObject;
            }
            if (PlayerPed.Inventory.EquippedWeapon != null)
            {
                AttachmentSystem.EquippedWeaponDescriptor = PlayerPed.Inventory.EquippedWeapon;
            }
            Game.ShowPoliceBlipsOnRadar = false;
            UpdateHealth();
        }

        private static void UpdateHealth()
        {
            if (PlayerPed.Health != LastHealth)
            {
                HasBeenDamaged = true;
            }
            if (PlayerPed.Health > LastHealth + 10)
            {
                BleedSystem.CurrentBlood += 1000f;
                BleedSystem.CurrentBlood.Clamp(0f, 2000f);
                BleedSystem.Bleeds.Clear();
                HasBeenDamaged = true;
            }
            if (BleedSystem.CurrentBlood != LastBlood)
            {
                LastBlood = BleedSystem.CurrentBlood;
                HasBeenDamaged = true;
            }
            LastHealth = PlayerPed.Health;
        }

        private static void CheckDogs()
        {
            if (!PlayerPed || Game.LocalPlayer == null || Game.LocalPlayer.WantedLevel < 1)
            {
                return;
            }
            Ped[] nearbyPeds = PlayerPed.GetNearbyPeds(16);
            foreach (Ped ped in nearbyPeds)
            {
                if (!ped)
                {
                    break;
                }
                if (ped.CurrentVehicle && ped.CurrentVehicle.IsPoliceVehicle && ped.CurrentVehicle.Speed < 1f && ped.CurrentVehicle.Speed > -1f)
                {
                    TaskSystem.CopGetDog(ped, ped.CurrentVehicle, inCar: true);
                }
                else if (ped.LastVehicle && ped.LastVehicle.IsPoliceVehicle && !ped.IsInAnyVehicle(atGetIn: false))
                {
                    TaskSystem.CopGetDog(ped, ped.LastVehicle, inCar: false);
                }
            }
        }

        private static void PlayWhistle()
        {
            if (!PlayerPed || !PlayerPed.IsInAnyVehicle(atGetIn: false))
            {
                return;
            }
            VehicleDoor[] doors = PlayerPed.CurrentVehicle.GetDoors();
            foreach (VehicleDoor vehicleDoor in doors)
            {
                if (vehicleDoor.IsValid() && !vehicleDoor.IsDamaged && vehicleDoor.IsOpen)
                {
                    
                }
            }
        }

        private static void MenuRender(object sender, GraphicsEventArgs e)
        {
            RealismMenu.RealismMenuMain.Draw();
            RealismMenu.RealismMenuToggle.Draw();
            RealismMenu.RealismMenuKeyBinds.Draw();
            RealismMenu.RealismMenuMechanic.Draw();
            RealismMenu.GarageMenu.Draw();
            TrunkLoadout.TrunkMenu.Draw();
            TrunkLoadout.TrunkRestockMenu.Draw();
        }

        private static void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            if (RealismMenu.isBleedingOn)
            {
                if (BleedSystem.BloodBarTexture != null)
                {
                    e.Graphics.DrawTexture(BleedSystem.BloodBarTexture, 2f + BleedSystem.DrawOffset + 5f, BleedSystem.DrawAnchor.Y, 128f, 32f);
                }
                if (BleedSystem.BloodTexture != null)
                {
                    e.Graphics.DrawTexture(BleedSystem.BloodTexture, 2f + BleedSystem.DrawOffset + 5f, BleedSystem.DrawAnchor.Y, BleedSystem.BloodBarWidth, 32f);
                }
                e.Graphics.DrawText("Blood", "Chalet", 20f, BleedSystem.DrawAnchor, Color.White);
                e.Graphics.DrawText(BleedSystem.CurrentBlood.ToString("N0") + "ml", "Chalet", 20f, BleedSystem.DrawAnchor2, Color.White);
                e.Graphics.DrawText(BleedSystem.Bleeds.Count + " Bleeding Wounds", "Chalet", 20f, BleedSystem.DrawAnchor3, Color.White);
            }
            e.Graphics.DrawText("This version is for testing!", "Chalet", 20f, new PointF(0f, 0f), Color.Red);
        }
    }
}