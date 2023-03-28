using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShopMenuToggle : UdonSharpBehaviour
{

    [SerializeField]
    private GameObject innerShopMenu;
    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;
    private VRCPlayerApi owner;
    private float playerGravity;
    [SerializeField]
    private CustomTrackedObject trackedObj;
    [SerializeField]
    private CustomGrip trackedGrip;

    // [SerializeField]
    // private WorldSpaceLogger wsLogger;

    private bool menuOpen = false;
    private bool keyPressed = false;

    public void CustomStart()
    {
        this.trackedObj.CustomStart();
        this.trackedGrip.CustomStart();
        this.localVRMode = this.ownerStore.localVRMode;
        this.owner = this.ownerStore.playerApiSafe.Get();
        this.playerGravity = this.owner.GetGravityStrength();
    }

    public void CustomUpdate()
    {
        if (this.ReadInput())
        {
            // toggle key pressed
            this.menuOpen = !this.menuOpen;
            this.HandleMenuToggle();
        }
        if (this.localVRMode.IsVR() && this.menuOpen)
        {
            // VR mode
            // keep updating trackedObj 
            this.trackedObj.CustomUpdate();
        }
    }

    private bool ReadInput()
    {

        bool keyHeld;

        if (this.localVRMode.IsVR())
        {
            keyHeld = Input
                .GetAxis("Oculus_CrossPlatform_PrimaryHandTrigger") > 0;
        }
        else
        {
            keyHeld = Input.GetKey(KeyCode.M);
        }

        // wsLogger.Log($"keyHeld: {keyHeld}");

        // since GetButton and GetKey read held-keys
        // below if-else returns correctly pressed-keys
        if (keyHeld)
        {
            if (this.keyPressed)
            {
                return false;
            }
            else
            {
                this.keyPressed = true;
                return true;
            }
        }
        else
        {
            this.keyPressed = false;
            return false;
        }

    }

    private void HandleMenuToggle()
    {

        if (this.menuOpen)
        {

            // menuClose -> menuOpen

            if (!this.localVRMode.IsVR())
            {
                // desktop mode
                // player unable to move when menu is open
                this.FreezePlayerCompletely(true);
            }
            // update trackedObj
            // (effectively teleporting menu to player)
            this.trackedObj.SendCustomEventDelayedFrames(
                nameof(this.trackedObj.CustomUpdate),
                0
            );
            // activate menu
            this.innerShopMenu.SetActive(true);

        }
        else
        {

            // menuOpen -> menuClose

            // deactivate menu
            this.innerShopMenu.SetActive(false);

            if (!this.localVRMode.IsVR())
            {
                // desktop mode 
                // player can move again after menu closes
                this.FreezePlayerCompletely(false);
            }

        }

    }

    private void FreezePlayerCompletely(bool freeze)
    {
        if (freeze)
        {
            this.owner.SetVelocity(Vector3.zero);
            if (!this.owner.IsPlayerGrounded())
            {
                // if plyaer in air
                // set gravity to zero so player doesn't fall
                this.owner.SetGravityStrength(0);
            }
            this.owner.Immobilize(true);
        }
        else
        {
            if (!this.owner.IsPlayerGrounded())
            {
                // restore gravity
                this.owner.SetGravityStrength(this.playerGravity);
            }
            this.owner.Immobilize(false);
        }
    }

}