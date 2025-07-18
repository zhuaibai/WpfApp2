using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Tools
{
    public class TimeTracker
    {
        // 记录任务开始时间
        public static DateTime Start()
        {
            return DateTime.UtcNow; // 使用UTC时间避免时区变化影响
        }

        // 计算任务耗时（毫秒）
        public static long GetElapsedMilliseconds(DateTime startTime)
        {
            return (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
        }

        // 计算任务耗时（TimeSpan格式）
        public static TimeSpan GetElapsedTime(DateTime startTime)
        {
            return DateTime.UtcNow - startTime;
        }
    }
}
