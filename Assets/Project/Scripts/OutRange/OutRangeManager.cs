using UnityEngine;

public class OutRangeManager : Singleton<OutRangeManager> {
    public string outRangeTag = "OutRange";
    private GameObject[] outRangeObjs;

    void Start() {
        Initial_OutRange();
    }

    public void Initial_OutRange() {
        outRangeObjs = GameObject.FindGameObjectsWithTag(outRangeTag);
        foreach (GameObject obj in outRangeObjs)
            obj.AddComponent<OutRangeCollisionEvents>();
    }

    /// <summary>
    /// 球是否超過範圍 (檢查 Tag)
    /// </summary>
    public bool IsOutRange_CheckTag(Collision collision) {
        return collision.gameObject.tag == outRangeTag;
    }
}
