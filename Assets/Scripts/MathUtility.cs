using UnityEngine;
using System.Collections;

public class MathUtility {
	
	public static bool NearlyEqual(float a, float b, float epsilon)
	{
		float absA = Mathf.Abs(a);
		float absB = Mathf.Abs(b);
		float diff = Mathf.Abs(a - b);
		
		if (a == b)
		{ // shortcut, handles infinities
			return true;
		} 
		else if (a == 0 || b == 0 || diff < float.MinValue) 
		{
			// a or b is zero or both are extremely close to it
			// relative error is less meaningful here
			return diff < (epsilon * float.MinValue);
		}
		else
		{ // use relative error
			return diff / (absA + absB) < epsilon;
		}
	}

	public static bool NearlyEqualOrGreater(float a, float b, float epsilon)
	{
		return a > b || NearlyEqual(a, b, epsilon);
	}

	public static bool NearlyEqualOrLesser(float a, float b, float epsilon)
	{
		return a < b || NearlyEqual(a, b, epsilon);
	}
}
