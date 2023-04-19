using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class CustomGameManager : UdonSharpBehaviour
{

    [SerializeField]
    private GameObject[] deactivateWhenGameStarts;
    private GameStateControls[] gameStateControlsArr;
    private int stateChangeSubscribersCount = 0;
    [SerializeField]
    private int maxSubscriberCount = 1024;
    private bool gameStarted = false;

    [SerializeField]
    private PlayerStoreCollection playerStoreCollection;

    public void GameStart()
    {
        foreach (GameObject go in this.deactivateWhenGameStarts)
        {
            go.SetActive(false);
        }
        // notify all subscribers game is about to start
        for (int i = 0; i < this.stateChangeSubscribersCount; ++i)
        {
            this.gameStateControlsArr[i].OnBeforeGameStarts();
        }
        // this.gameStarted = true;
        if (Networking.IsMaster)
        {
            this.CountDownBeforeStart();
        }
    }

    public void CountDownBeforeStart()
    {
        // wait for 7 seconds
        // for game to initialize
        this.SendCustomEventDelayedSeconds(
            nameof(this.CountDownBeforeStartBroadcast),
            7f
        );
    }

    public void CountDownBeforeStartBroadcast()
    {
        this.SendCustomNetworkEvent(
            VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
            nameof(this.CountDownBeforeStartLocal)
        );
    }

    public void CountDownBeforeStartLocal()
    {

        this
            .playerStoreCollection
            .GetLocal()
            .hud
            .gameStartCountDown
            .gameObject
            .SetActive(true);

    }

    public void OnGameStartCountDownFinishes()
    {
        for (int i = 0; i < this.stateChangeSubscribersCount; ++i)
        {
            this.gameStateControlsArr[i].OnAfterGameStarts();
        }
    }

    public void GameEnd()
    {
        foreach (GameObject go in this.deactivateWhenGameStarts)
        {
            go.SetActive(true);
        }
        this.gameStarted = false;
    }

    public void GameToggle()
    {
        if (this.gameStarted)
        {
            // game started, toggle it off
            this.SendCustomNetworkEvent(
                VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
                nameof(this.GameEnd)
            );
        }
        else
        {
            // game has not started, toggle it on
            this.SendCustomNetworkEvent(
                VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
                nameof(this.GameStart)
            );
        }
    }

    public void SubscribeToGameStateChanges(GameStateControls gameStateControls)
    {
        if (this.stateChangeSubscribersCount == 0)
        {
            // first subscription ever, instantiate gameStateControlsArr
            this.gameStateControlsArr = 
                new GameStateControls[this.maxSubscriberCount];
        }
        this.gameStateControlsArr[this.stateChangeSubscribersCount] = 
            gameStateControls;
        ++this.stateChangeSubscribersCount;
    }

}