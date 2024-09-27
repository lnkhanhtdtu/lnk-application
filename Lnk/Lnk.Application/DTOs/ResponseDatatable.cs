using System;

namespace Lnk.Application.DTOs;

public class ResponseDatatable<T>
{
    public int RecordsFiltered { get; set; }
    public int RecordsTotal { get; set; }
    public int Draw { get; set; }
    public List<T> Data { get; set; }
}
