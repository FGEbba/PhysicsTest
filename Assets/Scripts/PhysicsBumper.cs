using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBumper : PhysicsReflectiveObject
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
            if (t.onBumper == this)
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
        if (IsColliding(sphere) == false)
            return;

        InverseRelativeVelocity(sphere, Reflect(RelativeVelocity(sphere), energyDissipation));
        //if (IsSphereStaticOnPlane(sphere))
        //{
        //    //sphere.transform.position = CorrectedPosition(sphere);
        //    sphere.ApplyForce(-sphere.mass * Physics.gravity);
        //}
        //else // sphere is dynamic
        //{
        //    //sphere.transform.position = CorrectedPosition(sphere);

        //}
    }

}
