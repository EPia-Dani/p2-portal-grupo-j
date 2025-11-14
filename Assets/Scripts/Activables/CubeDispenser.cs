using UnityEngine;

namespace Activables
{
    public class CubeDispenser : MonoBehaviour, IActivable
    {
        public GameObject cubePrefab;
        private GameObject currentCube;
        
        public void Activate()
        {
            if (currentCube)
            {
                Destroy(currentCube);
            }
            
            currentCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
        }

        public void Deactivate()
        {
            throw new System.NotImplementedException();
        }
    }
}