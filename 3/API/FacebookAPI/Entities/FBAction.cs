namespace FacebookAPI.Entities
{
    public class FBAction
    {
        public string ActionType { get; set; }
        public int? Num_click { get; set; }
        public int? Num_view { get; set; }
        public decimal? Val_click { get; set; }
        public decimal? Val_view { get; set; }
        public string ClickAttrWindow { get; set; }
        public string ViewAttrWindow { get; set; }
    }
}
