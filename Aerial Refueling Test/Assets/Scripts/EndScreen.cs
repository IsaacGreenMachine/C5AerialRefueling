using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public GameObject Instance;
    
    public void SetUp()
    {
        Instance.SetActive(false);
        gameObject.SetActive(true);
    }
}
