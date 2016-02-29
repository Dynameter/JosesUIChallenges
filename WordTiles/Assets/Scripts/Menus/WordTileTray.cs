using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordTileTray : MonoBehaviour
{
    private const float SHUFFLE_IN_DURATION = 0.25f;
    private const float RETURN_TILE_SPEED = 2000f;

    private const float TILE_STARTING_Y = -100f;

    [SerializeField]
    private List<WordTile> m_wordTiles;

    private Vector3[] m_wordTilesStartingPos = new Vector3[WordTilesMainMenu.NUMBER_OF_PLAYABLE_TILES];

    public void Start()
    {
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTilesStartingPos[i] = m_wordTiles[i].GetRectTransform().parent.localPosition;
            m_wordTiles[i].gameObject.SetActive(false);
        }
    }

    public void ShuffleInNewTiles(System.Action argOnShuffleComplete)
    {
        StartCoroutine(ShuffleInNewTilesCo(argOnShuffleComplete));
    }

    private IEnumerator ShuffleInNewTilesCo(System.Action argOnShuffleComplete)
    {
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTiles[i].gameObject.SetActive(false);
            m_wordTiles[i].interactable = false;

            m_wordTiles[i].GetRectTransform().parent.SetParent(this.transform);
            m_wordTiles[i].GetRectTransform().parent.localPosition = m_wordTilesStartingPos[i];
        }

        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            //Fetch a tile from the pool
            m_wordTiles[i].gameObject.SetActive(true);

            //Randomly assign it a letter
            m_wordTiles[i].SetLetter((char)UnityEngine.Random.Range(65, 90));

            //Move the tile off-screen
            RectTransform tileTransform = m_wordTiles[i].GetRectTransform();
            tileTransform.localPosition = new Vector3(tileTransform.localPosition.x, TILE_STARTING_Y, tileTransform.localPosition.z);

            //Lerp it up from the bottom to the top
            float currTime = 0f;
            while (currTime < SHUFFLE_IN_DURATION)
            {
                //Increment the time that has passed
                currTime += Time.deltaTime;

                //Move the position of the tile
                float currY = Mathf.Lerp(tileTransform.localPosition.y, 0f, (currTime / SHUFFLE_IN_DURATION));
                tileTransform.localPosition = new Vector3(tileTransform.localPosition.x, currY, tileTransform.localPosition.z);

                yield return null;
            }
        }

        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTiles[i].interactable = true;
        }

        //Call shuffle complete
        if (argOnShuffleComplete != null)
        {
            argOnShuffleComplete();
        }
    }

    public void ReturnTile(WordTile argTileToReturn)
    {
        StartCoroutine(ReturnTileCo(argTileToReturn));
    }

    public IEnumerator ReturnTileCo(WordTile argTileToReturn)
    {
        //Disable interactions while it moves
        argTileToReturn.interactable = false;

        //Set parent to the tray
        RectTransform tileParentTransform = argTileToReturn.GetParent();
        tileParentTransform.SetParent(this.transform);

        Vector3 originalPos = m_wordTilesStartingPos[m_wordTiles.IndexOf(argTileToReturn)];
        while (Vector3.Distance(tileParentTransform.localPosition, originalPos) > 0.05f)
        {
            tileParentTransform.localPosition = Vector3.MoveTowards(tileParentTransform.localPosition, originalPos, (Time.deltaTime * RETURN_TILE_SPEED));
            yield return null;
        }

        tileParentTransform.localPosition = originalPos;
        argTileToReturn.interactable = true;
    }
}
