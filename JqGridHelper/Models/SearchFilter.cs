using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JqGridHelper.Models
{
    public class SearchFilter
    {
        public string groupOp { set; get; }
        public List<SearchGroup> groups { set; get; }
        public List<SearchRule> rules { set; get; }
    }

    public class SearchRule
    {
        public string field { set; get; }
        public string op { set; get; }
        public string data { set; get; }

        public override string ToString()
        {
            return $"'{field}' {op} '{data}'";
        }
    }

    public class SearchGroup
    {
        public string groupOp { set; get; }
        public List<SearchRule> rules { set; get; }
    }
}
