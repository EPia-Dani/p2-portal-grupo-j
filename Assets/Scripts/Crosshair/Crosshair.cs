using System;
using UnityEngine;
using UnityEngine.UI;

namespace Crosshair
{
    public class Crosshair : MonoBehaviour
    {
        Image crosshair;
        
        public Sprite n;
        public Sprite b;
        public Sprite o;
        public Sprite bo;

        private void Start()
        {
            crosshair.sprite = n;
        }

        private void Update()
        {
            if (PortalGun.bluePortal && PortalGun.orangePortal)
            {
                crosshair.sprite = bo;
            }
            else if (PortalGun.bluePortal)
            {
                crosshair.sprite = b;
            }
            else if (PortalGun.orangePortal)
            {
                crosshair.sprite = o;
            }
            else
            {
                crosshair.sprite = n;
            }
        }
    }
}
