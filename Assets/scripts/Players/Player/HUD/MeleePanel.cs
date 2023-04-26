using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class MeleePanel : UdonSharpBehaviour
{
    [SerializeField]
    private TextMeshProUGUI meleeText;
    [SerializeField]
    private Color readyColor;
    [SerializeField]
    private Color cooldownColor;
    public void Ready()
    {
        this.meleeText.color = this.readyColor;
        this.meleeText.text = "MELEE:  READY.--";
    }
    public void Cooldown()
    {
        this.meleeText.color = this.cooldownColor;
        this.meleeText.text = "MELEE: COOLDOWN";
    }
}