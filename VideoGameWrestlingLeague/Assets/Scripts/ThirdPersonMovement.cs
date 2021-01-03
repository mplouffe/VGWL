using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform target;
    public Animator animator;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float gravity = 0f;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        gravity -= 9.81f * Time.deltaTime;
        Vector3 direction = new Vector3(0, gravity, 0).normalized;
        Vector3 vectorToTarget = (target.position - transform.position).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * speed * Time.deltaTime);
        }

        //var localVelocity = transform.InverseTransformDirection(controller.velocity);
        //var projectedVelocity = Vector3.Project(localVelocity, transform.forward).normalized;
        var projectedVelocity = new Vector3(horizontal, 0, vertical);
        animator.SetFloat("ForwardVelocity", projectedVelocity.z);
        animator.SetFloat("LateralVelocity", projectedVelocity.x);
        animator.SetBool("SideStepping", Mathf.Abs(projectedVelocity.x) > Mathf.Abs(projectedVelocity.z));

        // Debug.Log("ForwardVeloicty: " + animator.GetFloat("ForwardVelocity") + " LateralVelocity: " + animator.GetFloat("LateralVelocity") + " SideStepping: " + animator.GetBool("SideStepping"));
        

        if (controller.isGrounded)
        {
            gravity = 0;
        }
    }
}
