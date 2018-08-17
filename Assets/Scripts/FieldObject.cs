using System;
using UnityEngine;

public interface IGameObjectable
{
    GameObject GetGameObject();
}

public interface IDestroyable
{
    void Destroy(Action callback);
}

public interface IComparable
{
    bool EqualsTo(IComparable obj);
}

[RequireComponent(typeof(Animation))]
public class FieldObject : MonoBehaviour, IGameObjectable, IDestroyable, IComparable
{
    [SerializeField]
    private int fieldObjectType;

    private Animation animationComponent;
    private Action callback;

    public bool EqualsTo(IComparable obj)
    {
        if (obj.GetType() == typeof(FieldObject)) return ((FieldObject)obj).fieldObjectType == fieldObjectType;
        else return false;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Destroy(Action callback)
    {
        this.callback = callback;

        animationComponent.Play();
        Invoke("AfterDestroyAnimationRoutine", animationComponent.GetClip("fieldObjectDestroyAnim").length);
    }

    private void AfterDestroyAnimationRoutine()
    {
        Destroy(gameObject);

        if (callback != null) callback();
    }

    private void Awake()
    {
        animationComponent = GetComponent<Animation>();
    }
}
