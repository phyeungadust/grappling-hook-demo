using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class WaterSprayGunBehaviourGameStateControls : GameStateControls
{
    [SerializeField]
    private WaterSprayGunBehaviour sprayGun;
    public override void OnBeforeGameStarts()
    {
        this.sprayGun.OnBeforeGameStarts();
    }
}