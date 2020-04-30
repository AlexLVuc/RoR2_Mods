using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;

namespace Thornmail
{
    internal static class Assets
    {
        internal static GameObject ThornmailPrefab;
        internal static EquipmentIndex ThornmailEquipmentIndex;

        private const string ModPrefix = "@Thornmail:";
        private const string PrefabPath = ModPrefix + "Assets/Import/belt/belt.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/belt_icon/belt_icon.png";

        internal static void Init()
        {
            // First registering your AssetBundle into the ResourcesAPI with a modPrefix that'll also be used for your prefab and icon paths
            // note that the string parameter of this GetManifestResourceStream call will change depending on
            // your namespace and file name
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Thornmail.rampage")) 
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                ThornmailPrefab = bundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
            }

            ThornmailAsEquipment();
        }

        private static void ThornmailAsEquipment()
        {
            var ThornmailEquipmentDef = new EquipmentDef
            {
                name = "Thornmail", // its the internal name, no spaces, apostrophes and stuff like that
                cooldown = 5f,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Thornmail", // stylised name
                pickupToken = "Grants <style=cIsUtility>immunity</style> and <style=cIsUtility>reflects</style> all incoming attacks back to the attacker. Attackers are also <style=cIsUtility>entangled</style>. <style=cStack>(3s Equipment Duration)</style>",
                descriptionToken = "Grants <style=cIsUtility>immunity</style> and <style=cIsUtility>reflects</style> all incoming attacks back to the attacker. Attackers are also <style=cIsUtility>entangled</style>. <style=cStack>(3s Equipment Duration)</style>",
                loreToken = "Welcome to the League of Legends!",
                canDrop = true,
                enigmaCompatible = false
            };

            var itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = ThornmailPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var thornmail = new CustomEquipment(ThornmailEquipmentDef, itemDisplayRules);

            ThornmailEquipmentIndex = ItemAPI.Add(thornmail);
        }
    }
}
