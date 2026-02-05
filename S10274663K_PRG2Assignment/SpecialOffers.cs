//==========================================================
// Student Number : S10273819
// Student Name : Marcus Mah En Hao
// Partner Name : HNAING HTET KYAW
//==========================================================

public class SpecialOffer
{
    private string offerCode;
    private string offerDesc;
    private double discount;

    public string OfferCode
    {
        get { return offerCode; }
        set { offerCode = value; }
    }
    public string OfferDesc
    {
        get { return offerDesc; }
        set { offerDesc = value; }
    }
    public double Discount
    {
        get { return discount; }
        set { discount = value; }
    }

    public SpecialOffer(string offerCode, string offerDesc, double discount)
    {
        OfferCode = offerCode;
        OfferDesc = offerDesc;
        Discount = discount;
    }

    public override string ToString()
    {
        return $"{OfferCode} - {OfferDesc} ({Discount}% off)";
    }
}