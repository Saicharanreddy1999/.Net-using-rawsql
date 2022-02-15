using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Drawing;

namespace Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Employee : ControllerBase
    {
        [HttpGet("GetEmployee")]
        public EmployeeData Get(int id)
        {
            EmployeeData e = new EmployeeData();
            string connectionString = "datasource=localhost;uid=sai;pwd=sai;database=employee;";
            string query = "SELECT EmpId,emp_name,dept_Id,Address,ST_X(Loc),ST_Y(Loc) FROM emp where EmpId=@id";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand command1 = new MySqlCommand(query, databaseConnection);
            command1.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader;
            MySqlDataReader reader1;

            command1.CommandTimeout = 1000;
            {
                databaseConnection.Open();
                reader1 = command1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {

                        e.EmpId = reader1.GetString(0);
                        e.EmpName = reader1.GetString(1);
                        e.DeptID = reader1.GetString(2);
                        e.Address = reader1.GetString(3);
                        Point p = new Point(reader1.GetInt32(4), reader1.GetInt32(5));
                        e.Loc = p;

                        return (e);
                    }
                }
                else
                {
                    //  return Content("No rows found.");
                }

                databaseConnection.Close();
            }
            //   catch (Exception ex)
            {
                //      return Content(ex.Message);
            }
            return (e);
        }
        [HttpGet("GetEmployeesusingGet")]
        public List<EmployeeData> GetEmployeesusingGet()
        {
            List<EmployeeData> en=new List<EmployeeData>();
            string query = "select EmpId from emp";
            string connectionString = "datasource=localhost;uid=sai;pwd=sai;database=employee;";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, databaseConnection);


            MySqlDataReader reader;
            databaseConnection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    en.Add(Get(reader.GetInt32(0)));
                }
                return en;
            }
            return en;

        }
        [HttpGet("GetEmployees")]
        public List<EmployeeData> GetEmployees()
        {
            List<EmployeeData> en = new List<EmployeeData>();
            string query = "select EmpId,emp_name,dept_id,Address,ST_X(Loc),ST_Y(Loc) from emp";
            string connectionString = "datasource=localhost;uid=sai;pwd=sai;database=employee;";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, databaseConnection);
            MySqlDataReader reader;
            databaseConnection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                
                while (reader.Read())
                {
                    EmployeeData e = new EmployeeData();

                    e.EmpId = reader.GetString(0);
                    e.EmpName = reader.GetString(1);
                    e.DeptID = reader.GetString(2);
                    e.Address = reader.GetString(3);
                    int X= reader.GetInt32(4);
                    int Y = reader.GetInt32(5);
                    Point p=new Point(X,Y);
                    e.Loc = p;
                    en.Add(e);
                }
            }
            return en;
        }
        [HttpGet("FindEmployeesinRangeusingEmployeeId")]
        public List<EmployeeData> Find(int Id, int ran)
        {
            List<EmployeeData> en = new List<EmployeeData>();
            string connectionString = "datasource=localhost;uid=sai;pwd=sai;database=employee;";
            string query1 = "select ST_X(Loc),ST_Y(Loc) from emp where EmpId=@Id";
            string query2 = "select EmpId,emp_name,dept_id,Address,ST_X(Loc),ST_Y(Loc) from emp where not EmpId=@Id";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);

            MySqlCommand command1 = new MySqlCommand(query1, databaseConnection);
            MySqlCommand command2 = new MySqlCommand(query2, databaseConnection);
            command1.Parameters.AddWithValue("@Id",Id);
            command2.Parameters.AddWithValue("@Id",Id);

            MySqlDataReader reader1;
            MySqlDataReader reader2;
            
            databaseConnection.Open();
            reader1=command1.ExecuteReader();
            reader1.Read();
            int EX = reader1.GetInt32(0);
            int EY = reader1.GetInt32(1);
            reader1.Close();

            reader2=command2.ExecuteReader();

            if (reader2.HasRows)
            {
                while (reader2.Read())
                {

                    int X = reader2.GetInt32(4);
                    int Y = reader2.GetInt32(5);
                    if (Math.Sqrt((Math.Pow((EX - X), 2) + Math.Pow((EY - Y), 2))) <= ran)
                    {
                        EmployeeData e=new EmployeeData();
                        e.EmpId = reader2.GetString(0);
                        e.EmpName = reader2.GetString(1);
                        e.DeptID = reader2.GetString(2);
                        e.Address = reader2.GetString(3);
                        Point p=new Point(X,Y);
                        e.Loc = p;
                        en.Add(e);
                    }
                }
                
            }
            return en;
        }
        [HttpGet("GetEmployeesInRangeusingLatLong")]
        public List<EmployeeData> GetEmployeesInRange(int lati, int longi, int radius)
        {
            List<EmployeeData> en = new List<EmployeeData>();
            string connectionString = "datasource=localhost;uid=sai;pwd=sai;database=employee;";
            string query= "select EmpId,emp_name,dept_id,Address,ST_X(Loc),ST_Y(Loc) from emp";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, databaseConnection);
            MySqlDataReader reader;
            databaseConnection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int X = reader.GetInt32(4);
                    int Y = reader.GetInt32(5);
                    if (Math.Sqrt((Math.Pow((lati - X), 2) + Math.Pow((longi - Y), 2))) <= radius)
                    {
                        EmployeeData e = new EmployeeData();
                        e.EmpId = reader.GetString(0);
                        e.EmpName = reader.GetString(1);
                        e.DeptID = reader.GetString(2);
                        e.Address = reader.GetString(3);
                        Point p = new Point(X, Y);
                        e.Loc = p;
                        en.Add(e);
                    }
                }
            }
            return en;



        }
    }
}