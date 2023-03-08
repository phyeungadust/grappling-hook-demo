using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class InteractionMediator : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore senderStore;
    private PlayerStore localStore;
    private bool initialized = false;

    public void CustomStart()
    {
        VRCPlayerApi sender = this.senderStore.playerApiSafe.Get();
        Networking.SetOwner(sender, this.gameObject);
        this.localStore = this.senderStore.playerStoreCollection.GetLocal();
        this.initialized = true;
    }

    public void ShellHitUnicast(int playerID, float stunTime)
    {
        this.ShellHitUnicastSyncString = string.Join(
            " ",
            System.Guid.NewGuid().ToString().Substring(0, 6),
            playerID,
            stunTime
        );
    }

    [UdonSynced, FieldChangeCallback(nameof(ShellHitUnicastSyncString))]
    private string shellHitUnicastSyncString;
    public string ShellHitUnicastSyncString
    {
        get => this.shellHitUnicastSyncString;
        set
        {
            this.shellHitUnicastSyncString = value;
            if (this.senderStore == this.localStore)
            {
                Debug.Log($"player {this.senderStore.playerApiSafe.GetID()} serializes to other clients");
                Debug.Log($"serialized content: {value}");
                this.RequestSerialization();
            }
            string[] args = value.Split(' ');
            string nonce = args[0];
            int receiverID = System.Int32.Parse(args[1]);
            float stunTime = float.Parse(args[2]);
            if (this.localStore.playerApiSafe.GetID() == receiverID)
            {
                this.ShellHitLocal(stunTime);
            }
        }
    }

    public void ShellHitLocal(float stunTime)
    {

        Tether.TetherController controller = this.localStore.tetherController;
        Tether.StunnedState state = controller.TetherStatesDict.StunnedState;
        HUDStatusPopUpBehaviour popup = this.localStore.hud.popup;

        controller.SwitchStateBroadcast(state.Initialize(stunTime));
        popup.ShowPopUp("STUNNED");

    }

}