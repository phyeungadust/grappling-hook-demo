using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BuyEquipButton : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore ownerStore;
    private Wallet wallet;

    [SerializeField]
    private ShopMenuController shopMenuController;

    [SerializeField]
    private Color defaultTextColor;
    [SerializeField]
    private Color unavailableTextColor;

    [SerializeField]
    private Image buttonFrameImg;
    [SerializeField]
    private Image buttonHighlightImg;

    [SerializeField]
    private Material disabledMaterial;
    [SerializeField]
    private Material defaultHighlightMaterial;
    [SerializeField]
    private Material hoveredHighlightMaterial;
    [SerializeField]
    private Material pressedHighlightMaterial;
    [SerializeField]
    private Material unavailableHighlightMaterial;

    [SerializeField]
    private Material defaultFrameMaterial;
    [SerializeField]
    private Material unavailableFrameMaterial;

    private bool enoughMoneyToBuy = false;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    private AvatarInfo equippedAvatar = null;
    private AvatarInfo selectedAvatar = null;

    private int pointerDownCount = 0;
    private int pointerUpCount = 0;

    public void CustomStart()
    {

        this.wallet = this.ownerStore.wallet;

        if (this.selectedAvatar != null)
        {
            // there is selected avatar,
            // update the button display
            this.UpdateButtonDisplay(this.selectedAvatar);
        }
        else
        {
            // nothing is selected at first
            // don't show the button
            this.buttonFrameImg.material = this.disabledMaterial;
            this.buttonHighlightImg.material = this.disabledMaterial;
            this.buttonText.text = "";
        }

    }

    public void UpdateButtonDisplay(AvatarInfo selectedAvatar)
    {

        this.selectedAvatar = selectedAvatar;

        this.enoughMoneyToBuy = 
            this.wallet.GetAmount() >= this.selectedAvatar.price;

        switch (this.selectedAvatar.avatarState)
        {

            case "notPurchased":

                if (this.enoughMoneyToBuy)
                {
                    this.buttonFrameImg.material = this.defaultFrameMaterial;
                    this.buttonHighlightImg.material = this.defaultHighlightMaterial;
                    this.buttonText.color = this.defaultTextColor;
                }
                else
                {
                    this.buttonFrameImg.material = this.unavailableFrameMaterial;
                    this.buttonHighlightImg.material = this.unavailableHighlightMaterial;
                    this.buttonText.color = this.unavailableTextColor;
                }
                this.buttonText.text = $"Buy for ${this.selectedAvatar.price}";

                break;
            
            case "purchased":
                this.buttonFrameImg.material = this.defaultFrameMaterial;
                this.buttonHighlightImg.material = this.defaultHighlightMaterial;
                this.buttonText.text = "Equip";
                this.buttonText.color = this.defaultTextColor;
                break;

            case "equipped":
                this.buttonFrameImg.material = this.unavailableFrameMaterial;
                this.buttonHighlightImg.material = this.unavailableHighlightMaterial;
                this.buttonText.text = "Eqiupped";
                this.buttonText.color = this.unavailableTextColor;
                break;

        }

    }

    public void OnPointerEnter()
    {

        Debug.Log("pointer enter called");

        // no avatar selected, no buy button effect
        if (this.selectedAvatar == null) return;

        switch (this.selectedAvatar.avatarState)
        {
            case "notPurchased":
                if (this.enoughMoneyToBuy)
                {
                    this.buttonHighlightImg.material = this.hoveredHighlightMaterial;
                    Debug.Log("button highlighted");
                }
                // no change in material if no money to buy avatar
                break;
            case "purchased":
                this.buttonHighlightImg.material = this.hoveredHighlightMaterial;
                break;
        }

    }

    public void OnPointerExit()
    {

        Debug.Log("pointer exit called");

        // no avatar selected, no buy button effect
        if (this.selectedAvatar == null) return;

        switch (this.selectedAvatar.avatarState)
        {
            case "notPurchased":
                if (this.enoughMoneyToBuy)
                {
                    this.buttonHighlightImg.material = this.defaultHighlightMaterial;
                    Debug.Log("button unhighlighted");
                }
                // no change in material if no money to buy avatar
                break;
            case "purchased":
                this.buttonHighlightImg.material = this.defaultHighlightMaterial;
                break;
        }

    }

    public void OnPointerDown()
    {

        // if (this.pointerDownCount == 0)
        // {
            switch (this.selectedAvatar.avatarState)
            {
                case "notPurchased":
                    if (this.enoughMoneyToBuy)
                    {
                        this.buttonHighlightImg.material = this.pressedHighlightMaterial;
                    }
                    // no change in material if no money to buy avatar
                    break;
                case "purchased":
                    this.buttonHighlightImg.material = this.pressedHighlightMaterial;
                    break;
            }
        // }

        // this.pointerDownCount = (this.pointerDownCount + 1) % 3;

    }

    public void OnPointerUp()
    {

        // if (this.pointerUpCount == 0)
        // {
            switch (this.selectedAvatar.avatarState)
            {

                case "notPurchased":
                    if (this.enoughMoneyToBuy)
                    {
                        this.BuyAndEquipSelectedAvatar();
                    }
                    // do nothing if no money to buy avatar
                    break;

                case "purchased":
                    this.EquipSelectedAvatar();
                    break;

            }
        // }

        // this.pointerUpCount = (this.pointerUpCount + 1) % 3;

    }

    private void BuyAndEquipSelectedAvatar()
    {
        // play money deduct animation
        // and actually deduct amount from wallet
        this
            .shopMenuController
            .ChangeMoneyAmount(-this.selectedAvatar.price);
        this.EquipSelectedAvatar();
    }

    private void EquipSelectedAvatar()
    {

        // actually equipping the selected avatar
        this
            .selectedAvatar
            .pedestal
            .SetAvatarUse(
                Networking.LocalPlayer
            );
        
        for (int i = 0; i < 10; ++i)
        {
            Debug.Log("called change avatar");
            Debug.Log("called change avatar.");
        }

        if (this.equippedAvatar != null)
        {
            // unequip previously equipped avatar
            // by resetting the avatarState to purchased
            this.equippedAvatar.avatarState = "purchased";
        }
        // set selected avatar's state to equipped
        this.selectedAvatar.avatarState = "equipped";
        // keep reference of newly equipped avatar, which is the selected avatar
        this.equippedAvatar = this.selectedAvatar;

        this.UpdateButtonDisplay(this.selectedAvatar);

    }

}