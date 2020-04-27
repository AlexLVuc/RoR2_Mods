using IL.RoR2;
using IL.RoR2.Orbs;
using On.RoR2;
using RoR2;
using System;

namespace CustomItem
{
    public class Hooks
    {
        internal static void Init()
        {
            On.RoR2.HealthComponent.TakeDamage += delegate (On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
            {
                int itemCount = self.body.inventory.GetItemCount(Assets.BiscoLeashItemIndex);
                if (itemCount > 0)
                {
                    RoR2.Chat.AddMessage(damageInfo.dotIndex.ToString());
                    if (damageInfo.dotIndex == RoR2.DotController.DotIndex.Burn || damageInfo.dotIndex == RoR2.DotController.DotIndex.PercentBurn)
                    {
                        Random randomChance = new Random();
                        if (randomChance.NextDouble() >= (float)itemCount * 0.1)
                        {
                            RoR2.Chat.AddMessage("Running Original TakeDamage");
                            orig(self, damageInfo);
                        }
                    }
                    
                    else
                    {
                        orig(self, damageInfo);
                    }
                }
                else
                {
                    orig(self, damageInfo);
                }
            };
        }
    }
}
    