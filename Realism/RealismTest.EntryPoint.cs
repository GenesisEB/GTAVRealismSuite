[assembly: Rage.Attributes.Plugin("Realism Script Suite", Description = "Designed specifically for the GTA 5 Realism Project", Author = "Genesis")]



namespace RealismTest
{
    using Rage;
    using Rage.Native;
    using RAGENativeUI;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
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

        //Engine Controller
        internal static Vehicle PreviousVehicle;
        internal static Vehicle PlayerVehicle;
        internal static bool NewVehicle = true;
        internal static float OriginalDriveForce = 0;
        internal static float OriginalTopSpeed = 0;
        //End Engine Controller

        internal static Vector3 MarkerVector;
        private static float LastBlood;
        internal static List<Vector3> DebugPoints = new List<Vector3>();
        internal static bool DebugMode = false;
        internal static uint Delay;

        private static void Main()
        {
            new KeyboardState();
            
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
            //Game.RawFrameRender += Game_FrameRender;
            while (true)
            {
                if (!Game.LocalPlayer.HasControl)
                    goto Yield;
                UpdateStaticVariables();
                RegisterInputs();

                if(RealismMenu.isPBlipsOn)
                    Game.ShowPoliceBlipsOnRadar = false;
                else
                    Game.ShowPoliceBlipsOnRadar = true;

                if (ChangingKey)
                {
                    RealismMenu.ChangeKey(null, null);
                }
                
                if (AttachmentSystem.EquippedWeapon && AttachmentSystem.EquippedWeaponDescriptor != null && PlayerPed 
                    && AttachmentSystem.LastWeaponDescriptor != null 
                    && AttachmentSystem.LastWeaponDescriptor != AttachmentSystem.EquippedWeaponDescriptor && RealismMenu.isAttachmentOn)
                {
                    AttachmentSystem.EquipAttachments(PlayerPed);
                    AttachmentSystem.ProcessAttachments();
                    AttachmentSystem.EquippedWeapon.CanBeDamaged = true;
                }
                AttachmentSystem.LastWeapon = AttachmentSystem.EquippedWeapon;
                AttachmentSystem.LastWeaponDescriptor = AttachmentSystem.EquippedWeaponDescriptor;
                
                if (PlayerPed)
                {
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.Oemplus))
                    {
                        DebugMode = !DebugMode;
                        Game.DisplayNotification("Debug Mode: " + DebugMode.ToString());
                    }
#if DEBUG
                    if (DebugMode && Game.IsControlJustPressed(1, GameControl.Jump))
                    {
                        
                    }
                    if(DebugMode && Game.IsKeyDown(System.Windows.Forms.Keys.S) && Game.IsControlKeyDownRightNow)
                    {
                        SaveDebug();
                    }
#endif
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
                    if(!(PlayerPed.CurrentVehicle.HeightAboveGround > 0.1f))
                        PlayerPed.CurrentVehicle.ApplyForce(new Vector3(0f, 0f, -(downforcePercent / fps)), new Vector3(0f, 0f, 0f), isForceRelative: true, isOffsetRelative: true);
                    if(RealismMenu.isECOn)
                        EngineController(PlayerPed.CurrentVehicle);

                }
                //CheckCollision();
                RampagePatch();
                Yield:
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
#if DEBUG
            if (DebugMode && Game.IsKeyDown(Keys.OemMinus))
            {

                DebugAddPoint(PlayerPed);
                Game.DisplayNotification("Point Saved");
                
            }
#endif
            
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
                if (ped.CurrentVehicle && ped.CurrentVehicle.IsPoliceVehicle 
                    && ped.CurrentVehicle.Speed < 1f && ped.CurrentVehicle.Speed > -1f && !ped.IsPassenger)
                {
                    TaskSystem.CopGetDog(ped, ped.CurrentVehicle, inCar: true);
                }
                else if (ped.LastVehicle && ped.LastVehicle.IsPoliceVehicle && !ped.IsInAnyVehicle(atGetIn: false))
                {
                    TaskSystem.CopGetDog(ped, ped.LastVehicle, inCar: false);
                }
            }
        }

        internal static void EngineController(Vehicle v)
        {
            
            if (NewVehicle)
            {
                if (PreviousVehicle && PreviousVehicle != PlayerVehicle)
                {
                    PreviousVehicle.DriveForce = OriginalDriveForce;
                    PreviousVehicle.TopSpeed = OriginalTopSpeed;
                }
                if (v.DriveForce == 0 || v.TopSpeed == 0)
                    return;
                OriginalDriveForce = v.HandlingData.InitialDriveForce;
                OriginalTopSpeed = v.HandlingData.TopSpeed * 0.6f;
                switch (v.Class)
                {
                    case VehicleClass.Compact:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Sedan:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.SUV:
                        v.TopSpeed = 0.7f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Coupe:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Muscle:
                        v.TopSpeed = 1.1f * OriginalTopSpeed;
                        break;
                    case VehicleClass.SportClassic:
                        v.TopSpeed = 1.2f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Sport:
                        v.TopSpeed = 1.4f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Super:
                        v.TopSpeed = 1.7f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Motorcycle:
                        v.TopSpeed = 1.5f * OriginalTopSpeed;
                        break;
                    case VehicleClass.OffRoad:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Industrial:
                        v.TopSpeed = 0.7f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Utility:
                        v.TopSpeed = 0.8f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Van:
                        v.TopSpeed = 0.75f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Cycle:
                        v.TopSpeed = 0.05f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Boat:
                        break;
                    case VehicleClass.Helicopter:
                        break;
                    case VehicleClass.Plane:
                        break;
                    case VehicleClass.Service:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Emergency:
                        v.TopSpeed = 1.5f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Military:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Commercial:
                        v.TopSpeed = 0.9f * OriginalTopSpeed;
                        break;
                    case VehicleClass.Rail:
                        break;
                    default:
                        break;
                }
                NewVehicle = false;
            }
            else if (v)
            {
                

                if (v && v.CurrentGear != 0)
                {
                    float driveForceCalc = (OriginalDriveForce * (float)v.NumberOfGears / 1.5f / ((float)v.NumberOfGears - (float)v.CurrentGear + 0.5f)) * ((v.EngineRevolutionsRatio + 0.0001f) / 2f);
                    if (v.Speed > v.TopSpeed)
                    {
                        float DFReduction = v.Speed - v.TopSpeed;
                        DFReduction /= 100f;
                        driveForceCalc -= DFReduction * 10f;
                    }
                    if (v.CurrentGear == v.NumberOfGears)
                        driveForceCalc /= 2.3f;

                    float Modifier = 2f;
                    if (Config.CurrentConfig.isKMOn)
                        Modifier = 1f;

                    if (v && v.CurrentGear < 2)
                        v.DriveForce = OriginalDriveForce * Modifier;
                    else if (driveForceCalc > 0 && v)
                        v.DriveForce = driveForceCalc;

                    if (v?.DriveForce < OriginalDriveForce / 2f)
                        v.DriveForce = OriginalDriveForce / 2f;
                }
                if (v.IsPlane)
                    v.DriveForce = 1;
                //if (v?.TopSpeed < 100f)
                //    v.TopSpeed = 100f;
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
            Game_FrameRender(sender, e);
        }

        private static void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            if (!Game.LocalPlayer.HasControl)
                return;
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
        #if DEBUG
            Vehicle v = null;
            if(PlayerPed.CurrentVehicle)
                v = PlayerPed.CurrentVehicle;
            if (v != null)
            {
                e.Graphics.DrawText("Current Car: " + v?.Model.Name, "Arial", 20, new PointF(0f, 30f), Color.White);
                e.Graphics.DrawText("Drive Force: " + v?.DriveForce, "Arial", 20, new PointF(0f, 60f), Color.White);
                e.Graphics.DrawText("Speed: " + (v.Speed * 2.23694).ToString("N2") + "mph / " + (v.TopSpeed * 2.23694).ToString("N2") + "mph", "Arial", 20, new PointF(0f, 90f), Color.White); ;
                e.Graphics.DrawText("Gear: " + v.CurrentGear + " / "+ v.NumberOfGears, "Arial", 20, new PointF(0f, 120f), Color.White);
                

            }
        #endif
        }

#if DEBUG
        //Extra
        static internal void DebugAddPoint(Ped player)
        {
            DebugPoints.Add(player.Position);
        }
        static internal void SaveDebug()
        {

                StreamWriter streamWriter = new StreamWriter(".\\Plugins\\Realism\\debug.points", append: false);
                foreach (Vector3 v in DebugPoints)
                {
                    streamWriter.WriteLine(v.ToString());
                }
                streamWriter.Close();
                Game.DisplayNotification("Points Saved!");
            
        }
#endif
    }
}