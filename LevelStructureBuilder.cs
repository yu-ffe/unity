using System.Collections;
using UnityEngine;

public class LevelStructureBuilder : MonoBehaviour {
    public GameObject[] levelStructures;
    public GameObject[] monsters;
    
    void Start() {
        StartCoroutine(BuildLevelStructureCoroutine());
        StartCoroutine(GenLevelMonsterCroutine());
    }

    void Update() {

    }


    private IEnumerator BuildLevelStructureCoroutine() {
        float delay = 2.0f / levelStructures.Length; // 2초 안에 모든 블럭 배치
        foreach (GameObject structure in levelStructures) {
            structure.SetActive(true); // State 켜기
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator GenLevelMonsterCroutine() {
        yield return new WaitForSeconds(2.3f);
        float delay = 1.0f / monsters.Length; // 2초 안에 모든 블럭 배치
        foreach (GameObject monster in monsters) {
            monster.SetActive(true); // State 켜기
            yield return new WaitForSeconds(delay);
        }
    }
}
