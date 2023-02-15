using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerHitbox : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore playerStore;
    private Collider collider;

    public Collider GetCollider() => this.collider;

    void Awake()
    {
        this.collider = this.GetComponent<Collider>();
    }

    public void CustomUpdate()
    {
        this.UpdateHitboxPos();
    }

    private void UpdateHitboxPos()
    {

        VRCPlayerApi playerApi = this.playerStore.playerApiSafe.Get();

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