using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character unit = collider.GetComponent<Character>();
        if (unit && unit is Character)
        {
            unit.GetDamage();
        }
    }
}
