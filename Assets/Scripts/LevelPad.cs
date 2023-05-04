using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPad : MonoBehaviour
{
    [SerializeField] int levelNum;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(levelNum);
    }
}
