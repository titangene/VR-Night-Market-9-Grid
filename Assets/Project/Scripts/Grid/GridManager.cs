using UnityEngine;

public class GridManager : Singleton<GridManager> {
    /// <summary>
    /// 宮格的資訊：宮格編號(id)、宮格物件(obj)、是否被丟中(isHit)、是否已翻轉(isTurn)
    /// </summary>
    private Grid[] grid;
    /// <summary>
    /// 所有宮格物件
    /// </summary>
    private GameObject[] gridObjs;

    public string gridTag = "Grid";
    /// <summary>
    /// 宮格總數
    /// </summary>
    public int grid_Sum = 9;
    /// <summary>
    /// 宮格被球丟到後，翻轉的速度
    /// </summary>
    public float gridRotationSpeed = 1.7f;
    /// <summary>
    /// 該宮格翻轉的角度 x : 90
    /// </summary>
    public Quaternion gridTurnRotation = Quaternion.Euler(90, 0, 0);

    void Awake() {
        Reload();
    }

    void Start() {
        Initial_Grid();
    }

    /// <summary>
    /// 初始化設定所有宮格資料
    /// </summary>
    public void Initial_Grid() {
        ResetAllGrid();
    }

    /// <summary>
    /// 取得 宮格編號
    /// </summary>
    public int Get_GirdID(GameObject GridGameObj) {
        return System.Int32.Parse(GridGameObj.name.Replace("Grid", "")) - 1;
    }

    /// <summary>
    /// 取得 某宮格是否被丟中
    /// </summary>
    public bool Get_GridIsHit(int grid_ID) {
        return grid[grid_ID].isHit;
    }

    /// <summary>
    /// 取得 某宮格是否已翻轉
    /// </summary>
    public bool Get_GridIsTurn(int grid_ID) {
        return grid[grid_ID].isTurn;
    }

    /// <summary>
    /// 設定 某宮格是否被丟中
    /// </summary>
    public void Set_GridIsHit(int grid_ID, bool isHit) {
        grid[grid_ID].isHit = isHit;
    }

    /// <summary>
    /// 設定 某宮格是否已翻轉
    /// </summary>
    public void Set_GridIsTurn(int grid_ID, bool isTurn) {
        grid[grid_ID].isTurn = isTurn;
    }

    /// <summary>
    /// 球是否打中某宮格 (檢查 Tag)
    /// </summary>
    public bool IsGrid_CheckTag(Collision collision) {
        return collision.gameObject.tag == gridTag;
    }

    /// <summary>
    /// 重設 所有宮格的資訊：GameObject、id、未被丟中 (isHit)、未翻轉 (isTurn)
    /// </summary>
    public void ResetAllGrid() {
        // 找出所有有 "Grid" Tag 的物件，並排序
        gridObjs = GameObject.FindGameObjectsWithTag("Grid");
        System.Array.Sort(gridObjs, (a, b) => a.name.CompareTo(b.name));

        grid = new Grid[grid_Sum];
        for (int i = 0; i < grid_Sum; ++i) {
            grid[i] = new Grid();
            grid[i].id = i + 1;
            grid[i].obj = gridObjs[i];
            grid[i].obj.AddComponent<GridCollisionEvents>();
        }
    }

    /// <summary>
    /// 重設 所有宮格的角度：x, y, z = (0, 0, 0)
    /// </summary>
    public void ResetAllGridObjRotation() {
        foreach (Grid g in grid)
            g.obj.transform.rotation = Quaternion.identity;
    }
}
