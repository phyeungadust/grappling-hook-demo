using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUDScoreChangeAnim : UdonSharpBehaviour
{

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private CanvasGroup hitMsgPanel;
    [SerializeField]
    private TextMeshProUGUI hitMsg;
    [SerializeField]
    private TextMeshProUGUI getHitMsg;
    [SerializeField]
    private AnimationCurve intensityCurve;
    [SerializeField]
    private float duration = 3.75f;
    private float progress = 0.0f;
    private int cachedScore = 0;
    private string hitMsgString = "";

    // states:
    // scoreUp
    // scoreDown
    // noChange
    private string currentState = "noChange";

    void Update()
    {
        float intensity;
        switch (this.currentState)
        {
            case "noChange":
                break;
            case "scoreUp":
                intensity = this.intensityCurve.Evaluate(
                    this.progress / this.duration
                );
                this.scoreText.color = new Color(
                    intensity,
                    1.0f,
                    intensity,
                    1.0f
                );
                this.hitMsgPanel.alpha = 1.0f - intensity;
                this.progress += Time.deltaTime;
                if (this.progress > this.duration)
                {
                    this.SwitchState("noChange");
                }
                break;
            case "scoreDown":
                intensity = this.intensityCurve.Evaluate(
                    this.progress / this.duration
                );
                this.scoreText.color = new Color(
                    1.0f,
                    intensity,
                    intensity,
                    1.0f
                );
                this.hitMsgPanel.alpha = 1.0f - intensity;
                this.progress += Time.deltaTime;
                if (this.progress > this.duration)
                {
                    this.SwitchState("noChange");
                }
                break;
            default:
                break;
        }
    }

    private void Enter()
    {
        switch (this.currentState)
        {
            case "noChange":
                // disable Update()
                this.enabled = false;
                this.hitMsgPanel.gameObject.SetActive(false);
                this.scoreText.color = Color.white;
                this.progress = 0.0f;
                break;
            case "scoreUp":

                // change messageString and reset panel opacity
                this.hitMsg.text = this.hitMsgString;
                this.hitMsgPanel.alpha = 1.0f;

                // activate hitMsgPanel if not already activated
                if (!this.hitMsgPanel.gameObject.activeSelf)
                {
                    this.hitMsgPanel.gameObject.SetActive(true);
                }

                // reactivate hit msg
                this.hitMsg.gameObject.SetActive(true);

                this.scoreText.color = Color.green;
                this.progress = 0.0f;

                break;
            case "scoreDown":

                // change messageString and reset panel opacity
                this.getHitMsg.text = this.hitMsgString;
                this.hitMsgPanel.alpha = 1.0f;

                // activate hitMsgPanel if not already activated
                if (!this.hitMsgPanel.gameObject.activeSelf)
                {
                    this.hitMsgPanel.gameObject.SetActive(true);
                }

                // reactivate hit msg
                this.getHitMsg.gameObject.SetActive(true);

                this.scoreText.color = Color.red;
                this.progress = 0.0f;

                break;
            default:
                break;
        }
    }

    private void Exit()
    {
        switch (this.currentState)
        {
            case "noChange":
                // enable Update()
                this.enabled = true;
                break;
            case "scoreUp":
                this.hitMsg.gameObject.SetActive(false);
                break;
            case "scoreDown":
                this.getHitMsg.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void SwitchState(string newState)
    {
        this.Exit();
        this.currentState = newState;
        this.Enter();
    }

    public void ScoreChangeAnimStart(int amount, string message = "")
    {
        this.cachedScore += amount;
        this.hitMsgString = message;
        this.scoreText.text = this.cachedScore.ToString();
        if (amount >= 0)
        {
            this.SwitchState("scoreUp");
        }
        else
        {
            this.SwitchState("scoreDown");
        }
    }

    public void ResetToZero()
    {
        this.cachedScore = 0;
        this.scoreText.text = "0";
    }

}