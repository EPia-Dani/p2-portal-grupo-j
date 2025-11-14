using UnityEngine;

public class CompanionCube : GrabbableObject
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrabbableObject();
    }
}
