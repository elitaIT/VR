using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlankParent : MonoBehaviour
{
    public Rigidbody rb; // ������ �� Rigidbody �������������� �������
    public Collider axeCollider; // ������ �� ��������� ������
    public float detachmentForce = 1f; // ����, � ������� ������ ��������

    private bool isAttached = true; // ����, �����������, ���������� �� ������

    private void OnCollisionEnter(Collision collision)
    {
        // ���������, ��� ����������� � ������� � ������ ��� ����������
        if (isAttached && collision.collider == axeCollider)
        {
            DetachObject();
        }
    }

    private void DetachObject()
    {
        isAttached = false;

        // ���������� ������, ���� ��� ���� ���������
        if (rb != null)
        {
            rb.isKinematic = false;

            // ��������� ��������� ������� � ����������� �� ������
            Vector3 forceDirection = (transform.position - axeCollider.transform.position).normalized;
            rb.AddForce(forceDirection * detachmentForce, ForceMode.Impulse);
        }

        // ��������� ���� ������, ��� ��� ������ ��� ���������
        enabled = false;
    }
}