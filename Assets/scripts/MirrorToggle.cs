using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class MirrorToggle : UdonSharpBehaviour
{

    [SerializeField]
    private GameObject mirror;
    [SerializeField]
    private GameObject light;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            this.mirror.SetActive(true);
            this.light.SetActive(true);
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            this.mirror.SetActive(false);
            this.light.SetActive(false);
        }
    }

}