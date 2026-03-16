using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Tools
{
    public static class HelpTools
    {
        /// <summary>
        /// 去掉首尾的两个值，求平均值
        /// </summary>
        /// <param name="samples">数据集合</param>
        /// <returns></returns>
        public static double FilterAverage(List<double> samples)
        {
            var ordered = samples.OrderBy(x => x).ToList();

            //去掉最大最小
            ordered.RemoveAt(0);
            ordered.RemoveAt(ordered.Count - 1);

            return ordered.Average();
        }
    }
}
