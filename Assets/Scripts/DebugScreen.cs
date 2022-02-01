using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugScreen : MonoBehaviour
{
    Animal animal;
    // Start is called before the first frame update
    void Start()
    {
        animal = FindObjectOfType<Animal>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = animal.animalHunger + " <----  ---->" + animal.animalHealth;
    }
}
