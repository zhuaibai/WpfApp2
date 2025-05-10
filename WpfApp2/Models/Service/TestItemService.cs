using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Tools;

namespace WpfApp2.Models.Service
{
    public class TestItemService
    {
        // 创建表（如果不存在）
        public void CreateTable()
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS TestItems (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    IsImportant INTEGER NOT NULL
                )";

            SQLiteHelper.ExecuteNonQuery(sql);
        }

        // 添加项目
        public void AddTestItem(TestItem item)
        {
            string sql = "INSERT INTO TestItems (Name, IsImportant) VALUES (@Name, @IsImportant)";
            SQLiteHelper.ExecuteNonQuery(sql, item.Name, item.IsImportant ? 1 : 0);
        }

        // 更新项目
        public void UpdateTestItem(TestItem item)
        {
            string sql = "UPDATE TestItems SET Name = @Name, IsImportant = @IsImportant WHERE Id = @Id";
            SQLiteHelper.ExecuteNonQuery(sql, item.Name, item.IsImportant ? 1 : 0, item.Id);
        }

        // 删除项目
        public void DeleteTestItem(int id)
        {
            string sql = "DELETE FROM TestItems WHERE Id = @Id";
            SQLiteHelper.ExecuteNonQuery(sql, id);
        }

        // 获取所有项目
        public List<TestItem> GetAllTestItems()
        {
            string sql = "SELECT * FROM TestItems";
            DataSet ds = SQLiteHelper.ExecuteDataSet(sql);

            var items = new List<TestItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                items.Add(new TestItem
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    IsImportant = Convert.ToInt32(row["IsImportant"]) == 1
                });
            }

            return items;
        }

    }
}
