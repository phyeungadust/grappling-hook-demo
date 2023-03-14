using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShellShootBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private VRCObjectPool shellPool;
    private ShellBehaviour shell;

    [SerializeField]
    private string vrButtonName = "Oculus_CrossPlatform_PrimaryIndexTrigger";
    [SerializeField]
    private string desktopButtonName = "Fire1";

    [SerializeField]
    private PlayerStore ownerStore;
    private VRCPlayerApi owner;
    private LocalVRMode localVRMode;

    [SerializeField]
    private float defaultCDAfterEquip = 1.0f;
    private float cdAfterEquip;

    [SerializeField]
    private ItemManager itemManager;

    public void Init()
    {
        this.owner = this.ownerStore.playerApiSafe.Get();
        this.localVRMode = this.ownerStore.localVRMode;
        Networking.SetOwner(
            this.owner,
            this.shellPool.gameObject
        );
    }

    public void Equip()
    {
        if (this.localVRMode.IsLocal())
        {
            GameObject spawnedShellObj = this.shellPool.TryToSpawn();
            if (spawnedShellObj != null)
            {
                // spawn success
                Networking.SetOwner(this.owner, spawnedShellObj);
                // get shell reference, when this reference is not null, 
                // shell is being held by player
                this.shell = spawnedShellObj.GetComponent<ShellBehaviour>();
                // set cooldown right after equip
                this.cdAfterEquip = this.defaultCDAfterEquip;
            }
        }
    }

    public void UnEquip()
    {
        if (this.localVRMode.IsLocal())
        {
            if (this.shell != null)
            {
                // shell still being held
                // return shell to shellPool if shell not launched
                this.shellPool.Return(this.shell.gameObject);
            }
        }
    }

    public void ItemUpdate()
    {

        if (this.localVRMode.IsLocal())
        {

            // local
            
            if (this.cdAfterEquip <= 0.0f)
            {
                // cooldown finishes, can read input
                if (this.ReadInput())
                {
                    // launch shell
                    this.shell.Launch();
                    // no need to keep reference to launched shell
                    this.shell = null;
                    // switch back to null item
                    this.itemManager.EquipNullItem();
                }
            }
            else
            {
                // decrease cooldown
                this.cdAfterEquip -= Time.deltaTime;
            }

        }

    }

    private bool ReadInput()
    {
        if (this.localVRMode.IsLocal())
        {
            if (this.localVRMode.IsVR())
            {
                // localVR
                return Input.GetAxis(this.vrButtonName) > 0;
            }
            else
            {
                // localNonVR
                return Input.GetButtonDown(this.desktopButtonName);
            }
        }
        // nonLocal, no need to read input
        return false;
    }

}