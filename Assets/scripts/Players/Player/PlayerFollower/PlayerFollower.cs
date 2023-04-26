using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerFollower : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;
    private VRCPlayerApi owner;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftFoot;
    public Transform rightFoot;

    public void CustomStart()
    {
        this.owner = this.ownerStore.playerApiSafe.Get();
    }

    public void CustomUpdate()
    {
        this.UpdateHead();
        this.UpdateLeftHand();
        this.UpdateRightHand();
        this.UpdateLeftFoot();
        this.UpdateRightFoot();
    }

    private void UpdateHead()
    {
        VRCPlayerApi.TrackingData tt = this
            .owner
            .GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        this.head.transform.SetPositionAndRotation(
            tt.position,
            tt.rotation
        );
    }

    private void UpdateLeftHand()
    {
        VRCPlayerApi.TrackingData tt = this
            .owner
            .GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
        this.leftHand.transform.SetPositionAndRotation(
            tt.position,
            tt.rotation
        );
    }

    private void UpdateRightHand()
    {
        VRCPlayerApi.TrackingData tt = this
            .owner
            .GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand);
        this.rightHand.transform.SetPositionAndRotation(
            tt.position,
            tt.rotation
        );
    }

    private void UpdateLeftFoot()
    {
        Vector3 leftFootPos = this
            .owner
            .GetBonePosition(HumanBodyBones.LeftFoot);
        Quaternion leftFootRotation = this
            .owner
            .GetBoneRotation(HumanBodyBones.LeftFoot);
        this.leftFoot.transform.SetPositionAndRotation(
            leftFootPos,
            leftFootRotation
        );
    }

    private void UpdateRightFoot()
    {
        Vector3 rightFootPos = this
            .owner
            .GetBonePosition(HumanBodyBones.RightFoot);
        Quaternion rightFootRotation = this
            .owner
            .GetBoneRotation(HumanBodyBones.RightFoot);
        this.rightFoot.transform.SetPositionAndRotation(
            rightFootPos,
            rightFootRotation
        );
    }

}