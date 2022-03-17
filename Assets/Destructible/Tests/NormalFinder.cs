using UnityEngine;

public class NormalFinder : MonoBehaviour
{
    public Transform GOTest;

    [ContextMenu("Test")]
    void MenuTest()
    {
        Vector3 normal;
        if (Find(GOTest, out normal))
        {
            Debug.Log(normal);
        }
        else
        {
            Debug.Log("Don't Hit");
        }
    }

    public bool Find(Transform GO, out Vector3 normal)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(GO.position), out hit, Mathf.Infinity))
        {
            normal = hit.normal;
            return true;
        }
        normal = Vector3.zero;
        return false;
    }
}
