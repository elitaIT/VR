using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlankParent : MonoBehaviour
{
    [SerializeField] string _tag;
    public void Unparent(ActivateEventArgs args)
    {
        foreach (GameObject tagged in GameObject.FindGameObjectsWithTag(_tag))
        {
            tagged.transform.SetParent(null);
            
        }
    }
}
