using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class test2 : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStoreCollection playerStoreCollection;
    private ItemManager localItemManager = null;

    public override void Interact()
    {
        if (this.localItemManager == null)
        {

            PlayerStore localPlayerStore = this
                .playerStoreCollection
                .GetLocal();

            Debug.Log($"localPlayerStore: {localPlayerStore}");

            this.localItemManager = localPlayerStore.itemManager;

            Debug.Log($"localItemManager: {this.localItemManager}");

        }
        this.localItemManager.EquipShellShoot();
    }

}