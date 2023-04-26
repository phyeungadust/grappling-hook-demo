using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShopMenuController : UdonSharpBehaviour
{

    [SerializeField]
    private ShopMenuToggle toggler;
    [SerializeField]
    private ShopMenuMoneyTextAnimation moneyTextAnim;
    [SerializeField]
    private BuyEquipButton buyEquipButton;

    [SerializeField]
    private PlayerStore ownerStore;
    private Wallet wallet;

    private bool toggled = false;

    [SerializeField]
    private GameStateControls gameStateControls;

    public void OnBeforeGameStarts()
    {
        // force close menu
        this.toggler.SwitchState("menuClosed");
        // // "menuClosed" state mobilizes player,
        // // we want the player to be immobilized
        // this.ownerStore.playerApiSafe.Get().Immobilize(true);
        // deactivate this gameobject, so that updates won't run
        this.gameObject.SetActive(false);
    }

    public void OnAfterGameEnds()
    {
        // reactivate this gameobject, so that updates run
        this.gameObject.SetActive(true);
    }

    public void CustomStart()
    {

        this.toggler.CustomStart();
        this.wallet = this.ownerStore.wallet;

        // subscribe to game state changes
        this
            .ownerStore
            .playerStoreCollection
            .customGameManager
            .SubscribeToGameStateChanges(this.gameStateControls);

    }

    public void CustomUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // deduct value test
            this.ChangeMoneyAmount(-10);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // add value test
            this.ChangeMoneyAmount(10);
        }

        this.toggler.CustomUpdate();
        if (this.toggled)
        {
            this.moneyTextAnim.CustomUpdate();
        }

    }

    public void OnMenuToggledOn()
    {
        this.toggled = true;
        this.moneyTextAnim.CustomStart();
        this.moneyTextAnim.MoneyChangeAnimStart(
            this.wallet.GetAmount(),
            true
        );
        this.buyEquipButton.CustomStart();
    }

    public void OnMenuToggledOff()
    {
        this.toggled = false;
    }

    public void ChangeMoneyAmount(int amount)
    {
        this.wallet.SetAmount(this.wallet.GetAmount() + amount);
        this.moneyTextAnim.MoneyChangeAnimStart(amount);
    }

}