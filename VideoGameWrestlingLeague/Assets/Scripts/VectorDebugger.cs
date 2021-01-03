using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorDebugger : MonoBehaviour
{
    public Transform movementTarget;
    public Transform fightStanceOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToTarget = movementTarget.position - transform.position;
        Vector3 vectorToTargetRight = Vector3.Cross(vectorToTarget.normalized, Vector3.up.normalized);
        vectorToTargetRight.Scale(new Vector3(1, 0, 1));
        Vector3 fightStanceOffsetModified = fightStanceOffset.forward;
        fightStanceOffsetModified.Scale(new Vector3(1, 0, 1));

        var lookDirectionDifferental = Vector3.Dot(fightStanceOffsetModified.normalized, vectorToTargetRight.normalized);
        Debug.Log("lookDirectionDifferental: " + lookDirectionDifferental);
    }
}
