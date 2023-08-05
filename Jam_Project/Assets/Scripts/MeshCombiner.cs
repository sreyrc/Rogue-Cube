using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;


    public void Combine()
    {
        GrabMeshes();
        CombineMeshes();
    }

    private void GrabMeshes()
    {
        // Grab all platform tiles loaded in this round
        var gameObjects = GameObject.FindGameObjectsWithTag("RoomTile");
        foreach (var gameObject in gameObjects)
        {
            // Get the meshfilter from each and add them to the list
            sourceMeshFilters.Add(gameObject.GetComponent<MeshFilter>());
        }
    }

    private void CombineMeshes()
    {
        var combine = new CombineInstance[sourceMeshFilters.Count];

        // Go through each source mesh filter and add them to the combine array
        for(int i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;    
        }

        // Combine all meshes
        var mesh  = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;
    }

    private void OnDestroy()
    {
        if (targetMeshFilter != null) { targetMeshFilter.mesh = null; }
    }

}
