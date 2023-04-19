using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ItemManagerGameStateControls : GameStateControls
{
    [SerializeField]
    private ItemManager itemManager;
    public override void OnBeforeGameStarts()
    {
        this.itemManager.OnBeforeGameStarts();
    }
}