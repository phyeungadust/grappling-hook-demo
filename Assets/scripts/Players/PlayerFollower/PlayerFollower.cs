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
        
    }

    private void UpdateLeftHand()
    {

    }

    private void UpdateRightHand()
    {

    }

    private void UpdateLeftFoot()
    {

    }

    private void UpdateRightFoot()
    {

    }

}