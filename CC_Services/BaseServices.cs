namespace CC_Services
{
    using AppSettings_Reader;
    using CC_Infrastructure.Data;
    using CC_Model;

    public class BaseServices
    {
        public DataContext _dbcontext;
        public Helper _appsettinghelper;

        public static PaginatedResult<T> GetPaginatedData<T>(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            int totalRecords = source.Count();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            List<T> items = source.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            return new PaginatedResult<T>
            {
                Items = items,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}