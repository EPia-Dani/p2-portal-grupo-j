using UnityEngine;

namespace Activables
{
    public class StandButton : MonoBehaviour, IActivable
    {
        public CubeDispenser cubeDispenser;
    
        public void Activate()
        {
            cubeDispenser.Activate();
        }

        public void Deactivate()
        {
            throw new System.NotImplementedException();
        }
    }
}