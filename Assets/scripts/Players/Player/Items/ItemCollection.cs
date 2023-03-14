﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemCollection : UdonSharpBehaviour
{

    public ShellShootBehaviour shellShoot;
    public WaterSprayGunBehaviour sprayGun;
    public ItemControls nullItemControls;
    [HideInInspector]
    public ShellShootBehaviourControls shellShootBehaviourControls;
    [HideInInspector]
    public WaterSprayGunBehaviourControls waterSprayGunBehaviourControls;
    [HideInInspector]
    public ItemControls[] itemControlsArr;

    public void Init()
    {
        this.shellShootBehaviourControls = this
            .shellShoot
            .GetComponent<ShellShootBehaviourControls>();
        this.waterSprayGunBehaviourControls = this
            .sprayGun
            .GetComponent<WaterSprayGunBehaviourControls>();
        this.itemControlsArr = new ItemControls[] {
            this.nullItemControls,
            this.shellShootBehaviourControls,
            this.waterSprayGunBehaviourControls
        };
        // assign every item its own ID
        for (int i = 0; i < this.itemControlsArr.Length; ++i)
        {
            this.itemControlsArr[i].SetItemID(i);
        }
    }

}