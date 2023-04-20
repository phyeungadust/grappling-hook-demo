using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GameStartCube : UdonSharpBehaviour
{

    [SerializeField]
    private CustomGameManager customGameManager;

    public override void Interact()
    {
        if (Networking.IsMaster)
        {
            this.customGameManager.ActivateGame();
        }
    }

}