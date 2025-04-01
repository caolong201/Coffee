using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerModelController : SingletonMonoBehaviour<CustomerModelController>
{
    public Transform GetRandomCustomer()
    {
        return transform.GetChild(Random.Range(0, transform.childCount));
    }
}
