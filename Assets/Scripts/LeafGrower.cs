using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGrower : MonoBehaviour
{
    public GameObject[] leaf_Prefabs;
    public float chanceToGrowLeaf; //value between 0.0f and 1.0f
    // Start is called before the first frame update

    public void growLeaves(Vector3 growPoint, Vector2 headDirection)
    {
        if (shouldGrowLeaf())
        {
            int index = Random.Range(0, leaf_Prefabs.Length-2);
            GameObject newLeaf = Instantiate(leaf_Prefabs[index]);
            
            Vector3 offsetDirection = Vector2.Perpendicular(headDirection).normalized;
            
            float scale = 0.05f;
            Vector3 scaleVector = new Vector3(scale, scale, scale);
            offsetDirection = Vector3.Scale(offsetDirection, scaleVector);
            Vector3 translate = growPoint + offsetDirection;

            Vector2 targetPos = new Vector2(translate.x - transform.position.x, translate.y - transform.position.y);
            newLeaf.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg));
            
            newLeaf.transform.position = translate;

            newLeaf.transform.SetParent(this.transform);
        }

    }

    bool shouldGrowLeaf()
    {
        float number = Random.value;
        return number < chanceToGrowLeaf;
    }
}
