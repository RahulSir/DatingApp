namespace DatingApp.API.Helpers
{
    public class PaginationHeader
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public PaginationHeader(int CurrentPage , int itemsperpage , int totalpages , int totalitems)
        {
            this.CurrentPage = CurrentPage;
            this.ItemsPerPage = itemsperpage;
            this.TotalPages = totalpages;
            this.TotalItems = totalitems;
        }
        
    }
}