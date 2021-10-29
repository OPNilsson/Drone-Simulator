using UnityEngine;

public class GridManager : MonoBehaviour
{
	public Sprite sprite;
	public int Width, Height;
	public float CellSize;
	public Vector3 OriginPos = new Vector3(-20, -10);

	// Start is called before the first frame update
	void Start()
    {
		Grid grid = new Grid(Screen.width/2, Screen.height/2, CellSize, OriginPos);

		for (var i = 0; i < grid.GetGrid.GetLength(0); i++)
		{
			for(var j = 0; j < grid.GetGrid.GetLength(1); j++)
			{
				MakeTile(i, j, 0);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void MakeTile(int x, int y, int value)
	{
		GameObject g = new GameObject();
		g.transform.position = new Vector3(x, y);
		var spriteRenderer = g.AddComponent<SpriteRenderer>();
		spriteRenderer.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		spriteRenderer.sprite = sprite;
	}
}
