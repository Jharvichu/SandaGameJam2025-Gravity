using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace customized_objects_gravity_system
{
public class customized_objects_gravity : MonoBehaviour
{
    private Vector3 final_vector;
    private Vector3 closest_point;
    public float gravity_force = 9.81f;
    private Collider current_gravity_center;
    private Collider[] nearby_colliders;
    public float radius = 7f;
    public LayerMask layer_mask;
    private ConstantForce cf;
    private int counter;
    void Awake()
    {
        final_vector = Vector3.zero;
        cf = GetComponent<ConstantForce>();
    }
    void FixedUpdate()
    {
        counter++;
        if (counter % 5 == 0)
        {
            nearby_colliders = Physics.OverlapSphere(transform.position, radius, layer_mask);
            if (nearby_colliders.Length == 0)
            {
                current_gravity_center = null;
            }
            else
            {
                float min_dest = 10000f;
                foreach (var col in nearby_colliders)
                {
                    if ((col.ClosestPoint(transform.position) - transform.position).sqrMagnitude <= min_dest && col.gameObject != this.gameObject)
                    {
                        min_dest=(col.ClosestPoint(transform.position) - transform.position).sqrMagnitude;
                        current_gravity_center = col;
                    }
                }
            }
        }
        if (current_gravity_center)
        {
            closest_point = current_gravity_center.ClosestPoint(transform.position);
            RaycastHit hit;
            Physics.Raycast(transform.position, (closest_point - transform.position).normalized, out hit, Mathf.Infinity, layer_mask);
            if (Vector3.Angle(hit.normal.normalized, final_vector) > 7f || final_vector == Vector3.zero)
            {
                final_vector = hit.normal.normalized;
            }
        }
        else
        {
            final_vector = new Vector3(0, 0, 0);
        }
        cf.force = final_vector * -gravity_force;
    }
}
}