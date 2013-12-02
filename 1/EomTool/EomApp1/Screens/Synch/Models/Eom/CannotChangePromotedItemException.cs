using System;

namespace EomApp1.Screens.Synch.Models.Eom
{
    public class CannotChangePromotedItemException : Exception
    {
        public CannotChangePromotedItemException(Item item)
            : base(string.Format("Cannot update item id={0} because one or more of its statuses are not default ({1}).",
                                  item.id, string.Format("campaign_status_id={0},item_accounting_status_id={1},item_reporting_status_id={2}",
                                  item.campaign_status_id, item.item_accounting_status_id, item.item_reporting_status_id)))
        {
        }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityTypeName, string notFoundName)
            : base(string.Format("{0} \"{1}\" doesn't exist", entityTypeName, notFoundName))
        {
        }
    }
}
