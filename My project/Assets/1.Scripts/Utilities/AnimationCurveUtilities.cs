using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveUtilities 
{
   public static Vector3 EvaluateCurves(this AnimationCurve[] animationCurves, float time)
   {
        if(animationCurves == null || animationCurves.Length != 3)
        {
            return default;
        }

        return new Vector3
        {
            x = animationCurves[0].Evaluate(time),
            y = animationCurves[1].Evaluate(time),
            z = animationCurves[2].Evaluate(time),
        };
   }
}
