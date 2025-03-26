using System.Collections.Generic;

namespace Cloud5mins.ShortenerTools.Core.Domain
{
    public class ClickStatsEntityList
    {
        public List<ClickStatsEntity> ClickStatsList { get; set; }

        public ClickStatsEntityList() { }
        public ClickStatsEntityList(List<ClickStatsEntity> list)
        {
            ClickStatsList = list;
        }
    }
}