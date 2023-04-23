using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class MoneyTextAnim : UdonSharpBehaviour
{

    [SerializeField]
    private TextMeshProUGUI moneyText;
    private int cachedMoneyValue = 0;
    private int targetMoneyValue = 0;
    private int cachedChangeAmount = 0;
    [SerializeField]
    private float moneyTextChangeTime = 2.0f;

    [SerializeField]
    private TextMeshProUGUI moneyChangeText;
    [SerializeField]
    private AnimationCurve moneyChangeOpacityCurve;
    [SerializeField]
    private Color moneyUpTextColor;
    [SerializeField]
    private Color moneyDownTextColor;
    [SerializeField]
    private float moneyChangeTextWearTime = 3.75f;
    private float timeElapsedSinceLastMoneyChange = 0.0f;
    private bool moneyChangeTextDisplaying = false;

    [SerializeField]
    private GameEndSequence gameEndSequence;

    // public void CustomStart()
    // {
    //     this.cachedMoneyValue = 0;
    //     this.targetMoneyValue = 0;
    //     this.moneyChangeText.enabled = false;
    //     this.moneyChangeTextDisplaying = false;
    // }

    public void Update()
    {

        if (this.cachedMoneyValue != this.targetMoneyValue)
        {

            if (this.cachedMoneyValue < this.targetMoneyValue)
            {

                // the goal here is to increase this.cachedMoneyValue by a variable
                // step amount every frame, until this.cachedMoneyValue == this.targetMoneyValue

                // this variable step amount is
                // calculated in a way such that
                // the animation takes this.moneyTextChangeTime
                // to complete

                // this variable step amount, named changeAmtPerFrame,
                // is calculated by taking a fractional portion of the total
                // this.cachedChangeAmount

                // such fraction is simply, (Time.deltaTime / this.moneyTextChangeTime)
                // which denotes a fractional step of animation represented in float

                // in particular, this.cachedChangeAmount * (Time.deltaTime / this.moneyTextChangeTime)
                // would be how much value should be increased based on how much time has passed since last frame
                // aka, changeAmtPerFrame

                int changeAmtPerFrame = Mathf.CeilToInt(
                    this.cachedChangeAmount
                    * (Time.deltaTime / this.moneyTextChangeTime)
                );

                this.cachedMoneyValue = Mathf.Min(
                    this.cachedMoneyValue + changeAmtPerFrame,
                    this.targetMoneyValue
                );

            }
            else
            {

                // basically, this.cachedMoneyValue > this.targetMoneyValue

                // similar to the branch above, but this time we decrease
                // this.cachedMoneyValue by a variable step amount
                // until this.cachedMoneyValue == this.targetMoneyValue

                int changeAmtPerFrame = Mathf.FloorToInt(
                    this.cachedChangeAmount
                    * (Time.deltaTime / this.moneyTextChangeTime)
                );
                
                this.cachedMoneyValue = Mathf.Max(
                    this.cachedMoneyValue + changeAmtPerFrame,
                    this.targetMoneyValue
                );

            }

            // actually display the cachedMoneyValue on the menu
            this.moneyText.text = $"${this.cachedMoneyValue}";

        }

        if (this.moneyChangeTextDisplaying)
        {

            if (
                this.timeElapsedSinceLastMoneyChange 
                > this.moneyChangeTextWearTime
            )
            {
                this.moneyChangeText.enabled = false;
                this.moneyChangeTextDisplaying = false;
                this.OnTextAnimFinished();
            }
            else
            {

                float AnimProgress = 
                    this.timeElapsedSinceLastMoneyChange 
                    / this.moneyChangeTextWearTime;

                float opacity = this
                    .moneyChangeOpacityCurve
                    .Evaluate(AnimProgress);

                this.moneyChangeText.color = new Color(
                    this.moneyChangeText.color.r,
                    this.moneyChangeText.color.g,
                    this.moneyChangeText.color.b,
                    opacity
                );

                this.timeElapsedSinceLastMoneyChange += Time.deltaTime;

            }

        }

    }

    private void OnTextAnimFinished()
    {

        // disable script such that Update() stops running
        this.enabled = false;

        // wait for some time before notifying GameEndSequence
        // moneyTextAnim has finished
        this.SendCustomEventDelayedSeconds(
            nameof(this.NotifyGameEndSequenceAnimFinished),
            10.0f
        );

    }

    public void NotifyGameEndSequenceAnimFinished()
    {
        this.gameEndSequence.OnTextAnimFinished();
    }

    public void MoneyChangeAnimStart(int amount, bool openingMenu = false)
    {

        // activate gameObject
        this.gameObject.SetActive(true);
        // enable script such that Update() runs
        this.enabled = true;

        this.targetMoneyValue += amount;

        this.cachedChangeAmount = this.targetMoneyValue - this.cachedMoneyValue;

        if (!openingMenu)
        {

            // if this animation plays when menu is being opened
            // no need to show moneyChangeText

            // reset time elapsed
            this.timeElapsedSinceLastMoneyChange = 0.0f;
            // enable moneyChangeText
            this.moneyChangeText.enabled = true;

            this.moneyChangeTextDisplaying = true;

            if (amount >= 0)
            {
                // moneyUp
                this.moneyChangeText.text = $"(+${amount})";
                this.moneyChangeText.color = this.moneyUpTextColor;
            }
            else
            {
                // moneyDown
                this.moneyChangeText.text = $"(-${-amount})";
                this.moneyChangeText.color = this.moneyDownTextColor;
            }

        }

    }

    public MoneyTextAnim SetInitAmount(int amount)
    {
        this.cachedMoneyValue = amount;
        this.targetMoneyValue = amount;
        this.moneyText.text = $"${amount}";
        return this;
    }

}