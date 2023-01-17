using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGrenadeCurrent : UIElementText
{


    protected override void Tick()
    {
        float current = characterBehaviour.GetGrenadesCurrent();

        textMesh.text = current.ToString();
    }
}
