using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerManager : UdonSharpBehaviour
{

    [SerializeField]
    private CustomControls[] customControls;

    void Start()
    {
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

    void FixedUpdate()
    {
        foreach (CustomControls controllable in this.customControls)
        {
            controllable.CustomFixedUpdate();
        }
    }

}