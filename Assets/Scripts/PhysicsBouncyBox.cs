using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBouncyBox : PhysicsReflectiveObject
{
    new const float staticVelocityLimit = 0.5f;

    private bool spaced = false;

    [SerializeField]
    float ballPushForce = 10f;

    void Start()
    {
        spheres = FindObjectsOfType<PhysicsSphere>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaced = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaced = false;
        }

        UpdateSpheres();
    }

    protected override void UpdateSpheres()
    {
        foreach (PhysicsSphere t in spheres)
        {
            if (t.onBouncyBox == this)
                Choc(t, GlobalParameters.chocEnergyDissipation);
        }
    }


    protected override void Choc(PhysicsSphere sphere, float energyDissipation = 0f)
    {
        //Debug.Log("choc: " + name);
        if (IsColliding(sphere) == false)
            return;

        if (spaced) // Yeet the ball
        {
            sphere.transform.position = CorrectedPosition(sphere);

            sphere.Velocity = Normal * ballPushForce;
            return;
        }


        if (IsSphereStaticOnBox(sphere))
        {
            sphere.transform.position = CorrectedPosition(sphere);
            sphere.ApplyForce(-sphere.mass * Physics.gravity);

        }
        else
        {
            sphere.transform.position = CorrectedPosition(sphere);
            InverseRelativeVelocity(sphere, Reflect(RelativeVelocity(sphere)));
        }

    }

    bool IsSphereStaticOnBox(PhysicsSphere sphere)
    {
        bool lowVelocity = RelativeVelocity(sphere).magnitude < staticVelocityLimit;

        return lowVelocity && TouchingThePlane(sphere);
    }

    bool TouchingThePlane(PhysicsSphere sphere)
    {
        // sphere speed
        // deltaTime
        float deltaMove = Mathf.Max(deltaMoveCoef * sphere.Radius, RelativeVelocity(sphere).magnitude * Time.deltaTime);
        return (CorrectedPosition(sphere) - sphere.transform.position).magnitude <= correctedPostionCoef * deltaMove;
    }

}
