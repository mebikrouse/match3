using System;
using System.Collections;
using UnityEngine;

public static class CoroutineAnimations
{
    private const float CALCULATION_ERROR = 0.0001f;

    public static IEnumerator AcceleratedMovement(GameObject target, Vector2 from, Vector2 to, float startSpeed, float velocity, Action callback)
    {
        float currentSpeed = startSpeed;
        target.transform.localPosition = from;

        while(Vector2.Distance(target.transform.localPosition, to) > 0 + CALCULATION_ERROR)
        {
            target.transform.localPosition = Vector2.MoveTowards(target.transform.localPosition, to, currentSpeed * Time.deltaTime);
            currentSpeed += velocity * Time.deltaTime;

            yield return null;
        }

        target.transform.localPosition = to;

        if (callback != null) callback();
    }

    public static IEnumerator MovementWithEasingFunction(GameObject target, Vector2 from, Vector2 to, float duration, Func<float, float, float, float> easingFunction, Action callback)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            target.transform.localPosition = Vector2.LerpUnclamped(from, to, easingFunction(0, 1, timeElapsed / duration));

            yield return null;

            timeElapsed += Time.deltaTime;
        }

        target.transform.localPosition = to;

        if (callback != null) callback();
    }
}
