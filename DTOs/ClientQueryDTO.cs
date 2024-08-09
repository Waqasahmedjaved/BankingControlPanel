using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.DTOs
{
    public class ClientQueryDTO
    {
        public string Filter { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
