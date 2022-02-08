using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpTest : MonoBehaviour
{

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
    }
}
