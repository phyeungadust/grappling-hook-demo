using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class WaterSprayGunBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private VRCObjectPool waterSprayParticlePool;

    [SerializeField]
    private string vrButtonName = "Oculus_CrossPlatform_PrimaryIndexTrigger";
    [SerializeField]
    private string desktopButtonName = "Fire1";

    [SerializeField]
    private PlayerStore ownerStore;
    private VRCPlayerApi owner;
    private LocalVRMode localVRMode;

    [SerializeField]
    private MeshRenderer gunMesh;
    [SerializeField]
    private CustomTrackedObject gunTrackedObj;
    [SerializeField]
    private CustomGrip gunGrip;

    private bool equipped = false;

    [SerializeField]
    private int defaultAmmoAmt = 50;
    private int ammo;

    [SerializeField]
    private ItemManager itemManager;

    public void Init()
    {

        this.owner = this.ownerStore.playerApiSafe.Get();
        this.localVRMode = this.ownerStore.localVRMode;

        // set gun tracking object
        // based on localVRMode
        this.gunTrackedObj.CustomStart();

        // adjust gun grip
        // based on localVRMode
        this.gunGrip.CustomStart();

        Networking.SetOwner(
            this.owner,
            this.waterSprayParticlePool.gameObject
        );

    }

    public void Equip()
    {
        // refill ammo
        this.ammo = this.defaultAmmoAmt;
        this.SendCustomNetworkEvent(
            VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
            nameof(EquipBroadcast)
        );
        Debug.Log("Equip() called");
    }

    public void UnEquip()
    {
        this.SendCustomNetworkEvent(
            VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
            nameof(UnEquipBroadcast)
        );
    }

    public void EquipBroadcast()
    {
        // enable gun mesh
        this.gunMesh.enabled = true;
        // enable gun tracking
        this.gunTrackedObj.tracking = true;
        this.equipped = true;
        Debug.Log("EquipBroadcast() called");
    }

    public void UnEquipBroadcast()
    {
        // disable gun mesh
        this.gunMesh.enabled = false;
        // disable gun tracking
        this.gunTrackedObj.tracking = false; 
        this.equipped = false;
    }

    public void ItemUpdate()
    {
        
        if (this.equipped)
        {

            // update gun tracking position
            this.gunTrackedObj.CustomUpdate();

            if (this.localVRMode.IsLocal())
            {
                if (this.ReadInput())
                {

                    // fire button pressed

                    // spawn spray particle
                    GameObject spawnedSprayParticle = this
                        .waterSprayParticlePool
                        .TryToSpawn();
                    if (spawnedSprayParticle != null)
                    {
                        Networking.SetOwner(this.owner, spawnedSprayParticle);
                    }

                    // decrease ammo amount
                    --this.ammo;

                    if (this.ammo <= 0)
                    {
                        // exhausted ammo, switch to null item
                        this.itemManager.EquipNullItem();
                    }

                }
            }

        }

    }

    private bool ReadInput()
    {
        if (this.localVRMode.IsVR())
        {
            return Input.GetAxis(this.vrButtonName) > 0;
        }
        else
        {
            return Input.GetButton(this.desktopButtonName);
        }
    }

}