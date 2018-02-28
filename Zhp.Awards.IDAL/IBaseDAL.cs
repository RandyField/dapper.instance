using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhp.Awards.IDAL
{
    public interface IBaseDAL 
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql语句</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool CreateEntity<T>(string cmd, T entity) where T : class, new();


        /// <summary>
        /// 根据主键获取一个实体
        /// cmd="SELECT * FROM [dbo].[TABLENAME] WHERE id=@id"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql语句</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        T RetriveOneEntityById<T>(string cmd,int id)  where T : class, new();

        /// <summary>
        /// 获取所有实体
        /// cmd="SELECT a,b,c,d FROM [dbo].[TABLENAME]"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql语句</param>
        /// <returns></returns>
        IEnumerable<T> RetriveAllEntity<T>(string cmd) where T : class, new();


        /// <summary>
        /// 更新实体
        /// cmd="UPDATE [dbo].[TABLENAME] SET a=@a,b=@b,c=@c WHERE id=@id"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql语句</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool UpdateEntity<T>(string cmd,T entity) where T : class, new();

        /// <summary>
        /// 根据主键删除一个实体
        /// cmd=@"DELETE FROM [dbo].[TABLENAME] WHERE Id = @Id"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql语句</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        bool DeleteEntityById<T>(string cmd,int id) where T : class, new();

        /// <summary>
        /// 异步查询-获取一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns></returns>
        Task<T> FindOneAsync<T>(string cmd, DynamicParameters param, bool flag = true) where T : class, new();

        /// <summary>
        /// 增、删、改同步操作
        ///  </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">链接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns>int</returns>
        int ExcuteNonQuery<T>(string cmd, DynamicParameters param, bool flag = true) where T : class, new();

        /// <summary>
        /// 同步获取一个实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="connection">链接字符串</param>
        /// <param name="cmd">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="flag">true存储过程，false sql语句</param>
        /// <returns></returns>
        T FindOne<T>(string cmd, DynamicParameters param, bool flag = true) where T : class,new();
    }
}
