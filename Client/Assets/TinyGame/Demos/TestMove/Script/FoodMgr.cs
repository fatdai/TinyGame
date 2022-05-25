using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FoodMgr : MonoBehaviour
{
    [SerializeField]
    GameObject pf_Foods;

    List<GameObject> m_Foods = new List<GameObject>();

    // 保持场景中只有20个
    void Start()
    {
        for(int i = 0; i < 20; ++i)
        {
            CreateRandomFood();
        }

        StartCoroutine(GenFoods());
    }

    IEnumerator GenFoods() {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if(m_Foods.Count < 20)
            {
                CreateRandomFood();
            }
        }
    }

    void CreateRandomFood() {
        GameObject objFood = Instantiate(pf_Foods, transform);
        float x = Random.Range(-50, 50);
        float z = Random.Range(-50, 50);
        float y = 1.5f;
        objFood.transform.position = new Vector3(x, y, z);
        m_Foods.Add(objFood);
    }

    public void RemoveFood(GameObject objFood) {
        m_Foods.Remove(objFood);
        Destroy(objFood);
    }
}
