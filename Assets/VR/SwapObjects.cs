using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapObjects : MonoBehaviour
{
    public GameObject Hand, Horn;
    public void Swap()
    {
        //Hand.SetActive(!Hand.activeSelf);
        Horn.SetActive(!Horn.activeSelf);
    }
}
