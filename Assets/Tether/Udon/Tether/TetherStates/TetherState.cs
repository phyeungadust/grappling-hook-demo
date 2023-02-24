using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    public class TetherState : UdonSharpBehaviour
    {
        public virtual bool CheckStateChange(TetherController tetherController) => false;
        public virtual void HandleUpdate(TetherController tetherController) {}
        public virtual void Enter(TetherController tetherController) {}
        public virtual void Exit(TetherController tetherController) {}
        public virtual void Accept(TetherStateVisitor tetherStateVisitor) {}
    }
}