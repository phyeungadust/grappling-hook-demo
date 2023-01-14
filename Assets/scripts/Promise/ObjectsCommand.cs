using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ObjectsCommand : Command
{
    protected object[] objs;

    public ObjectsCommand Init(params object[] list)
    {
        this.objs = new object[list.Length];
        for (int i = 0; i < list.Length; ++i)
        {
            this.objs[i] = list[i];
        }
        return this;
    }

}