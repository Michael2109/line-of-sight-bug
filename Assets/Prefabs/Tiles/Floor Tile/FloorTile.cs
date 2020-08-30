using UnityEngine;

namespace Prefabs.Tiles.Floor_Tile
{
    public class FloorTile : MonoBehaviour
    {
        public GameObject ceiling;
 
        public void SetCeilingDisplayed(bool ceilingDisplayed)
        {
            ceiling.SetActive(ceilingDisplayed);
        }
    }
}
