using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GOG;

namespace GOG
{
    [RequireComponent (typeof(Controller2D))]
    public class Player : MonoBehaviour
    {
        Controller2D controller;

        public InputActions playerControls;
        private Vector2 move;

        Vector2 input;
        float gravity;
        public float maxJumpHeight = 7f;
        public float minJumpHeight = 1f;
        public float timeToJumpApex = .4f;
        float maxJumpVelocity;
        float minJumpVelocity;

        Vector3 velocity;
        float moveSpeed = 7f;
        float velocityXSmoothing;
        float accelerationTimeAirborne = .2f;
        float accelerationTimeGrounded = .1f;

        public Vector2 wallJumpClimb;
        public Vector2 WallJumpOff;
        public Vector2 wallLeap;
        public float wallSlideSpeedMax = 3;
        bool wallSliding;
        int wallDirX;

        private void Awake()
        {
            playerControls = new InputActions();
            playerControls.Player.Jump.performed += ctx => jumpHeld();
            playerControls.Player.Jump.canceled += ctx => jumpStopped();
            playerControls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        } // function

        private void OnEnable() { playerControls.Player.Enable(); }

        private void OnDisable() { playerControls.Player.Disable(); }

        void Start()
        {
            controller = GetComponent<Controller2D>();
            gravity = -(2 * maxJumpHeight / Mathf.Pow(timeToJumpApex, 2));
            maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity)*minJumpHeight);
        } // function

        void Update()
        {

            wallSliding = false;
            wallDirX = (controller.collisions.left) ? -1 : 1;

            if ((controller.collisions.left || controller.collisions.right)&& !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;
                if (velocity.y < -wallSlideSpeedMax)
                    velocity.y = -wallSlideSpeedMax;
            }

            input = move;
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;
            controller.move(velocity * Time.deltaTime, input);

            if (controller.collisions.above || controller.collisions.below)
                velocity.y = 0;
        } // function

        private void jumpHeld()
        {

            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * WallJumpOff.x;
                    velocity.y = WallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
                velocity.y = maxJumpVelocity;

            controller.move(velocity * Time.deltaTime, input);

            if (controller.collisions.above || controller.collisions.below)
                velocity.y = 0;

        } // function

        private void jumpStopped()
        {
            if (velocity.y > minJumpVelocity)
                velocity.y = minJumpVelocity;

            controller.move(velocity * Time.deltaTime, input);
            Debug.Log("jump stopped");

            if (controller.collisions.above || controller.collisions.below)
                velocity.y = 0;
        } // function


    } // class
} // namespace
