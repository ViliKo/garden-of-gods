using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOG
{
    [RequireComponent (typeof(BoxCollider2D))]
    public class Controller2D : raycastController
    {
        public LayerMask collisionMask;

        private float maxClimbAngle = 80f;
        private float maxDescendAngle = 75f;

        public collisionInfo collisions;
        private Vector2 playerInput;

        public struct collisionInfo
        {
            public bool above, below, left, right;
            public bool climbingSlope, descendingSlope;
            public float slopeAngle, slopeAngleOld;
            public Vector3 velocityOld;
            public int faceDirection;
            public bool fallingThroughPlatform;

            public void Reset()
            {
                above = below = left = right = false;
                climbingSlope = descendingSlope = false;
                slopeAngleOld = slopeAngle;
                slopeAngle = 0f;
            }
        } // struct

        public override void Start()
        {
            base.Start();
            collisions.faceDirection = 1; // 1 means right -1 means left
        } // function

        void resetFallingThroughPlatform() { collisions.fallingThroughPlatform = false; }

        public void move(Vector3 velocity, bool standingOnPlatform = false) { move(velocity, Vector2.zero, standingOnPlatform); } // this is an override

        public void move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false)
        {
            updateRaycastOrigins();
            collisions.Reset();
            collisions.velocityOld = velocity;
            playerInput = input;

            if (velocity.x != 0)
                collisions.faceDirection = (int)Mathf.Sign(velocity.x); // what direction is the character facing


            if (velocity.y < 0)
                descendSlope(ref velocity);

            horizontalCollisions(ref velocity);


            if (velocity.y != 0)
                verticalCollisions(ref velocity);

            transform.Translate(velocity);

            if (standingOnPlatform)
                collisions.below = true;
        } // function


        void verticalCollisions(ref Vector3 velocity)
        {
            // 1. tarkista onko y-akselin suunta ylös vai alas
            // 2. päivitä säteiden koko skin width on marginaali henkilössä
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                // 3. katso säteen aloituskohta objektissa, jos suunta on vasemmalle, niin säde tulee vasemmalta ala reunasta, muuten tulee yläreunasta 
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                // 4. 1 * vertickaalinen ray lomitus kertaa i kertaa nopues x akselilla
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                // 5. Tunnistaa raycast osuman käyttämällä collision maskia
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
                // 6. piirtää säteen
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                // onko osuttu
                if (hit)
                {
                    // jos tagi on läpi
                    if (hit.collider.tag == "through")
                    {
                        // ja jos suunta on ylös älä välitä
                        if (directionY == 1)
                            continue;
                        // jos on jo kaatumassa läpi esteen älä välitä
                        if (collisions.fallingThroughPlatform)
                            continue;

                        // jos pelajaan input on alaspäin
                        if (playerInput.y == -1)
                        {
                            // pelaaja on kaatumassa läpi esteen
                            collisions.fallingThroughPlatform = true;
                            // vaihda viiden sekunnin päästä booli takaisin epätodeksi
                            Invoke("resetFallingThroughPlatform", .5f);
                            continue;
                        }
                    } // if


                    // y:n nopeus on osuman pituus * y:n suunta
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    // rayn pituus on osuman pituus
                    rayLength = hit.distance;

                    // jos osuu jyrkkään nousuun
                    if (collisions.climbingSlope)
                        velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    // silloin x:nopeus on y-aksein nopeus jaettuna tangtti(nousun kulma, muunnettuna radiaaneihin), kerrottun nopeuden suunta

                    // osumia 
                    collisions.below = directionY == -1;
                    collisions.right = directionY == 1;
                } // if
            } // for

            if (collisions.climbingSlope)
            {
                float directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + skinWidth;
                Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if(slopeAngle != collisions.slopeAngle)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        collisions.slopeAngle = slopeAngle;
                    } // if
                } // if
            } // if


        } // function



        void horizontalCollisions(ref Vector3 velocity)
        {
            // 1. ota colliderin laatikon ääripisteet. 2. päivitä raytten lokaatio. 
            // 3. laske raytten välit. 4. Lataa rayt kohtaukseen esimerkiksi

            // näistä kahdesta arvosta voisi saada funktion
            float directionX = collisions.faceDirection;
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            if (Mathf.Abs(velocity.x) < skinWidth)
                rayLength = 2 * skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing* i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {
                    if (hit.distance == 0 || hit.distance == 0)
                        continue;

                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (i == 0 && slopeAngle <= maxClimbAngle)
                    {
                        if (collisions.descendingSlope)
                        {
                            collisions.descendingSlope = false;
                            velocity = collisions.velocityOld;
                        }

                        float distanceToSlopeStart = 0;

                        if (slopeAngle != collisions.slopeAngleOld)
                        {
                            distanceToSlopeStart = hit.distance - skinWidth;
                            velocity.x -= distanceToSlopeStart * directionX;
                        }

                        climbSlope(ref velocity, slopeAngle);
                        velocity.x += distanceToSlopeStart * directionX;
                    } // if

                    if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        rayLength = hit.distance;

                        if(collisions.climbingSlope)
                            velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);

                        collisions.left = directionX == -1;
                        collisions.right = directionX == 1;
                    } // if
                } // if
            } // for
        } // function

        void climbSlope(ref Vector3 velocity, float slopeAngle)
        {
            // functionality for climbing slopes
            float moveDistance = Mathf.Abs(velocity.x);
            float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            if (velocity.y <= climbVelocityY)
            {
                velocity.y = climbVelocityY;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                collisions.below = true;
                collisions.climbingSlope = true;
                collisions.slopeAngle = slopeAngle;
            } // if


        } // function

        void descendSlope(ref Vector3 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                        {
                            float moveDistance = Mathf.Abs(velocity.x);
                            float descendbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                            velocity.y -= descendbVelocityY;
                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                        } // if
                    } // if
                } // if
            } // if
        } // function
    } // class
} // namespace


