using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUDStatusPopUpBehaviour : UdonSharpBehaviour
{

    private TextMeshProUGUI tmPro;

    [SerializeField]
    private float defaultTimeBeforeHidePopUp = 5.0f;
    [SerializeField]
    private AnimationCurve opacityCurve;
    [SerializeField]
    private AnimationCurve scaleCurve;
    [SerializeField]
    private Vector3 defaultPopUpPos;

    private bool displaying = false;
    private float timeBeforeHidePopUp;
    private Vector3 defaultScale;
    private Color defaultColor;

    public void CustomStart()
    {
        this.tmPro = this.GetComponent<TextMeshProUGUI>();
        this.tmPro.enabled = false;
        this.defaultScale = this.transform.localScale;
        this.defaultColor = this.tmPro.color;
    }

    public void CustomUpdate()
    {

        if (this.displaying)
        {
            if (this.timeBeforeHidePopUp <= 0.0f)
            {
                this.tmPro.enabled = false;
                this.displaying = false;
            }
            else
            {

                float timeElapsed = 
                    this.defaultTimeBeforeHidePopUp - this.timeBeforeHidePopUp;
                float totalAnimTime = this.defaultTimeBeforeHidePopUp;

                this.tmPro.color = new Color(
                    this.defaultColor.r,
                    this.defaultColor.g,
                    this.defaultColor.b,
                    this.opacityCurve.Evaluate(timeElapsed / totalAnimTime)
                );

                Debug.Log(this.tmPro.color);

                this.transform.localScale =
                    this.defaultScale 
                    * this.scaleCurve.Evaluate(timeElapsed / totalAnimTime);

                this.timeBeforeHidePopUp -= Time.deltaTime;

            }
        }

    }

    public void ShowPopUp(string msg)
    {
        this.tmPro.enabled = true;
        this.transform.localPosition = this.defaultPopUpPos;
        this.transform.localScale = this.defaultScale;
        this.timeBeforeHidePopUp = this.defaultTimeBeforeHidePopUp;
        this.tmPro.text = msg;
        this.displaying = true;
    }

}