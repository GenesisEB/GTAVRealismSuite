// RealismTest.TaskSystem
using Rage;
using Rage.Native;
using RealismTest;
using System.Collections.Generic;

namespace RealismTest
{

    internal class TaskSystem
    {
        internal static PersonalVehicle SavedVehicle;

        private static HashSet<Ped> CopsToIgnore;

        private static uint LastDog;

        internal static void TaskInit()
        {
            CopsToIgnore = new HashSet<Ped>();
        }

        public static void CopGetDog(Ped cop, Vehicle vehicle, bool inCar)
        {
            if (cop && vehicle && !CopsToIgnore.Contains(cop) && Game.GameTime > LastDog + WantedTime(Game.LocalPlayer.WantedLevel))
            {
                if (vehicle.IsHelicopter)
                {
                    CopsToIgnore.Add(cop);
                    return;
                }
                CopsToIgnore.Add(cop);
                new GameFiber(delegate
                {
                    CopDogThread(cop, vehicle, inCar);
                }).Start();
                LastDog = Game.GameTime;
            }
        }

        public static void GetCar(PersonalVehicle pv)
        {
            if (pv != null)
            {
                new GameFiber(delegate
                {
                    GetCarThread(pv);
                }).Start();
            }
        }

        public static void CopDogThread(Ped cop, Vehicle vehicle, bool inCar)
        {
            Game.Console.Print("Beginning Cop Thread...");
            vehicle.MakePersistent();
            cop.KeepTasks = true;
            cop.BlockPermanentEvents = true;
            Task task;
            if (inCar)
            {
                task = cop.Tasks.LeaveVehicle(vehicle, LeaveVehicleFlags.None);
                task.WaitForCompletion();
            }
            if (!cop || !vehicle || !vehicle.IsCar)
            {
                return;
            }
            task = cop.Tasks.GoToOffsetFromEntity(vehicle, 0f - (vehicle.Length + 0.05f) / 2f, 0f, 10f);
            if (task == null)
            {
                return;
            }
            task.WaitForCompletion();
            if (!cop || !vehicle)
            {
                return;
            }
            Game.Console.Print("Playing anim...");
            if (vehicle.Doors[5].IsValid())
            {
                vehicle.Doors[5].Open(instantly: false);
            }
            task = cop?.Tasks.PlayAnimation("misschop_vehicleenter_exit", "low_ds_open_door_for_chop", 0f, AnimationFlags.None);
            if (!cop || !vehicle)
            {
                return;
            }
            Game.Console.Print("Spawning Doggo...");
            GameFiber.Wait(100);
            Ped doggo = new Ped("a_c_shepherd", cop.GetOffsetPositionRight(1.2f), 0f);
            if (task == null)
            {
                return;
            }
            task.WaitForCompletion();
            if (!doggo)
            {
                Game.DisplayNotification("Couldn't Spawn a doggo. Much Sad. Wow.");
                Game.Console.Print("Couldn't Spawn a doggo. Much Sad. Wow.");
                Game.Console.Print("Cop thread dead...");
                return;
            }
            new GameFiber(delegate
            {
                DoggoDelete(doggo);
            }).Start();
            
            vehicle.IsPersistent = false;
            Game.Console.Print("Doggo spawned!");
            doggo.KeepTasks = true;
            doggo.BlockPermanentEvents = true;
            Game.Console.Print("Setting relations...");
            doggo.RelationshipGroup = RelationshipGroup.Cop;
            Game.Console.Print("Checking group...");
            
            cop.BlockPermanentEvents = false;
            doggo.CanAttackFriendlies = false;
            doggo.CanOnlyBeDamagedByPlayer = true;
            vehicle.IsPersistent = false;
            Game.Console.Print("Assigning tasks...");
            if (!doggo)
            {
                return;
            }
            Task temptask = doggo.Tasks.FollowNavigationMeshToPosition(EntryPoint.PlayerPed.Position, EntryPoint.PlayerPed.Heading, 15f);
            if (temptask == null)
            {
                if (EntryPoint.PlayerPed)
                {
                    doggo.Tasks.FightAgainst(EntryPoint.PlayerPed);
                }
                return;
            }
            temptask.WaitForStatus(TaskStatus.InProgress);
            GameFiber.Wait(100);
            GameFiber.WaitUntil(() => temptask == null || !cop || !doggo || !EntryPoint.PlayerPed || !temptask.IsActive || doggo.DistanceTo(EntryPoint.PlayerPed.Position) < 5f);
            if (cop && doggo && EntryPoint.PlayerPed)
            {
                doggo.Tasks.FightAgainst(EntryPoint.PlayerPed);
                cop.Tasks.FightAgainst(EntryPoint.PlayerPed);
                Game.Console.Print("Cop thread finished!");
            }
        }

        public static void DoggoDelete(Ped doggo)
        {
            GameFiber.WaitWhile(() => doggo && doggo.IsAlive && EntryPoint.PlayerPed && EntryPoint.PlayerPed.IsAlive && Game.LocalPlayer.WantedLevel > 1);
            GameFiber.Wait(5000);
            GameFiber.WaitWhile(() => doggo && doggo.IsOnScreen);
            doggo?.Delete();
        }

        public static void SaveCar(Vehicle v)
        {
            if (v)
            {
                SavedVehicle = new PersonalVehicle(v);
            }
        }

        public static void GetCarThread(PersonalVehicle pv)
        {
            Ped player = Game.LocalPlayer.Character;
            SavedVehicle = pv;
            if (SavedVehicle == null || !player)
            {
                return;
            }
            Vector3 zero = Vector3.Zero;
            float heading = 0f;
            bool flag = true;
            int num = 0;
            while (flag || zero.DistanceTo(player) > 200f)
            {
                if (flag)
                {
                    flag = false;
                }
                NativeFunction.Natives.GET_RANDOM_VEHICLE_NODE(player.Position, 400f, true, true, true, out zero, out heading);
                if (SavedVehicle.ActualVehicle == null || !SavedVehicle.ActualVehicle.IsValid() || !SavedVehicle.ActualVehicle.IsDriveable)
                {
                    SavedVehicle.SpawnReplacementCar(zero, heading);
                    GameFiber.WaitUntil(SavedVehicle.ActualVehicle.IsValid);
                    if (SavedVehicle.ActualVehicle.IsPlane)
                        zero.Z += 200;
                    SavedVehicle.ActualVehicle.Heading = heading;
                    SavedVehicle.ActualVehicle.Position = zero;
                }
                else
                {
                    if (SavedVehicle.ActualVehicle.IsPlane)
                        zero.Z += 200;
                    SavedVehicle.ActualVehicle.Heading = heading;
                    SavedVehicle.ActualVehicle.Position = zero;
                }
                num++;
                if (num > 200)
                {
                    Game.DisplayNotification("Your mechanic can't deliver your car here.");
                    return;
                }
            } // Find Spawn Point
            Blip progressIndicator = SavedVehicle.ActualVehicle.AttachBlip(); // Add blip to car
            new GameFiber(delegate
            {
                PointDelete(progressIndicator);
            }).Start(); // Make new blip thread, remove blip when car delivered or time out.
            progressIndicator.Sprite = BlipSprite.GetawayCar; // Change blip to car
            NativeFunction.Natives.GET_RANDOM_VEHICLE_NODE(player.Position, 10f, true, true, true, out Vector3 vector, out heading); // Get point nearest player.
            Ped Driver = SavedVehicle.ActualVehicle.CreateRandomDriver(); // Create Driver
            if (Driver == null)
            {
                return;
            } //Stop if creating driver failed
            Driver.KeepTasks = true; // Disable Other tasks (fleeing, etc.)
            Driver.BlockPermanentEvents = true; // Disable interrupting current task
            Driver.MakePersistent(); // Disable driver despawn
            SavedVehicle.ActualVehicle.MakePersistent(); // Disable vehicle despawn
            SavedVehicle.ActualVehicle.SetLockedForPlayer(Game.LocalPlayer, locked: false); // Unlock car for player
            Vector3 offsetPositionFront = player.GetOffsetPositionFront(2f); // Get player position offset so mechanic doesn't hit player.
            Task DriverTask = Driver.Tasks.ParkVehicle(offsetPositionFront, 360f - player.Heading); // Tell driver to park vehicle in front of player.
            RealismMenu.RealismMenuMain.Visible = false; // Hide menu
            EntryPoint.DrawMarker = true; // No longer works
            EntryPoint.MarkerVector = offsetPositionFront; // No longer works
            Game.DisplayNotification("Your mechanic is on its way! Please wait."); // Tell player the mechanic is on its way.
            GameFiber.WaitUntil(() => !DriverTask.IsActive || SavedVehicle.ActualVehicle.DistanceTo(EntryPoint.PlayerPed) < 5f, 300000); // Begin drive task to near player.
            if (!SavedVehicle.ActualVehicle || SavedVehicle.ActualVehicle.DistanceTo(EntryPoint.PlayerPed) > 5f)
            {
                EntryPoint.DrawMarker = false;
                EntryPoint.MarkerVector = Vector3.Zero;
                Driver.Dismiss();
                Driver.Delete();
                if(progressIndicator)
                    progressIndicator.Delete();
                return;
            } // If vehicle is close to player end driver task.
            EntryPoint.DrawMarker = false; // Remove marker
            EntryPoint.MarkerVector = Vector3.Zero; // No longer works
            DriverTask = Driver.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen); // Leave vehicle if needed still
            DriverTask.WaitForStatus(TaskStatus.None); // Wait for driver to finish all tasks
            Game.DisplaySubtitle("Here's your vehicle."); // Obvious
            DriverTask = Driver.Tasks.Wander(); // Tell driver to wander away from vehicle
            if (progressIndicator)
            {
                progressIndicator.Flash(1000, 15000);
            } // If the vehicle blip still exists, flash it.
            GameFiber.Wait(15000); // Wait 15 seconds
            if (progressIndicator)
            {
                progressIndicator.Delete();
            } // Stop flashing vehicle blip and delete it.
            GameFiber.WaitWhile(() => Driver.IsOnScreen); // Wait until the driver is no longer on screen
            if (Driver)
            {
                Driver.Dismiss();
                Driver.Delete();
            } // If the driver still exists and is off screen, despawn and delete the reference.
        }

        public static void PointDelete(Blip blip)
        {
            GameFiber.Wait(120000);
            if (blip)
            {
                blip.Delete();
            }
        }

        public static uint WantedTime(int wantedLevel)
        {
            switch (wantedLevel)
            {
                case 1:
                case 2:
                    return 120000u;
                case 3:
                    return 90000u;
                case 4:
                    return 60000u;
                case 5:
                    return 45000u;
                case 6:
                    return 20000u;
                default:
                    return 120000u;
            }
        }
    }
}