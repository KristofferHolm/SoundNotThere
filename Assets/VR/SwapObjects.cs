using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapObjects : MonoBehaviour
{
    public GameObject Hand, Horn;
    public void Swap()
    {
        if(Hand != null)
            Hand.SetActive(!Hand.activeSelf);
        if (Horn != null)
            Horn.SetActive(!Horn.activeSelf);
    }
}
