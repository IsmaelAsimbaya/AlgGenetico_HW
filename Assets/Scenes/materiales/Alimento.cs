using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alimento : MonoBehaviour
{
    public float calorias;

    void Awake(){
        calorias = Random.Range(500,1500);
    }

}
