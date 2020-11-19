using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlane : PhysicsReflectiveObject
{


    void Start()
    {
        spheres = FindObjectsOfType<PhysicsSphere>();
    }
    private void FixedUpdate()
    {
        UpdateSpheres();
    }

    protected override void UpdateSpheres()
    {
        foreach (PhysicsSphere t in spheres)
        {
            if (t.onPlane == this)
                Choc(t, GlobalParameters.chocEnergyDissipation);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sphere"></param>
    /// <param name="energyDissipation" the impact of the energy dissipation on the reflected velocity></param>
    protected override void Choc(PhysicsSphere sphere, float energyDissipation = 0f)
    {
        //Debug.Log("choc: " + name);
        if (IsColliding(sphere) == false)
            return;

        //Debug.Log("Sphere plane collision on: " + name);
        //Debug.Log("Velocity Error: isVerlet: " + sphere.isVerlet + " " + sphere.ErrorVelocityOnTheGround());

        //sphere.Velocity = Vector3.Reflect(sphere.Velocity, Normal);

        if (IsSphereStaticOnPlane(sphere))
        {
            sphere.transform.position = CorrectedPosition(sphere);
            sphere.ApplyForce(-sphere.mass * Physics.gravity);
        }
        else // sphere is dynamic
        {
            sphere.transform.position = CorrectedPosition(sphere);
            InverseRelativeVelocity(sphere, Reflect(RelativeVelocity(sphere), energyDissipation));
            //sphere.Velocity = Reflect(RelativeVelocity(sphere), energyDissipation);
        }
    }

    bool IsSphereStaticOnPlane(PhysicsSphere sphere)
    {
        bool lowVelocity = RelativeVelocity(sphere).magnitude < staticVelocityLimit;

        return lowVelocity && TouchingThePlane(sphere);
    }

    bool TouchingThePlane(PhysicsSphere sphere)
    {
        // sphere speed
        // deltaTime
        float deltaMove = Mathf.Max(deltaMoveCoef * sphere.Radius, RelativeVelocity(sphere).magnitude * Time.fixedDeltaTime);
        return (CorrectedPosition(sphere) - sphere.transform.position).magnitude <= correctedPostionCoef * deltaMove;
    }
}

