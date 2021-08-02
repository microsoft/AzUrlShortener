using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyBlazorAdmin.Data
{
    public class ClickStatsEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string Datetime { get; set; }

        public ClickStatsEntity() { }

        public ClickStatsEntity(string vanity)
        {
            PartitionKey = vanity;
            RowKey = Guid.NewGuid().ToString();
            Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
