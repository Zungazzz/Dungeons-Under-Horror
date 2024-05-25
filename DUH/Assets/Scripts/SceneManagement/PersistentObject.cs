using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PersistentObject : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
