using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomTrackedObject : UdonSharpBehaviour
{

    public bool tracking = true;
    private Transform trackedTransform;
    
    [SerializeField]
    private string localVRTrackedPartString;
    [SerializeField]
    private string localNonVRTrackedPartString;
    [SerializeField]
    private string nonLocalVRTrackedPartString;
    [SerializeField]
    private string nonLocalNonVRTrackedPartString;

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;

    public void CustomStart()
    {
        this.localVRMode = this.ownerStore.localVRMode;
        if (this.localVRMode.IsLocal())
        {
            if (this.localVRMode.IsVR())
            {
                // localVR
                this.SetTrackedTransformByPartString(
                    this.localVRTrackedPartString
                );
            }
            else
            {
                // localNonVR
                this.SetTrackedTransformByPartString(
                    this.localNonVRTrackedPartString
                );
            }
        }
        else
        {
            if (this.localVRMode.IsVR())
            {
                // nonLocalVR
                this.SetTrackedTransformByPartString(
                    this.nonLocalVRTrackedPartString
                );
            }
            else
            {
                // nonLocalNonVR
                this.SetTrackedTransformByPartString(
                    this.nonLocalNonVRTrackedPartString
                );
            }
        }
    }

    public void CustomUpdate()
    {
        if (this.tracking)
        {
            this.transform.SetPositionAndRotation(
                this.trackedTransform.position,
                this.trackedTransform.rotation
            );
        }
    }

    private void SetTrackedTransformByPartString(string partString)
    {
        switch (partString)
        {
            case nameof(this.ownerStore.follower.head):
                this.trackedTransform = this.ownerStore.follower.head;
                break;
            case nameof(this.ownerStore.follower.leftHand):
                this.trackedTransform = this.ownerStore.follower.leftHand;
                break;
            case nameof(this.ownerStore.follower.rightHand):
                this.trackedTransform = this.ownerStore.follower.rightHand;
                break;
            case nameof(this.ownerStore.follower.leftFoot):
                this.trackedTransform = this.ownerStore.follower.leftFoot;
                break;
            case nameof(this.ownerStore.follower.rightFoot):
                this.trackedTransform = this.ownerStore.follower.rightFoot;
                break;
        }
    }

}