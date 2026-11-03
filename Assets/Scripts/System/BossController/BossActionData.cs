using UnityEngine;

[CreateAssetMenu(fileName = "BossActionData", menuName = "Scriptable Objects/BossActionData")]
public class BossActionData : ScriptableObject
{
    [SerializeField] public string attackName = "empty"; 
    [SerializeField] public float minDistance = 0f; 
    [SerializeField] public float maxDistance = float.MaxValue;
    [SerializeField] public float cooldown = 2f;
    [SerializeField] public float windUpDuration = 2f;
    [SerializeField] public float actionDuration = 2f;
    [SerializeField] public float recoverDuration = 2f;
    [SerializeField] public float weigth = 10f;
    [SerializeField] public float preferredRange = 2f;
    [SerializeField] public string windUpName;
    [SerializeField] public string actionName;
    [SerializeField] public GameObject projectile;
}
