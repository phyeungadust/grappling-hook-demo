using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class NullItem : Item
{
    [SerializeField]
    private ItemControls nullItemControls;
    [SerializeField]
    private GameStateControls nullGameStateControls;
    public override ItemControls GetItemControls()
    {
        return this.nullItemControls;
    }
    public override GameStateControls GetGameStateControls()
    {
        return this.nullGameStateControls;
    }
}