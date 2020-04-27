using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using UnityEngine;
using RoR2;

namespace CustomItem
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class CustomItem : BaseUnityPlugin
    {
        private const string ModVer = "1.0.0";
        private const string ModName = "MyCustomItemPlugin";
        public const string ModGuid = "com.MyName.MyCustomItemPlugin";

        internal new static ManualLogSource Logger; // allow access to the logger across the plugin classes

        public void Awake()
        {
            Logger = base.Logger;

            Assets.Init();
            Hooks.Init();
            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                var dropEList = Run.instance.availableEquipmentDropList;
                var nextEquipmentItem = Run.instance.treasureRng.RangeInt(0, dropEList.Count);
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(dropEList[nextEquipmentItem], transform.position, transform.forward * 20f);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                var drop3List = Run.instance.availableTier3DropList;
                var next3Item = Run.instance.treasureRng.RangeInt(0, drop3List.Count);
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(drop3List[next3Item], transform.position, transform.forward * 20f);
            }
        }
    }
}