using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Vector2 XZ(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    public static Vector2 XY(this Vector3 vec)
    {
    	return new Vector2(vec.x, vec.y);
    }

    public static Vector3 XYZ(this Vector2 vec, float z)
    {
    	return new Vector3(vec.x, vec.y, z);
    } 
	
    public static Vector3 XSetYZ(this Vector2 vec, float y)
    {
        return new Vector3(vec.x, y, vec.y);  
    }
	public static float DistanceSqr(this Vector2 p1, Vector2 p2)
	{
		float distX = p1.x - p2.x;
		float distY = p1.y - p2.y;

		return (distX * distX) + (distY * distY);
	}

    public static Vector3 OrthoNormalize(Vector3 vector1, Vector3 vector2)
    {
        vector1.Normalize();
        Vector3 temp = Vector3.Cross(vector1, vector2);
        temp.Normalize();
        vector2 = Vector3.Cross(temp, vector2);
        return vector2;
        //var output = new Vector3[] {vector1, vector2};
        //return output;
    }


    public static void AddTorqueAtPosition(this Rigidbody2D thisRigidbody, Vector3 torque, Vector3 position)
    {
        //http://forum.unity3d.com/threads/torque-at-offset.187297/
        Vector3 torqueAxis = torque.normalized;
        Vector3 ortho = new Vector3(1, 0, 0);
        // prevent torqueAxis and ortho from pointing in the same direction
        if ((torqueAxis - ortho).sqrMagnitude < float.Epsilon)
        {
            ortho = new Vector3(0, 1, 0);
        }

        var orthoNorm = ExtensionMethods.OrthoNormalize(torqueAxis, ortho);


        // calculate force
        Vector3 force = Vector3.Cross(0.5f * torque, orthoNorm);
        thisRigidbody.AddForceAtPosition(force, position + orthoNorm);
        thisRigidbody.AddForceAtPosition(-force, position - orthoNorm);

    }
}