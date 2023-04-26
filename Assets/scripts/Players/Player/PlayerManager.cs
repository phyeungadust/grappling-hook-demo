using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerManager : UdonSharpBehaviour
{

    [SerializeField]
    private CustomControls playerApiSafeControls;
    [SerializeField]
    private CustomControls localVRModeDeterminerControls;
    [SerializeField]
    private CustomControls[] customControls;
    [SerializeField]
    private PlayerStore ownerStore;
    [SerializeField]
    private GameObject[] deactivateIfNonLocal;
    
    public void CustomStart()
    {
        this.playerApiSafeControls.CustomStart();
        this.localVRModeDeterminerControls.CustomStart();
        if (!this.ownerStore.localVRMode.IsLocal())
        {
            foreach (GameObject go in this.deactivateIfNonLocal)
            {
                go.SetActive(false);
            }
        }
        foreach (CustomControls controllable in this.customControls)
        {
            if (controllable.gameObject.activeInHierarchy)
            {
                controllable.CustomStart();
            }
        }
    }

    public void CustomUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            if (controllable.gameObject.activeInHierarchy)
            {
                controllable.CustomUpdate();
            }
        }
    }

    public void CustomLateUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            if (controllable.gameObject.activeInHierarchy)
            {
                controllable.CustomLateUpdate();
            }
        }
    }

    public void CustomFixedUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            if (controllable.gameObject.activeInHierarchy)
            {
                controllable.CustomFixedUpdate();
            }
        }
    }

}