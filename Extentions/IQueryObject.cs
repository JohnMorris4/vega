namespace vega.Extentions
{
    public interface IQueryObject
    {
        string SortBy { get; set; }

        bool IsSortAscending { get; set; }
        
        int Page { set; get; }
        int PageSize { set; get; }
    }
}