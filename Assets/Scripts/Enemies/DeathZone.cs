using UnityEngine;

namespace Enemies
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable?.Die();
        }
    }
}
