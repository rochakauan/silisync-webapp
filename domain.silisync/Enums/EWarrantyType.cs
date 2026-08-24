namespace domain.silisync.Enums;

public enum EWarrantyType : byte
{
    /// <summary>
    /// The product does not come with any type of extended or contractual warranty.
    /// </summary>
    None,

    /// <summary>
    /// Over-the-counter warranty, offered and covered directly by the seller.
    /// </summary>
    SellerWarranty,

    /// <summary>
    /// Official manufacturer's warranty, covered by the component brand (e.g., Asus, Corsair).
    /// </summary>
    FactoryWarranty
}