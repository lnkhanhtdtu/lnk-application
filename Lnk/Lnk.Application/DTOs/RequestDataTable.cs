namespace Lnk.Application.DTOs;

public class RequestDataTable
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public string Keyword { get; set; }
    public List<Column> Columns { get; set; }
    public List<OrderColumn> Order { get; set; }
    public SearchValue Search { get; set; }

    public int PageSize => Length;
    public int SkipItems => Start;

    public class Column
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchValue Search { get; set; }
    }

    public class OrderColumn
    {
        public int Column { get; set; }
        public string Dir { get; set; }
        public string Name { get; set; }
    }

    public class SearchValue
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

}
