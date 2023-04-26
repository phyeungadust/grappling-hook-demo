using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GameEndSequence : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;

    [SerializeField]
    private TextMeshProUGUI popupText;
    
    [SerializeField]
    private AnimationCurve finishedScaleCurve;
    [SerializeField]
    private AnimationCurve finishedOpacityCurve;
    [SerializeField]
    private float finishedDuration = 4.0f;

    [SerializeField]
    private AnimationCurve placeScaleCurve;
    [SerializeField]
    private AnimationCurve placeOpacityCurve;
    [SerializeField]
    private float placeDuration = 4.0f;

    private int actualPlace;
    private string actualPlaceInStr;

    [SerializeField]
    private AnimationCurve translationWalletCurve;
    [SerializeField]
    private AnimationCurve opacityWalletCurve;
    [SerializeField]
    private float walletDuration = 1.5f;

    [SerializeField]
    private GameObject moneyPanel;
    private float moneyPanelInitialY;
    [SerializeField]
    private float moneyPanelFinalOffset = -150.0f;

    [SerializeField]
    private MoneyTextAnim moneyTextAnim;

    private float progress = 0.0f;
    
    // states:
    // none
    // finished
    // place
    // wallet
    // wallet increase
    private string currentState = "none";

    void Start()
    {
        this.moneyPanelInitialY = this.moneyPanel.transform.localPosition.y;
    }

    public void PlaySequence()
    {
        // enable gameobject and UdonBehaviour so that Update() runs
        this.gameObject.SetActive(true);
        this.enabled = true;
        this.SwitchState("finished");
    }

    void Update()
    {
        switch (this.currentState)
        {
            case "finished":
                this.popupText.color = new Color(
                    this.popupText.color.r,
                    this.popupText.color.g,
                    this.popupText.color.b,
                    this.finishedOpacityCurve.Evaluate(
                        this.progress / this.finishedDuration
                    )
                );
                this.popupText.transform.localScale = Vector3.one * this
                    .finishedScaleCurve
                    .Evaluate(
                        this.progress / this.finishedDuration
                    );
                this.progress += Time.deltaTime;
                if (this.progress > this.finishedDuration)
                {
                    this.SwitchState("place");
                }
                break;
            case "place":
                this.popupText.color = new Color(
                    this.popupText.color.r,
                    this.popupText.color.g,
                    this.popupText.color.b,
                    this.placeOpacityCurve.Evaluate(
                        this.progress / this.placeDuration
                    )
                );
                this.popupText.transform.localScale = Vector3.one * this
                    .placeScaleCurve
                    .Evaluate(
                        this.progress / this.placeDuration
                    );
                this.progress += Time.deltaTime;
                if (this.progress > this.placeDuration)
                {
                    this.SwitchState("wallet");
                }
                break;
            case "wallet":

                float newYPos = 
                    this.moneyPanelInitialY 
                    + (1 - this.translationWalletCurve.Evaluate(
                        this.progress / this.walletDuration
                    )) 
                    * this.moneyPanelFinalOffset;

                this.moneyPanel.transform.localPosition = new Vector3(
                    this.moneyPanel.transform.localPosition.x,
                    newYPos,
                    this.moneyPanel.transform.localPosition.z
                );
                this.progress += Time.deltaTime;
                if (this.progress > this.walletDuration)
                {
                    this.moneyPanel.transform.localPosition = new Vector3(
                        this.moneyPanel.transform.localPosition.x,
                        this.moneyPanelInitialY + this.moneyPanelFinalOffset,
                        this.moneyPanel.transform.localPosition.z
                    );
                    this.SwitchState("walletIncrease");
                }
                break;
            case "walletIncrease":
                break;
        }
    }

    void Enter()
    {
        switch (this.currentState)
        {
            case "none":
                this.popupText.enabled = false;
                this.moneyPanel.SetActive(false);
                this.moneyPanel.transform.localPosition = new Vector3(
                    this.moneyPanel.transform.localPosition.x,
                    this.moneyPanelInitialY,
                    this.moneyPanel.transform.localPosition.z
                );
                this.gameObject.SetActive(false);
                break;
            case "finished":
                this.popupText.enabled = false;
                this.progress = 0.0f;
                this.popupText.text = "FINISHED!";
                this.popupText.color = new Color(
                    this.popupText.color.r,
                    this.popupText.color.g,
                    this.popupText.color.b,
                    this.finishedOpacityCurve.Evaluate(
                        this.progress / this.finishedDuration
                    )
                );
                this.popupText.transform.localScale = Vector3.one * this
                    .finishedScaleCurve
                    .Evaluate(
                        this.progress / this.finishedDuration
                    );
                this.popupText.enabled = true;
                break;
            case "place":
                this.popupText.enabled = false;
                this.progress = 0.0f;
                this.CalculatePlace();
                this.popupText.text = $"You got {this.actualPlaceInStr} place!";
                this.popupText.color = new Color(
                    this.popupText.color.r,
                    this.popupText.color.g,
                    this.popupText.color.b,
                    this.placeOpacityCurve.Evaluate(
                        this.progress / this.placeDuration
                    )
                );
                this.popupText.transform.localScale = Vector3.one * this
                    .placeScaleCurve
                    .Evaluate(
                        this.progress / this.placeDuration
                    );
                this.popupText.enabled = true;
                break;
            case "wallet":

                this
                    .moneyTextAnim
                    .SetInitAmount(this.ownerStore.wallet.GetAmount());

                this.moneyPanel.SetActive(true);
                this.progress = 0.0f;


                break;
            case "walletIncrease":

                // play wallet increase animation
                int rewardAmount = 125 - 25 * this.actualPlace;
                this.moneyTextAnim.MoneyChangeAnimStart(rewardAmount);

                // actually reward the player with money (outside the animation)
                this.ownerStore.wallet.Add(rewardAmount);

                // disable script so that Update() stops running
                this.enabled = false;

                break;

        }
    }

    public void OnTextAnimFinished()
    {
        this.SwitchState("none");
        this
            .ownerStore
            .playerStoreCollection
            .customGameManager
            .OnGameEndSequenceFinishes();
    }

    private void SwitchState(string newState)
    {
        this.currentState = newState;
        this.Enter();
    }

    private void CalculatePlace()
    {

        int target = this.ownerStore.score.GetAmount();

        PlayerStore[] playerStores = this
            .ownerStore
            .playerStoreCollection
            .GetAll();

        int playerCount = this.ownerStore.playerStoreCollection.GetCount();

        int[] scores = new int[playerCount];

        for (int i = 0; i < scores.Length; ++i)
        {
            scores[i] = playerStores[i + 1].score.GetAmount();
        }

        int position = 1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > target)
            {
                position++;
            }
            else if (scores[i] == target)
            {
                break;
            }
        }

        this.actualPlace = position;

        switch (position)
        {
            case 1: this.actualPlaceInStr = "1st"; break;
            case 2: this.actualPlaceInStr = "2nd"; break;
            case 3: this.actualPlaceInStr = "3rd"; break;
            case 4: this.actualPlaceInStr = "4th"; break;
            default: break;
        }

    }

}