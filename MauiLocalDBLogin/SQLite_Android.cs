using SQLite;
using static SQLite.SQLite3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiLocalDBLogin
{
    public class SQLite_Android 
    {
        SQLiteConnection database;

        public SQLite_Android()
        {
            database = new SQLiteConnection(DbConnection.DBPath, DbConnection.flags);
            database.CreateTable<User>();
        }
        public bool DeleteEmployee(int id)
        {
            bool res = false;
            try
            {
                string sql = $"Delete from User where id={id}";
                database.Execute(sql);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
            
        }

        public List<User> GetEmployees(User user)
        {
            string sql = $"Select * from User Where UserId ='{user.UserId}' and password='{user.Password}'";
            List<User> employees = database.Query<User>(sql);
            return employees;
        }

        public User GetUser(User user)
        {
            //bool res = false;
            try
            {
                //     string sql = $"Select * from User Where UserId='{user.UserId}' and Password='{user.Password}'";
                //    int temp = database.Execute(sql);


                //    if (temp >= 0)
                //    {
                //        res = true;
                //    }
                //    //database.Update(user);
                //    else
                //    {
                //        res = false;
                //    }

                //}
                //catch (Exception ex)
                //{
                //    res = false;
                //}

                return database.Table<User>().Where(x => x.UserId == user.UserId && x.Password == user.Password).SingleOrDefault();

            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        public bool SaveEmployee(User user)
        {
            bool res = false;
            try
            {
                database.Insert(user);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        public bool UpdateEmployee(User user)
        {
            bool res = false;
            try
            {
                // string sql = $"Update Employee set Name='{employee.Name}', Address='{employee.Address}',PhoneNumber='{employee.PhoneNumber}',Email='{employee.Email}', myArray='{employee.myArray}' Where id={employee.id}";
                // database.Execute(sql);
                database.Update(user);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
    }
}
