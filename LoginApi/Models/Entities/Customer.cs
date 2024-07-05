using System;
using System.Collections.Generic;

namespace LoginApi.Models.Entities;

public partial class Customer
{
    public long Id { get; set; }

    public string? Codigo { get; set; }

    public string? Nome { get; set; }

    public string? Status { get; set; }
}
