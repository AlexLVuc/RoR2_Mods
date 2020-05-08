using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using UnityEngine;
using RoR2;
using IL.EntityStates.HermitCrab;
using EntityStates.Croco;
using UnityEngine.Networking;

namespace Thornmail
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class CustomItem : BaseUnityPlugin
    {
        private const string ModVer = "0.0.1";
        private const string ModName = "Thornmail";
        public const string ModGuid = "com.MyName.Thornmail";

        internal new static ManualLogSource Logger; // allow access to the logger across the plugin classes

        public void Awake()
        {
            Logger = base.Logger;

            Assets.Init();
            Hooks.Init();
            
        }

        public void Update()
        {
            // TODO: Remove this section of code when modding is done.
            // To enable easier testing, F2 will drop the newest added item. 
            if (Input.GetKeyDown(KeyCode.F2))
            {
                var dropEList = Run.instance.availableEquipmentDropList;
                // When items are added to the availableEquipmentDropList, it is appended to the second last item in the droplist
                var nextEquipmentItem = dropEList.Count - 1;
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(dropEList[nextEquipmentItem], transform.position, transform.forward * 20f);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                RoR2.Chat.AddMessage("Hello World!");
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                GameObject gameObject = MasterCatalog.FindMasterPrefab("WispMaster");
                GameObject bodyPrefab = gameObject.GetComponent<CharacterMaster>().bodyPrefab;
                CharacterMaster playerMaster = LocalUserManager.GetFirstLocalUser().cachedMasterController.master;
                GameObject spawnThing = UnityEngine.Object.Instantiate<GameObject>(gameObject, playerMaster.GetBody().transform.position, Quaternion.identity);
                CharacterMaster component = spawnThing.GetComponent<CharacterMaster>();
                NetworkServer.Spawn(spawnThing);
                component.SpawnBody(bodyPrefab, playerMaster.GetBody().transform.position, Quaternion.identity);
            }
        }
    }
}