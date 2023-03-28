namespace iSOL_Enterprise.Models.sale
{
    public class tbl_OWHS
    {
        public string? whscode { get; set; }
        public string? whsname { get; set; }

        public double MinStock { get;set; }
        public double MaxStock { get;set; }
        public double MinOrder { get;set; }
        public char? Locked { get;set; }

        public bool? isEditable { get; set; } = true;

    }
}
