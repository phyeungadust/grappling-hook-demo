using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GameStartCountDown : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;
    [SerializeField]
    private TextMeshProUGUI tmPro;

    [SerializeField]
    private AnimationCurve scaleCurve;
    private int timer = 3;
    private float progressForEachSecond = 0.0f;
    
    void OnEnable()
    {
        Debug.Log("game start count down enabled!");
        this.timer = 3;
        this.tmPro.text = this.timer.ToString();
        this.progressForEachSecond = 0.0f;
    }

    void Update()
    {
        
        this.transform.localScale = Vector3.one * this.scaleCurve.Evaluate(
            this.progressForEachSecond
        );

        this.progressForEachSecond += Time.deltaTime;

        if (this.progressForEachSecond > 1.0f)
        {
            if (this.timer == 0)
            {
                // deactivate this gameObject (timer)
                this.gameObject.SetActive(false);
            }
            else
            {
                if (this.timer == 1)
                {
                    --this.timer;
                    this.tmPro.text = "GO!";
                    // tell custom manager game starts
                    this
                        .ownerStore
                        .playerStoreCollection
                        .customGameManager
                        .OnGameStartCountDownFinishes();
                }
                else
                {
                    --this.timer;
                    this.tmPro.text = this.timer.ToString();
                }
                this.progressForEachSecond = 0.0f;
            }
        }

    }

}