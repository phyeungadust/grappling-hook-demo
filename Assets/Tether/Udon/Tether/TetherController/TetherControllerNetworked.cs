using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TetherControllerNetworked : UdonSharpBehaviour
    {

        private TetherController controller;
        [SerializeField]
        private SwitchStateBySyncStringVisitor switchStateBySyncStringVisitor;
        [SerializeField]
        private GetSyncStringFromStateVisitor getSyncStringFromStateVisitor;

        [SerializeField]
        private PlayerStore ownerStore;
        private VRCPlayerApi owner;
        private VRCPlayerApi localPlayer;

        private WorldSpaceLogger wsLogger;

        private bool initialized = false;

        public TetherControllerNetworked Init
        (
            TetherController controller
        )
        {

            this.wsLogger = GameObject
                .Find("WorldSpaceLogger")
                .GetComponent<WorldSpaceLogger>();

            this.controller = controller;

            this.switchStateBySyncStringVisitor.Init(this.controller, this);
            this.getSyncStringFromStateVisitor.Init(this);

            this.owner = this.ownerStore.playerApiSafe.Get();
            Networking.SetOwner(
                this.owner,
                this.gameObject
            );
            this.localPlayer = Networking.LocalPlayer;

            wsLogger.Log("networked tethercontroller Init called!");
            wsLogger.Log($"player {localPlayer.playerId} sets player {owner.playerId} to be the owner of {gameObject.name}");

            this.initialized = true;

            return this;

        }

        public void CustomStart()
        {
            this.owner = this.ownerStore.playerApiSafe.Get();
            Networking.SetOwner(
                this.owner,
                this.gameObject
            );
            this.localPlayer = Networking.LocalPlayer;
        }

        public void SwitchStateBroadcast(TetherState tetherState)
        {
            // serializes state to string and
            // sets SwitchStateBroadcastSyncString to this serialized string
            tetherState.Accept(this.getSyncStringFromStateVisitor);
        }

        [UdonSynced, FieldChangeCallback(nameof(SwitchStateBroadcastSyncString))]
        private string switchStateBroadcastSyncString;
        public string SwitchStateBroadcastSyncString
        {
            get => this.switchStateBroadcastSyncString;
            set
            {

                this.switchStateBroadcastSyncString = value;
                if (this.localPlayer == this.owner)
                {
                    this.RequestSerialization();
                    if (wsLogger != null)
                    {
                        wsLogger.Log($"player {owner.playerId} of {gameObject.name} serialized to other clients");
                        wsLogger.Log($"serialized content: {this.switchStateBroadcastSyncString}");
                    }
                    // Debug.Log("player " + this.owner.playerId + " serialized to other clients");
                }

                // deserializes SwitchStateBroadcastSyncString to state and
                // SwitchState into that state

                if (!this.initialized) return;

                wsLogger.Log("SwitchState deserialized!");

                wsLogger.Log($"player {localPlayer.playerId} accepts serialization from player {owner.playerId} of {gameObject.name}");
                wsLogger.Log($"serialized content: {this.switchStateBroadcastSyncString}");

                string[] args = value.Split(' ');
                string tetherStateString = args[1];

                if (wsLogger != null)
                {
                    wsLogger.Log("player " + owner.playerId + " switches to " + tetherStateString);
                }
                // Debug.Log("player " + owner.playerId + " switches to " + tetherStateString);

                switch (tetherStateString)
                {
                    case nameof(TetherNoneState):
                        this
                            .controller
                            .TetherStatesDict
                            .TetherNoneState
                            .Accept(this.switchStateBySyncStringVisitor);
                        break;
                    case nameof(TetherAccelState):
                        this
                            .controller
                            .TetherStatesDict
                            .TetherAccelState
                            .Accept(this.switchStateBySyncStringVisitor);
                        break;
                    case nameof(TetherBrakeState):
                        this
                            .controller
                            .TetherStatesDict
                            .TetherBrakeState
                            .Accept(this.switchStateBySyncStringVisitor);
                        break;
                    case nameof(StunnedState):
                        this
                            .controller
                            .TetherStatesDict
                            .StunnedState
                            .Accept(this.switchStateBySyncStringVisitor);
                        break;
                }

            }
        }

    }
}