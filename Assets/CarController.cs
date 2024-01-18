using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] WheelCollider frontLeftCollider;
    [SerializeField] WheelCollider frontRightCollider;
    [SerializeField] WheelCollider backLeftCollider;
    [SerializeField] WheelCollider backRightCollider;

    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform backLeftTransform;
    [SerializeField] Transform backRightTransform;

    [SerializeField] float acceleration = 500f;
    [SerializeField] float breakingForce = 300f;
    [SerializeField] float maxTurnAngle = 15f;
    float currentAcceleration = 0;
    float currentBreakForce = 0;
    float currentTurnAngle = 0;
    void FixedUpdate()
    {
        
        currentAcceleration = Input.GetAxis("Vertical") * acceleration;

        if(Input.GetKey(KeyCode.Space)){
            currentBreakForce = breakingForce;
        }else{
            currentBreakForce = 0;
        }


        frontLeftCollider.motorTorque = currentAcceleration;
        frontRightCollider.motorTorque = currentAcceleration;

        frontLeftCollider.brakeTorque = currentBreakForce;
        frontRightCollider.brakeTorque = currentBreakForce;
        backLeftCollider.brakeTorque = currentBreakForce;
        backRightCollider.brakeTorque = currentBreakForce;

        currentTurnAngle = Input.GetAxis("Horizontal") * maxTurnAngle;

        frontLeftCollider.steerAngle = currentTurnAngle;
        frontRightCollider.steerAngle = currentTurnAngle;

        UpdateTire(frontLeftTransform, frontLeftCollider);
        UpdateTire(frontRightTransform, frontRightCollider);
        UpdateTire(backRightTransform, backRightCollider);
        UpdateTire(backLeftTransform, backLeftCollider);
    }

    void UpdateTire(Transform mesh, WheelCollider coll){
        Vector3 position;
        Quaternion rotation;
        coll.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "SlipperyZone"){
            WheelFrictionCurve wfc = backLeftCollider.sidewaysFriction;
            wfc.stiffness = 0.7f;
            backLeftCollider.sidewaysFriction = wfc;
            backRightCollider.sidewaysFriction = wfc;
            wfc.stiffness = 0.5f;
            frontLeftCollider.forwardFriction = wfc;
            frontRightCollider.forwardFriction = wfc;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "SlipperyZone"){
            WheelFrictionCurve wfc = backLeftCollider.sidewaysFriction;
            wfc.stiffness = 2f;
            backLeftCollider.sidewaysFriction = wfc;
            backRightCollider.sidewaysFriction = wfc;
            wfc.stiffness = 1f;
            frontLeftCollider.forwardFriction = wfc;
            frontRightCollider.forwardFriction = wfc;
        }
    }
}
