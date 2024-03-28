using UnityEngine;
using System.Collections.Generic;

public class ChangeMaterial : MonoBehaviour
{
    public Material whiteMaterial; // Reference to the white material
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>(); // Dictionary to store original materials
    private Dictionary<Terrain, Material> originalTerrainMaterials = new Dictionary<Terrain, Material>(); // Dictionary to store original terrain materials
    private List<TreeInstance[]> originalTreeInstances = new List<TreeInstance[]>(); // List to store original tree instances

    void Start()
    {
        // Store the original materials of all renderers in the scene
        StoreOriginalMaterials();
    }

    void Update()
    {
        // Change materials to white when 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetMaterialsToWhite();
        }

        // Revert materials back to original when 'O' key is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            RevertMaterials();
        }
    }

    void StoreOriginalMaterials()
    {
        // Get all renderers in the scene and store their original materials
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] rendererMaterials = renderer.materials;
            if (!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials.Add(renderer, rendererMaterials); // Store all materials associated with the renderer
            }
        }

        // Get all terrain objects in the scene and store their original materials
        Terrain[] terrains = Terrain.activeTerrains;
        foreach (Terrain terrain in terrains)
        {
            if (!originalTerrainMaterials.ContainsKey(terrain))
            {
                originalTerrainMaterials.Add(terrain, terrain.materialTemplate); // Store the terrain's material
            }

            // Store original tree instances
            originalTreeInstances.Add(terrain.terrainData.treeInstances);
        }
    }

    void SetMaterialsToWhite()
    {
        Terrain.activeTerrain.drawTreesAndFoliage = false;
        // Iterate through each renderer in the dictionary
        foreach (KeyValuePair<Renderer, Material[]> pair in originalMaterials)
        {
            // Create a new array to hold the white material for each sub-material
            Material[] whiteMaterials = new Material[pair.Value.Length];
            for (int i = 0; i < pair.Value.Length; i++)
            {
                whiteMaterials[i] = whiteMaterial;
            }
            pair.Key.materials = whiteMaterials; // Assign the white materials to the renderer
        }

        // Set the terrain materials to white
        foreach (KeyValuePair<Terrain, Material> pair in originalTerrainMaterials)
        {
            pair.Key.materialTemplate = whiteMaterial; // Assign the white material to the terrain
        }

        // Change tree colors to white
        int terrainIndex = 0;
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
            for (int i = 0; i < treeInstances.Length; i++)
            {
                treeInstances[i].color = Color.white; // Set tree color to white
            }
            terrain.terrainData.treeInstances = treeInstances; // Assign the updated tree instances to the terrain
            terrainIndex++;
        }
    }

    void RevertMaterials()
    {
        Terrain.activeTerrain.drawTreesAndFoliage = true;
        // Iterate through each renderer in the dictionary
        foreach (KeyValuePair<Renderer, Material[]> pair in originalMaterials)
        {
            // Revert all materials of the renderer back to the original materials
            pair.Key.materials = pair.Value;
        }

        // Revert the terrain materials back to the original materials
        foreach (KeyValuePair<Terrain, Material> pair in originalTerrainMaterials)
        {
            pair.Key.materialTemplate = pair.Value; // Assign the original material back to the terrain
        }

        // Revert tree colors back to the original colors
        int terrainIndex = 0;
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            terrain.terrainData.treeInstances = originalTreeInstances[terrainIndex]; // Revert tree instances to their original colors
            terrainIndex++;
        }
    }
}
