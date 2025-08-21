using UnityEngine;
[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    public GameObject model;
    [Range(1, 10)]public int shootDamage;
    [Range(0.1f, 15f)]public float shootRate;
    public int ammoCurrent;
    [Range(1, 45)]public int ammoMax;
    [Range(1, 50)]public int shootDistance;
    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)]public float shootVolume;
}
