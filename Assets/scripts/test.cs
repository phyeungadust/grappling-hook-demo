using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UIElements;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class test : UdonSharpBehaviour
{

    [SerializeField]
    private UnityEngine.UI.Button avatarButton;
    [SerializeField]
    private UnityEngine.UI.Image buttonFrameImg;
    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private Sprite pressedSprite;
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material selectedMaterial;
    [SerializeField]
    private Vector3 defaultScale = Vector3.one;
    [SerializeField]
    private Vector3 hoveredScale = Vector3.one * 1.15f;

    private bool selected = false;

    public void TestMethod()
    {
        this.avatarButton.Select();
        this.buttonFrameImg.material = this.selectedMaterial;
    }

    public void OnPointerEnter()
    {

    }

    public void OnPointerExit()
    {

    }

    public void OnPointerClick()
    {
        
    }

}