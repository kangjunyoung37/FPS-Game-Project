using UnityEngine;
using UnityEngine.EventSystems;

public interface IDamageable
{
    void TakeDamage(int damage, Vector3 pos, Quaternion rot, int team,int viewID, bool bullet,string enemyPlayerName, int index);
}