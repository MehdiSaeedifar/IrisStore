using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JqGridHelper.Models
{
    public class JqGridData
    {
        /// <summary>
        /// تعداد صفحات
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// شماره صفحه جاری
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// تعداد کل ردیف‌ها
        /// </summary>
        public int Records { get; set; }

        /// <summary>
        /// اطلاعات ردیف‌ها
        /// </summary>
        public IList<JqGridRowData> Rows { get; set; }

        /// <summary>
        /// اطلاعاتی اختیاری جهت نمایش در فوتر گرید
        /// </summary>
        public object UserData { get; set; }
    }

    public class JqGridRowData
    {
        public int Id { set; get; }
        public IList<object> RowCells { set; get; }
    }
}
