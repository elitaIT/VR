using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [Header("Настройки оружия")]
    public int maxAmmo = 30;
    public float fireRate = 0.1f;
    public float reloadAngleThreshold = 45f; // Угол наклона для перезарядки
    public Transform muzzleTransform;

    [Header("Эффекты")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;
    public ParticleSystem muzzleFlash;

    private int currentAmmo;
    private AudioSource audioSource;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isReloading = false;
    private bool isShooting = false;
    private Coroutine shootingCoroutine;
    private Vector3 lastPosition;
    private bool needsReloadMotion = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        currentAmmo = maxAmmo;

        grabInteractable.activated.AddListener(StartShooting);
        grabInteractable.deactivated.AddListener(StopShooting);
    }

    void Update()
    {
        if (needsReloadMotion && !isReloading)
        {
            CheckReloadMotion();
        }
    }

    private void CheckReloadMotion()
    {
        // Определяем текущее направление оружия
        float angle = Vector3.Angle(transform.up, Vector3.up);

        // Если оружие наклонено достаточно сильно
        if (angle > reloadAngleThreshold)
        {
            StartCoroutine(ReloadWeapon());
        }
    }

    private IEnumerator ReloadWeapon()
    {
        isReloading = true;
        needsReloadMotion = false;

        PlaySound(reloadSound);
        yield return new WaitForSeconds(1f); // Время на анимацию перезарядки

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private void StartShooting(ActivateEventArgs args)
    {
        if (isShooting || isReloading) return;
        isShooting = true;
        shootingCoroutine = StartCoroutine(ShootingProcess());
    }

    private void StopShooting(DeactivateEventArgs args)
    {
        if (!isShooting) return;
        isShooting = false;
        if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
    }

    private IEnumerator ShootingProcess()
    {
        while (isShooting)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                yield return new WaitForSeconds(fireRate);
            }
            else
            {
                PlaySound(emptySound);
                needsReloadMotion = true; // Требуется движение для перезарядки
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void Shoot()
    {
        currentAmmo--;

        PlaySound(shootSound);
        if (muzzleFlash != null) muzzleFlash.Play();

        if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out RaycastHit hit, 100f))
        {
            Debug.Log($"Попадание в {hit.collider.name}");

            // Проверяем, есть ли у объекта компонент Damageable
            Damageable damageable = hit.collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10f); // Наносим 10 единиц урона
            }
        }

    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(StartShooting);
            grabInteractable.deactivated.RemoveListener(StopShooting);
        }
    }
}