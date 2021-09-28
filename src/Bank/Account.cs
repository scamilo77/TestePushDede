using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Banking
{
    [ExcludeFromCodeCoverage]
    public class Account
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public decimal Balance { get; set; }
    }
}

