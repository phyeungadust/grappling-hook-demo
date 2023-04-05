using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class AvatarButtonController : UdonSharpBehaviour
{

    [SerializeField]
    private AvatarSlot selectedSlot = null;
    [SerializeField]
    private TextMeshProUGUI avatarNameDisplay;
    [SerializeField]
    private BuyEquipButton buyEquipButton;

    public void Select(AvatarSlot avatarSlot)
    {

        if (this.selectedSlot != null)
        {

            // deselect previously selected slot
            this.selectedSlot.OnDeselect();

        }

        // set newly selected slot
        this.selectedSlot = avatarSlot;

        this.DisplayAvatarName();
        this.UpdateBuyEquipButton();

    }

    private void DisplayAvatarName()
    {
        this.avatarNameDisplay.text = this
            .selectedSlot
            .avatarInfo
            .avatarName;
    }

    private void UpdateBuyEquipButton()
    {
        this.buyEquipButton.UpdateButtonDisplay(this.selectedSlot.avatarInfo);
    }

}