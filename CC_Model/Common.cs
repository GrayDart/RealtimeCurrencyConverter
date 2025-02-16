using CC_Model.ExchangeRateVM;
using System.ComponentModel.DataAnnotations;

namespace CC_Model
{
    public class Result
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }

    public class PaginatedResult<T> : HistoryResponse
    {
        public List<T>? Items { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class InternalReq
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public TrackingInfo TrackingInfo { get; set; }
        public string VirtualPath { get; set; }
        public string Lang { get; set; }
        public bool IsValidUser { get; set; }
        public string Message { get; set; }
    }

    public class TrackingInfo
    {
        public string IPAddress { get; set; }
        public string GeoLocation { get; set; }
    }

    public class PagingRequest
    {
        [Range(1,int.MaxValue)]
        public int CurrentPage { get; set; }
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
}