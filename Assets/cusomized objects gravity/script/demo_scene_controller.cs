using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace customized_objects_gravity_system
{
public class demo_scene_controller : MonoBehaviour
{
    public Rigidbody rotating_object;
    public float rotation_speed=10f;
        void FixedUpdate()
    {
        rotating_object.angularVelocity=new Vector3(rotation_speed,rotation_speed,rotation_speed);
    }
}
}