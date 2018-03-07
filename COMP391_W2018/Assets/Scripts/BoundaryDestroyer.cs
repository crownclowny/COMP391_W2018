using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDestroyer : MonoBehaviour {
    //runs when every an object exist collider zone
    //runs once

    void onTriggerExist2D(Collider2D other)
    {
        Destroy(other.gameObject);

    }
}
