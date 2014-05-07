using UnityEngine;
using System.Collections;

public class KemetTerrain : Monosingleton<KemetTerrain>
{

  [SerializeField]
  private TerrainCollider
    _cachedTerrainColliderReference;

  public TerrainCollider terrainCollider
  {
    get
    {
      if (_cachedTerrainColliderReference == null)
      {
        _cachedTerrainColliderReference = GetComponent<TerrainCollider>();
      }
      return _cachedTerrainColliderReference;
    }
  }

  private void OnMouseDown()
  {
    /*RaycastHit rayHit = new RaycastHit();
    Ray ray = new Ray();
      
    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (terrainCollider.Raycast(ray, out rayHit, 100f))
    {
      Player.MyPlayer.MovementTargetPosition = rayHit.point;
    }*/
  }
  
}
