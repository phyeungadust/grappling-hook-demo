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
        GameObject spawnedShellObj = this
            .shellPool
            .TryToSpawn();
        if (spawnedShellObj != null)
        {
            // spawn success
            Networking.SetOwner(
                this.owner,
                spawnedShellObj
            );
            this.shell = spawnedShellObj.GetComponent<ShellBehaviour>();
            // set cooldown right after equip
            this.cdAfterEquip = this.defaultCDAfterEquip;
        }
    }

    public void UnEquip()
    {
        if (this.shell != null)
        {
            // shell really is equipped
            if (!this.shell.launched)
            {
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
            
            bool fireButtonPressed = false;
            if (this.cdAfterEquip <= 0.0f)
            {
                // cooldown finishes, can read input
                fireButtonPressed = this.ReadInput();
            }

            if (fireButtonPressed)
            {
                // launch shell and switch to null item
                this.shell.Launch();
                this.itemManager.EquipNullItem();
            }

            this.cdAfterEquip -= Time.deltaTime;

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