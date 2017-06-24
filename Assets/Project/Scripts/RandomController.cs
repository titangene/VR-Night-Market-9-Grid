public class RandomController {
    // 亂數 value
    public System.Random random;

    /// <summary>
    /// 產生新的亂數 value
    /// </summary>
    public void GeneratorRandom() {
        // 使用 DateTime.Now.Ticks 可產生不重複的隨機亂數
        // DateTime.Now.Ticks 是指從 DateTime.MinValue 之後過了多少時間，10000000 為一秒
        random = new System.Random((int) System.DateTime.Now.Ticks);
    }

    /// <summary>
    /// 產生新的 min ~ max 範圍亂數
    /// </summary>
    public int GetRandom(int min, int max) {
        return random.Next(min, max + 1);
    }
}
