using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System
{
    /// <summary>
    /// 数据仓储基类，封装数据增、删、改、查询常用操作
    /// </summary>
    /// <typeparam name="T">实体模型</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 根据编号获取模型实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回模型实体</returns>
        T GetById(object id);

        /// <summary>
        /// 实例化DbContext
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <returns>返回IRepository</returns>
        IRepository<T> Instance(DbContext context);

        /// <summary>
        /// 根据Lambda表达式获取模型实体
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns>返回模型实体</returns>
        T GetModel(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>返回是否成功</returns>
        bool Insert(T entity);

        /// <summary>
        /// 添加列表数据
        /// </summary>
        /// <param name="list">实体模型集合</param>
        /// <returns>返回影响行数</returns>
        int InsertBatch(List<T> list);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>返回执行成功</returns>
        bool Update(T entity);

        void UpdatNoSaveChanges(T entity);

        /// <summary>
        /// 修改指定属性
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <param name="proNames">属性名称</param>
        /// <returns>返回影响行数</returns>
        int Modify(T model, params string[] proNames);

        /// <summary>
        /// 根据Lambda表达式修改指定属性
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <param name="modifiedProNames">需要修改的属性</param>
        /// <returns>返回影响行数</returns>
        int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>返回影响行数</returns>
        int Delete(T entity);

        /// <summary>
        /// 删除集合数据
        /// </summary>
        /// <param name="list">实体模型集合</param>
        /// <returns>返回影响行数</returns>
        int DelBatch(List<T> list);

        /// <summary>
        /// 根据Lambda表达式获取数据
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns>返回模型实体集合</returns>
        List<T> GetListBy(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 根据Lambda表达式获取第一条数据
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns>模型实体</returns>
        T FirstOrDefault(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 根据Lambda表达式获取排序后的数据集合
        /// </summary>
        /// <typeparam name="TKey">排序的类型</typeparam>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <param name="orderLambda">Lambda表达式</param>
        /// <returns>返回实体模型集合</returns>
        List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda);

        /// <summary>
        /// 根据sql语句获取实体模型集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回实体集合</returns>
        List<T> SqlQuery(string sql);

        /// <summary>
        /// 根据sql语句获取字符串
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回字符串</returns>
        string GetString(string sql);

        /// <summary>
        /// 数据分页
        /// </summary>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">显示条数</param>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <param name="orderBy">Lambda表达式</param>
        /// <returns>返回实体模型集合</returns>
        List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda,
                                   Expression<Func<T, TKey>> orderBy);

        /// <summary>
        /// 数据分页
        /// </summary>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">显示条数</param>
        /// <param name="rowCount">总条数</param>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <param name="orderBy">Lambda表达式</param>
        /// <param name="isAsc">是否排序</param>
        /// <returns>返回实体模型集合</returns>
        List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda,
                                   Expression<Func<T, TKey>> orderBy, bool isAsc = true);

        /// <summary>
        ///执行sql语句 
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns>返回影响行数</returns>
        int ExcuteSql(string strSql, params object[] paras);

        /// <summary>
        /// 不采用跟踪获取实体模型
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns>返回实体模型</returns>
        T GetModelWithOutTrace(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 根据sql语句获取模型实体集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>返回模型实体集合</returns>
        List<T> GetModelWithSql(string sql, params object[] parameters);

        /// <summary>
        /// 表示针对 DbContext 的 LINQ to Entities 查询。
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns>返回LINQ to Entities 查询</returns>
        DbQuery<T> GetDbQuery(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 提供对数据类型已知的特定数据源的查询进行计算的功能。
        /// </summary>
        IQueryable<T> Table { get; }
    }
}
