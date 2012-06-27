using System.Collections.Generic;
using System.Linq;
namespace EomApp1.Formss.Affiliate.Data
{
    public class DataItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class DataItemList
    {
        public DataItemList(System.Data.Linq.Table<MediaBuyer> table)
        {
            _table = table;
            _table.ToList().ForEach(c => TheList.Add(new DataItem { Id = c.id, Name = c.name }));
        }

        public DataItemList(System.Data.Linq.Table<Currency> table)
        {
            _table1 = table;
            _table1.ToList().ForEach(c => TheList.Add(new DataItem { Id = c.id, Name = c.name }));
        }
        public List<DataItem> TheList = new List<DataItem>();
        private System.Data.Linq.Table<MediaBuyer> _table;
        private System.Data.Linq.Table<Currency> _table1;
    }
}
