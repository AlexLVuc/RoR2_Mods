using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;

namespace CustomItem
{
    internal static class Assets
    {
        internal static GameObject BiscoLeashPrefab;
        internal static ItemIndex BiscoLeashItemIndex;
        internal static EquipmentIndex BiscoLeashEquipmentIndex;

        private const string ModPrefix = "@CustomItem:";
        private const string PrefabPath = ModPrefix + "Assets/Import/belt/belt.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/belt_icon/belt_icon.png";

        internal static void Init()
        {
            // First registering your AssetBundle into the ResourcesAPI with a modPrefix that'll also be used for your prefab and icon paths
            // note that the string parameter of this GetManifestResourceStream call will change depending on
            // your namespace and file name
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomItem.rampage")) 
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                BiscoLeashPrefab = bundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
            }

            BiscoLeashAsRedTierItem();
            BiscoLeashAsEquipment();
        }

        private static void BiscoLeashAsRedTierItem()
        {
            var biscoLeashItemDef = new ItemDef
            {
                name = "BiscosLeash", // its the internal name, no spaces, apostrophes and stuff like that
                tier = ItemTier.Tier3,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Bisco's Leash", // stylised name
                pickupToken = "Grants <style=cDeath>RAMPAGE</style> on kill. \n<style=cDeath>RAMPAGE</style> : Specifics rewards for reaching kill streaks. \nIncreases <style=cIsUtility>movement speed</style> by <style=cIsUtility>1%</style> <style=cIsDamage>(+1% per item stack)</style> <style=cStack>(+1% every 20 Rampage Stacks)</style>. \nIncreases <style=cIsUtility>damage</style> by <style=cIsUtility>2%</style> <style=cIsDamage>(+2% per item stack)</style> <style=cStack>(+2% every 20 Rampage Stacks)</style>.",
                descriptionToken = "Grants <style=cDeath>RAMPAGE</style> on kill. \n<style=cDeath>RAMPAGE</style> : Specifics rewards for reaching kill streaks. \nIncreases <style=cIsUtility>movement speed</style> by <style=cIsUtility>1%</style> <style=cIsDamage>(+1% per item stack)</style> <style=cStack>(+1% every 20 Rampage Stacks)</style>. \nIncreases <style=cIsUtility>damage</style> by <style=cIsUtility>2%</style> <style=cIsDamage>(+2% per item stack)</style> <style=cStack>(+2% every 20 Rampage Stacks)</style>.",
                loreToken = "Should passion die down,\r\nShould light be extinguished,\r\nWill you bring me patience?\r\nWill you die with me?\"\r\n\r\n-The Syzygy of Io and Europa",
                tags = new[]
                {
                    ItemTag.Utility,
                    ItemTag.Damage
                }
            };

            var itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = BiscoLeashPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var biscoLeash = new R2API.CustomItem(biscoLeashItemDef, itemDisplayRules);

            BiscoLeashItemIndex = ItemAPI.Add(biscoLeash); // ItemAPI sends back the ItemIndex of your item
        }

        private static void BiscoLeashAsEquipment()
        {
            var biscoLeashEquipmentDef = new EquipmentDef
            {
                name = "BiscosLeashEquipment", // its the internal name, no spaces, apostrophes and stuff like that
                cooldown = 5f,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Bisco's Leash Equipment", // stylised name
                pickupToken = "Grants <style=cDeath>RAMPAGE</style> on kill. \n<style=cDeath>RAMPAGE</style> : Specifics rewards for reaching kill streaks. \nIncreases <style=cIsUtility>movement speed</style> by <style=cIsUtility>1%</style> <style=cIsDamage>(+1% per item stack)</style> <style=cStack>(+1% every 20 Rampage Stacks)</style>. \nIncreases <style=cIsUtility>damage</style> by <style=cIsUtility>2%</style> <style=cIsDamage>(+2% per item stack)</style> <style=cStack>(+2% every 20 Rampage Stacks)</style>.",
                descriptionToken = "Grants <style=cDeath>RAMPAGE</style> on kill. \n<style=cDeath>RAMPAGE</style> : Specifics rewards for reaching kill streaks. \nIncreases <style=cIsUtility>movement speed</style> by <style=cIsUtility>1%</style> <style=cIsDamage>(+1% per item stack)</style> <style=cStack>(+1% every 20 Rampage Stacks)</style>. \nIncreases <style=cIsUtility>damage</style> by <style=cIsUtility>2%</style> <style=cIsDamage>(+2% per item stack)</style> <style=cStack>(+2% every 20 Rampage Stacks)</style>.",
                loreToken = "Should passion die down,\r\nShould light be extinguished,\r\nWill you bring me patience?\r\nWill you die with me?\"\r\n\r\n-The Syzygy of Io and Europa",
                canDrop = true,
                enigmaCompatible = false
            };

            var itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = BiscoLeashPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var biscoLeash = new CustomEquipment(biscoLeashEquipmentDef, itemDisplayRules);

            BiscoLeashEquipmentIndex = ItemAPI.Add(biscoLeash); // ItemAPI sends back the EquipmentIndex of your equipment
        }
    }
}
