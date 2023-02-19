using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerManager : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;
    private VRCPlayerApi owner;

    [SerializeField]
    private CustomControls[] customControls;

    [SerializeField]
    private LocalStratChooser[] localStratChoosers;

    [SerializeField]
    private VRStratChooser[] VRStratChoosers;

    void Start()
    {

        this.ownerStore.playerApiSafe.CustomStart();

        this.owner = this.ownerStore.playerApiSafe.Get();

        if (this.owner.isLocal)
        {
            foreach (LocalStratChooser stratChooser in this.localStratChoosers)
            {
                stratChooser.ChooseLocal();
            }
        }
        else
        {
            foreach (LocalStratChooser stratChooser in this.localStratChoosers)
            {
                stratChooser.ChooseNonLocal();
            }
        }

        if (this.owner.IsUserInVR())
        {
            foreach (VRStratChooser stratChooser in this.VRStratChoosers)
            {
                stratChooser.ChooseVR();
            }
        }
        else
        {
            foreach (VRStratChooser stratChooser in this.VRStratChoosers)
            {
                stratChooser.ChooseNonVR();
            }
        }

        foreach (CustomControls controllable in this.customControls)
        {
            controllable.CustomStart();
        }

    }

    void Update()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            controllable.CustomUpdate();
        }
    }

    void LateUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            controllable.CustomLateUpdate();
        }
    }

    void FixedUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            controllable.CustomFixedUpdate();
        }
    }

}