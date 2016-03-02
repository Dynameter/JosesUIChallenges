using System.Collections.Generic;

/// <summary>
/// Interface for pools.
/// </summary>
public interface IPool
{
    /// <summary>
    /// Initializes the pool.
    /// </summary>
    void Init();

    /// <summary>
    /// Clears the pool of all items.
    /// </summary>
    void Clear();
}

/// <summary>
/// A resource pool. Recycles unused objects and creates new ones when needed.
/// </summary>
/// <typeparam name="PoolType">The type of the pooled items.</typeparam>
public sealed class Pool<PoolType> : IPool where PoolType : class
{
    #region StaticMembers
    /// <summary>
    /// Default number of starting pool items created on init.
    /// </summary>
    public const int DEFAULT_POOL_SIZE = 32;
    #endregion StaticMembers

    #region PrivateMembers
    /// <summary>
    /// Stack used to contain the pooled items
    /// </summary>
    private readonly Stack<PoolType> _stack;

    /// <summary>
    /// Method used to create new pool objects.
    /// </summary>
    private readonly System.Func<PoolType> _factoryCreate;

    /// <summary>
    /// Method used to destroy a pool object.
    /// </summary>
    private readonly System.Action<PoolType> _factoryDestroy;

    /// <summary>
    /// Method called before the item is fetched.
    /// </summary>
    private readonly System.Action<PoolType> _factoryFetch;

    /// <summary>
    /// Method called before the item is realeased.
    /// </summary>
    private readonly System.Action<PoolType> _factoryRelease;

    /// <summary>
    /// The initial number of items the pool to create on init.
    /// </summary>
    private readonly int _initialPoolSize;
    #endregion PrivateMembers

    #region PublicMethods
    /// <summary>
    /// Constructor that takes in the pool factory create method to create items and sets the starting items to the number specified.
    /// </summary>
    /// <param name="argCreate">Method used to create new pool objects.</param>
    /// <param name="argInitialPoolSize">The initial number of items the pool to create on init.</param>
    public Pool(System.Func<PoolType> argCreate, int argInitialPoolSize) : this(argCreate, null, null, null, argInitialPoolSize)
    {
    }

    /// <summary>
    /// Constructor that takes in the pool factory methods to create items and sets the starting items to the number specified.
    /// </summary>
    /// <param name="argCreate">Method used to create new pool objects.</param>
    /// <param name="argDestroy">Method used to destroy a pool object.</param>
    /// <param name="argFetch">Method called before the item is fetched.</param>
    /// <param name="argRelease">Method called before the item is realeased.</param>
    /// <param name="argInitialPoolSize">The initial number of items the pool to create on init.</param>
    public Pool(System.Func<PoolType> argCreate, System.Action<PoolType> argDestroy, System.Action<PoolType> argFetch, System.Action<PoolType> argRelease, int argInitialPoolSize)
    {
        //Save callbacks
        this._factoryCreate = argCreate;
        this._factoryDestroy = argDestroy;
        this._factoryFetch = argFetch;
        this._factoryRelease = argRelease;

        //Init pool
        this._initialPoolSize = argInitialPoolSize;
        this._stack = new Stack<PoolType>(this._initialPoolSize);
    }

    /// <summary>
    /// Initializes the pool
    /// </summary>
    public void Init()
    {
        this.Clear();

        for (int i = 0; i < this._initialPoolSize; i++)
        {
            this._stack.Push(this._factoryCreate());
        }
    }

    /// <summary>
    /// Clears the pool of all items.
    /// </summary>
    public void Clear()
    {
        while (this._stack.Count < 0)
        {
            PoolType poppedItem = this._stack.Pop();
            if (this._factoryDestroy != null)
            {
                this._factoryDestroy(poppedItem);
            }
        }
    }

    /// <summary>
    /// Fetches a pooled item. Creates one if there is no pooled objects available.
    /// </summary>
    /// <returns>A pooled object</returns>
    public PoolType Fetch()
    {
        PoolType item = null;
        if (this._stack.Count > 0)
        {
            //Make sure the resource did not get destroyed somehow first.
            while (this._stack.Peek() == null)
            {
                this._stack.Pop();
            }

            //If stack is not empty, get the top most item
            if (this._stack.Count > 0)
            {
                item = this._stack.Pop();
            }
        }

        //If stack had no item, create one
        if (item == null)
        {
            item = this._factoryCreate();
        }

        //Call the fetch method to prepare it.
        if (this._factoryFetch != null)
        {
            this._factoryFetch(item);
        }

        return item;
    }

    /// <summary>
    /// Return a pooled object to the pool.
    /// </summary>
    /// <param name="argPoolObject">The object to return to the pool.</param>
    public void Release(PoolType argPoolObject)
    {
        if (argPoolObject != null)
        {
            //Call the release method to prepare it to be returned.
            if (this._factoryRelease != null)
            {
                this._factoryRelease(argPoolObject);
            }

            this._stack.Push(argPoolObject);
        }
    }
    #endregion PublicMethods
}