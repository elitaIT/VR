using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] Button _button;
    public void OnClick()
    {
        SceneManager.LoadSceneAsync("loca 1");
    }
}
