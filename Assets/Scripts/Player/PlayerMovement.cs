using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



namespace GOG
{
    [RequireComponent(typeof(Controller2D))]
    public class PlayerMovement : MonoBehaviour
    {
        bool facingRight;
        public Animator animator;

        Controller2D controller;

        public InputActions playerControls;
        private Vector2 move;
        Vector2 input;


        Vector3 velocity;
        float moveSpeed = 6.8f;
        float velocityXSmoothing;
        float accelerationTimeAirborne = .2f;
        float accelerationTimeGrounded = .15f;

        public Vector2 wallJumpClimb;
        public Vector2 WallJumpOff;
        public Vector2 wallLeap;
        public float wallSlideSpeedMax = 2.5f;
        bool wallSliding;
        int wallDirX;

        // tässä ainaki laitetaan imputit

        private void Awake()
        {
            playerControls = new InputActions();
            playerControls.Player.Jump.performed += ctx => jumpHeld();
            playerControls.Player.Jump.canceled += ctx => jumpStopped();
            playerControls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            playerControls.Player.Attack.performed += ctx => attack();
        } // function

        private void OnEnable() { playerControls.Player.Enable(); }
        
        private void OnDisable() { playerControls.Player.Disable(); }

        float gravity;
        public float maxJumpHeight = 6f;
        public float minJumpHeight = 1.5f;
        public float timeToJumpApex = .3f;
        float maxJumpVelocity;
        float minJumpVelocity;


        // tässä tekee matskuu
        void Start()
        {
            // laskee laskut
            calculations();

            // tässä tekee komponentin 2d controller
            controller = GetComponent<Controller2D>();
        } // function


        void Update()
        {

     

            

            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // MOVING
            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // flip the character depending on the direction and set the run animation and if no running then idle
            // if is death lock movement totally
            if (!controller.collisions.death == true)
            {
                input = move;
            } // if statement

            // calling move function from controller
            wallDirX = (controller.collisions.left) ? -1 : 1;

            // somwhere here is move
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;

            controller.move(velocity * Time.deltaTime, input);


            // flip the character depending on the direction and set the run animation and if no running then idle
            if (velocity.x > 0.1 || velocity.x < -0.1)
            {
                animator.SetInteger("animation-select", 2);

                if (velocity.x > 0.1 && facingRight)
                {
                    Flip();
                }
                else if (velocity.x < -0.1 && !facingRight)
                {
                    Flip();
                }
            }
            else
            {
                animator.SetInteger("animation-select", 1);
            }

            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // CLIMBING
            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;
                if(!animator.GetBool("is-climbing") == true)
                {

                    Debug.Log("im here");
                    animator.SetBool("is-climbing", true);

                }
                    
                Debug.Log("Wall sliding");
                // start wall slide animation
                if (velocity.y < -wallSlideSpeedMax)
                    velocity.y = -wallSlideSpeedMax;
            } else
            {
                wallSliding = false;
                animator.SetBool("is-climbing", false);
            }

            if (controller.collisions.below)
            {
                animator.SetBool("is-jumping", false);
            }
                
            






            if (controller.collisions.above || controller.collisions.below)
                velocity.y = 0;

        } // function


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // JUMPING
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // Jos hyppy aloitetaan tämä

        private bool jumpHeld()
        {
            // If death then return
            if (controller.collisions.death == true) return false;

            if(!animator.GetBool("is-jumping"))
            {
                Debug.Log("is jumping");
                animator.SetBool("is-jumping", true);
            }
                
            
            // jump is put somewhere here
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

            if (controller.collisions.above || controller.collisions.below) {
                Debug.Log("Jumped");
                velocity.y = 0;
            }

            return true;
        } // function


        // Jos hyppy lopetetaan tämä

        private bool jumpStopped()
        {
            // return if character is dead
            if (controller.collisions.death == true) return false;

            // jump fall is somewhere here
            if (velocity.y > minJumpVelocity)
                velocity.y = minJumpVelocity;

            controller.move(velocity * Time.deltaTime, input);
            Debug.Log("jump stopped");

            // if collided stop y velocity?
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            return true;
        } // function

        private void attack()
        {

            Debug.Log("Attack");
        }


        // This is a flip function

        void Flip()
        {
            // Switch the way the player is labelled as facing
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // CALCULATIONS
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public void calculations()
        {
            gravity = calculateGravity(maxJumpHeight, timeToJumpApex);
            maxJumpVelocity = calculateMaxJumpVelocity(gravity, timeToJumpApex);
            minJumpVelocity = calculateMinJumpVelocity(gravity, minJumpHeight);
        }


        public float calculateGravity(float maxJumpHeight, float timeToJumpApex)
        {
            return -(2 * maxJumpHeight / Mathf.Pow(timeToJumpApex, 2));
        }

        public float calculateMaxJumpVelocity(float gravity, float timeToJumpApex)
        {
            return Mathf.Abs(gravity) * timeToJumpApex;
        }


        public float calculateMinJumpVelocity(float gravity, float minJumpHeight)
        {
            return Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        }



    } // class
} // namespace
