using IL.RoR2;
using IL.RoR2.Orbs;
using On.RoR2;
using Rewired;
using RoR2;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

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
                        // Modify the damage info to set the attacker as the player
                        // We do this to enable money to be gained from attacker death
                        RoR2.DamageInfo modifiedDamageInfo = damageInfo;
                        RoR2.LocalUser firstLocalUser = RoR2.LocalUserManager.GetFirstLocalUser();
                        GameObject player = firstLocalUser.cachedBody.gameObject;

                        modifiedDamageInfo.attacker = player;
                        modifiedDamageInfo.inflictor = player;

                        attackerBody.healthComponent.TakeDamage(modifiedDamageInfo);
                        
                        // If attacker survives, give them a debuff them
                        if (attackerBody.healthComponent.alive)
                        {
                            // Add debuff to attacker
                            BuffIndex buffIndex = RoR2.BuffCatalog.FindBuffIndex("Entangle");
                            attackerBody.AddTimedBuff(buffIndex, 3);
                        }
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
                    // Add immunity to player
                    BuffIndex buffIndex = RoR2.BuffCatalog.FindBuffIndex("Immune");
                    self.characterBody.AddTimedBuff(buffIndex, 3);

                    // Timer for figuring out when damage should be reflected
                    Thread timedThornmailThread = new Thread(
                        () => TimeThornmail()
                    );
                    timedThornmailThread.Start();
                    return true;
                }
                return orig(self, equipmentIndex);

            };

            // Use a thread to time when deflections should happen
            void TimeThornmail()
            {
                thornmailTimer = true;
                Thread.Sleep(3000);
                thornmailTimer = false;
            }
        }
    }
}