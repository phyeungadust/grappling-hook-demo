using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DeactivatorCustomControlsManager : CustomControlsManager
{

    [SerializeField]
    private PlayerStore ownerStore;
    [SerializeField]
    private GameObject[] deactivateIfLocalVR;
    [SerializeField]
    private GameObject[] deactivateIfLocalNonVR;
    [SerializeField]
    private GameObject[] deactivateIfNonLocalVR;
    [SerializeField]
    private GameObject[] deactivateIfNonLocalNonVR;

    public override void CustomStart()
    {

        LocalVRMode localVRMode = this.ownerStore.localVRMode;

        if (localVRMode.IsLocal())
        {
            if (localVRMode.IsVR())
            {
                // localVR
                foreach (GameObject go in this.deactivateIfLocalVR)
                {
                    go.SetActive(false);
                }
            }
            else
            {
                // localNonVR
                foreach (GameObject go in this.deactivateIfLocalNonVR)
                {
                    go.SetActive(false);
                }
            }
        }
        else
        {
            if (localVRMode.IsVR())
            {
                // nonLocalVR
                foreach (GameObject go in this.deactivateIfNonLocalVR)
                {
                    go.SetActive(false);
                }
            }
            else
            {
                // nonLocalNonVR
                foreach (GameObject go in this.deactivateIfNonLocalNonVR)
                {
                    go.SetActive(false);
                }
            }
        }

        base.CustomStart();

    }

}