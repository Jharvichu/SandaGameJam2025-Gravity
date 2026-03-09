using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Run : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Movement move;
    [SerializeField] private Dash dash;
    [SerializeField] private PhysicsManager physics;

    [Header("RunTime")]
    [SerializeField] private float currentMaxSpeed;
    [SerializeField] private float desiredMaxSpeed;
    [SerializeField] private float lastDesiredMaxSpeed;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool runAction;
    private Coroutine speedControl;

    [Header("Walking")]
    [SerializeField] private float walkMaxSpeed;
    [SerializeField] private float walkAcceleration;

    [Header("Running")]
    [SerializeField] private float runMaxSpeed;
    [SerializeField] private float runAcceleration;

    [Header("Dashing")]
    [SerializeField] private float dashMaxSpeed;
    [SerializeField] private float dashAcceleration;

    [Header("Additional Configurations")]
    [SerializeField] private float timeSmoothing;
    [SerializeField] private float smoothingThreshold;
    [SerializeField] private float coyoteRunTime;

    private void Update()
    {
        ProcessInput();
        SetDesiredMaxSpeedAndAcceleration();
        SetCurrentMaxSpeed();

        move.SetMaxSpeed(currentMaxSpeed);
    }

    public void GetInput(InputAction.CallbackContext context)
    {
        if(context.performed) runAction = true;

        if(context.canceled) runAction = false;
    }

    private void ProcessInput()
    {
        if(runAction && Time.time - physics.LastGroundTime <= coyoteRunTime) isRunning = true;

        if(!runAction) isRunning = false;
    }

    private void SetDesiredMaxSpeedAndAcceleration()
    {
        float acceleration;

        if(isRunning)
        {
            desiredMaxSpeed = runMaxSpeed;
            acceleration = runAcceleration;
        }
        if (dash.IsDashing)
        {
            desiredMaxSpeed = dashMaxSpeed;
            acceleration = dashAcceleration;
        }
        else
        {
            desiredMaxSpeed = walkMaxSpeed;
            acceleration = walkAcceleration;
        }

        move.SetAcceleration(acceleration); 
    }

    private IEnumerator SmoothlyLerpMaxSpeed()
    {
        float time = 0f;
        float difference = Mathf.Abs(desiredMaxSpeed - currentMaxSpeed) * timeSmoothing;
        float startValue = currentMaxSpeed;

        while(time < difference)
        {
            currentMaxSpeed = Mathf.Lerp(startValue, desiredMaxSpeed, time/difference);
            time += Time.deltaTime;

            yield return null;
        }

        currentMaxSpeed = desiredMaxSpeed;
    }

    private void SetCurrentMaxSpeed()
    {
        if(lastDesiredMaxSpeed == 0) lastDesiredMaxSpeed = desiredMaxSpeed;

        if(Mathf.Abs(desiredMaxSpeed - lastDesiredMaxSpeed) > smoothingThreshold)
        {
            if(speedControl != null) StopCoroutine(speedControl);
            speedControl = StartCoroutine(SmoothlyLerpMaxSpeed());
        } else currentMaxSpeed = desiredMaxSpeed;

        lastDesiredMaxSpeed = desiredMaxSpeed;
    }

}