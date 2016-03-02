using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordTileTray : MonoBehaviour
{
    #region StaticMembers
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
    #endregion StaticMembers

    #region PrivateMembers
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
    /// Used to randomly select a letter
    /// </summary>
    private System.Random m_rand = new System.Random((int)System.DateTime.UtcNow.Ticks);

    /// <summary>
    /// Weighted letter container.
    /// </summary>
    private List<char> m_availableLetters = new List<char>();
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// Saves the starting positions of the tiles and hides them
    /// </summary>
    private void Start()
    {
        //Store starting position of the tiles
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            m_wordTilesStartingPos[i] = m_wordTiles[i].GetParent().localPosition;
        }

        //Populate letters
        PopulateCharacters();

        //Hide tiles
        Reset();
    }

    /// <summary>
    /// Adds the weighted letters to the letter pool.
    /// </summary>
    private void PopulateCharacters()
    {
        AddCharacter('E', 12);
        AddCharacter('A', 9);
        AddCharacter('I', 9);
        AddCharacter('O', 8);
        AddCharacter('N', 6);
        AddCharacter('R', 6);
        AddCharacter('T', 6);
        AddCharacter('L', 4);
        AddCharacter('S', 4);
        AddCharacter('U', 4);
        AddCharacter('D', 4);
        AddCharacter('G', 3);
        AddCharacter('B', 2);
        AddCharacter('C', 2);
        AddCharacter('M', 2);
        AddCharacter('P', 2);
        AddCharacter('F', 2);
        AddCharacter('H', 2);
        AddCharacter('V', 2);
        AddCharacter('W', 2);
        AddCharacter('Y', 2);
        AddCharacter('K', 1);
        AddCharacter('J', 1);
        AddCharacter('X', 1);
        AddCharacter('Q', 1);
        AddCharacter('Z', 1);

        //Shuffle the contents
        int count = m_availableLetters.Count;
        for (int a = 0; a < count; a++)
        {
            int b = Random.Range(0, count);
            char tmp = m_availableLetters[a];
            m_availableLetters[a] = m_availableLetters[b];
            m_availableLetters[b] = tmp;
        }
    }

    /// <summary>
    /// Adds a character to the list with a weight.
    /// </summary>
    /// <param name="argCharacterToAdd">The character to add.</param>
    /// <param name="argWeight">How frequent the character should show</param>
    private void AddCharacter(char argCharacterToAdd, int argWeight)
    {
        for (int i = 0; i < argWeight; ++i)
        {
            m_availableLetters.Add(argCharacterToAdd);
        }
    }

    /// <summary>
    /// Returns a random letter.
    /// </summary>
    /// <returns>Returns a randomly selected letter.</returns>
    private char GetRandomCharacter()
    {
        return m_availableLetters[m_rand.Next(m_availableLetters.Count)];
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
            m_wordTiles[i].SetLetter(GetRandomCharacter());

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
    /// <returns>Returns the enumerator use to run the coroutine.</returns>
    private IEnumerator ReturnTileCo(WordTile argTileToReturn)
    {
        //Disable interactions while it moves
        argTileToReturn.interactable = false;
        argTileToReturn.SetOnDetachedCallback(null);

        //Set parent to the tray
        int tileIndex = m_wordTiles.IndexOf(argTileToReturn);
        argTileToReturn.SetGrandParent(this.transform, tileIndex);

        RectTransform tileParentTransform = argTileToReturn.GetParent();
        Vector3 originalPos = m_wordTilesStartingPos[tileIndex];
        while (Vector3.Distance(tileParentTransform.localPosition, originalPos) > 0.05f)
        {
            tileParentTransform.localPosition = Vector3.MoveTowards(tileParentTransform.localPosition, originalPos, (Time.deltaTime * RETURN_TILE_SPEED));
            yield return null;
        }

        tileParentTransform.localPosition = originalPos;
        argTileToReturn.interactable = true;
    }
    #endregion PrivateMethods

    #region PublicMethods
    /// <summary>
    /// Resets the position of the tiles and hides them.
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < m_wordTiles.Count; ++i)
        {
            //Set back to the initial values
            m_wordTiles[i].gameObject.SetActive(false);
            m_wordTiles[i].interactable = false;

            m_wordTiles[i].SetGrandParent(this.transform, i);
            m_wordTiles[i].GetParent().localPosition = m_wordTilesStartingPos[i];

            m_wordTiles[i].SetOnDetachedCallback(null);

            //Stop any movement coroutines
            m_wordTiles[i].StopAllCoroutines();
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
    /// Returns a tile from it's current position to the tray.
    /// </summary>
    /// <param name="argTileToReturn">The tile to return to the tray.</param>
    public void ReturnTile(WordTile argTileToReturn)
    {
        argTileToReturn.StartCoroutine(ReturnTileCo(argTileToReturn));
    }
    #endregion PublicMethods
}
