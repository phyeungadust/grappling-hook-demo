using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalStratChoosersManager : LocalStratChooser
{

    [SerializeField]
    private LocalStratChooser[] localStratChoosers;

    public override void ChooseLocal()
    {
        foreach (LocalStratChooser strat in this.localStratChoosers)
        {
            strat.ChooseLocal();
        }
    }

    public override void ChooseNonLocal()
    {
        foreach (LocalStratChooser strat in this.localStratChoosers)
        {
            strat.ChooseNonLocal();
        }
    }

}