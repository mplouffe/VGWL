using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StepDirection
{
    Forward,
    Back,
    Right,
    Left,

    Count       // always leave last
}

public enum StepLength
{
    Long,
    Medium,
    Short,
    Pivot,
    Step,       // TODO: This naming is confusing and should be refactored, but it's this way to refelct the animation names

    Count       // always leave last
}

public class WrestlerMovement : MonoBehaviour
{
    public Transform movementTarget;
    public Transform fightStanceOffset;

    public CharacterController controller;
    public Animator animator;

    public float movementTargetRadius;
    public float turnSmoothTime = 0.1f;

    public float sideStepDiffThreshold;
    public float pivotDiffThreshold;

    public float longStepThreshold;
    public float mediumStepThreshold;
    public float shortStepThreshold;

    public bool step;

    private bool playingStepAnimation;
    private bool playingIdle;

    private float gravity = 0f;

    float turnSmoothVelocity;

    private int[,] animationMap;

    private int idle;

    // Start is called before the first frame update
    void Awake()
    {
        animationMap = new int[(int)StepDirection.Count, (int)StepLength.Count];
        idle = Animator.StringToHash("Idle");

        // This is clunky but better than nothing right now
        // At least it's clear how it works (if proabably not performant right now)
        animationMap[(int)StepDirection.Forward, (int)StepLength.Long] = Animator.StringToHash("Forward_Long");
        animationMap[(int)StepDirection.Forward, (int)StepLength.Medium] = Animator.StringToHash("Forward_Medium");
        animationMap[(int)StepDirection.Forward, (int)StepLength.Short] = Animator.StringToHash("Forward_Short");
        animationMap[(int)StepDirection.Forward, (int)StepLength.Step] = Animator.StringToHash("Forward_Step");             // TODO: Step name refactor
        animationMap[(int)StepDirection.Back, (int)StepLength.Long] = Animator.StringToHash("Back_Long");
        animationMap[(int)StepDirection.Back, (int)StepLength.Medium] = Animator.StringToHash("Back_Medium");
        animationMap[(int)StepDirection.Back, (int)StepLength.Short] = Animator.StringToHash("Back_Short");
        animationMap[(int)StepDirection.Back, (int)StepLength.Step] = Animator.StringToHash("Back_Step");                   // TODO: Step name refactor
        animationMap[(int)StepDirection.Left, (int)StepLength.Pivot] = Animator.StringToHash("Left_Pivot");
        animationMap[(int)StepDirection.Left, (int)StepLength.Long] = Animator.StringToHash("Left_Side_Long");
        animationMap[(int)StepDirection.Left, (int)StepLength.Medium] = Animator.StringToHash("Left_Side_Medium");
        animationMap[(int)StepDirection.Left, (int)StepLength.Short] = Animator.StringToHash("Left_Side_Short");
        animationMap[(int)StepDirection.Left, (int)StepLength.Step] = Animator.StringToHash("Left_Step");                   // TODO: Step refactor
        animationMap[(int)StepDirection.Right, (int)StepLength.Pivot] = Animator.StringToHash("Right_Pivot");
        animationMap[(int)StepDirection.Right, (int)StepLength.Long] = Animator.StringToHash("Right_Side_Long");
        animationMap[(int)StepDirection.Right, (int)StepLength.Medium] = Animator.StringToHash("Right_Side_Medium");
        animationMap[(int)StepDirection.Right, (int)StepLength.Short] = Animator.StringToHash("Right_Side_Short");
        animationMap[(int)StepDirection.Right, (int)StepLength.Step] = Animator.StringToHash("Right_Step");                 // TODO: Step refactor
    }

    // Update is called once per frame
    void Update()
    {
        if (!playingStepAnimation && step)
        {
            var distanceFromTarget = (movementTarget.position - transform.position).magnitude;

            if (distanceFromTarget > movementTargetRadius)
            {
                Vector3 vectorToTarget = movementTarget.position - transform.position;
                Vector3 vectorToTargetRight = Vector3.Cross(vectorToTarget.normalized, Vector3.up.normalized);
                vectorToTargetRight.Scale(new Vector3(1, 0, 1));
                Vector3 fightStanceOffsetModified = fightStanceOffset.forward;
                fightStanceOffsetModified.Scale(new Vector3(1, 0, 1));

                var lookDirectionDifferental = Vector3.Dot(fightStanceOffsetModified.normalized, vectorToTargetRight.normalized);
                Debug.Log("lookDirectionDifferental: " + lookDirectionDifferental);
                Debug.Log("DistanceFromTarget: " + distanceFromTarget);
                StepDirection stepDirection;
                StepLength stepLength = StepLength.Step;

                if (lookDirectionDifferental < -sideStepDiffThreshold)
                {
                    stepDirection = StepDirection.Right;
                }
                else if (lookDirectionDifferental > sideStepDiffThreshold)
                {
                    stepDirection = StepDirection.Left;
                }
                else
                {
                    stepDirection = StepDirection.Forward;
                }



                if (lookDirectionDifferental < -pivotDiffThreshold || lookDirectionDifferental > pivotDiffThreshold)
                {
                    stepLength = StepLength.Pivot;
                }
                else
                {
                    if (distanceFromTarget > longStepThreshold)
                    {
                        stepLength = StepLength.Long;
                    }
                    else if (distanceFromTarget > mediumStepThreshold)
                    {
                        stepLength = StepLength.Medium;
                    }
                    else if (distanceFromTarget > shortStepThreshold)
                    {
                        stepLength = StepLength.Short;
                    }
                }

                Debug.Log("StepCalled: " + stepDirection + " " + stepLength);
                animator.Play(animationMap[(int)stepDirection, (int)stepLength], -1, 0);
                playingStepAnimation = true;
            }
            else if (!playingIdle)
            {
                playingIdle = true;
                animator.Play(idle);
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            playingStepAnimation = false;
        }

        // apply gravity
        gravity -= 9.81f * Time.deltaTime;
        Vector3 direction = new Vector3(0, 1, 0).normalized;


        if (direction.magnitude >= 0.1f)
        {
            // rotate torward target
            // this should be changed to try and lerp the direction on the animations towards the desired target
            //float targetAngle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * gravity * Time.deltaTime);
        }
    }

    private void PlayStepAnimation(StepDirection direction, StepLength length)
    {
        
    }
}
