﻿using UdonSharp;
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
            }
            // enable innerUI 
            // (this inner UI remains the same regardless of world/ screen space)
            this.innerUI.SetActive(true);
            this.innerUI.transform.localPosition = Vector3.zero;
        }
        // nonLocal
        // no need to enable UI 
    }

    public void CustomUpdate()
    {
        // int playerID = this.ownerStore.playerApiSafe.GetID();
        // Debug.Log($"player {playerID}'s hudWrapper called CustomUpdate()!");
    }

}