using IL.RoR2;
using IL.RoR2.Orbs;
using On.RoR2;
using Rewired;
using RoR2;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CustomItem
{
    public class Hooks
    {

        internal static void Init()
        {
            // Item that causes damage to be returned
            // Causes damage dealers to be stunned
            // Gives immunity to user
            bool biscoLeashItem = false;
            On.RoR2.HealthComponent.TakeDamage += delegate (On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
            {
                RoR2.CharacterBody playerBody = self.body;
                RoR2.EquipmentIndex index = playerBody.inventory.currentEquipmentIndex;
                if (index == Assets.BiscoLeashEquipmentIndex)
                {
                    RoR2.CharacterBody attackerBody = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();
                    if (biscoLeashItem)
                    {
                        attackerBody.healthComponent.TakeDamage(damageInfo);
                    }
                    else
                    {
                        orig(self, damageInfo);
                    }
                }
                orig(self, damageInfo);
            };

            On.RoR2.EquipmentSlot.PerformEquipmentAction += delegate (On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, RoR2.EquipmentSlot self, RoR2.EquipmentIndex equipmentIndex)
            {

                if (equipmentIndex == Assets.BiscoLeashEquipmentIndex)
                {
                    BuffIndex buffIndex = RoR2.BuffCatalog.FindBuffIndex("Immune");
                    self.characterBody.AddTimedBuff(buffIndex, 2);
                    Thread timedBiscoLeashThread = new Thread(
                        () => TimeBiscoLeash()
                    );
                }
                return orig(self, equipmentIndex);

            };

            void TimeBiscoLeash()
            {
                biscoLeashItem = true;
                Thread.Sleep(5000);
                biscoLeashItem = false;
            }
        }
    }
}