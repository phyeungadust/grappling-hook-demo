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

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         this.ownerStore.score.Deduct(10);
    //         this.hudScoreChangeAnim.ScoreChangeAnimStart(-10, "SAMPLE TEXT");
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         this.ownerStore.score.Add(10);
    //         this.hudScoreChangeAnim.ScoreChangeAnimStart(10, "SAMPLE TEXT");
    //     }
    // }

    public void ChangeScoreAmount(int amount, string hitMsg)
    {
        this.ownerStore.score.SetAmount(
            this.ownerStore.score.GetAmount() + amount
        );
        this.hudScoreChangeAnim.ScoreChangeAnimStart(amount, hitMsg);
    }

}