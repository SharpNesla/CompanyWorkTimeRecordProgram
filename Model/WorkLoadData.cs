using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.Model
{
    /// <summary>
    /// Вспомогательный класс данных, использующийся при
    /// построении диаграмм загруженности работников по дням недели.
    /// </summary>
    public class WorkLoadData
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public int Average { get; set; }
    }
}
