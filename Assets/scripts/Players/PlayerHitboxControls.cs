using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerHitboxControls : CustomControls
{

    [SerializeField]
    private PlayerHitbox playerHitbox;

    public override void CustomUpdate()
    {
        this.playerHitbox.CustomUpdate();
    }

}