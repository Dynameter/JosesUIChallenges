using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordTileTray : MonoBehaviour
{
    /// <summary>
    /// Duration in seconds that each tile should take to "shuffle" in.
    /// </summary>
    private const float SHUFFLE_IN_DURATION = 0.25f;

    /// <summary>
    /// Starting Y position for the tiles
    /// </summary>
    private const float TILE_STARTING_Y = -100f;

    /// <summary>
    /// The speed at which the tiles should move when returning to their tray.
    /// </summary>
    private const float RETURN_TILE_SPEED = 2000f;

    /// <summary>
    /// Sound played when a tile is shuffled in.
    /// </summary>
    private const string PATH_TO_SHUFFLE_SOUND = "Sounds/whoosh";

    /// <summary>
    /// The playable tiles.
    /// </summary>
    [SerializeField]
    private List<WordTile> m_wordTiles;

    /// <summary>
    /// The starting positions for each of the tiles.
    /// </summary>
    private Vector3[] m_wordTilesStartingPos = new Vector3[WordTilesGameManager.NUMBER_OF_PLAYABLE_TILES];

    /// <summary>
    /// If the tray is being shuffled
    /// </summary>
    private bool m_isShuffling = false;

    /// <summary>
    /// Sound played when a tile is shuffled in.
    /// </summary>
    private static AudioClip _shuffleSound;

    /// <summary>
    /// Sound played when a tile is shuffled in.
    /// </summary>
    public static AudioClip ShuffleSound
    {
        get
        {
            if (_shuffleSound == null)
            {
                _shuffleSound = Resources.Load<AudioClip>(PATH_TO_SHUFFLE_SOUND);
            }

            return _shuffleSound;
        }
    }

    /// <summary>
    /// Saves the starting positions of the tiles and hides them
    /// </summary>
    public void Start()
    {
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTilesStartingPos[i] = m_wordTiles[i].GetParent().localPosition;
        }

        Reset();
    }

    /// <summary>
    /// Resets the position of the tiles and hides them.
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTiles[i].gameObject.SetActive(false);
            m_wordTiles[i].interactable = false;

            m_wordTiles[i].SetGrandParent(this.transform);
            m_wordTiles[i].GetParent().localPosition = m_wordTilesStartingPos[i];

            m_wordTiles[i].SetOnDetachedCallback(null);
        }
    }

    /// <summary>
    /// Checks if the tray tiles are being shuffled.
    /// </summary>
    /// <returns>Returns true if the tiles are being shuffled.</returns>
    public bool IsShuffling()
    {
        return m_isShuffling;
    }

    /// <summary>
    /// Shuffles in the new tiles and assigns a new letter to them.
    /// </summary>
    /// <param name="argOnShuffleComplete">Callback to call when the shuffle has finished animating.</param>
    public void ShuffleInNewTiles(System.Action argOnShuffleComplete)
    {
        if (m_isShuffling == false)
        {
            m_isShuffling = true;
            StartCoroutine(ShuffleInNewTilesCo(argOnShuffleComplete));
        }
    }

    /// <summary>
    /// Shuffles in the new tiles and assigns a new letter to them.
    /// </summary>
    /// <param name="argOnShuffleComplete">Callback to call when the shuffle has finished animating.</param>
    /// <returns>Returns the enumerator for the coroutine</returns>
    private IEnumerator ShuffleInNewTilesCo(System.Action argOnShuffleComplete)
    {
        //Move tiles back into place
        Reset();

        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            //Fetch a tile from the pool
            m_wordTiles[i].gameObject.SetActive(true);

            //Randomly assign it a letter
            m_wordTiles[i].SetLetter((char)UnityEngine.Random.Range(65, 90));

            //Move the tile off-screen
            RectTransform tileTransform = m_wordTiles[i].GetRectTransform();
            tileTransform.localPosition = new Vector3(tileTransform.localPosition.x, TILE_STARTING_Y, tileTransform.localPosition.z);

            //Play the shuffle sound
            AudioManager.Instance.PlaySound(ShuffleSound);

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

        //Re-enable the tiles
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTiles[i].interactable = true;
        }

        //Mark as complete
        m_isShuffling = false;

        //Call shuffle complete
        if (argOnShuffleComplete != null)
        {
            argOnShuffleComplete();
        }
    }

    /// <summary>
    /// Returns a tile from it's current position to the tray.
    /// </summary>
    /// <param name="argTileToReturn">The tile to return to the tray.</param>
    public void ReturnTile(WordTile argTileToReturn)
    {
        StartCoroutine(ReturnTileCo(argTileToReturn));
    }

    /// <summary>
    /// Returns a tile from it's current position to the tray.
    /// </summary>
    /// <param name="argTileToReturn">The tile to return to the tray.</param>
    /// <returns>Returns the enumerator use to run the coroutine.</returns>
    private IEnumerator ReturnTileCo(WordTile argTileToReturn)
    {
        //Disable interactions while it moves
        argTileToReturn.interactable = false;
        argTileToReturn.SetOnDetachedCallback(null);

        //Set parent to the tray
        argTileToReturn.SetGrandParent(this.transform);

        RectTransform tileParentTransform = argTileToReturn.GetParent();
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
