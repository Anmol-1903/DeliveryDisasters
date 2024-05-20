using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class ReflectionProbeManager : MonoBehaviour
{
    public ReflectionProbe reflectionProbe;
    private ReflectionProbeMode originalMode;

    void Start()
    {
        // Check if the reflectionProbe is assigned
        if (reflectionProbe == null)
        {
            Debug.LogError("ReflectionProbe is not assigned in the inspector.");
            return;
        }

        // Store the original refresh mode
        originalMode = reflectionProbe.mode;

        // Set the reflection probe's refresh mode to Realtime
        reflectionProbe.mode = ReflectionProbeMode.Realtime;

        // Start the coroutine to refresh the probe every 5 seconds
        StartCoroutine(RefreshProbePeriodically(10f));
    }

    IEnumerator RefreshProbePeriodically(float interval)
    {
        while (true)
        {
            // Refresh the reflection probe
            reflectionProbe.RenderProbe();

            // Wait for the specified interval
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDestroy()
    {
        // Restore the original refresh mode when the object is destroyed
        reflectionProbe.mode = originalMode;
    }
}
