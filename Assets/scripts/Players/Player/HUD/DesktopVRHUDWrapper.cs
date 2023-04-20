using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DesktopVRHUDWrapper : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;
    [SerializeField]
    private GameObject desktopHUD;
    [SerializeField]
    private GameObject vrHUD;
    [SerializeField]
    private GameObject innerUI;
    [SerializeField]
    private GameObject raceUI;
    [SerializeField]
    private CustomTrackedObject trackedObj;

    [SerializeField]
    private GameStateControls timerGameStateControls;

    public void OnBeforeGameStarts()
    {
        // // deactivate raceUI
        // this.raceUI.SetActive(false);
        // // popup text remains active
    }

    public void CustomStart()
    {

        this.localVRMode = this.ownerStore.localVRMode;

        if (this.localVRMode.IsLocal())
        {

            if (this.localVRMode.IsVR())
            {
                // localVR
                this.innerUI.transform.parent = this.vrHUD.transform;
                this.vrHUD.SetActive(true); // enable world-space HUD
            }
            else
            {
                // localNonVR
                this.innerUI.transform.parent = this.desktopHUD.transform;
                this.desktopHUD.SetActive(true); // enable screen-space HUD
                this.trackedObj.tracking = false; // HUD rendered in screen-space, tracking not needed
            }
            // enable innerUI 
            // (this inner UI remains the same regardless of world/ screen space)
            this.innerUI.SetActive(true);
            this.innerUI.transform.localPosition = Vector3.zero;
            this.innerUI.transform.localScale = Vector3.one;

            // subscribe timer to game state changes
            this
                .ownerStore
                .playerStoreCollection
                .customGameManager
                .SubscribeToGameStateChanges(this.timerGameStateControls);

        }
        // nonLocal
        // no need to enable UI 

    }

}