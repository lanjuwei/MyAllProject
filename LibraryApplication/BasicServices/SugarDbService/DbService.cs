using Model;
using SqlSugar;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;

namespace BasicServices.SugarDbService
{
    /// <summary>
    /// 数据库服务 该服务只能new 详细用法参照官网 http://www.codeisbug.com/Doc/8/1165
    /// </summary>
    public class DbService
    {
        /// <summary>
        /// 全部的表初始化都放在这里 表的数组 用于生产多个表
        /// </summary>
        private static Type[] _myAllTables = new Type[] 
        {
            typeof(FaceUserModel)
        };

        /// <summary>
        /// sugar对象每次必须new 用于各种数据库操作
        /// </summary>
        public static SqlSugarClient GetDbService()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=.;uid=.;pwd=@12345678l;Data Source=Db\\Database.sqlite",
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
            });
            //调式代码 用来打印SQL 
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return db;
        }

        #region 程序启动调用一次即可
        
        /// <summary>
        /// 初始化数据库并创建表
        /// </summary>
        public static void InitDb()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Db\\Database.sqlite";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Db");
            }
            SQLiteConnection.CreateFile(path);//创建数据库文件
            var dbConnection = new SQLiteConnection($"Data Source=Db\\Database.sqlite;Version=3");//连接数据库
            dbConnection?.SetPassword("12345678l");//修改数据库密码
            dbConnection?.Dispose();//释放

            //用orm来连接数据库 舍弃原始的sqlite的连接操作语句
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=.;uid=.;pwd=@12345678l;Data Source=Db\\Database.sqlite",
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
            });
            db.CodeFirst.InitTables(_myAllTables);
        }
        #endregion


    }
}