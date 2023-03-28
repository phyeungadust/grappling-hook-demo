using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Wallet : UdonSharpBehaviour
{

    [SerializeField]
    private int amount = 100;

    public int GetAmount() => this.amount;

    public void SetAmount(int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }
        this.amount = amount;
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