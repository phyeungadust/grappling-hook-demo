using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUDScoreControllerGameStateControls : GameStateControls
{
    [SerializeField]
    private HUDScoreController hudScoreController;
    public override void OnBeforeGameStarts()
    {
        this.hudScoreController.OnBeforeGameStarts();
    }
    public override void OnAfterGameEnds()
    {
        this.hudScoreController.OnAfterGameEnds();
    }
}