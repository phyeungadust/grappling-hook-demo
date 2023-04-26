using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Timer : UdonSharpBehaviour
{

    [SerializeField]
    private float defaultTime = 180f; // 3 mins
    private float timeLeft;
    [SerializeField]
    private TextMeshProUGUI timerTMP;
    [SerializeField]
    private PlayerStore ownerStore;

    public void OnBeforeGameStarts()
    {
        this.timeLeft = this.defaultTime;
        this.DisplayTime(this.timeLeft);
    }

    public void OnAfterGameStarts()
    {
        // enable this script such that Update() runs
        this.enabled = true;
    }

    void Update()
    {
        this.timeLeft -= Time.deltaTime;
        this.DisplayTime(this.timeLeft);
        if (this.timeLeft < 0)
        {
            // disable script to stop timer
            // ( Update() stops running )
            this.enabled = false;
            if (Networking.IsMaster)
            {
                // tell game manager to do something
                this
                    .ownerStore
                    .playerStoreCollection
                    .customGameManager
                    .EndGame();
            }
        }
    }

    public void OnBeforeGameEnds()
    {
        this.timeLeft = -0.5f;
        this.DisplayTime(this.timeLeft);
        // disable script to stop timer
        // ( Update() stops running )
        this.enabled = false;
    }

    public void OnAfterGameEnds()
    {
        this.timeLeft = this.defaultTime;
        this.DisplayTime(this.timeLeft);
    }

    public void DisplayTime(float currentTime)
    {
        int currentTimeInInt = Mathf.CeilToInt(currentTime);
        int minutes = currentTimeInInt / 60;
        int seconds = currentTimeInInt % 60;
        this.timerTMP.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }

}