using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public List<Enemy_Refs> Enemies = new List<Enemy_Refs>();
    public int enemiesAlive;
    public int totalEnemies;
    public bool noEnemiesLeft;

    public int AddEnemyToList(Enemy_Refs enemyRef) {
        Enemies.Add(enemyRef);
        totalEnemies++;
        enemiesAlive++;
        return Enemies.Count-1;
    }

    public void EnemyHasDied() {
        enemiesAlive--;
        AnyEnemiesLeft();
    }

    public void AnyEnemiesLeft() {
        if (enemiesAlive <= 0) {
            noEnemiesLeft = true;
        }
    }
}
