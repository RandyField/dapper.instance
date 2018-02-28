using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhp.Awards.Common.Helper
{
    public class DapperSqlHelper
    {
        /// <summary>
        /// 默认链接字符串
        /// </summary>
        private static string DefaultDbConnectionString = @"Server=localhost;Database=School;user=root;Password=randy1992;pooling=true;CharSet=utf8;port=3306;sslmode=none";

        /// <summary>
        /// 链接字符串
        /// </summary>
        private static string connection;

        #region +DapperSqlHelper构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlConnectionString"></param>
        public DapperSqlHelper(string sqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                connection = DefaultDbConnectionString;
            }
            else
            {
                connection = sqlConnectionString;
            }
        }

        #endregion

        #region +增加实体

        /// <summary>
        /// 增加实体操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CreateEntity<T>(string cmd, T entity) where T : class, new()
        {
            using (IDbConnection conn = new SqlConnection(connection))
            {
                return conn.Execute(cmd, entity) > 0;
            }
        }

        #endregion

        #region +删除实体，根据主键
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEntityById(string cmd, int id)
        {
            using (IDbConnection conn = new SqlConnection(connection))
            {
                return conn.Execute(cmd, new { id = id }) > 0;
            }
        }
        #endregion

        #region +ExcuteNonQuery 增、删、改同步操作
        /// <summary>
        /// 增、删、改同步操作
        ///  </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">链接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>int</returns>
        public int ExcuteNonQuery<T>(string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    result = con.Execute(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    result = con.Execute(cmd, param, null, null, CommandType.Text);
                }
            }
            return result;
        }
        #endregion

        #region +ExcuteNonQueryAsync 增、删、改异步操作
        /// <summary>
        /// 增、删、改异步操作
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">链接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>int</returns>
        public async Task<int> ExcuteNonQueryAsync<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    result = await con.ExecuteAsync(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    result = await con.ExecuteAsync(cmd, param, null, null, CommandType.Text);
                }
            }
            return result;
        }
        #endregion

        #region +ExecuteScalar 同步查询操作
        /// <summary>
        /// 同步查询操作
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>object</returns>
        public object ExecuteScalar<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            object result = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    result = con.ExecuteScalar(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    result = con.ExecuteScalar(cmd, param, null, null, CommandType.Text);
                }
            }
            return result;
        }
        #endregion

        #region +ExecuteScalarAsync 异步查询操作
        /// <summary>
        /// 异步查询操作
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>object</returns>
        public async Task<object> ExecuteScalarAsync<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            object result = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    result = await con.ExecuteScalarAsync(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    result = con.ExecuteScalarAsync(cmd, param, null, null, CommandType.Text);
                }
            }
            return result;
        }
        #endregion

        #region +FindOne  同步查询一条数据
        /// <summary>
        /// 同步查询一条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public T FindOne<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                T t = new T();
                foreach (var item in type.GetProperties())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        //属性名与查询出来的列名比较
                        if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                        var kvalue = dataReader[item.Name];
                        if (kvalue == DBNull.Value) continue;
                        item.SetValue(t, kvalue, null);
                        break;
                    }
                }
                return t;
            }
        }
        #endregion

        #region +FindOne  异步查询一条数据
        /// <summary>
        /// 异步查询一条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public async Task<T> FindOneAsync<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = await con.ExecuteReaderAsync(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = await con.ExecuteReaderAsync(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                T t = new T();
                foreach (var item in type.GetProperties())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        //属性名与查询出来的列名比较
                        if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                        var kvalue = dataReader[item.Name];
                        if (kvalue == DBNull.Value) continue;
                        item.SetValue(t, kvalue, null);
                        break;
                    }
                }
                return t;
            }
        }
        #endregion

        #region +FindToList  同步查询数据集合
        /// <summary>
        /// 同步查询数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public IList<T> FindToList<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                List<T> tlist = new List<T>();
                while (dataReader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            //属性名与查询出来的列名比较
                            if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                            var kvalue = dataReader[item.Name];
                            if (kvalue == DBNull.Value) continue;
                            item.SetValue(t, kvalue, null);
                            break;
                        }
                    }
                    if (tlist != null) tlist.Add(t);
                }
                return tlist;
            }
        }
        #endregion

        #region +FindToListAsync  异步查询数据集合
        /// <summary>
        /// 异步查询数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public async Task<IList<T>> FindToListAsync<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = await con.ExecuteReaderAsync(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = await con.ExecuteReaderAsync(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                List<T> tlist = new List<T>();
                while (dataReader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            //属性名与查询出来的列名比较
                            if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                            var kvalue = dataReader[item.Name];
                            if (kvalue == DBNull.Value) continue;
                            item.SetValue(t, kvalue, null);
                            break;
                        }
                    }
                    if (tlist != null) tlist.Add(t);
                }
                return tlist;
            }
        }
        #endregion

        #region +FindToList  同步查询数据集合
        /// <summary>
        /// 同步查询数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public IList<T> FindToListAsPage<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                List<T> tlist = new List<T>();
                while (dataReader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            //属性名与查询出来的列名比较
                            if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                            var kvalue = dataReader[item.Name];
                            if (kvalue == DBNull.Value) continue;
                            item.SetValue(t, kvalue, null);
                            break;
                        }
                    }
                    if (tlist != null) tlist.Add(t);
                }
                return tlist;
            }
        }
        #endregion

        #region +FindToListByPage  同步分页查询数据集合
        /// <summary>
        /// 同步分页查询数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public IList<T> FindToListByPage<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = con.ExecuteReader(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                List<T> tlist = new List<T>();
                while (dataReader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            //属性名与查询出来的列名比较
                            if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                            var kvalue = dataReader[item.Name];
                            if (kvalue == DBNull.Value) continue;
                            item.SetValue(t, kvalue, null);
                            break;
                        }
                    }
                    if (tlist != null) tlist.Add(t);
                }
                return tlist;
            }
        }
        #endregion

        #region +FindToListByPageAsync  异步分页查询数据集合
        /// <summary>
        /// 异步分页查询数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">连接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>t</returns>
        public async Task<IList<T>> FindToListByPageAsync<T>(string connection, string cmd, DynamicParameters param, bool flag = true) where T : class, new()
        {
            IDataReader dataReader = null;
            using (IDbConnection con = new SqlConnection(connection))
            {
                if (flag)
                {
                    dataReader = await con.ExecuteReaderAsync(cmd, param, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    dataReader = await con.ExecuteReaderAsync(cmd, param, null, null, CommandType.Text);
                }
                if (dataReader == null || !dataReader.Read()) return null;
                Type type = typeof(T);
                List<T> tlist = new List<T>();
                while (dataReader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            //属性名与查询出来的列名比较
                            if (item.Name.ToLower() != dataReader.GetName(i).ToLower()) continue;
                            var kvalue = dataReader[item.Name];
                            if (kvalue == DBNull.Value) continue;
                            item.SetValue(t, kvalue, null);
                            break;
                        }
                    }
                    if (tlist != null) tlist.Add(t);
                }
                return tlist;
            }
        }
        #endregion

    }
}
