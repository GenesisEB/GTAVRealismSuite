// RealismTest.WeaponDrop
using Rage;
using RealismTest;

namespace RealismTest
{

    internal class WeaponDrop
    {
        internal static void ProcessHandDamageNearby(Ped PlayerPed)
        {
            Ped[] nearbyPeds = PlayerPed.GetNearbyPeds(16);
            foreach (Ped ped in nearbyPeds)
            {
                if (ped == null)
                {
                    continue;
                }
                if ((ped.LastDamageBone == PedBoneId.LeftHand || ped.LastDamageBone == PedBoneId.RightHand) && ped.Inventory.EquippedWeapon != null)
                {
                    ped.Inventory.EquippedWeapon.Drop();
                }
            }
        }

        internal static void ProcessHandDamagePlayer(Ped PlayerPed)
        {
            if (AttachmentSystem.EquippedWeapon)
            {
                Weapon equippedWeapon = AttachmentSystem.EquippedWeapon;
                if (equippedWeapon)
                {
                    AttachmentSystem.ProcessAttachments();
                    AttachmentSystem.EquippedWeaponDescriptor.DropToGround();
                    equippedWeapon.GetLastCollision(out Vector3 position, out Vector3 normal, out string _);
                    equippedWeapon.ApplyForce(normal * 2f, position, isForceRelative: true, isOffsetRelative: false);
                }
            }
        }
    }
}