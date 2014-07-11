using System.ComponentModel.DataAnnotations.Schema;

namespace ClientPortal.Data.Entities.TD.DBM
{
    public class InsertionOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InsertionOrderID { get; set; }
        public string InsertionOrderName { get; set; }
    }
}
