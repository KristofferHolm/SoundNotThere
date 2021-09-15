using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    Dictionary<string,Stack<GameObject>> poolTable;


    private void Start()
    {
        poolTable = new Dictionary<string, Stack<GameObject>>();
    }


    public void PoolObj(GameObject obj, string key)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        if (poolTable.TryGetValue(key, out var stack))
        {
            stack.Push(obj);
            poolTable[key] = stack;
        }
        else
        {
            Stack<GameObject> newStack = new Stack<GameObject>();
            newStack.Push(obj);
            poolTable.Add(key, newStack);
        }

    }
    /// <summary>
    /// If there is no obj to get, it will return false and with a null GO
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool GetObj(string key, out GameObject obj)
    {
        obj = null;
        if (!poolTable.TryGetValue(key, out var stack))
            return false;
        if (stack.Count > 0)
        {
            obj = stack.Pop();
            //maybe not activate and all that.
            obj.transform.parent = null;
            obj.SetActive(true);
            return true;
        }
        return false;

    }
}
