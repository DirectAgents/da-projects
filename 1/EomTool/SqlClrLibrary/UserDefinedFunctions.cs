using System;
using System.Data.SqlTypes;
using System.Globalization;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 ToInt(SqlString wrappedInput)
    {
        string input = wrappedInput.Value;
        decimal decimalValue;
        if (decimal.TryParse(input, out decimalValue))
        {
            if (decimal.Truncate(decimalValue) != decimalValue)
            {
                throw new Exception("converting " + input + " results in truncation");
            }
            int intValue = Convert.ToInt32(decimalValue);
            return intValue;
        }
        throw new Exception("parse error converting string to int");
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble ToNum(SqlString wrappedStringInput)
    {
        string stringInput = wrappedStringInput.Value.TrimEnd('%');
        decimal decimalValue;
        CultureInfo culture = CultureInfo.InvariantCulture;
        if (Decimal.TryParse(stringInput, NumberStyles.Currency, culture, out decimalValue))
        {
            return Convert.ToDouble(decimalValue);
        }
        throw new Exception("parse error converting string to decimal");
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString LastWord(SqlString wrappedStringInput)
    {
        string stringInput = wrappedStringInput.Value;
        if (stringInput.Length < 2 || !stringInput.Contains(" "))
        {
            return wrappedStringInput;
        }
        else
        {
            string[] wordsSeparatedBySpaces = stringInput.Split(' ');
            string lastWord = wordsSeparatedBySpaces[wordsSeparatedBySpaces.Length - 1];
            return new SqlString(lastWord);
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString LastPiece(string target, string delim)
    {
        if (target == null || target.Length < 2 || !target.Contains(delim))
        {
            return target;
        }
        else
        {
            string[] pieces = target.Split(new string [] { delim }, StringSplitOptions.RemoveEmptyEntries);
            return pieces[pieces.Length - 1];
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString LastPiece2(string target, string delim, string delim2, string def)
    {
        if (target == null || target.Length < 2 || !(target.Contains(delim) && target.Contains(delim2)) || (target.LastIndexOf(delim) > target.LastIndexOf(delim2)))
        {
            return def;
        }
        else
        {
            string[] pieces = target.Split(new string[] { delim, delim2 }, StringSplitOptions.RemoveEmptyEntries);
            return pieces[pieces.Length - 1];
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString UrlServerPart(string url)
    {
        if (url == null || !url.StartsWith("http:"))
        {
            return url;
        }
        else
        {
            string[] pieces = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return pieces[1];
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString Piece(string target, string delim, int index)
    {
        if (target == null || !target.Contains(delim))
        {
            return target;
        }
        else
        {
            string[] pieces = target.Split(new string[] { delim }, StringSplitOptions.RemoveEmptyEntries);
            return pieces[index];
        }
    }
};
