using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Collections;
using System.Data.SQLite;
using System.Configuration; // 添加.net引用

namespace WpfApp2.Tools
{
    /// <summary>
    /// 对SQLite操作的类
    /// 引用：System.Data.SQLite.dll【版本：3.6.23.1（原始文件名：SQlite3.DLL）】
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// 所有成员函数都是静态的，构造函数定义为私有
        /// </summary>
        private SQLiteHelper()
        {
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            //"Data Source=Test.db3;Pooling=true;FailIfMissing=false";
            get
            ;set;
        }

        private static SQLiteConnection _Conn = null;

        /// <summary>
        /// 连接对象
        /// </summary>
        public static SQLiteConnection Conn
        {
            get
            {
                if (_Conn == null)
                {
                    _Conn = new SQLiteConnection(ConnectionString);
                }

                return _Conn;
            }
            set => _Conn = value;
        }

        #region CreateCommand(commandText,SQLiteParameter[])
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="commandText">语句</param>
        /// <param name="commandParameters">语句参数</param>
        /// <returns>SQLite Command</returns>
        public static SQLiteCommand CreateCommand(string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand(commandText, Conn);
            if (commandParameters.Length > 0)
            {
                foreach (SQLiteParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }

            return cmd;
        }
        #endregion

        #region CreateParameter(parameterName,parameterType,parameterValue)
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterType">参数类型</param>
        /// <param name="parameterValue">参数值</param>
        /// <returns>返回创建的参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, DbType parameterType, object parameterValue)
        {
            SQLiteParameter parameter = new SQLiteParameter
            {
                DbType = parameterType,
                ParameterName = parameterName,
                Value = parameterValue
            };

            return parameter;
        }
        #endregion

        #region ExecuteDataSet(commandText,paramList[])
        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="commandText">查询语句</param>
        /// <param name="paramList">object参数列表</param>
        /// <returns>数据集</returns>
        public static DataSet ExecuteDataSet(string commandText, params object[] paramList)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                if (paramList != null)
                {
                    AttachParameters(cmd, commandText, paramList);
                }

                DataSet ds = new DataSet();
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(ds);
                }
                finally
                {
                    conn.Close();
                }

                return ds;
            }
        }
        #endregion

        #region ExecuteDataSet(SQLiteCommand)
        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="cmd">SQLiteCommand对象</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataSet(SQLiteCommand cmd)
        {
            // 标记是否需要关闭连接
            bool mustCloseConnection = false;
            // 检查当前连接状态
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();// 打开数据库连接
                mustCloseConnection = true;// 标记连接是本方法打开的，需要关闭
            }
            // 创建DataSet对象，用于存储查询结果
            DataSet ds = new DataSet();
            try
            {
                // 创建数据适配器，用于填充DataSet
                // SQLiteDataAdapter会使用cmd来执行查询并将结果映射到DataSet
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                // 执行查询并将结果填充到DataSet中
                // Fill方法内部会执行SELECT语句，并将返回的数据行填充到DataSet的DataTable中
                da.Fill(ds);
            }
            finally
            {
                if (mustCloseConnection)
                {
                    cmd.Connection.Close();// 关闭数据库连接，释放资源
                }
                // 释放命令对象资源
                // Dispose方法会释放SQLiteCommand占用的非托管资源
                cmd.Dispose();
            }
            // 返回填充好的DataSet
            return ds;
        }
        #endregion

        #region ExecuteDataSet(SQLiteTransaction,commandText,params SQLiteParameter[])
        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="transaction">SQLiteTransaction对象</param>
        /// <param name="commandText">查询语句</param>
        /// <param name="commandParameters">命令的参数列表</param>
        /// <returns>DataSet</returns>
        /// <remarks>必须手动执行关闭连接transaction.connection.Close</remarks>
        public static DataSet ExecuteDataSet(SQLiteTransaction transaction, string commandText, params SQLiteParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rolled back or committed, please provide an open transaction.", "transaction");

            using (SQLiteCommand cmd = new SQLiteCommand(commandText, transaction.Connection, transaction))
            {
                foreach (SQLiteParameter parm in commandParameters)
                {
                    cmd.Parameters.Add(parm);
                }

                return ExecuteDataSet(cmd);
            }
        }
        #endregion

        #region ExecuteDataSet(SQLiteTransaction,commandText,object[] commandParameters)
        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="transaction">SQLiteTransaction对象</param>
        /// <param name="commandText">查询语句</param>
        /// <param name="commandParameters">命令参数列表</param>
        /// <returns>返回数据集</returns>
        /// <remarks>必须手动执行关闭连接transaction.connection.Close</remarks>
        public static DataSet ExecuteDataSet(SQLiteTransaction transaction, string commandText, object[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rolled back or committed, please provide an open transaction.", "transaction");

            using (SQLiteCommand cmd = new SQLiteCommand(commandText, transaction.Connection, transaction))
            {
                AttachParameters(cmd, cmd.CommandText, commandParameters);
                return ExecuteDataSet(cmd);
            }
        }
        #endregion

        #region UpdateDataset(insertCommand,deleteCommand,updateCommand,dataSet,tableName)
        /// <summary>
        /// 更新数据集中数据到数据库
        /// </summary>
        /// <param name="insertCommand">insert语句</param>
        /// <param name="deleteCommand">delete语句</param>
        /// <param name="updateCommand">update语句</param>
        /// <param name="dataSet">要更新的DataSet</param>
        /// <param name="tableName">数据集中要更新的table名</param>
        public static void UpdateDataset(SQLiteCommand insertCommand, SQLiteCommand deleteCommand, SQLiteCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null)
                throw new ArgumentNullException("insertCommand");

            if (deleteCommand == null)
                throw new ArgumentNullException("deleteCommand");

            if (updateCommand == null)
                throw new ArgumentNullException("updateCommand");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            // 创建一个SQLiteDataAdapter，并在使用后释放它
            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter())
            {
                // 设置数据适配器命令
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // 更新数据源中数据集的更改
                dataAdapter.Update(dataSet, tableName);

                // 提交对DataSet所做的所有更改
                dataSet.AcceptChanges();
            }
        }
        #endregion

        #region ExecuteReader(SQLiteCommand,commandText, object[] paramList)
        /// <summary>
        /// ExecuteReader方法
        /// </summary>
        /// <param name="cmd">查询命令</param>
        /// <param name="commandText">含有类似@colume参数的sql语句</param>
        /// <param name="paramList">语句参数列表</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(SQLiteCommand cmd, string commandText, object[] paramList)
        {
            if (cmd.Connection == null)
                throw new ArgumentException("没有为命令指定活动的连接.", "cmd");

            cmd.CommandText = commandText;
            AttachParameters(cmd, commandText, paramList);

            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        #region ExecuteNonQuery(commandText,paramList)
        /// <summary>
        /// 执行ExecuteNonQuery方法
        /// </summary>
        /// <param name="commandText">语句</param>
        /// <param name="paramList">参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, params object[] paramList)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                AttachParameters(cmd, commandText, paramList);

                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region ExecuteNonQuery(SQLiteTransaction,commandText,paramList)
        /// <summary>
        /// 执行ExecuteNonQuery方法,带事务
        /// </summary>
        /// <param name="transaction">之前创建好的SQLiteTransaction对象</param>
        /// <param name="commandText">语句</param>
        /// <param name="paramList">参数</param>
        /// <returns>返回影响的行数</returns>
        /// <remarks>
        /// 定义事务  DbTransaction trans = conn.BeginTransaction();
        ///     或者：SQLiteTransaction trans = Conn.BeginTransaction();
        /// 操作代码示例：
        /// try
        ///{
        ///    // 连续操作记录 
        ///    for (int i = 0; i < 1000; i++)
        ///    {
        ///        ExecuteNonQuery(trans,commandText,[] paramList);
        ///    }
        ///    trans.Commit();
        ///}
        ///catch
        ///{
        ///    trans.Rollback();
        ///    throw;
        ///}
        ///trans.Connection.Close();//关闭事务连接
        ///transaction.Dispose();//释放事务对象
        /// </remarks>
        public static int ExecuteNonQuery(SQLiteTransaction transaction, string commandText, params object[] paramList)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rolled back or committed, please provide an open transaction.", "transaction");

            using (SQLiteCommand cmd = new SQLiteCommand(commandText, transaction.Connection, transaction))
            {
                AttachParameters(cmd, cmd.CommandText, paramList);
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region ExecuteNonQuery(IDbCommand)
        /// <summary>
        /// 执行ExecuteNonQuery方法
        /// </summary>
        /// <param name="cmd">创建好的命令</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(IDbCommand cmd)
        {
            bool mustCloseConnection = false;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
                mustCloseConnection = true;
            }

            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (mustCloseConnection)
                {
                    cmd.Connection.Close();
                }

                cmd.Dispose();
            }
        }
        #endregion

        #region ExecuteScalar(commandText,paramList)
        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="commandText">语句</param>
        /// <param name="paramList">参数</param>
        /// <returns>返回查询结果的第一行第一列</returns>
        public static object ExecuteScalar(string commandText, params object[] paramList)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                AttachParameters(cmd, commandText, paramList);

                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    return cmd.ExecuteScalar();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region ExecuteXmlReader(IDbCommand)
        /// <summary>
        /// ExecuteXmlReader返回xml格式
        /// </summary>
        /// <param name="command">语句</param>
        /// <returns>返回XmlTextReader对象</returns>
        public static XmlReader ExecuteXmlReader(IDbCommand command)
        {
            // 如果连接未打开，则打开连接，但要确保我们知道在完成后关闭它
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            // 获取一个数据适配器
            SQLiteDataAdapter da = new SQLiteDataAdapter((SQLiteCommand)command);
            DataSet ds = new DataSet();

            // 填充数据集，并返回架构信息
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds);

            // 将我们的数据集转换为XML
            StringReader stream = new StringReader(ds.GetXml());
            command.Connection.Close();

            // 将我们的文本流转换为XmlReader
            return new XmlTextReader(stream);
        }
        #endregion

        #region AttachParameters(SQLiteCommand,commandText,object[] paramList)
        /// <summary>
        /// 增加参数到命令（自动判断类型）
        /// </summary>
        /// <param name="commandText">命令语句</param>
        /// <param name="paramList">object参数列表</param>
        /// <returns>返回SQLiteParameterCollection参数列表</returns>
        /// <remarks>Status experimental. Regex appears to be handling most issues. Note that parameter object array must be in same ///order as parameter names appear in SQL statement.</remarks>
        private static SQLiteParameterCollection AttachParameters(SQLiteCommand cmd, string commandText, params object[] paramList)
        {
            if (paramList == null || paramList.Length == 0)
                return null;

            SQLiteParameterCollection coll = cmd.Parameters;
            string parmString = commandText.Substring(commandText.IndexOf("@"));

            // 预处理字符串，确保逗号后至少有一个空格
            parmString = parmString.Replace(",", " ,");

            // 将命名参数放入匹配集合中
            string pattern = @"(@)\S*(.*?)\b";
            Regex ex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = ex.Matches(parmString);
            string[] paramNames = new string[mc.Count];

            int i = 0;
            foreach (Match m in mc)
            {
                paramNames[i] = m.Value;
                i++;
            }

            // 现在让我们对参数进行类型化
            int j = 0;
            Type t = null;

            foreach (object o in paramList)
            {
                t = o.GetType();

                SQLiteParameter parm = new SQLiteParameter();
                switch (t.ToString())
                {
                    case ("DBNull"):
                    case ("Char"):
                    case ("SByte"):
                    case ("UInt16"):
                    case ("UInt32"):
                    case ("UInt64"):
                        throw new SystemException("Invalid data type");

                    case ("System.String"):
                        parm.DbType = DbType.String;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (string)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Byte[]"):
                        parm.DbType = DbType.Binary;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (byte[])paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Int32"):
                        parm.DbType = DbType.Int32;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (int)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Boolean"):
                        parm.DbType = DbType.Boolean;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (bool)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.DateTime"):
                        parm.DbType = DbType.DateTime;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDateTime(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Double"):
                        parm.DbType = DbType.Double;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDouble(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Decimal"):
                        parm.DbType = DbType.Decimal;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDecimal(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Guid"):
                        parm.DbType = DbType.Guid;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (System.Guid)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Object"):
                        parm.DbType = DbType.Object;
                        parm.ParameterName = paramNames[j];
                        parm.Value = paramList[j];
                        coll.Add(parm);
                        break;

                    default:
                        throw new SystemException("Value is of unknown data type");
                } // end switch

                j++;
            }

            return coll;
        }
        #endregion

        #region ExecuteNonQueryTypedParams(IDbCommand, DataRow)
        /// <summary>
        /// 从DataRow执行非查询类型参数
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="dataRow">数据行</param>
        /// <returns>整数结果代码</returns>
        public static int ExecuteNonQueryTypedParams(IDbCommand command, DataRow dataRow)
        {
            int retVal = 0;

            // 如果行有值，则必须初始化存储过程参数
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 设置参数值
                AssignParameterValues(command.Parameters, dataRow);
                retVal = ExecuteNonQuery(command);
            }
            else
            {
                retVal = ExecuteNonQuery(command);
            }

            return retVal;
        }
        #endregion

        #region AssignParameterValues
        /// <summary>
        /// 此方法将dataRow列值分配给IDataParameterCollection
        /// </summary>
        /// <param name="commandParameters">要分配值的IDataParameterCollection</param>
        /// <param name="dataRow">用于保存命令参数值的dataRow</param>
        /// <exception cref="System.InvalidOperationException">如果任何参数名称无效，则抛出异常</exception>
        protected internal static void AssignParameterValues(IDataParameterCollection commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                // 如果没有数据，则不执行任何操作
                return;
            }

            DataColumnCollection columns = dataRow.Table.Columns;
            int i = 0;

            // 设置参数值
            foreach (IDataParameter commandParameter in commandParameters)
            {
                // 检查参数名称
                if (string.IsNullOrEmpty(commandParameter.ParameterName) || commandParameter.ParameterName.Length <= 1)
                    throw new InvalidOperationException(string.Format(
                        "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                        i, commandParameter.ParameterName));

                if (columns.Contains(commandParameter.ParameterName))
                    commandParameter.Value = dataRow[commandParameter.ParameterName];
                else if (columns.Contains(commandParameter.ParameterName.Substring(1)))
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];

                i++;
            }
        }
        #endregion

        #region AssignParameterValues
        /// <summary>
        /// 此方法将dataRow列值分配给IDataParameters数组
        /// </summary>
        /// <param name="commandParameters">要分配值的IDataParameters数组</param>
        /// <param name="dataRow">用于保存存储过程参数值的dataRow</param>
        /// <exception cref="System.InvalidOperationException">如果任何参数名称无效，则抛出异常</exception>
        protected void AssignParameterValues(IDataParameter[] commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                // 如果没有数据，则不执行任何操作
                return;
            }

            DataColumnCollection columns = dataRow.Table.Columns;
            int i = 0;

            // 设置参数值
            foreach (IDataParameter commandParameter in commandParameters)
            {
                // 检查参数名称
                if (string.IsNullOrEmpty(commandParameter.ParameterName) || commandParameter.ParameterName.Length <= 1)
                    throw new InvalidOperationException(string.Format(
                        "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                        i, commandParameter.ParameterName));

                if (columns.Contains(commandParameter.ParameterName))
                    commandParameter.Value = dataRow[commandParameter.ParameterName];
                else if (columns.Contains(commandParameter.ParameterName.Substring(1)))
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];

                i++;
            }
        }
        #endregion

        #region AssignParameterValues
        /// <summary>
        /// 此方法将值数组分配给IDataParameters数组
        /// </summary>
        /// <param name="commandParameters">要分配值的IDataParameters数组</param>
        /// <param name="parameterValues">保存要分配值的对象数组</param>
        /// <exception cref="System.ArgumentException">如果传递的参数数量不正确，则抛出异常</exception>
        protected void AssignParameterValues(IDataParameter[] commandParameters, params object[] parameterValues)
        {
            if (commandParameters == null || parameterValues == null)
            {
                // 如果没有数据，则不执行任何操作
                return;
            }

            // 我们必须拥有与参数数量相同的值，才能将它们放入相应的参数中
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // 遍历IDataParameters，从值数组中的相应位置分配值
            for (int i = 0, j = commandParameters.Length, k = 0; i < j; i++)
            {
                if (commandParameters[i].Direction != ParameterDirection.ReturnValue)
                {
                    // 如果当前数组值派生自IDataParameter，则分配其Value属性
                    if (parameterValues[k] is IDataParameter)
                    {
                        IDataParameter paramInstance;
                        paramInstance = (IDataParameter)parameterValues[k];
                        if (paramInstance.Direction == ParameterDirection.ReturnValue)
                        {
                            paramInstance = (IDataParameter)parameterValues[++k];
                        }

                        if (paramInstance.Value == null)
                        {
                            commandParameters[i].Value = DBNull.Value;
                        }
                        else
                        {
                            commandParameters[i].Value = paramInstance.Value;
                        }
                    }
                    else if (parameterValues[k] == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = parameterValues[k];
                    }

                    k++;
                }
            }
        }
        #endregion
    }
}