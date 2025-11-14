using System.Collections.Generic;
using UnityEngine;

namespace Activables
{
    public class FloorButton : MonoBehaviour, IActivable
    {
        public BlastDoor blastDoor;

        private List<Collider> colliders = new List<Collider>();
        
        private void OnTriggerEnter(Collider other)
        {
            colliders.Add(other);
            Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            colliders.Remove(other);
            if (colliders.Count == 0)
            {
                Deactivate();
            }
        }

        public void Activate()
        {
            blastDoor.Open();
        }
        
        public void Deactivate()
        {
            blastDoor.Close();
        }
    }
}
