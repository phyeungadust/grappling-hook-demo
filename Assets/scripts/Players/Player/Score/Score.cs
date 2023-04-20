using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Score : UdonSharpBehaviour
{

    [SerializeField, UdonSynced]
    private int amount = 0;

    public int GetAmount() => this.amount;

    public void SetAmount(int amount)
    {
        this.amount = amount;
        this.RequestSerialization();
    }

    public void Add(int amount)
    {
        this.SetAmount(this.amount + amount);
    }

    public void Deduct(int amount)
    {
        this.SetAmount(this.amount - amount);
    }

}