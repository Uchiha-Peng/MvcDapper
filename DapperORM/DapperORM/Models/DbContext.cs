using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DapperORM.Models
{
    public class DbContext
    {
        public int Add(UserInfo model)
        {
            using (var conn = DapperConn.GetConnection())
            {
                string sql = "Insert into UserInfo (UserName,UserPwd,Email) Values(@UserName,@UserPwd,@Email)";
                return conn.Execute(sql, model);
            }
        }

        public int AddOne(UserInfo model)
        {
            using (var conn = DapperConn.GetConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserName", model.UserName);
                dp.Add("@UserPwd", model.UserPwd);
                dp.Add("@Email", model.Email);
                return conn.Execute("UserInfo", dp, null, null, CommandType.TableDirect);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(UserInfo model)
        {
            using (var conn = DapperConn.GetConnection())
            {
                string sql = "update UserInfo set UserName=@UserName,UserPwd=@UserPwd,Email=@Email where ID=@ID";
                return conn.Execute(sql, model);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Del(int ID)
        {
            using (var conn = DapperConn.GetConnection())
            {
                string sql = "Delete from UserInfo where ID="+ID;
                return conn.Execute(sql);
            }
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <returns></returns>
        public UserInfo GetModel(int ID)
        {
            using (var conn = DapperConn.GetConnection())
            {
                var sql = "Select ID,UserName,UserPwd,Email from UserInfo where ID="+ID;
                return conn.QueryFirstOrDefault<UserInfo>(sql);
            }
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetModels()
        {
            using (var conn = DapperConn.GetConnection())
            {
                var sql = "Select ID,UserName,UserPwd,Email from UserInfo";
                return conn.Query<UserInfo>(sql).ToList();
            }
        }

        /// <summary>
        /// 登录并添加Log
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="userLogModel"></param>
        public void ImplementAffair(UserInfo userModel, LoginLog userLogModel)
        {
            using (var conn = DapperConn.GetConnection())
            {
                IDbTransaction tran = conn.BeginTransaction();
                try
                {
                    string query = "Update User set Key='测试' where ID=@ID";//更新一条记录
                    conn.Execute(query, userModel, tran, null, null);

                    query = "insert into UserLoginLog (userId,CreateTime) value (@userId,@CreateTime)";//删除一条记录
                    conn.Execute(query, userLogModel, tran, null, null);

                    //提交
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    //提交错误
                    //回滚事务
                    tran.Rollback();
                }
            }
        }

        /// <summary>
        /// 执行无参数存储过程 返回列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UserInfo> ExecuteStoredProcedureNoParms()
        {
            using (IDbConnection con = DapperConn.GetConnection())
            {
                var userList = new List<UserInfo>();
                userList = con.Query<UserInfo>("QueryRoleNoParms",
                                        null,
                                        null,
                                        true,
                                        null,
                                        CommandType.StoredProcedure).ToList();
                return userList;
            }
        }

        /// <summary>
        /// 执行无参数存储过程 返回int
        /// </summary>
        /// <returns></returns>
        private int ExecutePROC()
        {
            using (IDbConnection con = DapperConn.GetConnection())
            {
                return con.Execute("QueryRoleWithParms", null, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ExecutePROC(UserInfo model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ID", "1");
            dp.Add("@msg", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection con = DapperConn.GetConnection())
            {
                con.Execute("Proc", dp, null, null, CommandType.StoredProcedure);
                string roleName = dp.Get<string>("@msg");
                return roleName;
            }
        }
    }
}