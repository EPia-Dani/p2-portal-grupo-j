using UnityEngine;
using UnityEngine.SceneManagement;

namespace Turrets
{
    public class Turret : MonoBehaviour
    {
        public Transform firingPoint;
        public float range = 50f;
        public float fireInterval = 0.1f;

        private LineRenderer laserLine;

        bool active = true;
        float timer;

        void Start()
        {
            laserLine = GetComponent<LineRenderer>();
            laserLine.enabled = true;
        }

        void Update()
        {
            if (!active)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= fireInterval)
            {
                timer = 0f;
                FireLaser();
            }
        }

        void FireLaser()
        {
            Vector3 origin = firingPoint.position;
            Vector3 dir = transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, range))
            {
                if (laserLine)
                {
                    {
                        laserLine.SetPosition(0, origin);
                        laserLine.SetPosition(1, hit.point);
                    }

                    var hitObj = hit.collider.gameObject;

                    if (hitObj.CompareTag("Player"))
                    {
                        SceneManager.LoadScene(0);
                    }
                    else if (hitObj.CompareTag("Turret"))
                    {
                        hitObj.GetComponent<Turret>().Deactivate();
                    }
                }
                else
                {
                    laserLine.SetPosition(0, origin);
                    laserLine.SetPosition(1, origin + dir * range);

                }
            }
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.collider.CompareTag("Cube") || col.collider.CompareTag("Turret"))
            {
                Deactivate();
            }
        }

        public void Deactivate()
        {
            active = false;
            laserLine.enabled = false;
            // opcional: reproducir efecte de desactivació
        }
    }
}