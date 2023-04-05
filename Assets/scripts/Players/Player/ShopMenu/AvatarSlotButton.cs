using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class AvatarSlotButton : UdonSharpBehaviour
{

    [SerializeField]
    private AvatarSlot avatarSlot;
    [SerializeField]
    private UnityEngine.UI.Image buttonFrameImg;
    [SerializeField]
    private Sprite defaultFrameSprite;
    [SerializeField]
    private Sprite pressedFrameSprite;
    [SerializeField]
    private Sprite selectedFrameSprite;
    [SerializeField]
    private Material defaultFrameMaterial;
    [SerializeField]
    private Material selectedFrameMaterial;
    [SerializeField]
    private Vector3 defaultScale = Vector3.one;
    [SerializeField]
    private Vector3 hoveredScale = Vector3.one * 1.15f;

    private bool selected = false;

    private int pointerDownCount = 0;
    private int pointerUpCount = 0;

    public void OnPointerEnter()
    {
        this.transform.localScale = this.hoveredScale;
    }

    public void OnPointerExit()
    {
        this.transform.localScale = this.defaultScale;
    }

    public void OnPointerDown()
    {
        if (this.pointerDownCount == 0)
        {
            this.buttonFrameImg.sprite = this.pressedFrameSprite;
            this.transform.localScale = this.defaultScale;
        }
        // prevent duplicated invocations of OnPointerDown
        // it was tested OnPointerDown is called 3 times
        // for every actual pointer down on the button
        this.pointerDownCount = (this.pointerDownCount + 1) % 3;
    }

    public void OnPointerUp()
    {
        if (this.pointerUpCount == 0)
        {
            this.OnSelect();
        }
        // prevent duplicated invocations of OnPointerUp
        // it was tested OnPointerUp is called 3 times
        // for every actual pointer up on the button
        this.pointerDownCount = (this.pointerDownCount + 1) % 3;
    }

    public void OnSelect()
    {

        // this select button effect is played
        // even if the slotbutton has already been selected previously
        this.OnSelectButtonEffect();

        if (!this.selected)
        {

            // if it's not already selected,
            // notify avatarSlot that it is selected
            // if it's already selected,
            // no need to notify
            this.avatarSlot.OnSelect();

            // mark selected
            this.selected = true;

        }

    }

    public void OnDeselect()
    {
        this.selected = false;
        this.OnDeselectButtonEffect();
    }

    private void OnSelectButtonEffect()
    {
        this.buttonFrameImg.sprite = this.selectedFrameSprite;
        this.transform.localScale = this.hoveredScale;
        this.buttonFrameImg.material = this.selectedFrameMaterial;
    }

    private void OnDeselectButtonEffect()
    {
        this.buttonFrameImg.sprite = this.defaultFrameSprite;
        this.transform.localScale = this.defaultScale;
        this.buttonFrameImg.material = this.defaultFrameMaterial;
    }

}