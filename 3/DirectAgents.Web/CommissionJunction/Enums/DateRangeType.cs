namespace CommissionJunction.Enums
{
    /// <summary>
    /// Enum to determine the type of date filters used for API requests.
    /// </summary>
    public enum DateRangeType
    {
        Event,   //sinceEventDate / beforeEventDate
        Posting, //sincePostingDate / beforePostingDate
        Locking  //sinceLockingDate / beforeLockingDate
    }
}
