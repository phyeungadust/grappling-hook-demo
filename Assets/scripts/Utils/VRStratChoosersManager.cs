using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class VRStratChoosersManager : VRStratChooser
{

    [SerializeField]
    private VRStratChooser[] VRStratChoosers;

    public override void ChooseVR()
    {
        foreach (VRStratChooser strat in this.VRStratChoosers)
        {
            strat.ChooseVR();
        }
    }

    public override void ChooseNonVR()
    {
        foreach (VRStratChooser strat in this.VRStratChoosers)
        {
            strat.ChooseNonVR();
        }
    }

}