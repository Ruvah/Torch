using UnityEngine;

public class MonoBehaviourSingleton<_INSTANCE_> : MonoBehaviour where _INSTANCE_ : MonoBehaviourSingleton<_INSTANCE_>
{
    // -- PUBLIC

    public static _INSTANCE_ Instance
    {
        get => _Instance;
        private set => _Instance = value;
    }
    
    public static bool HasInstance()
    {
        return _Instance != null;
    }
    
    // -- PRIVATE


    private static _INSTANCE_ _Instance;
    
    // -- UNITY


    protected virtual void Awake()
    {
        Debug.Assert( _Instance == null, "MonobehaviorSingleton:" + typeof( _INSTANCE_ ) + ". An instance already exists", this );
        Instance = this as _INSTANCE_;
    }
}
