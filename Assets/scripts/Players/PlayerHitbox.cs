using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerHitbox : UdonSharpBehaviour
{

    [SerializeField]
    private VRCPlayerApiSafe vrcPlayerApiSafe;
    [SerializeField]
    private PlayerCameraFollower playerCameraFollower;

}