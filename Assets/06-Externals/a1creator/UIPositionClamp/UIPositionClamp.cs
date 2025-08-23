using UnityEngine;

namespace a1creator
{
    public static class UIPositionClamp
    {
        private static Rect _screenBounds;
        private static Rect ScreenBounds
        {
            get
            {
                if (_screenBounds == null || _screenBounds == default(Rect))
                    _screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
                return _screenBounds;
            }
        }

        public struct Corner
        {
            public Vector3 Position;
            public bool InsideScreen;
        }

        public struct Corners
        {
            public Corner TopLeft;
            public Corner TopRight;
            public Corner BottomLeft;
            public Corner BottomRight;
            public int VisibleCount;
        }

        /// <summary>
        /// Set position of RectTransform but shove it back to the edge of screen if it gets placed outside.
        /// </summary>
        public static void SetPositionInsideScreen(this RectTransform rt, Vector3 position)
        {
            rt.position = position;
            var corners = CornersVisible(rt);

            // If the rect is already inside the screen, do nothing
            if (corners.VisibleCount == 4) return;

            var newpos = (Vector2)rt.position;

            // If the rect is completely outside the screen, place it on the nearest edge
            if (corners.VisibleCount <= 0)
            {
                var distance = CalcDistanceFromEdge(newpos);
                newpos += distance;
                rt.position = newpos;
                corners = CornersVisible(rt);
            }


            // If the rect is touching an edge, move it inside the screen
            if (!corners.TopRight.InsideScreen&& !corners.BottomRight.InsideScreen)
            {
                var distance = CalcDistanceFromEdge(corners.TopRight.Position);
                //Debug.Log("Distance from edge (Right): " + distance);
                newpos.x += distance.x;
            }
            if (!corners.TopLeft.InsideScreen&& !corners.BottomLeft.InsideScreen)
            {
                var distance = CalcDistanceFromEdge(corners.TopLeft.Position);
                //Debug.Log("Distance from edge (Left): " + distance);
                newpos.x += distance.x;
            }
            if (!corners.TopLeft.InsideScreen && !corners.TopRight.InsideScreen)
            {
                var distance = CalcDistanceFromEdge(corners.TopLeft.Position);
                //Debug.Log("Distance from edge (Top): " + distance);
                newpos.y += distance.y;
            }
            if (!corners.BottomLeft.InsideScreen && !corners.BottomRight.InsideScreen)
            {
                var distance = CalcDistanceFromEdge(corners.BottomRight.Position);
                //Debug.Log("Distance from edge (Bottom): " + distance);
                newpos.y += distance.y;
            }

            rt.position = newpos;
        }

        private static Vector2 CalcDistanceFromEdge(Vector2 position)
        {
            var newX = 0f;
            var newY = 0f;

            if (position.x <= 0f)
                newX = -position.x;

            if (position.y <= 0f)
                newY = -position.y;

            if (position.x > ScreenBounds.size.x)
                newX = ScreenBounds.size.x - position.x;

            if (position.y > ScreenBounds.size.y)
                newY = ScreenBounds.size.y - position.y;

            return new Vector2(newX, newY);
        }

        public static Corners CornersVisible(this RectTransform rectTransform, Camera camera = null)
        {
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            Corner[] corners = new Corner[4];
            Corners visibleCorners = new Corners();

            corners[0] = GetInsideCorner(objectCorners[0]);
            corners[1] = GetInsideCorner(objectCorners[1]);
            corners[2] = GetInsideCorner(objectCorners[2]);
            corners[3] = GetInsideCorner(objectCorners[3]);

            for (int i = 0; i < corners.Length; i++)
            {
                if (corners[i].InsideScreen)
                    visibleCorners.VisibleCount++;
            }

            visibleCorners.BottomLeft = corners[0];
            visibleCorners.TopLeft = corners[1];
            visibleCorners.TopRight = corners[2];
            visibleCorners.BottomRight = corners[3];

            return visibleCorners;
        }

        private static Corner GetInsideCorner(Vector2 position)
        {
            var corner = new Corner();
            corner.Position = position;
            corner.InsideScreen = CornerIsInsideScreen(position);
            return corner;
        }

        // I forgot where I found this. Usually I give credit so I'm sorry to that person :(
        private static bool CornerIsInsideScreen(Vector3 corner, Camera camera = null)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            Vector3 tmpScreenSpaceCorner;
            if (camera != null)
                tmpScreenSpaceCorner = camera.WorldToScreenPoint(corner); // Transform world space position of corner to screen space
            else
            {
                tmpScreenSpaceCorner = corner; // If no camera is provided we assume the canvas is Overlay and world space == screen space
            }

            if (screenBounds.Contains(tmpScreenSpaceCorner)) // If the corner is inside the screen
                return true;
            return false;
        }
    }
}