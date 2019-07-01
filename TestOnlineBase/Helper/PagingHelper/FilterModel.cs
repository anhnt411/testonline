using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineBase.Helper.PagingHelper
{
    public class FilterModel
    {
        public IList<FilterTypeModel> Filter { get; set; }
        /// <summary>
        /// Sort
        /// </summary>
        public IList<SortTypeModel> Sort { get; set; }

        public string MultipeFilter { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public bool? IsExport { get; set; }
  
    }

    public class FilterTypeModel
    {
        /// <summary>
        /// Field Name
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Value String
        /// </summary>
        public string ValueString { get; set; }
        /// <summary>
        /// Value DateTime From
        /// </summary>
        public DateTime? ValueDateTimeFrom { get; set; }
        /// <summary>
        /// Value DateTime To
        /// </summary>
        public DateTime? ValueDateTimeTo { get; set; }
        /// <summary>
        /// Value Decimal From
        /// </summary>
        [DataType("decimal(18,0)")]
        public decimal? ValueDecimalFrom { get; set; }
        /// <summary>
        /// Value Decimal To
        /// </summary>
        [DataType("decimal(18,0)")]
        public decimal? ValueDecimalTo { get; set; }
        /// <summary>
        /// Value bool
        /// </summary>
        public bool? ValueBit { get; set; }
        /// <summary>
        /// Is Active
        /// </summary>
        public bool IsActive { get; set; }
    }

    public class SortTypeModel
    {
        /// <summary>
        /// Field Name
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Order by ascending
        /// </summary>
        public bool? Asc { get; set; }
        /// <summary>
        /// Is Active
        /// </summary>
        public bool IsActive { get; set; }
    }

}
