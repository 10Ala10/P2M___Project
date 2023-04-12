using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nbrFruit : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> fruits;
    public int currentNumber()
    {
        return fruits.Count;
    }
    public int incrementNbr(int i)
    {
        ++i;
        return i;
    }
    public int decrementNbr(int i)
    {
        ++i;
        return i;
    }
}
