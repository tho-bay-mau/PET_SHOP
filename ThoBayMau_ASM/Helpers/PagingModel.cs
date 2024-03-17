namespace ThoBayMau_ASM.Helpers
{
    public class PagingModel
    {
        public int currentpage { get; set; }
        public int countpages { get; set; }
        public required Func<int?, string> generateUrl { get; set; }
    }
}
