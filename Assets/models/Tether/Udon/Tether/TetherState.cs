using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    public class TetherState : UdonSharpBehaviour
    {
        public virtual TetherState HandleInput(TetherController tetherController) => null;
        public virtual void HandleUpdate(TetherController tetherController) {}
        public virtual void Enter(TetherController tetherController) {}
    }
}