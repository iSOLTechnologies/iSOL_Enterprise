namespace iSOL_Enterprise.Models
{
    public class TreeModel
    {

        public string id;

        public string? text;

        public long? population = null;

        public string? flagUrl = null;
        public bool @checked { get; set; }

        public bool hasChildren = false;

        public List<TreeModel> children = new List<TreeModel>();
    }
}
