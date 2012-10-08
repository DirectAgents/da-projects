using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedAggregate(
    Format.UserDefined,
    IsInvariantToDuplicates = true,
    IsInvariantToNulls = true,
    MaxByteSize = -1
    )]
public struct SumKeys : IBinarySerialize
{
    private const char Sep = ',';
    private SqlString _result;

    #region IBinarySerialize Members

    public void Read(BinaryReader r)
    {
        _result = r.ReadString();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(_result.Value.TrimEnd(Sep));
    }

    #endregion

    public void Init()
    {
        _result = string.Empty;
    }

    public void Accumulate(SqlInt32 value)
    {
        if (!value.IsNull && !Contains(value))
            Add(value);
    }

    private void Add(SqlInt32 value)
    {
        _result += Wrap(value);
    }

    private static string Wrap(SqlInt32 value)
    {
        return value.Value.ToString() + Sep;
    }

    private bool Contains(SqlInt32 value)
    {
        return _result.Value.Contains(Wrap(value));
    }

    public void Merge(SumKeys group)
    {
        foreach (SqlInt32 value in Items(group))
            if (!Contains(value))
                Add(value);
    }

    private static IEnumerable<SqlInt32> Items(SumKeys group)
    {
        foreach (string value in group._result.Value.Split(Sep))
        {
            int i;
            if (Int32.TryParse(value, out i))
                yield return i;
        }
    }

    public SqlString Terminate()
    {
        return _result.Value.TrimEnd(Sep);
    }
}