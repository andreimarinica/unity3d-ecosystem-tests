using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisualDebug : MonoBehaviour
{
    Stats stats;
    string sex;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.sex == 0) 
        {
            sex = "Male";
        } 
        else if(stats.sex == 1)
        {
            sex = "Female";
        }
    }

    // draw gizmos to show visual range
    private void OnDrawGizmos() {
        if(stats.Species == Species.Fox)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.2f); 
            Gizmos.DrawSphere(transform.position, GetComponent<Movement>().visualRange * 4);
        }

        if(stats.Species == Species.Rabbit)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawSphere(transform.position, GetComponent<Movement>().visualRange * 2);
        }

        if(stats.Species == Species.Food)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); 
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
        else
        {
        GUIStyle paramsGUI = new GUIStyle();
        paramsGUI.fontSize = 15;
        paramsGUI.fontStyle = FontStyle.Bold;
        paramsGUI.normal.textColor = Color.black;

        GUIStyle stateGUI = new GUIStyle();
        stateGUI.fontSize = 17;
        stateGUI.fontStyle = FontStyle.Bold;
        stateGUI.normal.textColor = Color.red;

        GUIStyle maleGUI = new GUIStyle();
        maleGUI.fontSize = 17;
        maleGUI.fontStyle = FontStyle.Bold;
        maleGUI.normal.textColor = Color.blue;

        GUIStyle femaleGUI = new GUIStyle();
        femaleGUI.fontSize = 17;
        femaleGUI.fontStyle = FontStyle.Bold;
        femaleGUI.normal.textColor = Color.magenta;

        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), "health " + (int)stats.health, paramsGUI);

        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), "hunger " + (int)stats.hunger, paramsGUI);

        Handles.Label(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), "reprod " + (int)stats.reproductiveUrge, paramsGUI);

        if(sex == "Male") Handles.Label(new Vector3(transform.position.x, transform.position.y + 2.7f, transform.position.z), "Sex: " + sex, maleGUI);

        if(sex == "Female") Handles.Label(new Vector3(transform.position.x, transform.position.y + 2.7f, transform.position.z), "Sex: " + sex, femaleGUI);
        
        
        Handles.Label(new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z), GetComponent<PlayerStateManager>().currentState.ToString(), stateGUI);  
        }                        
        
    }
}
