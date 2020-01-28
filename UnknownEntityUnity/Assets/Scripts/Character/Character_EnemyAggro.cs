using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_EnemyAggro : MonoBehaviour
{
    public CircleCollider2D myAggroCol;
    public ContactFilter2D filter;
    public float updateListDelay;
    public List<Enemy_Aggro> enemyAggros = new List<Enemy_Aggro>();

    private void Start() {
        StartCoroutine(ContinuousEnemyCheck());
    }

    IEnumerator ContinuousEnemyCheck() {
        while(true) {
            Debug.Log("Character is checking his aggro detection range for enemies.");
            //foreach (Enemy_Aggro enemyAggro in enemyAggros) {
            //    enemyAggro.DisableAggro();
            //}
            //enemyAggros.Clear();
            List<Collider2D> enemyCols = new List<Collider2D>();
            Physics2D.OverlapCollider(myAggroCol, filter, enemyCols);
            if (enemyCols.Count > 0) {
                int index = 0;
                if (enemyCols.Count > 1) {
                    Debug.Log(enemyCols.Count + " enemies within activation range.");
                } 
                else {
                    Debug.Log(enemyCols.Count + " enemy within activation range.");
                }
                foreach (Collider2D enemyCol in enemyCols) {
                    Enemy_Aggro enemyAggro = enemyCol.GetComponent<Enemy_Aggro>();
                    if (!enemyAggro.checkingAggro) {
                        enemyAggro.EnableAggro(myAggroCol.radius);
                    }
                    //enemyAggros.Add(enemyAggro);
                    index++;
                }
            }
            yield return new WaitForSeconds(updateListDelay);
        }
    }
}