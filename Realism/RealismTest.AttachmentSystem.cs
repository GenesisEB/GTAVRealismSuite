// RealismTest.AttachmentSystem
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

namespace RealismTest
{

    internal class AttachmentSystem
    {
        internal static Weapon EquippedWeapon;

        internal static Weapon LastWeapon;

        internal static WeaponDescriptor EquippedWeaponDescriptor;
        internal static WeaponDescriptor LastWeaponDescriptor;

        internal static string[] WeaponComponentNames = new string[520]
        {
        "COMPONENT_KNUCKLE_VARMOD_BASE",
        "COMPONENT_KNUCKLE_VARMOD_PIMP",
        "COMPONENT_KNUCKLE_VARMOD_BALLAS",
        "COMPONENT_KNUCKLE_VARMOD_DOLLAR",
        "COMPONENT_KNUCKLE_VARMOD_DIAMOND",
        "COMPONENT_KNUCKLE_VARMOD_HATE",
        "COMPONENT_KNUCKLE_VARMOD_LOVE",
        "COMPONENT_KNUCKLE_VARMOD_PLAYER",
        "COMPONENT_KNUCKLE_VARMOD_KING",
        "COMPONENT_KNUCKLE_VARMOD_VAGOS",
        "COMPONENT_SWITCHBLADE_VARMOD_BASE",
        "COMPONENT_SWITCHBLADE_VARMOD_VAR1",
        "COMPONENT_SWITCHBLADE_VARMOD_VAR2",
        "COMPONENT_PISTOL_CLIP_01",
        "COMPONENT_PISTOL_CLIP_02",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_PI_SUPP_02",
        "COMPONENT_PISTOL_VARMOD_LUXE",
        "COMPONENT_COMBATPISTOL_CLIP_01",
        "COMPONENT_COMBATPISTOL_CLIP_02",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER",
        "COMPONENT_APPISTOL_CLIP_01",
        "COMPONENT_APPISTOL_CLIP_02",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_APPISTOL_VARMOD_LUXE",
        "COMPONENT_PISTOL50_CLIP_01",
        "COMPONENT_PISTOL50_CLIP_02",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_PISTOL50_VARMOD_LUXE",
        "COMPONENT_REVOLVER_VARMOD_BOSS",
        "COMPONENT_REVOLVER_VARMOD_GOON",
        "COMPONENT_REVOLVER_CLIP_01",
        "COMPONENT_SNSPISTOL_CLIP_01",
        "COMPONENT_SNSPISTOL_CLIP_02",
        "COMPONENT_SNSPISTOL_VARMOD_LOWRIDER",
        "COMPONENT_HEAVYPISTOL_CLIP_01",
        "COMPONENT_HEAVYPISTOL_CLIP_02",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_HEAVYPISTOL_VARMOD_LUXE",
        "COMPONENT_REVOLVER_MK2_CLIP_01",
        "COMPONENT_REVOLVER_MK2_CLIP_TRACER",
        "COMPONENT_REVOLVER_MK2_CLIP_INCENDIARY",
        "COMPONENT_REVOLVER_MK2_CLIP_HOLLOWPOINT",
        "COMPONENT_REVOLVER_MK2_CLIP_FMJ",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MACRO_MK2",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_PI_COMP_03",
        "COMPONENT_REVOLVER_MK2_CAMO",
        "COMPONENT_REVOLVER_MK2_CAMO_02",
        "COMPONENT_REVOLVER_MK2_CAMO_03",
        "COMPONENT_REVOLVER_MK2_CAMO_04",
        "COMPONENT_REVOLVER_MK2_CAMO_05",
        "COMPONENT_REVOLVER_MK2_CAMO_06",
        "COMPONENT_REVOLVER_MK2_CAMO_07",
        "COMPONENT_REVOLVER_MK2_CAMO_08",
        "COMPONENT_REVOLVER_MK2_CAMO_09",
        "COMPONENT_REVOLVER_MK2_CAMO_10",
        "COMPONENT_REVOLVER_MK2_CAMO_IND_01",
        "COMPONENT_SNSPISTOL_MK2_CLIP_01",
        "COMPONENT_SNSPISTOL_MK2_CLIP_02",
        "COMPONENT_SNSPISTOL_MK2_CLIP_TRACER",
        "COMPONENT_SNSPISTOL_MK2_CLIP_INCENDIARY",
        "COMPONENT_SNSPISTOL_MK2_CLIP_HOLLOWPOINT",
        "COMPONENT_SNSPISTOL_MK2_CLIP_FMJ",
        "COMPONENT_AT_PI_FLSH_03",
        "COMPONENT_AT_PI_RAIL_02",
        "COMPONENT_AT_PI_SUPP_02",
        "COMPONENT_AT_PI_COMP_02",
        "COMPONENT_SNSPISTOL_MK2_CAMO",
        "COMPONENT_SNSPISTOL_MK2_CAMO_02",
        "COMPONENT_SNSPISTOL_MK2_CAMO_03",
        "COMPONENT_SNSPISTOL_MK2_CAMO_04",
        "COMPONENT_SNSPISTOL_MK2_CAMO_05",
        "COMPONENT_SNSPISTOL_MK2_CAMO_06",
        "COMPONENT_SNSPISTOL_MK2_CAMO_07",
        "COMPONENT_SNSPISTOL_MK2_CAMO_08",
        "COMPONENT_SNSPISTOL_MK2_CAMO_09",
        "COMPONENT_SNSPISTOL_MK2_CAMO_10",
        "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01",
        "COMPONENT_SNSPISTOL_MK2_CAMO_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_02_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_03_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_04_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_05_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_06_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_07_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_08_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_09_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_10_SLIDE",
        "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01_SLIDE",
        "COMPONENT_PISTOL_MK2_CLIP_01",
        "COMPONENT_PISTOL_MK2_CLIP_02",
        "COMPONENT_PISTOL_MK2_CLIP_TRACER",
        "COMPONENT_PISTOL_MK2_CLIP_INCENDIARY",
        "COMPONENT_PISTOL_MK2_CLIP_HOLLOWPOINT",
        "COMPONENT_PISTOL_MK2_CLIP_FMJ",
        "COMPONENT_AT_PI_RAIL",
        "COMPONENT_AT_PI_FLSH_02",
        "COMPONENT_AT_PI_SUPP_02",
        "COMPONENT_AT_PI_COMP",
        "COMPONENT_PISTOL_MK2_CAMO",
        "COMPONENT_PISTOL_MK2_CAMO_02",
        "COMPONENT_PISTOL_MK2_CAMO_03",
        "COMPONENT_PISTOL_MK2_CAMO_04",
        "COMPONENT_PISTOL_MK2_CAMO_05",
        "COMPONENT_PISTOL_MK2_CAMO_06",
        "COMPONENT_PISTOL_MK2_CAMO_07",
        "COMPONENT_PISTOL_MK2_CAMO_08",
        "COMPONENT_PISTOL_MK2_CAMO_09",
        "COMPONENT_PISTOL_MK2_CAMO_10",
        "COMPONENT_PISTOL_MK2_CAMO_IND_01",
        "COMPONENT_PISTOL_MK2_CAMO_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_02_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_03_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_04_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_05_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_06_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_07_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_08_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_09_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_10_SLIDE",
        "COMPONENT_PISTOL_MK2_CAMO_IND_01_SLIDE",
        "COMPONENT_VINTAGEPISTOL_CLIP_01",
        "COMPONENT_VINTAGEPISTOL_CLIP_02",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_RAYPISTOL_VARMOD_XMAS18",
        "COMPONENT_MICROSMG_CLIP_01",
        "COMPONENT_MICROSMG_CLIP_02",
        "COMPONENT_AT_PI_FLSH",
        "COMPONENT_AT_SCOPE_MACRO",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_MICROSMG_VARMOD_LUXE",
        "COMPONENT_SMG_CLIP_01",
        "COMPONENT_SMG_CLIP_02",
        "COMPONENT_SMG_CLIP_03",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_MACRO_02",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_SMG_VARMOD_LUXE",
        "COMPONENT_ASSAULTSMG_CLIP_01",
        "COMPONENT_ASSAULTSMG_CLIP_02",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_MACRO",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER",
        "COMPONENT_MINISMG_CLIP_01",
        "COMPONENT_MINISMG_CLIP_02",
        "COMPONENT_SMG_MK2_CLIP_01",
        "COMPONENT_SMG_MK2_CLIP_02",
        "COMPONENT_SMG_MK2_CLIP_TRACER",
        "COMPONENT_SMG_MK2_CLIP_INCENDIARY",
        "COMPONENT_SMG_MK2_CLIP_HOLLOWPOINT",
        "COMPONENT_SMG_MK2_CLIP_FMJ",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SIGHTS_SMG",
        "COMPONENT_AT_SCOPE_MACRO_02_SMG_MK2",
        "COMPONENT_AT_SCOPE_SMALL_SMG_MK2",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_SB_BARREL_01",
        "COMPONENT_AT_SB_BARREL_02",
        "COMPONENT_SMG_MK2_CAMO",
        "COMPONENT_SMG_MK2_CAMO_02",
        "COMPONENT_SMG_MK2_CAMO_03",
        "COMPONENT_SMG_MK2_CAMO_04",
        "COMPONENT_SMG_MK2_CAMO_05",
        "COMPONENT_SMG_MK2_CAMO_06",
        "COMPONENT_SMG_MK2_CAMO_07",
        "COMPONENT_SMG_MK2_CAMO_08",
        "COMPONENT_SMG_MK2_CAMO_09",
        "COMPONENT_SMG_MK2_CAMO_10",
        "COMPONENT_SMG_MK2_CAMO_IND_01",
        "COMPONENT_MACHINEPISTOL_CLIP_01",
        "COMPONENT_MACHINEPISTOL_CLIP_02",
        "COMPONENT_MACHINEPISTOL_CLIP_03",
        "COMPONENT_AT_PI_SUPP",
        "COMPONENT_COMBATPDW_CLIP_01",
        "COMPONENT_COMBATPDW_CLIP_02",
        "COMPONENT_COMBATPDW_CLIP_03",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_AT_SCOPE_SMALL",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SR_SUPP",
        "COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER",
        "COMPONENT_SAWNOFFSHOTGUN_VARMOD_LUXE",
        "COMPONENT_ASSAULTSHOTGUN_CLIP_01",
        "COMPONENT_ASSAULTSHOTGUN_CLIP_02",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_PUMPSHOTGUN_MK2_CLIP_01",
        "COMPONENT_PUMPSHOTGUN_MK2_CLIP_INCENDIARY",
        "COMPONENT_PUMPSHOTGUN_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_PUMPSHOTGUN_MK2_CLIP_HOLLOWPOINT",
        "COMPONENT_PUMPSHOTGUN_MK2_CLIP_EXPLOSIVE",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MACRO_MK2",
        "COMPONENT_AT_SCOPE_SMALL_MK2",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SR_SUPP_03",
        "COMPONENT_AT_MUZZLE_08",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_02",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_03",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_04",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_05",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_06",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_07",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_08",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_09",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_10",
        "COMPONENT_PUMPSHOTGUN_MK2_CAMO_IND_01",
        "COMPONENT_HEAVYSHOTGUN_CLIP_01",
        "COMPONENT_HEAVYSHOTGUN_CLIP_02",
        "COMPONENT_HEAVYSHOTGUN_CLIP_03",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_ASSAULTRIFLE_CLIP_01",
        "COMPONENT_ASSAULTRIFLE_CLIP_02",
        "COMPONENT_ASSAULTRIFLE_CLIP_03",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_MACRO",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_ASSAULTRIFLE_VARMOD_LUXE",
        "COMPONENT_CARBINERIFLE_CLIP_01",
        "COMPONENT_CARBINERIFLE_CLIP_02",
        "COMPONENT_CARBINERIFLE_CLIP_03",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_MEDIUM",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_CARBINERIFLE_VARMOD_LUXE",
        "COMPONENT_ADVANCEDRIFLE_CLIP_01",
        "COMPONENT_ADVANCEDRIFLE_CLIP_02",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_SMALL",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE",
        "COMPONENT_SPECIALCARBINE_CLIP_01",
        "COMPONENT_SPECIALCARBINE_CLIP_02",
        "COMPONENT_SPECIALCARBINE_CLIP_03",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_MEDIUM",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER",
        "COMPONENT_BULLPUPRIFLE_CLIP_01",
        "COMPONENT_BULLPUPRIFLE_CLIP_02",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SCOPE_SMALL",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_BULLPUPRIFLE_VARMOD_LOW",
        "COMPONENT_BULLPUPRIFLE_MK2_CLIP_01",
        "COMPONENT_BULLPUPRIFLE_MK2_CLIP_02",
        "COMPONENT_BULLPUPRIFLE_MK2_CLIP_TRACER",
        "COMPONENT_BULLPUPRIFLE_MK2_CLIP_INCENDIARY",
        "COMPONENT_BULLPUPRIFLE_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_BULLPUPRIFLE_MK2_CLIP_FMJ",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MACRO_02_MK2",
        "COMPONENT_AT_SCOPE_SMALL_MK2",
        "COMPONENT_AT_BP_BARREL_01",
        "COMPONENT_AT_BP_BARREL_02",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_AR_AFGRIP_02",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_02",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_03",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_04",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_05",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_06",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_07",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_08",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_09",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_10",
        "COMPONENT_BULLPUPRIFLE_MK2_CAMO_IND_01",
        "COMPONENT_SPECIALCARBINE_MK2_CLIP_01",
        "COMPONENT_SPECIALCARBINE_MK2_CLIP_02",
        "COMPONENT_SPECIALCARBINE_MK2_CLIP_TRACER",
        "COMPONENT_SPECIALCARBINE_MK2_CLIP_INCENDIARY",
        "COMPONENT_SPECIALCARBINE_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_SPECIALCARBINE_MK2_CLIP_FMJ",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MACRO_MK2",
        "COMPONENT_AT_SCOPE_MEDIUM_MK2",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_AR_AFGRIP_02",
        "COMPONENT_AT_SC_BARREL_01",
        "COMPONENT_AT_SC_BARREL_02",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_02",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_03",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_04",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_05",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_06",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_07",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_08",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_09",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_10",
        "COMPONENT_SPECIALCARBINE_MK2_CAMO_IND_01",
        "COMPONENT_ASSAULTRIFLE_MK2_CLIP_01",
        "COMPONENT_ASSAULTRIFLE_MK2_CLIP_02",
        "COMPONENT_ASSAULTRIFLE_MK2_CLIP_TRACER",
        "COMPONENT_ASSAULTRIFLE_MK2_CLIP_INCENDIARY",
        "COMPONENT_ASSAULTRIFLE_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_ASSAULTRIFLE_MK2_CLIP_FMJ",
        "COMPONENT_AT_AR_AFGRIP_02",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MACRO_MK2",
        "COMPONENT_AT_SCOPE_MEDIUM_MK2",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_AR_BARREL_01",
        "COMPONENT_AT_AR_BARREL_02",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_02",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_03",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_04",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_05",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_06",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_07",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_08",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_09",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_10",
        "COMPONENT_ASSAULTRIFLE_MK2_CAMO_IND_01",
        "COMPONENT_CARBINERIFLE_MK2_CLIP_01",
        "COMPONENT_CARBINERIFLE_MK2_CLIP_02",
        "COMPONENT_CARBINERIFLE_MK2_CLIP_TRACER",
        "COMPONENT_CARBINERIFLE_MK2_CLIP_INCENDIARY",
        "COMPONENT_CARBINERIFLE_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_CARBINERIFLE_MK2_CLIP_FMJ",
        "COMPONENT_AT_AR_AFGRIP_02",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MACRO_MK2",
        "COMPONENT_AT_SCOPE_MEDIUM_MK2",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_CR_BARREL_01",
        "COMPONENT_AT_CR_BARREL_02",
        "COMPONENT_CARBINERIFLE_MK2_CAMO",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_02",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_03",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_04",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_05",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_06",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_07",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_08",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_09",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_10",
        "COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01",
        "COMPONENT_COMPACTRIFLE_CLIP_01",
        "COMPONENT_COMPACTRIFLE_CLIP_02",
        "COMPONENT_COMPACTRIFLE_CLIP_03",
        "COMPONENT_MG_CLIP_01",
        "COMPONENT_MG_CLIP_02",
        "COMPONENT_AT_SCOPE_SMALL_02",
        "COMPONENT_MG_VARMOD_LOWRIDER",
        "COMPONENT_COMBATMG_CLIP_01",
        "COMPONENT_COMBATMG_CLIP_02",
        "COMPONENT_AT_SCOPE_MEDIUM",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_COMBATMG_VARMOD_LOWRIDER",
        "COMPONENT_COMBATMG_MK2_CLIP_01",
        "COMPONENT_COMBATMG_MK2_CLIP_02",
        "COMPONENT_COMBATMG_MK2_CLIP_TRACER",
        "COMPONENT_COMBATMG_MK2_CLIP_INCENDIARY",
        "COMPONENT_COMBATMG_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_COMBATMG_MK2_CLIP_FMJ",
        "COMPONENT_AT_AR_AFGRIP_02",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_SMALL_MK2",
        "COMPONENT_AT_SCOPE_MEDIUM_MK2",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_MG_BARREL_01",
        "COMPONENT_AT_MG_BARREL_02",
        "COMPONENT_COMBATMG_MK2_CAMO",
        "COMPONENT_COMBATMG_MK2_CAMO_02",
        "COMPONENT_COMBATMG_MK2_CAMO_03",
        "COMPONENT_COMBATMG_MK2_CAMO_04",
        "COMPONENT_COMBATMG_MK2_CAMO_05",
        "COMPONENT_COMBATMG_MK2_CAMO_06",
        "COMPONENT_COMBATMG_MK2_CAMO_07",
        "COMPONENT_COMBATMG_MK2_CAMO_08",
        "COMPONENT_COMBATMG_MK2_CAMO_09",
        "COMPONENT_COMBATMG_MK2_CAMO_10",
        "COMPONENT_COMBATMG_MK2_CAMO_IND_01",
        "COMPONENT_GUSENBERG_CLIP_01",
        "COMPONENT_GUSENBERG_CLIP_02",
        "COMPONENT_SNIPERRIFLE_CLIP_01",
        "COMPONENT_AT_AR_SUPP_02",
        "COMPONENT_AT_SCOPE_LARGE",
        "COMPONENT_AT_SCOPE_MAX",
        "COMPONENT_SNIPERRIFLE_VARMOD_LUXE",
        "COMPONENT_HEAVYSNIPER_CLIP_01",
        "COMPONENT_AT_SCOPE_LARGE",
        "COMPONENT_AT_SCOPE_MAX",
        "COMPONENT_MARKSMANRIFLE_MK2_CLIP_01",
        "COMPONENT_MARKSMANRIFLE_MK2_CLIP_02",
        "COMPONENT_MARKSMANRIFLE_MK2_CLIP_TRACER",
        "COMPONENT_MARKSMANRIFLE_MK2_CLIP_INCENDIARY",
        "COMPONENT_MARKSMANRIFLE_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_MARKSMANRIFLE_MK2_CLIP_FMJ",
        "COMPONENT_AT_SIGHTS",
        "COMPONENT_AT_SCOPE_MEDIUM_MK2",
        "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM_MK2",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_MUZZLE_01",
        "COMPONENT_AT_MUZZLE_02",
        "COMPONENT_AT_MUZZLE_03",
        "COMPONENT_AT_MUZZLE_04",
        "COMPONENT_AT_MUZZLE_05",
        "COMPONENT_AT_MUZZLE_06",
        "COMPONENT_AT_MUZZLE_07",
        "COMPONENT_AT_MRFL_BARREL_01",
        "COMPONENT_AT_MRFL_BARREL_02",
        "COMPONENT_AT_AR_AFGRIP_02",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_02",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_03",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_04",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_05",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_06",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_07",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_08",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_09",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_10",
        "COMPONENT_MARKSMANRIFLE_MK2_CAMO_IND_01",
        "COMPONENT_HEAVYSNIPER_MK2_CLIP_01",
        "COMPONENT_HEAVYSNIPER_MK2_CLIP_02",
        "COMPONENT_HEAVYSNIPER_MK2_CLIP_INCENDIARY",
        "COMPONENT_HEAVYSNIPER_MK2_CLIP_ARMORPIERCING",
        "COMPONENT_HEAVYSNIPER_MK2_CLIP_FMJ",
        "COMPONENT_HEAVYSNIPER_MK2_CLIP_EXPLOSIVE",
        "COMPONENT_AT_SCOPE_LARGE_MK2",
        "COMPONENT_AT_SCOPE_MAX",
        "COMPONENT_AT_SCOPE_NV",
        "COMPONENT_AT_SCOPE_THERMAL",
        "COMPONENT_AT_SR_SUPP_03",
        "COMPONENT_AT_MUZZLE_08",
        "COMPONENT_AT_MUZZLE_09",
        "COMPONENT_AT_SR_BARREL_01",
        "COMPONENT_AT_SR_BARREL_02",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_02",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_03",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_04",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_05",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_06",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_07",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_08",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_09",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_10",
        "COMPONENT_HEAVYSNIPER_MK2_CAMO_IND_01",
        "COMPONENT_MARKSMANRIFLE_CLIP_01",
        "COMPONENT_MARKSMANRIFLE_CLIP_02",
        "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_SUPP",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_MARKSMANRIFLE_VARMOD_LUXE",
        "COMPONENT_GRENADELAUNCHER_CLIP_01",
        "COMPONENT_AT_AR_FLSH",
        "COMPONENT_AT_AR_AFGRIP",
        "COMPONENT_AT_SCOPE_SMALL"
        };

        internal static HashSet<uint> OwnedHashes = new HashSet<uint>();

        internal static Dictionary<string, int> WeaponTints = new Dictionary<string, int>();
       

        internal static void ProcessAttachments()
        {
            
            if (!EquippedWeapon || !EquippedWeaponDescriptor.Asset.IsValid || !EquippedWeapon.Model.IsValid)
            {
                return;
            }
            uint hash = (uint)EquippedWeaponDescriptor.Hash;
            string[] weaponComponentNames = WeaponComponentNames;
            foreach (string text in weaponComponentNames)
            {
                if ((NativeFunction.Natives.DOES_WEAPON_TAKE_WEAPON_COMPONENT<bool>(hash, Game.GetHashKey(text))) && ((!OwnedHashes.Contains(Game.GetHashKey(text)) && NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(EquippedWeapon, Game.GetHashKey(text))) ? true : false))
                {
                    OwnedHashes.Add(Game.GetHashKey(text));
                }
            }
            int num = NativeFunction.Natives.GET_WEAPON_OBJECT_TINT_INDEX<int>(EquippedWeapon);
            if (!WeaponTints.ContainsKey(EquippedWeapon.Model.Name) && num != 0)
            {
                WeaponTints.Add(EquippedWeapon.Model.Name, NativeFunction.Natives.GET_WEAPON_OBJECT_TINT_INDEX<int>(EquippedWeapon));
            }
            Game.DisplayNotification("Your attachments have been saved. " + OwnedHashes.Count + " and " + WeaponTints.Count);
        }

        internal static void EquipAttachments(Ped PlayerPed)
        {
            foreach (uint hash in OwnedHashes)
            {
                if (NativeFunction.Natives.DOES_WEAPON_TAKE_WEAPON_COMPONENT<bool>((uint)EquippedWeaponDescriptor.Hash, hash) && !NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(EquippedWeapon, hash))
                {
                    string componentName = WeaponComponentNames.FirstOrDefault((string n) => Game.GetHashKey(n) == hash);
                    if (NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(EquippedWeapon, Game.GetHashKey("COMPONENT_AT_SCOPE_MAX")) &&  componentName == "COMPONENT_AT_SCOPE_LARGE"){
                        continue;
                    }
                    if (NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(EquippedWeapon, Game.GetHashKey("COMPONENT_AT_SCOPE_LARGE")) && componentName == "COMPONENT_AT_SCOPE_MAX")
                    {
                        continue;
                    }

                    PlayerPed.Inventory.AddComponentToWeapon(EquippedWeaponDescriptor.Asset, componentName);
                }
                int num = 0;
                if (WeaponTints.ContainsKey(EquippedWeapon.Model.Name))
                {
                    num = WeaponTints[EquippedWeapon.Model.Name];
                }
                if (num != 0)
                {
                    NativeFunction.Natives.SET_WEAPON_OBJECT_TINT_INDEX(EquippedWeapon, num);
                }
            }
        }
    }
}