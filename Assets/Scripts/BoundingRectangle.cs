using UnityEngine;
using cyclone;
using Vector3 = cyclone.Vector3;

public class BoundingRectangle
{
    public Vector3 center;
    public Vector3 contactNormal;
    public float length;
    public float width;
    public float interpenetratingX;
    public float interpenetratingY;
    public float penetration;
    public bool overlaps;

    public BoundingRectangle(Vector3 center, float length, float width)
    {
        this.center = center;
        this.length = length;
        this.width = width;
    }

    public bool Overlaps(BoundingRectangle boundingRectangle)
    {
        overlaps = false;

        float rightX = center.x + length / 2;
        float leftX = center.x - length / 2;
        float upperY = center.y + width / 2;
        float lowerY = center.y - width / 2;

        float otherRectangleRightX = boundingRectangle.center.x + boundingRectangle.length / 2;
        float otherRectangleLeftX = boundingRectangle.center.x - boundingRectangle.length / 2;
        float otherRectangleUpperY = boundingRectangle.center.y + boundingRectangle.width / 2;
        float otherRectangleLowerY = boundingRectangle.center.y - boundingRectangle.width / 2;

        if (otherRectangleLeftX <= leftX && otherRectangleRightX >= leftX)
        {
            if(otherRectangleLowerY <= lowerY && otherRectangleUpperY >= lowerY)
            {
                interpenetratingX = Mathf.Abs(leftX - otherRectangleRightX);
                if(length < interpenetratingX)
                {
                    interpenetratingX = length;
                }
                interpenetratingY = Mathf.Abs(lowerY - otherRectangleUpperY);
                if(width < interpenetratingY)
                {
                    interpenetratingY = width;
                }
                overlaps = true;
            }
            else if (otherRectangleLowerY <= upperY && otherRectangleUpperY >= upperY)
            {
                interpenetratingX = Mathf.Abs(leftX - otherRectangleRightX);
                if (length < interpenetratingX)
                {
                    interpenetratingX = length;
                }
                interpenetratingY = Mathf.Abs(upperY - otherRectangleLowerY);
                if (width < interpenetratingY)
                {
                    interpenetratingY = width;
                }
                overlaps = true;
            }
            else if (otherRectangleLowerY >= lowerY && otherRectangleUpperY <= upperY)
            {
                interpenetratingX = Mathf.Abs(leftX - otherRectangleRightX);
                if (length < interpenetratingX)
                {
                    interpenetratingX = length;
                }
                interpenetratingY = width;
                overlaps = true;
            }
        }
        else if (otherRectangleLeftX <= rightX && otherRectangleRightX >= rightX)
        {
            if (otherRectangleLowerY <= lowerY && otherRectangleUpperY >= lowerY)
            {
                interpenetratingX = Mathf.Abs(rightX - otherRectangleLeftX);
                if (length < interpenetratingX)
                {
                    interpenetratingX = length;
                }
                interpenetratingY = Mathf.Abs(lowerY - otherRectangleUpperY);
                if (width < interpenetratingY)
                {
                    interpenetratingY = width;
                }
                overlaps = true;
            }
            else if (otherRectangleLowerY <= upperY && otherRectangleUpperY >= upperY)
            {
                interpenetratingX = Mathf.Abs(rightX - otherRectangleLeftX);
                if (length < interpenetratingX)
                {
                    interpenetratingX = length;
                }
                interpenetratingY = Mathf.Abs(upperY - otherRectangleLowerY);
                if (width < interpenetratingY)
                {
                    interpenetratingY = width;
                }
                overlaps = true;
            }
            else if (otherRectangleLowerY >= lowerY && otherRectangleUpperY <= upperY)
            {
                interpenetratingX = Mathf.Abs(rightX - otherRectangleLeftX);
                if (length < interpenetratingX)
                {
                    interpenetratingX = length;
                }
                interpenetratingY = width;
                overlaps = true;
            }
        }
        if (overlaps)
        {
            if(interpenetratingX >= interpenetratingY)
            {
                penetration = interpenetratingY;
                if(center.y > boundingRectangle.center.y)
                {
                    contactNormal = new Vector3(0f, -1f, 0f);
                }
                else if (center.y < boundingRectangle.center.y)
                {
                    contactNormal = new Vector3(0f, 1f, 0f);
                }
            }
            else
            {
                penetration = interpenetratingX;
                if (center.x > boundingRectangle.center.x)
                {
                    contactNormal = new Vector3(-1f, 0f, 0f);
                }
                else if (center.x < boundingRectangle.center.x)
                {
                    contactNormal = new Vector3(1f, 0f, 0f);
                }
            }
        }
        return overlaps;
    }
}
