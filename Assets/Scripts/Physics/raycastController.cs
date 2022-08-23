using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOG
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class raycastController : MonoBehaviour
    {
        public const float skinWidth = .015f;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        [HideInInspector]
        public new BoxCollider2D collider;
        public RaycastOrigins raycastOrigins;

        [HideInInspector]
        public float horizontalRaySpacing;
        [HideInInspector]
        public float verticalRaySpacing;


        public virtual void Start()
        {
            collider = GetComponent<BoxCollider2D>();
            calculateRaySpacing();
        }

        public struct RaycastOrigins
        {
            // Ota talteen objektin laatikko colliderin ‰‰riarvot
            // Vector2 s‰ilytt‰‰ 2D lokaatio tietoja (x,y) (-1, 1) = one esimerkiksi yl‰ vasemmalla.
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
        public void updateRaycastOrigins()
        {
            // P‰ivit‰ pisteet mist‰ raycastataan
            // t‰m‰ ottaa kaikki tiedot eri ‰‰ripisteist‰ jatkuvasti talteen
            Bounds bounds = getBounds();
            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        public void calculateRaySpacing()
        {
            // p‰ivit‰ monta viivaa heijastetaan objektista vaaka ja pystytasossa
            Bounds bounds = getBounds();
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        private Bounds getBounds()
        {
            Bounds bounds = collider.bounds;
            bounds.Expand(skinWidth * -2);
            return bounds;
        }
    }

}

