using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DictionaryTreeNode
{
    #region PrivateMembers
    /// <summary>
    /// The letter stored by this node
	/// </summary>
	private readonly char m_letter;
	
	/// <summary>
    /// If we got up to this letter, is it a complete word?
	/// </summary>
	private bool m_isCompleteWord = false;
	
	/// <summary>
    /// Child nodes.
	/// </summary>
	private Dictionary<char, DictionaryTreeNode> m_childNodes = null;
    #endregion PrivateMembers

    #region PublicMethods
    /// <summary>
    /// Constructor that takes in the letter.
	/// </summary>
	/// <param name="nodeLetter">Letter to store in the node</param>
	public DictionaryTreeNode(char nodeLetter)
	{
		m_letter = nodeLetter;
	}
	
    /// <summary>
    /// Gets the letter stored in the node.
    /// </summary>
    /// <returns>Returns the letter stores in the node.</returns>
	public char GetLetter()
	{
		return m_letter;
	}
	
    /// <summary>
    /// Marks this node as the last letter in a word.
    /// </summary>
	public void MarkAsCompleteWord()
	{
		m_isCompleteWord = true;
	}
	
    /// <summary>
    /// Checks if this is the last node in a word.
    /// </summary>
    /// <returns>Returns true if this is the last node in a word.</returns>
	public bool IsCompleteWord()
	{
		return m_isCompleteWord;
	}
	
    /// <summary>
    /// Adds a letter as a child to this node.
    /// </summary>
    /// <param name="childLetter">The letter to add.</param>
    /// <returns>Returns the created child node.</returns>
	public DictionaryTreeNode AddChildLetter(char childLetter)
	{
		//We don't create the dictionary until we need it
		if(m_childNodes == null)
		{
			m_childNodes = new Dictionary<char, DictionaryTreeNode>();
		}
		
		//Does this node need to have a new child added?
		if(m_childNodes.ContainsKey(childLetter) == false)
		{
			m_childNodes.Add(childLetter, new DictionaryTreeNode(childLetter));
		}
		
		//Return the pointer to the node so we can continue adding letters to it
		return m_childNodes[childLetter];
	}
	
    /// <summary>
    /// Gets a child node for the given letter.
    /// </summary>
    /// <param name="childLetter">The letter to search for.</param>
    /// <returns>Returns a child node if it exists, or null if it does not.</returns>
	public DictionaryTreeNode GetChildNode(char childLetter)
	{
		if(m_childNodes != null && m_childNodes.ContainsKey(childLetter) == true)
		{
			return m_childNodes[childLetter];
		}
		else
		{
			return null;
		}
    }
    #endregion PublicMethods
}


public class DictionaryTree
{
    #region PrivateMembers
    /// <summary>
    /// The root node
	/// </summary>
	private DictionaryTreeNode m_rootNode = new DictionaryTreeNode('\0');
    #endregion PrivateMembers

    #region PublicMethods
    /// <summary>
    /// Adds a word to the tree.
    /// </summary>
    /// <param name="wordToAdd">The word to add.</param>
	public void AddWord(string wordToAdd)
	{
		//Start at the root node, and keep adding letter nodes
		DictionaryTreeNode currentNode = m_rootNode;
		
		foreach(char letter in wordToAdd)
		{
			currentNode = currentNode.AddChildLetter(letter);
		}
		
		//Mark the final node as being a complete word
		currentNode.MarkAsCompleteWord();
	}

    /// <summary>
    /// Adds a word to the tree
    /// </summary>
    /// <param name="wordToAdd">The word to add.</param>
	public void AddWord(char[] wordToAdd)
	{
		AddWord(new string(wordToAdd));
	}

    /// <summary>
    /// Checks if the given string is a word stored in the tree.
    /// </summary>
    /// <param name="wordToCheck">The word to check for.</param>
    /// <returns>Returns true if the word exists in the tree.</returns>
	public bool DoesWordExist(string wordToCheck)
	{
		//Start at the root node
		DictionaryTreeNode currentNode = m_rootNode;
		
		foreach(char letter in wordToCheck)
		{
			currentNode = currentNode.GetChildNode(letter);
			
			if(currentNode == null)
			{
				return false;
			}
		}
		
		//We made it down all the way to the last letter. But is is a valid word?
		return (currentNode.IsCompleteWord());
	}

    /// <summary>
    /// Checks if the given string is a word stored in the tree.
    /// </summary>
    /// <param name="wordToCheck">The word to check for.</param>
    /// <returns>Returns true if the word exists in the tree.</returns>
	public bool DoesWordExist(char[] wordToCheck)
	{
		return DoesWordExist(new string(wordToCheck));
    }
    #endregion PublicMethods
}
