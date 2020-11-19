using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSphere : PhysicsObject
{
    public PhysicsPlane onPlane = null;
    public PhysicsBouncyBox onBouncyBox = null;
    public PhysicsBumper onBumper = null;

    public float Radius => transform.localScale.x * 0.5f;
    void Start()
    {

    }

    protected override void FixedUpdateMethod()
    {
        base.FixedUpdateMethod();
    }

    /// <summary>
    /// Assuming the initial velocity is 0
    /// </summary>
    /// <returns></returns>
    public float VelocityOnGround(PhysicsPlane plane)
    {
        return Mathf.Sqrt(2f * Physics.gravity.magnitude * (transform.position.y - plane.transform.position.y));
    }

    protected override void OnTriggerEnterMethod(Collider other)
    {
        UpdateOnPlaneWhenEnter(other);
        UpdateOnBouncyBoxWhenEnter(other);
        UpdateOnBumperWhenEnter(other);
    }
    private void UpdateOnBouncyBoxWhenEnter(Collider other)
    {
        onBouncyBox = other.GetComponent<PhysicsBouncyBox>(); ;
    }
    private void UpdateOnPlaneWhenEnter(Collider other)
    {
        onPlane = other.GetComponent<PhysicsPlane>();
    }
    private void UpdateOnBumperWhenEnter(Collider other)
    {
        onBumper = other.GetComponent<PhysicsBumper>();
    }

    protected override void OnTriggerStayMethod(Collider other)
    {
        UpdateOnPlaneWhenStay(other);
        UpdateOnBouncyBoxStay(other);
        UpdateOnBumperStay(other);
    }
    private void UpdateOnBouncyBoxStay(Collider other)
    {
        onBouncyBox = other.GetComponent<PhysicsBouncyBox>(); ;
    }
    private void UpdateOnBumperStay(Collider other)
    {
        onBumper = other.GetComponent<PhysicsBumper>(); ;
    }
    private void UpdateOnPlaneWhenStay(Collider other)
    {
        onPlane = other.GetComponent<PhysicsPlane>();
    }

    protected override void OnTriggerExitMethod(Collider other)
    {
        UpdateOnPlaneWhenExit(other);
        UpdateOnBouncyBoxWhenExit(other);
        UpdateOnBumperWhenExit(other);
    }

    private void UpdateOnBouncyBoxWhenExit(Collider other)
    {
        PhysicsBouncyBox box = other.GetComponent<PhysicsBouncyBox>();
        if (box != null)
            onBouncyBox = null;
    }

    private void UpdateOnBumperWhenExit(Collider other)
    {
        PhysicsBumper bumper = other.GetComponent<PhysicsBumper>();
        if (bumper != null)
            onBumper = null;
    }


    private void UpdateOnPlaneWhenExit(Collider other)
    {
        PhysicsPlane plane = other.GetComponent<PhysicsPlane>();
        if (plane != null)
            onPlane = null;
    }
}
