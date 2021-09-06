using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSignManager : MonoBehaviour
{
    static public FlyingSignManager Instance;
    [SerializeField] GameObject FlyingSignPrefab;

    Dictionary<string, FlyingSign> IdToSigns;

    public Action<string> CloseSign;

    Stack<GameObject> flyingSignsPool;


    //testing flying signs
    //var horn = FindObjectOfType<findmehorn>().transform;
    
    //FlyingSignManager.Instance.GetSign(horn, horn, Vector3.up*.5f, "Pick me up", "horn");
    
    //FlyingSignManager.Instance.CloseSign?.Invoke("horn");


    private void Awake()
    {
        Instance = this;
        flyingSignsPool = new Stack<GameObject>();
        IdToSigns = new Dictionary<string, FlyingSign>();
        CloseSign += CloseSignWithID;
        //var dummyObjs = new GameObject[1];
        //dummyObjs[0] = gameObject;
        //var dummyTrans = new Transform[1];
        //dummyTrans[0] = dummyObjs[0].transform;
        //for (int i = 0; i < 10; i++)
        //{
        //    CreateSign(dummyTrans, dummyObjs[0].transform, Vector3.zero, "dummy", "dummy" + i);
        //}
        //for (int i = 0; i < 10; i++)
        //{
        //    CloseSignWithID("dummy" + i);
        //}

    }

    private void CloseSignWithID(string id)
    {
        if (IdToSigns.TryGetValue(id, out var sign))
        {
            sign.Close(()=> PoolSign(sign.gameObject));
            IdToSigns.Remove(id);
        }
    }

    public void CreateSign(Transform point, Transform parent, Vector3 offset, string text, string id,float scale = 1f)
    {
        var trans = new Transform[1];
        trans[0] = point;
        CreateSign(trans, parent, offset, text, id, scale);
    }


    public void CreateSign(Transform[] point, Transform parent, Vector3 offset, string text, string id, float scale = 1f)
    {
        GameObject obj;
        if (flyingSignsPool.Count == 0)
            obj = Instantiate(FlyingSignPrefab);
        else
            obj = flyingSignsPool.Pop().gameObject;
        obj.SetActive(true);
        FlyingSign signComp = obj.GetComponent<FlyingSign>();
        signComp.Activate(point, parent, offset, text,scale);
        AddEventToDict(id, signComp);
    }


    void AddEventToDict(string id, FlyingSign sign)
    {
        if (IdToSigns.ContainsKey(id))
        {
            Debug.Log("Same ID, instant removing the sign");
            PoolSign(sign.gameObject);
            return;
        }
        IdToSigns.Add(id, sign);
    }
    void PoolSign(GameObject signTrans)
    {
        signTrans.transform.parent = transform;
        signTrans.SetActive(false);
        flyingSignsPool.Push(signTrans);
    }

}
