using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShopMenuToggle : UdonSharpBehaviour
{

    [SerializeField]
    private ShopMenuController shopMenuController;

    [SerializeField]
    private GameObject innerShopMenu;
    [SerializeField]
    private CanvasGroup innerShopMenuCanvasGroup;

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;
    private VRCPlayerApi owner;
    private Tether.TetherController tetherController;
    private ItemManager itemManager;
    private float playerGravity;

    [SerializeField]
    private CustomTrackedObject trackedObj;
    [SerializeField]
    private CustomGrip trackedGrip;
    private float menuLocalScale;

    [SerializeField]
    private AnimationCurve opacityCurve;
    [SerializeField]
    private AnimationCurve scaleCurve;
    [SerializeField]
    private float toggleAnimationTime = 1.5f;
    private float timeElapsedSinceToggle = 0.0f;

    private bool playingToggleAnimation = false;
    private bool menuOpen = false;
    private bool keyPressed = false;

    private string currentState = "menuClosed";
    private float toggleProgress = 0.0f;

    // states:
    // menuOpened
    // menuClosed
    // menuOpening
    // menuClosing

    public void CustomStart()
    {

        // set tracking of shopMenu
        this.trackedObj.CustomStart();
        this.trackedGrip.CustomStart();

        // this is the scale of the menu when it is fully toggled open
        this.menuLocalScale = this.trackedGrip.transform.localScale.x;

        // menu scale and opacity are 0 before toggled open
        this.trackedGrip.transform.localScale = Vector3.zero;
        this.innerShopMenuCanvasGroup.alpha = 0;

        // menu is closed initially, deactivate it
        this.innerShopMenu.SetActive(false);

        this.localVRMode = this.ownerStore.localVRMode;
        this.owner = this.ownerStore.playerApiSafe.Get();

        this.playerGravity = this.owner.GetGravityStrength();

        this.tetherController = this.ownerStore.tetherController;
        this.itemManager = this.ownerStore.itemManager;

    }

    public void CustomUpdate()
    {

        // wsLogger.Log($"grounded: {this.owner.IsPlayerGrounded()}");

        switch (this.currentState)
        {

            case "menuOpened":

                if (this.localVRMode.IsVR())
                {
                    this.trackedObj.CustomUpdate();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.N))
                    {
                        // reposition menu if N is pressed
                        this.trackedObj.CustomUpdate();
                    }
                }
                if (this.ReadInput())
                {
                    this.SwitchState("menuClosing");
                }
                break;

            case "menuClosed":

                if (this.ReadInput())
                {
                    this.SwitchState("menuOpening");
                }
                break;

            case "menuOpening":

                if (this.localVRMode.IsVR())
                {
                    this.trackedObj.CustomUpdate();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.N))
                    {
                        // reposition menu if N is pressed
                        this.trackedObj.CustomUpdate();
                    }
                }

                this.EvaluateAnimationStep();

                if (Mathf.Abs(this.toggleProgress - 1.0f) <= 1e-4)
                {
                    // this.toggleProgress == 1.0f (approx.)
                    this.SwitchState("menuOpened");
                    return;
                }

                // progress does not go above 1.0f
                this.toggleProgress = Mathf.Min(
                    this.toggleProgress 
                    + Time.deltaTime / this.toggleAnimationTime,
                    1.0f
                );

                break;

            case "menuClosing":

                if (this.localVRMode.IsVR())
                {
                    this.trackedObj.CustomUpdate();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.N))
                    {
                        // reposition menu if N is pressed
                        this.trackedObj.CustomUpdate();
                    }
                }

                this.EvaluateAnimationStep();

                if (Mathf.Abs(this.toggleProgress - 0.0f) <= 1e-4)
                {
                    // this.toggleProgress == 0.0f (approx.)
                    this.SwitchState("menuClosed");
                    return;
                }

                // progress does not go below 0.0f
                this.toggleProgress = Mathf.Max(
                    this.toggleProgress 
                    - Time.deltaTime / this.toggleAnimationTime,
                    0.0f
                );

                break;

        }

    }

    private void Enter()
    {

        switch (this.currentState)
        {
            case "menuOpened":
                this.toggleProgress = 1.0f;
                break;
            case "menuClosed":
                this.toggleProgress = 0.0f;
                if (!this.localVRMode.IsVR())
                {
                    // desktop mode 
                    // player can move again after menu is closed
                    this.FreezePlayerCompletely(false);
                }
                // menu is closed, deactivate menu
                this.innerShopMenu.SetActive(false);
                // inform shopMenuController menu is toggled off
                this.shopMenuController.OnMenuToggledOff();

                // reenable tethering and tether raycast check
                this.tetherController.EnableUpdate(true);

                // reenable equipped item use
                this.itemManager.enabledItemUse = true;

                break;
            default:
                break;
        }

    }

    private void Exit()
    {

        switch (this.currentState)
        {
            case "menuClosed":

                // menu about to be opened

                if (!this.localVRMode.IsVR())
                {
                    // desktop mode
                    // player unable to move when menu is opening
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
                // inform shopMenuController menu is toggled on
                this.shopMenuController.OnMenuToggledOn();

                // disable tethering and tether raycast check
                this.tetherController.SwitchState(
                    this.tetherController.TetherStatesDict.TetherNoneState
                );
                this.tetherController.EnableUpdate(false);

                // disable equipped item Use
                this.itemManager.enabledItemUse = false;

                break;
            default:
                break;
        }

    }

    private void SwitchState(string newState)
    {
        this.Exit();
        this.currentState = newState;
        this.Enter();
    }

    private void EvaluateAnimationStep()
    {
        this.innerShopMenuCanvasGroup.alpha = this
            .opacityCurve
            .Evaluate(this.toggleProgress);
        this.trackedGrip.transform.localScale = this
            .scaleCurve
            .Evaluate(this.toggleProgress) 
                * this.menuLocalScale 
                * Vector3.one;
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

    private void FreezePlayerCompletely(bool freeze)
    {
        if (freeze)
        {
            if (!this.owner.IsPlayerGrounded())
            {
                // if plyaer in air
                // set gravity to zero so player doesn't fall
                this.owner.SetGravityStrength(0);
            }
            this.owner.SetVelocity(Vector3.zero);
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