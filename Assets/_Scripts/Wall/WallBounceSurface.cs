using UnityEngine;

public enum WallOrientation { Vertical, Horizontal }

// Attach to LeftWall/RightWall with Vertical, and TopWall with Horizontal.
public class WallBounceSurface : MonoBehaviour
{
    public WallOrientation orientation = WallOrientation.Vertical;
}