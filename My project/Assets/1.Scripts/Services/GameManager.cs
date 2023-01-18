using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        Physics.IgnoreLayerCollision(7, 10);//TPCharacter , Character
        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(6, 10);
        Physics.IgnoreLayerCollision(6, 15);   
        Physics.IgnoreLayerCollision(15, 15);
        Physics.IgnoreLayerCollision(14, 14);
    }
    

}
