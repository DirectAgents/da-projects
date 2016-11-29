using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Entities.RevTrack
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ClientProg Prog { get; set; }
    }
    public class ClientProg //"ProgInfo" for a client
    {
        [Key, ForeignKey("Client")]
        public int ClientId { get; set; }
        [MaxLength(50)] //, Index("CodeIndex", IsUnique = true)]
        public string Code { get; set; }

        public BudgetInfoVals DefaultBudgetInfo { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<ProgBudgetInfo> BudgetInfos { get; set; }
        public virtual ICollection<ProgVendorBudgetInfo> VendorBudgetInfos { get; set; }
    }

    // --- BudgetInfos ---

    public class ProgBudgetInfo : BudgetInfoVals
    {
        public int ClientId { get; set; }
        public DateTime Date { get; set; }

        public virtual Client Client { get; set; }
    }
    public class ProgVendorBudgetInfo : BudgetInfoVals
    {
        public int ClientId { get; set; }
        public int VendorId { get; set; }
        public DateTime Date { get; set; }

        public virtual Client Client { get; set; }
        public virtual Vendor Vendor { get; set; }
    }

    // --- Vendor ---

    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual VendorProg Prog { get; set; }
    }
    public class VendorProg //"ProgInfo" for a vendor
    {
        [Key, ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [MaxLength(50)] //, Index("CodeIndex", IsUnique = true)]
        public string Code { get; set; }

        public virtual Vendor Vendor { get; set; }
    }

}
