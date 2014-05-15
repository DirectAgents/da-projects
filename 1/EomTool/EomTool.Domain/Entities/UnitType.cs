
namespace EomTool.Domain.Entities
{
    public partial class UnitType
    {
        public static string ToItemCode(string unitTypeName)
        {
            if (unitTypeName == null)
                return null;
            var unitTypeNameLowered = unitTypeName.ToLower();

            if (unitTypeNameLowered.Contains("affiliate program"))
                return "Affiliate Management";
            switch (unitTypeNameLowered)
            {
                case "call tracking":
                    return "Search";
                case "gsp":
                    return "GSP";
                case "opm":
                    return "Affiiate Program Management";
                case "ppc":
                    return "Search Marketing";
                case "revshare":
                    return "CPA Fee";
                case "seo":
                    return "Search";
                case "trading desk":
                    return "Trading Desk";
                default:
                    if (unitTypeNameLowered.Contains(" fee"))
                        return unitTypeName;
                    else
                        return unitTypeName + " Fee";
            }
        }
    }
}
