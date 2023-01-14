using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ResolveCommand : Command
{

    private Promise promise;
    private object[] result;

    public ResolveCommand Init(Promise promise, params object[] result)
    {
        this.promise = promise;
        this.result = new object[result.Length];
        for (int i = 0; i < result.Length; ++i)
        {
            this.result[i] = result[i];
        }
        return this;
    }

    public override void Exec()
    {
        this.promise.state = "FULFILLED";
        this.promise.wrappedValues = this.result;
        foreach (ObjectsCommand c in this.promise.commands)
        {
            if (c == null)
            {
                break;
            }
            c.Exec();
        }
    }

}