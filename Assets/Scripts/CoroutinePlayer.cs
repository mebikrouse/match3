using System.Collections;
using UnityEngine;

public interface ICoroutinePlayer
{
    Coroutine PlayCoroutine(IEnumerator coroutineEnumerator);
    void StopCoroutine(Coroutine coroutine);
    void StopAllCoroutines();
}

public class CoroutinePlayer : MonoBehaviour, ICoroutinePlayer
{
    public Coroutine PlayCoroutine(IEnumerator coroutineEnumeration)
    {
        return StartCoroutine(coroutineEnumeration);
    }

    public new void StopCoroutine(Coroutine coroutine)
    {
        base.StopCoroutine(coroutine);
    }

    public new void StopAllCoroutines()
    {
        base.StopAllCoroutines();
    }
}
