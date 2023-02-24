using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GrapplableCollection : UdonSharpBehaviour
{

    [SerializeField]
    private Grapplable[] grapplables;

    void Start()
    {
        // set each grapplable's id to the index in the array
        for (int i = 0; i < this.grapplables.Length; ++i)
        {
            this.grapplables[i].id = i;
        }
    }

    public Grapplable[] GetAll()
    {
        return this.grapplables;
    }

    public Grapplable GetById(int id)
    {
        return this.grapplables[id];
    }

}