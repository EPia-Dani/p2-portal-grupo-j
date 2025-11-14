using System;
using UnityEngine;

namespace Activables
{
    public class BlastDoor : MonoBehaviour
    {
        public AnimationCurve animationCurve;
        public float animationDuration = 2f;

        private Vector3 closedPosition;
        private Vector3 openPosition;

        private bool isOpening = false;
        private float animationSample = 0f;

        void Awake()
        {
            closedPosition = transform.position;
            openPosition = closedPosition + Vector3.up * 4f;
        }

        public void Open()
        {
            isOpening = true;
        }

        public void Close()
        {
            isOpening = false;
        }

        private void Update()
        {
            float delta = Time.deltaTime / animationDuration;
            animationSample = Mathf.Clamp01(animationSample + (isOpening ? delta : -delta));
            
            if (animationSample <= 0f)
            {
                animationSample = 0f;
                transform.position = closedPosition;
            }

            if (animationSample >= 1f)
            {
                animationSample = 1f;
                transform.position = openPosition;
                return;
            }

            transform.position = Vector3.Lerp(closedPosition, openPosition, animationCurve.Evaluate(animationSample));
        }
    }
}