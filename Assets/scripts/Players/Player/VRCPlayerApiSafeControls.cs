using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class VRCPlayerApiSafeControls : CustomControls
{
    [SerializeField]
    private VRCPlayerApiSafe playerApiSafe;
    public override void CustomStart()
    {
        this.playerApiSafe.CustomStart();
    }
}