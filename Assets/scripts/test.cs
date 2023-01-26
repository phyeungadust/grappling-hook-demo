using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class test : UdonSharpBehaviour
{

    VRCPlayerApi player = null;
    VRCPlayerApi localPlayer;

    void Start()
    {
        this.localPlayer = Networking.LocalPlayer;
        while (this.player == null)
        {
            this.player = VRCPlayerApi.GetPlayerById(1);
        }
    }

    void Update()
    {

        if (this.player == null) return;

        

        Vector3 headPos = this.player.GetTrackingData(
            VRCPlayerApi.TrackingDataType.Head
        ).position + new Vector3(0, .5f, 0);
        Vector3 leftFootPos = this.player.GetBonePosition(HumanBodyBones.LeftFoot);
        Vector3 rightFootPos = this.player.GetBonePosition(HumanBodyBones.RightFoot);
        Vector3 feetPos = (leftFootPos + rightFootPos) / 2 - new Vector3(0, .5f, 0);
        Vector3 bodyCenter = (headPos + feetPos) / 2;
        this.transform.position = bodyCenter;
        this.transform.localScale = new Vector3(1, (headPos - bodyCenter).magnitude, 1);

        Debug.Log("playerID: " + this.localPlayer.playerId);

    }

}