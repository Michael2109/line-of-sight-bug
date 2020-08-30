using Prefabs.Person;
using Prefabs.Person.Soldier;
using Prefabs.Tiles.Floor_Tile;
using UnityEngine;
using Utilities;

public class Controller : MonoBehaviour
{
    void Update()
    {
        CheckVisibleTiles();

        // Left click to select person
        if (Input.GetMouseButtonDown(0))
        {
            GameObject[] soldierObjects = GameObject.FindGameObjectsWithTag(TagManager.Soldier);
            foreach (var soldierObject in soldierObjects)
            {
                Soldier soldier = soldierObject.GetComponent<Soldier>();
                soldier.SetSelected(false);
            }

            RaycastHit hit;
            if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Person person = hit.transform.gameObject.GetComponentInParent<Person>();

                if (person != null)
                {
                    person.SetSelected(true);
                }
            }
            else
            {
                Debug.Log("Not Selected");
            }
        }

        // Right click to move person
        if (Input.GetMouseButtonDown(1))
        {
            GameObject[] soldierObjects = GameObject.FindGameObjectsWithTag(TagManager.Soldier);

            foreach (var soldierObject in soldierObjects)
            {
                Soldier person = soldierObject.GetComponent<Soldier>();

                if (person.IsSelected())
                {
                    RaycastHit[] hits =
                        Physics.RaycastAll(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), 100);

                    foreach (var raycastHit in hits)
                    {
                        if (raycastHit.transform.parent.gameObject.CompareTag(TagManager.Tile))
                        {
                            person.SetDestination(raycastHit.transform.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }

    private void CheckVisibleTiles()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag(TagManager.Tile);
        foreach (var tileObject in tileObjects)
        {
            FloorTile tile = tileObject.GetComponent<FloorTile>();
            tile.SetCeilingDisplayed(true);
        }


        GameObject[] soldierObjects = GameObject.FindGameObjectsWithTag(TagManager.Soldier);


        foreach (var tileObject in tileObjects)
        {
            foreach (var personObject in soldierObjects)
            {
                FloorTile tile = tileObject.GetComponent<FloorTile>();
                RaycastHit hit;

                Vector3 startPosition =
                    new Vector3(personObject.transform.position.x, personObject.transform.position.y + 0.5f,
                        personObject.transform.position.z);

                Vector3 direction = (tileObject.transform.position - startPosition);
                direction /= direction.magnitude;

                if (Physics.Raycast(startPosition, direction, out hit))
                {
                    if (hit.transform.gameObject.GetComponentInParent<FloorTile>() == tile)
                    {
                        tile.SetCeilingDisplayed(false);
                        break;
                    }
                }
            }
        }
    }
}