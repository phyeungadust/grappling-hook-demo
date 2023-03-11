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
    
    void Start()
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
            if (
                controllable != null 
                && controllable.gameObject.activeInHierarchy
            )
            {
                controllable.CustomStart();
            }
        }
    }

    void Update()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            if (
                controllable != null 
                && controllable.gameObject.activeInHierarchy
            )
            {
                controllable.CustomUpdate();
            }
        }
    }

    void LateUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            if (
                controllable != null 
                && controllable.gameObject.activeInHierarchy
            )
            {
                controllable.CustomLateUpdate();
            }
        }
    }

    void FixedUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            if (
                controllable != null 
                && controllable.gameObject.activeInHierarchy
            )
            {
                controllable.CustomFixedUpdate();
            }
        }
    }

}