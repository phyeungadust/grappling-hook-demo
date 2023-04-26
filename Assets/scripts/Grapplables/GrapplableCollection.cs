using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GrapplableCollection : UdonSharpBehaviour
{

    // [SerializeField]
    // private Grapplable[] grapplables;

    [SerializeField]
    private GameObject[] grapplables; 

    [SerializeField]
    private int hashTableSize;
    private GameObject[] hashTable;

    void Start()
    {
        //// set each grapplable's id to the index in the array
        //for (int i = 0; i < this.grapplables.Length; ++i)
        //{
        //    this.grapplables[i].id = i;
        //}
        
        this.hashTable = new GameObject[this.hashTableSize];

        foreach (GameObject go in this.grapplables)
        {
            // fill hashtable
            this.hashTable[this.GetID(go, true)] = go;
        }

        // foreach (GameObject go in this.grapplables)
        // {
        //     Debug.Log($"{go.name}'s ID: {this.GetID(go)}");
        // }

    }

    public int GetID(GameObject go, bool insert = false)
    {

        int moddedHashCode = Djb2.GetHashCode(go.name) % this.hashTableSize;
            
        int linearProbeOffset = 0;
        int quadraticProbeOffset = 0;
        int idCandidate = moddedHashCode;

        while (true)
        {

            if (insert)
            {
                // insert mode, look for empty space
                if (this.hashTable[idCandidate] == null)
                {
                    // empty space found, break
                    break;
                }
            }
            else
            {
                // search mode, look for gameObject
                if (this.hashTable[idCandidate] == go)
                {
                    // gameObject found, break
                    break;
                }
            }

            // probe for target quadratically
            ++linearProbeOffset;
            quadraticProbeOffset = linearProbeOffset * linearProbeOffset;
            idCandidate = 
                (moddedHashCode + quadraticProbeOffset) % this.hashTableSize;

        }

        return idCandidate;

    }

    public GameObject GetByID(int id)
    {
        return this.hashTable[id];
    }

    // public Grapplable[] GetAll()
    // {
    //     return this.grapplables;
    // }

    // public Grapplable GetById(int id)
    // {
    //     return this.grapplables[id];
    // }

}