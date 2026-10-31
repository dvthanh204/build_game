using UnityEngine;

public static class RanchRuntimeBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureGameController()
    {
        if (Object.FindObjectOfType<RanchGameController>() != null)
        {
            return;
        }

        GameObject game = new GameObject("Chicken Ranch Game");
        game.AddComponent<RanchGameController>();
    }
}
