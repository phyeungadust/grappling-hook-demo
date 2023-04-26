using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerFollowerControls : CustomControls
{

    [SerializeField]
    private PlayerFollower follower;

    public override void CustomStart()
    {
        this.follower.CustomStart();
    }

    public override void CustomUpdate()
    {
        this.follower.CustomUpdate();
    }

}