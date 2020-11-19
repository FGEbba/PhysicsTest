using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsReflectiveObject : PhysicsObject
{
    public Vector3 Normal { get => transform.up; set => Normal = value; }
    protected Vector3 Position => transform.position;

    protected PhysicsSphere[] spheres = new PhysicsSphere[0];

    protected const float staticVelocityLimit = 0.2f;
    protected const float deltaMoveCoef = 0.2f;
    protected const float correctedPostionCoef = 5f;


    protected PhysicsObject ParentPhysicalObject => GetComponentInParent<PhysicsObject>();

    protected Vector3 ParentVelocity => ParentPhysicalObject == null ? Vector3.zero : ParentPhysicalObject.Velocity;



    void Start()
    {

    }

    protected virtual void UpdateSpheres() { }

    protected virtual void Choc(PhysicsSphere sphere, float energyDissipation = 0f) { }
    protected float Distance(PhysicsSphere sphere)
    {
        Vector3 sphereToPlane = Position - sphere.transform.position;

        //Vector3 sphereToPlane = sphere.transform.position - Position;

        return Vector3.Dot(sphereToPlane, Normal);
    }

    protected Vector3 Reflect(Vector3 v, float energyDissipation = 0f)
    {
        Vector3 r = (v - 2f * Vector3.Dot(v, Normal) * Normal) * (1f - energyDissipation);

        //Debug.Log(r.y);
        return r;
    }

    protected Vector3 Projection(PhysicsSphere sphere)
    {
        Vector3 sphereToProjection = Distance(sphere) * Normal;

        return sphere.transform.position + sphereToProjection;
    }

    protected bool IsColliding(PhysicsSphere sphere)
    {
        if (WillBeCollision(sphere) == false)
            return false;

        return Distance(sphere) >= 0f || Mathf.Abs(Distance(sphere)) <= sphere.Radius;

        // for dynamic ball the WillBeCollision is preventing the multi-collision;
        //return WillBeCollision(sphere) && TouchingThePlane(sphere);
    }

    protected bool WillBeCollision(PhysicsSphere sphere)
    {
        //Debug.Log("WillBeCollision: " + name);
        //return Vector3.Dot(sphere.Velocity, Normal) < 0f;
        return Vector3.Dot(RelativeVelocity(sphere), Normal) < 0f;
    }
    protected Vector3 CorrectedPosition(PhysicsSphere sphere)
    {
        return Projection(sphere) + Normal * sphere.Radius;
    }

    protected void InverseRelativeVelocity(PhysicsObject other, Vector3 vel)
    {
        other.Velocity = vel + 2f * ParentVelocity;
    }

}
