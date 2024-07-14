// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public abstract class GlobalSceneManagerClient : MonoBehaviour
{
    // Only accessible by inheritors, to avoid messy access to global state.
    protected GlobalSceneManager manager { get; private set; }

    private void Awake()
    {
        // Note: An alternative to this approach is to turn GameManager into a
        // singleton. However, this might be more prone to abuse as a "global
        // variable", since any script could mutate its state.
        manager = GameObject
            .FindWithTag(GlobalSceneManager.Tag)
            .GetComponent<GlobalSceneManager>();
    }
}