using Activables;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float interactionDistance;
    
    private Camera cam;
    
    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out var hit, interactionDistance))
            {
                hit.collider.gameObject.GetComponent<StandButton>()?.Activate();
            }
        }

    }
}
