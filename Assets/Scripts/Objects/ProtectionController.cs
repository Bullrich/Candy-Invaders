using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Obj {
	public class ProtectionController : RaycastController {

        public CollisionInfo collisions;

        public override void Start()
        {
            base.Start();
            collisions.faceDir = 1;

        }

        void TouchDetection()
        {
            Vector2 moveAmount;
            UpdateRaycastOrigins();

            collisions.Reset();

            moveAmount = Vector2.left;
            HorizontalCollisions(ref moveAmount);
            moveAmount = Vector2.right;
            HorizontalCollisions(ref moveAmount);
            moveAmount = Vector2.zero;
            VerticalCollisions(ref moveAmount);
        }

        private void Update()
        {
            TouchDetection();
        }

        public bool HasCollided()
        {
            if (collisions.below || collisions.above || collisions.left || collisions.right)
                return true;
            return false;
        }

        void HorizontalCollisions(ref Vector2 moveAmount)
        {
            float directionX = moveAmount.x;
            float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

            if (Mathf.Abs(moveAmount.x) < skinWidth)
            {
                rayLength = 2 * skinWidth;
            }

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

                if (hit)
                {

                    if (hit.distance == 0)
                    {
                        continue;
                    }
                    if (directionX == 1)
                        collisions.right = true;
                    else
                        collisions.left = true;
                }
            }
        }

        void VerticalCollisions(ref Vector2 moveAmount)
        {
            float directionY = Mathf.Sign(moveAmount.y);
            float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {

                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

                if (hit)
                {
                    moveAmount.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.above = directionY == 1;
                }
            }
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;
            public int faceDir;

            public void Reset()
            {
                above = below = false;
                left = right = false;
            }
        }
    }
}