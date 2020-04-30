using IL.RoR2;
using IL.RoR2.Orbs;
using On.RoR2;
using Rewired;
using RoR2;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Thornmail
{
    public class Hooks
    {

        internal static void Init()
        {
            bool thornmailTimer = false;
            On.RoR2.HealthComponent.TakeDamage += delegate (On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
            {
                RoR2.CharacterBody playerBody = self.body;
                RoR2.EquipmentIndex index = playerBody.inventory.currentEquipmentIndex;
                if (index == Assets.ThornmailEquipmentIndex)
                {
                    // Direct all damage to attacker
                    RoR2.CharacterBody attackerBody = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();
                    if (thornmailTimer)
                    {
                        attackerBody.healthComponent.TakeDamage(damageInfo);
                        
                        // Add entangle debuff to attacker
                        BuffIndex entangleIndex = RoR2.BuffCatalog.FindBuffIndex("Entangle");
                        attackerBody.AddTimedBuff(entangleIndex, 3);
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

                if (equipmentIndex == Assets.ThornmailEquipmentIndex)
                {
                    //add immunity buff to player for 3s
                    BuffIndex buffIndex = RoR2.BuffCatalog.FindBuffIndex("Immune");
                    self.characterBody.AddTimedBuff(buffIndex, 3);
                    Thread timedThornmailThread = new Thread(
                        () => TimeThornmail()
                    );
                    timedThornmailThread.Start();
                }
                return orig(self, equipmentIndex);

            };

            // We want to deflect damage for 3 seconds
            void TimeThornmail()
            {
                thornmailTimer = true;
                Thread.Sleep(3000);
                thornmailTimer = false;
            }
        }
    }
}