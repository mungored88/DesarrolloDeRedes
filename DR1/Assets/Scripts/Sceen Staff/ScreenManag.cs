using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManag : MonoBehaviour
{
    Stack<IScreen> _stack;

    public string lastResult;

    static public ScreenManag Instance;


    private void Awake()
    {
        Instance = this;

        _stack = new Stack<IScreen>();
    }

    public void Pop()
    {
        if (_stack.Count <= 1) return;

        lastResult = _stack.Pop().Free();

        if (_stack.Count > 0)
        {
            _stack.Peek().Activate();
        }
    }

    public bool Push(IScreen screen)
    {
        if (_stack.Count > 0)
        {
            _stack.Peek().Deactivate();
        }

        _stack.Push(screen);

        screen.Activate();

        return true;
    }

    public void Push(string resource)
    {
        var go = Instantiate(Resources.Load<GameObject>(resource));

        Push(go.GetComponent<IScreen>());

    }
}
