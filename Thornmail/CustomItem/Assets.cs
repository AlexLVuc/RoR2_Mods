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
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Thornmail.exampleitemmod")) 
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
            var ThornmailEquipmentDef = new RoR2.EquipmentDef
            {
                name = "ThornmailEquipment",
                cooldown = 5f,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Thornmail Equipment",
                pickupToken = "Grants <style=cIsUtility>immunity</style> and <style=cIsUtility>reflects</style> all incoming attacks back to the attacker. Attackers are also <style=cIsUtility>entangled</style>. <style=cStack>(3s Equipment Duration)</style>",
                descriptionToken = "Grants <style=cIsUtility>immunity</style> and <style=cIsUtility>reflects</style> all incoming attacks back to the attacker. Attackers are also <style=cIsUtility>entangled</style>. <style=cStack>(3s Equipment Duration)</style>",
                loreToken = "When sitting down in quarantine for a month they asked themselves 'how should I be productive?' and thus, Thornmail was created.",
                canDrop = true,
                enigmaCompatible = true
            };

            var defaultDisplayRule = new ItemDisplayRule[1];
            defaultDisplayRule[0].followerPrefab = ThornmailPrefab;
            defaultDisplayRule[0].childName = "Chest";
            defaultDisplayRule[0].localScale = new Vector3(20f, 20f, 20f);
            defaultDisplayRule[0].localAngles = new Vector3(0f, 0f, 0f);
            defaultDisplayRule[0].localPos = new Vector3(0.0f, 0f, 0f);

            var thornmail = new CustomEquipment(ThornmailEquipmentDef, defaultDisplayRule);

            ThornmailEquipmentIndex = ItemAPI.Add(thornmail);
        }
    }
}
