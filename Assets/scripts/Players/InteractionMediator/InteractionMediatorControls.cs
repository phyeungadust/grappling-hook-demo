using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class InteractionMediatorControls : CustomControls
{

    [SerializeField]
    private InteractionMediator mediator;

    public override void CustomStart()
    {
        this.mediator.CustomStart();
    }

}