namespace domain.silisync.Enums;

public enum ESyncStatus : byte
{
    /// <summary>
    /// Product created in the local system but not yet sent to Mercado Libre.
    /// </summary>
    NotSynced,
    
    /// <summary>
    /// Product successfully shipped and fully synchronized with Mercado Libre.
    /// </summary>
    Synced,
    
    /// <summary>
    /// The product has undergone local changes (price, stock, etc.) and requires an API update.
    /// </summary>
    OutOfSync,
    
    /// <summary>
    /// Houve uma falha ou rejeição por parte da API do Mercado Libre durante a última tentativa.
    /// </summary>
    Error
}