using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalVRModeDeterminer : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore playerStore;
    private VRCPlayerApi playerApi;

    [SerializeField]
    private LocalVR localVR;
    [SerializeField]
    private LocalNonVR localNonVR;
    [SerializeField]
    private NonLocalVR nonLocalVR;
    [SerializeField]
    private NonLocalNonVR nonLocalNonVR;

    public void CustomStart()
    {

        this.playerApi = this.playerStore.playerApiSafe.Get();

        if (this.playerApi.isLocal)
        {
            if (this.playerApi.IsUserInVR())
            {
                this.playerStore.localVRMode = this.localVR;
            }
            else
            {
                this.playerStore.localVRMode = this.localNonVR;
            }
        }
        else
        {
            if (this.playerApi.IsUserInVR())
            {
                this.playerStore.localVRMode = this.nonLocalVR;
            }
            else
            {
                this.playerStore.localVRMode = this.nonLocalNonVR;
            }
        }

    }

}