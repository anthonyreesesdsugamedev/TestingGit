using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpike : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}

