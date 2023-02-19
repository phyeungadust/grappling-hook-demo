using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherStateOperationsStrat : UdonSharpBehaviour
    {
        public virtual void Exec(TetherController controller) {}
    }
}