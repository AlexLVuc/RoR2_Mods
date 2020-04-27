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
            On.RoR2.EquipmentSlot.PerformEquipmentAction += delegate(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, RoR2.EquipmentSlot slot, RoR2.EquipmentIndex index)
            {
                if (index == Assets.BiscoLeashEquipmentIndex)
                {
                    BuffIndex buffIndex = RoR2.BuffCatalog.FindBuffIndex("Immune");
                    slot.characterBody.AddTimedBuff(buffIndex, 2);
                }

                return orig(slot, index);
            };
        }
    }
}
    