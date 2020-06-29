// RealismTest.PersonalVehicle
using Newtonsoft.Json;
using Rage;
using Rage.Attributes;
using RAGENativeUI.Elements;
using RealismTest;
using RealismTest.OtherExtensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace RealismTest
{
    [JsonObject]
    internal class Garage
    {
        [JsonIgnore]
        internal static List<Garage> Garages = new List<Garage>();

        [JsonProperty]
        internal int ID = 0;
        
        [JsonProperty]
        internal PersonalVehicle[] GarageInventory = new PersonalVehicle[5];

        public Garage(int id)
        {
            ID = id;
        }

        [JsonConstructor]
        public Garage(){}

        internal static void SaveGarages()
        {
            StreamWriter streamWriter = new StreamWriter(".\\Plugins\\Realism\\PersonalGarages.real", append: false);
            foreach (Garage g in Garages)
            {
                streamWriter.WriteLine(JsonConvert.SerializeObject(g));
            }
            streamWriter.Close();
        }
        internal static void LoadGarages()
        {
            if (File.Exists(".\\Plugins\\Realism\\PersonalGarages.real"))
            {
                StreamReader streamReader = new StreamReader(".\\Plugins\\Realism\\PersonalGarages.real");
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        Garage g = JsonConvert.DeserializeObject<Garage>(line);
                        Garages.Add(g);
                        UIMenuItem uIMenuItem = new UIMenuItem("Garage #" + g.ID, "Look inside your garage...");
                        uIMenuItem.Activated += RealismMenu.OpenGarage;
                        RealismMenu.RealismMenuMechanic.AddItem(uIMenuItem);
                    }
                    catch(Exception e)
                    {
                        Game.Console.Print(e.Message);
                    }
                    
                }
                RealismMenu.RealismMenuMechanic.RefreshIndex();
                Garage First;
                PersonalVehicle pv;
                if ((First = Garages.FirstOrDefault()) != null && First.GarageInventory.Length != 0 && (pv = First.GarageInventory[0]) != null && pv.ModelHash != 0)
                {
                    TaskSystem.SavedVehicle = pv;
                }
                if (!RealismMenu.RealismMenuMain.MenuItems.Contains(RealismMenu.FetchCar))
                {
                    RealismMenu.RealismMenuMain.AddItem(RealismMenu.FetchCar);
                }
                RealismMenu.RealismMenuMain.RefreshIndex();
                streamReader.Close();
            }
        }
    }

    [JsonObject]
    internal class PersonalVehicle
    {
        [JsonIgnore]
        internal Vehicle ActualVehicle;

        [JsonProperty]
        internal float DirtLevel;

        [JsonProperty]
        internal float EngineHealth;

        [JsonProperty]
        internal float FuelLevel;

        [JsonProperty]
        internal string LicensePlate;

        [JsonIgnore]
        internal LicensePlateStyle LicensePlateStyle;

        [JsonProperty]
        internal uint ModelHash;

        [JsonProperty]
        internal string ModelName;

        [JsonProperty]
        internal Color PrimaryColor;

        [JsonProperty]
        internal Color SecondaryColor;

        [JsonProperty]
        internal ModHelper Mods;

        [JsonConstructor]
        internal PersonalVehicle(){}

        internal PersonalVehicle(Vehicle v)
        {
            DirtLevel = v.DirtLevel;
            EngineHealth = v.EngineHealth;
            FuelLevel = v.FuelLevel;
            LicensePlate = v.LicensePlate;
            LicensePlateStyle = v.LicensePlateStyle;
            ModelHash = v.Model.Hash;
            ModelName = v.Model.Name;
            PrimaryColor = v.PrimaryColor;
            SecondaryColor = v.SecondaryColor;
            Mods = new ModHelper(v);
            ActualVehicle = v;
        }

        internal void SpawnReplacementCar(Vector3 position, float heading)
        {
            Vehicle vehicle = new Vehicle(ModelHash, position, heading)
            {
                DirtLevel = DirtLevel,
                EngineHealth = EngineHealth,
                FuelLevel = FuelLevel,
                LicensePlate = LicensePlate,
                LicensePlateStyle = LicensePlateStyle,
                PrimaryColor = PrimaryColor,
                RadioStation = RadioStation.Off,
                SecondaryColor = SecondaryColor
            };
            GameFiber.WaitUntil(vehicle.IsValid, 1000);
            vehicle.Mods.InstallModKit();
            Mods.ReplaceMods(vehicle);
            if (ActualVehicle != null && ActualVehicle.IsValid())
            {
                ActualVehicle.Dismiss();
                ActualVehicle.Delete();
            }
            ActualVehicle = vehicle;
        }

        internal void SaveVehicle()
        {
            string text = "";
            text = JsonConvert.SerializeObject(this);
            StreamWriter streamWriter = new StreamWriter(".\\Plugins\\Realism\\personal.real", append: false);
            streamWriter.WriteLine(text);
            streamWriter.Close();
        }

        internal static void LoadVehicle()
        {
            if (File.Exists(".\\Plugins\\Realism\\personal.real"))
            {
                StreamReader streamReader = new StreamReader(".\\Plugins\\Realism\\personal.real");
                PersonalVehicle personalVehicle;
                try
                { 
                    personalVehicle = JsonConvert.DeserializeObject<PersonalVehicle>(streamReader.ReadLine());
                
                    if (personalVehicle.ModelHash != 0)
                    {
                        TaskSystem.SavedVehicle = personalVehicle;
                    }
                    if (!RealismMenu.RealismMenuMain.MenuItems.Contains(RealismMenu.FetchCar))
                    {
                        RealismMenu.RealismMenuMain.AddItem(RealismMenu.FetchCar);
                    }
                    RealismMenu.RealismMenuMain.RefreshIndex();
                }
                catch (Exception e)
                {
                    Game.Console.Print(e.Message);
                }
                streamReader.Close();
            }
        }

        internal void Secret(string args)
        {
            if (ActualVehicle)
            {
                
                Game.Console.Print("Input: " + args.ToString());
                HandlingData handlingData = ActualVehicle.HandlingData;
                handlingData.TopSpeed = 1000f;
                handlingData.InitialDriveGears = 300;
                handlingData.DamageFlags = uint.Parse(args);
                



                Game.Console.Print("Ran all natives successfully....");
            }

        }

    }
}