using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUDScoreController : UdonSharpBehaviour
{

    [SerializeField]
    private HUDScoreChangeAnim hudScoreChangeAnim;
    [SerializeField]
    private PlayerStore ownerStore;

    public void OnBeforeGameStarts()
    {
        this.ownerStore.score.SetAmount(0);
        this.hudScoreChangeAnim.ResetToZero();
    }

    public void OnAfterGameEnds()
    {
        this.ownerStore.score.SetAmount(0);
        this.hudScoreChangeAnim.ResetToZero();
    }

    public void ChangeScoreAmount(int amount, string hitMsg)
    {
        this.ownerStore.score.SetAmount(
            this.ownerStore.score.GetAmount() + amount
        );
        this.hudScoreChangeAnim.ScoreChangeAnimStart(amount, hitMsg);
    }

}