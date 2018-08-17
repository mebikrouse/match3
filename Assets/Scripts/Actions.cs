using System;
using System.Collections.Generic;

public interface IAction
{
    void Play(Action callback);
}

public class ActionSingle : IAction
{
    private Action<Action> action;
    private Action callback;

    private bool isPlaying;

    public ActionSingle() { }

    public ActionSingle(Action<Action> action)
    {
        this.action = action;
    }

    public void ApplyAction(Action<Action> action)
    {
        if (isPlaying) throw new System.Exception("Action is playing");

        this.action = action;
    }

    public void Play(Action callback)
    {
        if (isPlaying) throw new System.Exception("Action is playing");

        this.callback = callback;

        if (action != null) action(OnActionEnd);
        else OnActionEnd();
    }

    private void OnActionEnd()
    {
        isPlaying = false;

        if (callback != null) callback();
    }
}

public class ActionPack : IAction
{
    private List<Action<Action>> actions;
    private int countOfExecutedActions;
    private Action callback;

    private bool isPlaying;

    public ActionPack() : this(new List<Action<Action>>()) { }

    public ActionPack(List<Action<Action>> actions)
    {
        this.actions = actions;
    }

    public void AddAction(Action<Action> action)
    {
        if (isPlaying) throw new System.Exception("Action pack is playing");

        actions.Add(action);
    }

    public void AddAction(IAction action)
    {
        AddAction((callback) => { action.Play(callback); });
    }

    public void AddActions(List<Action<Action>> actions)
    {
        if (isPlaying) throw new System.Exception("Action pack is playing");

        this.actions.AddRange(actions);
    }

    public void Play(Action callback)
    {
        if (isPlaying) throw new System.Exception("Action pack is playing");

        if (actions.Count == 0) throw new System.Exception("Action pack is empty");

        isPlaying = true;

        this.callback = callback;

        countOfExecutedActions = actions.Count;
        foreach (Action<Action> action in actions)
        {
            if (action != null) action(OnActionEnd);
            else OnActionEnd();
        }

        actions.Clear();
    }

    private void OnActionEnd()
    {
        countOfExecutedActions--;
        if (countOfExecutedActions == 0)
        {
            if (callback != null) callback();
            
            isPlaying = false;
        }
    }
}

public class ActionSequence : IAction
{
    private Queue<Action<Action>> actions;
    private Action callback;

    private bool isPlaying;

    public ActionSequence() : this(new Queue<Action<Action>>()) { }

    public ActionSequence(Queue<Action<Action>> actions)
    {
        this.actions = actions;
    }

    public void AddAction(Action<Action> action)
    {
        if (isPlaying) throw new System.Exception("Action queue is playing");

        actions.Enqueue(action);
    }

    public void AddAction(IAction action)
    {
        AddAction((callback) => { action.Play(callback); });
    }

    public void Play(Action callback)
    {
        if (isPlaying) throw new System.Exception("Action queue is playing");

        if (actions.Count == 0) throw new System.Exception("Action queue is empty");

        isPlaying = true;

        this.callback = callback;
        PlayNextAction();
    }

    private void PlayNextAction()
    {
        Action<Action> action = actions.Dequeue();
        if (action != null) action(OnActionEnd);
        else OnActionEnd();
    }

    private void OnActionEnd()
    {
        if (actions.Count > 0)
        {
            PlayNextAction();
        }
        else
        {
            if (callback != null) callback();

            actions.Clear();
            isPlaying = false;
        }
    }
}