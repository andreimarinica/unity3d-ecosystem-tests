using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpTest : MonoBehaviour
{
    GameObject example;

    void Start()
    {

        TernaryConditionalOperator();

    }

    void Update()
    {
        
    }

    void TernaryConditionalOperator()
    {
        int myNumber = Random.Range(0,2);
        string sex;

        // explained:
        // if(myNumber == 1) sex = female; 
        // else sex = male;
        sex = myNumber == 1 ? "female" : "male";

        Debug.Log(myNumber + " " + sex);

        // // check for null type
        // if(example is null) Debug.Log("is null");
        // if(example is not null) Debug.Log("is not null"); #DEV only in C#9.0
    }
}
