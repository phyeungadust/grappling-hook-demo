using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class WaterSprayGunBehaviour : Item
{

    [SerializeField]
    private WaterSprayGunBehaviourControls waterSprayGunBehaviourControls;
    [SerializeField]
    private WaterSprayGunBehaviourGameStateControls waterSprayGunBehaviourGameStateControls;

    public override ItemControls GetItemControls()
    {
        return this.waterSprayGunBehaviourControls;
    }

    public override GameStateControls GetGameStateControls()
    {
        return this.waterSprayGunBehaviourGameStateControls;
    }

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

    [SerializeField]
    private float defaultCDAfterEquip = 1.0f;
    private float cdAfterEquip;

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

    public void OnBeforeGameStarts()
    {
        // switch back to null item
        this.itemManager.EquipNullItem();
    }

    public void Equip()
    {
        if (this.localVRMode.IsLocal())
        {
            // start cooldown
            this.cdAfterEquip = this.defaultCDAfterEquip;
            // refill ammo
            this.ammo = this.defaultAmmoAmt;
        }
        // enable gun mesh
        this.gunMesh.enabled = true;
        // enable gun tracking
        this.gunTrackedObj.tracking = true;
        Debug.Log("Equip() called");
    }

    public void UnEquip()
    {
        // disable gun mesh
        this.gunMesh.enabled = false;
        // disable gun tracking
        this.gunTrackedObj.tracking = false; 
        Debug.Log("UnEquip() called");
    }

    public void ItemUpdate()
    {
        
        // update gun tracking position
        this.gunTrackedObj.CustomUpdate();

        if (this.localVRMode.IsLocal())
        {

            if (this.cdAfterEquip <= 0.0f)
            {

                // cooldown finishes, can read input

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
            else
            {
                // decrease equip cooldown
                this.cdAfterEquip -= Time.deltaTime;
            }

        }

    }

    private bool ReadInput()
    {

        if (!this.itemManager.enabledItemUse) return false;

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