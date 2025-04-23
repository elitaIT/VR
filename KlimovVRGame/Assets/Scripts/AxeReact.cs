using UnityEngine;

public class AxeReact : MonoBehaviour
{
    [SerializeField] string _tag;
    [SerializeField] Color _newColor;
    public void ChangeColor()
    {
        foreach(GameObject tagged in GameObject.FindGameObjectsWithTag(_tag))
        {
            if(tagged.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.material.color = _newColor;
            }
        }
    }
    public void ChangeColorBack()
    {
        foreach (GameObject tagged in GameObject.FindGameObjectsWithTag(_tag))
        {
            if (tagged.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.material.color = new Color(0, 0, 0, 1);
            }
        }
    }
}
