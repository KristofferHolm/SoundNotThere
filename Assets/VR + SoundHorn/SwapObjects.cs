using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapObjects : MonoBehaviour
{
    public GameObject Hand, Horn, HornOnTable;
    public void Update()
    {
        if (Vector3.Distance(transform.position, HornOnTable.transform.position) > 0.2f) return;
        Swap();
        Debug.Log("GAME START");
        SoundManager.Instance.StartGame();
        HornOnTable.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Swap()
    {
        if(Hand != null)
            Hand.SetActive(!Hand.activeSelf);
        if (Horn != null)
            Horn.SetActive(!Horn.activeSelf);
    }
}
