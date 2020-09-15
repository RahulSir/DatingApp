namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int maximumSize = 50;
        public int PageNumber { get; set; }  = 1;
        private int pageSize = 10;
        public int PageSize

        {
            get { return pageSize; }
            set { pageSize = (value > maximumSize) ? maximumSize : value; }
        }

        public int UserId{get;set;}

        public string Gender{get;set;}

        public int minAge{get;set;} = 18;

        public int maxAge{get;set;} = 99;

        public string OrderBy{get;set;}
    }
}