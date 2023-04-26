using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShopMenuGameStateControls : GameStateControls
{
    [SerializeField]
    private ShopMenuController shopMenuController;
    public override void OnBeforeGameStarts()
    {
        this.shopMenuController.OnBeforeGameStarts();
    }
    public override void OnAfterGameEnds()
    {
        this.shopMenuController.OnAfterGameEnds();
    }
}