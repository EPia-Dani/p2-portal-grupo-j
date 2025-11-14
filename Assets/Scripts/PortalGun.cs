using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject bluePortalPrefab;
    [SerializeField] private GameObject orangePortalPrefab;
    private static Portal bluePortal;
    private static Portal orangePortal;

    [SerializeField] private Transform attachPosition;
    private GrabbableObject grabbedObject;

    [SerializeField] private float portalGunDistance;
    [SerializeField] private float gravityGunDistance;
    [SerializeField] private float gravityGunForce;

    private InputController input;

    void Start()
    {
        input = GetComponent<InputController>();
    }

    void Update()
    {
        if (grabbedObject == null)
            UpdatePortalGun();
        else
            UpdateGravityGun();
    }

    private void UpdatePortalGun()
    {
        if (input.button1 || input.button2)
        {
            Ray ray = CastRay();
            if (!Physics.Raycast(ray, out var hit, gravityGunDistance)) return;

            if (hit.collider && hit.collider.TryGetComponent<GrabbableObject>(out var grabbable))
            {
                GrabObject(grabbable);
                return;
            }
        }
    }

    private void UpdateGravityGun()
    {
        if (input.button1)
            ThrowObject(gravityGunForce);

        if (input.button2)
            ThrowObject(0);
    }


    private void GrabObject(GrabbableObject grabbable)
    {
        grabbable.OnGrab(attachPosition);
        grabbedObject = grabbable;
    }

    private void ThrowObject(float force)
    {
        if (!grabbedObject) return;
        grabbedObject.OnThrow(force);
        grabbedObject = null;
    }

    private Ray CastRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    public void CreateBluePortal(RaycastHit hit)
    {
        if (bluePortal != null) bluePortal.Close();
        GameObject portalGameObject = Instantiate(bluePortalPrefab, hit.point + hit.normal.normalized * .1f, Quaternion.LookRotation(hit.normal));
        bluePortal = portalGameObject.GetComponent<Portal>();
        if (orangePortal == null) return;
        bluePortal.otherPortal = orangePortal;
        orangePortal.otherPortal = bluePortal;
    }

    public void CreateOrangePortal(RaycastHit hit)
    {
        if (orangePortal != null) orangePortal.Close();
        GameObject portalGameObject = Instantiate(orangePortalPrefab, hit.point + hit.normal.normalized * .1f, Quaternion.LookRotation(hit.normal));
        orangePortal = portalGameObject.GetComponent<Portal>();
        if (bluePortal == null) return;
        bluePortal.otherPortal = orangePortal;
        orangePortal.otherPortal = bluePortal;
    }
}
