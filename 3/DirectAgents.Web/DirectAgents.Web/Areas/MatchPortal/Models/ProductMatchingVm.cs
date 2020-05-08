using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace DirectAgents.Web.Areas.MatchPortal.Models
{
   /* public class MainTableVm
    {

        public List<MainTable> LatestsInfo { get; set; }
    }*/

    public class ProductMatchingVm
    {
        /*[Required(ErrorMessage = "Please supply the title.")]
        [Display(Name = "Title")]
        public  Category { get; set; }

        [Required(ErrorMessage = "Please supply the title.")]
        [Display(Name = "Title")]
        public Brand? Brand { get; set; }

        [Required(ErrorMessage = "Please supply the title.")]
        [Display(Name = "Title")]
        public Brand​​Matched? Brand​​Matched { get; set; }

      /*  public string ProductId { get; set; }

        public string ProductTitle { get; set; }*/

       // public List<MainTableVm> LatestsInfo { get; set; }
       public int number { get; set; }

    }
}