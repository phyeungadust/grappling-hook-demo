﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerHitbox : UdonSharpBehaviour
{

    public PlayerStore ownerStore;
    private Collider playerHitboxCollider;

    public Collider GetCollider() => this.playerHitboxCollider;

    void Awake()
    {
        this.playerHitboxCollider = this.GetComponent<Collider>();
    }

    public void CustomUpdate()
    {
        this.UpdateHitboxPos();
    }

    private void UpdateHitboxPos()
    {

        VRCPlayerApi playerApi = this.ownerStore.playerApiSafe.Get();

        Vector3 headPos = playerApi.GetTrackingData(
            VRCPlayerApi.TrackingDataType.Head
        ).position + new Vector3(0, .5f, 0);
        Vector3 leftFootPos = playerApi.GetBonePosition(HumanBodyBones.LeftFoot);
        Vector3 rightFootPos = playerApi.GetBonePosition(HumanBodyBones.RightFoot);
        Vector3 feetPos = (leftFootPos + rightFootPos) / 2 - new Vector3(0, .5f, 0);
        Vector3 bodyCenter = (headPos + feetPos) / 2;
        this.transform.position = bodyCenter;
        this.transform.localScale = new Vector3(1, (headPos - bodyCenter).magnitude, 1);

    }

}