using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Grip : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;
    [SerializeField]
    private GripSetTransformLocalVRVisitor setTransformLocalVRVisitor;

    public void CustomStart()
    {
        this.localVRMode = this.ownerStore.localVRMode;
        this.localVRMode.Accept(this.setTransformLocalVRVisitor);
    }

}