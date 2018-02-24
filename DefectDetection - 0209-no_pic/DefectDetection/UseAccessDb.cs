using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace DefectDetection
{
    public class UseSqlDb
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        public UseSqlDb()
        {
             
        }
        public bool openDb()
        {
            string strConnection = "Server=.;";//localhost
            if (!System.IO.Directory.Exists(@"D:\SqlServer2008\DataBase"))
            {
                System.IO.Directory.CreateDirectory(@"D:\SqlServer2008\DataBase");
            }
            if (File.Exists(@"D:\SqlServer2008\DataBase\ServerGlassInfo1_data.mdf"))
            {
                //strConnection += "initial catalog=ServerGlassInfo;";
                //strConnection += "integrated security=SSPI";

                strConnection += "initial catalog=ServerGlassInfo;integrated security=SSPI";
                con.ConnectionString = strConnection;

                try
                {
                    con.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                createDb();
                return false;
            }
        }
        public bool createDb()
        {
            string sql = "";
            string strConnection = "Server=.;";
            strConnection += "integrated security=SSPI";
            con.ConnectionString = strConnection;
            con.Open();
            cmd.Connection = con;

            sql = "Create database ServerGlassInfo On primary (" +
            "name=student_data, " +
            "filename='D:\\SqlServer2008\\DataBase\\ServerGlassInfo1_data.mdf'," +
            "size=3,maxsize=unlimited,filegrowth=1)" +
             "Log on  ( " +
             "name=GlassInfo_log," +
             "filename='D:\\SqlServer2008\\DataBase\\ServerGlassInfo1_log.ldf'," +
             "size=1, maxsize=20,filegrowth=10%)";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            int i = cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Dispose();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;    
        }
        public bool createGlassTable()
        {
           
            string sql = "";
            if (openDb())
            {
                cmd.Connection = con;

                sql = " Create table Serverglass_info(" +
                      "glass_id int IDENTITY(1,1) primary key not null," +
                      " glass_Length varchar(10)not null," +
                      "glass_width varchar(10)not null," +
                      "glass_thickness varchar(10)not null," +
                      "glass_quality int not null," +
                      "edge_defect_count int not null," +
                      "surface_defect_count int not null,);";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Dispose();
                con.Close();
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            else
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
                return false;
            }
        }
        public bool createSurfaceTable()
        {
           
            string sql = "";
            if (openDb())
            {
                cmd.Connection = con;
                sql = "Create table surface_Eigenvalues_info(" +
                "glass_time datetime not null," +
                "glass_id varchar(10)not null," +
                "surface_defect_id int IDENTITY(1,1) primary key not null," +
                "surface_defect_type nvarchar(20)not null," +
                "channel nvarchar(10)not null," +
                "region_area varchar(10)not null," +
                "region_row varchar(10)not null," +
                "region_cloumn varchar(10)not null," +
                "region_width varchar(10)not null," +
                "region_height varchar(10)not null," +
                "region_circularity varchar(10)not null," +
                "region_compactness varchar(10)not null," +
                "region_contlength varchar(10)not null," +
                "region_convexity varchar(10)not null," +
                "region_rectangularity varchar(10)not null," +
                "region_ra varchar(10)not null," +
                "region_rb varchar(10)not null," +
                "region_anisometry varchar(10)not null," +
                "region_bulkiness varchar(10)not null," +
                "region_struct_factor varchar(10)not null," +
                "region_outer_radius varchar(10)not null," +
                "region_inner_width varchar(10)not null," +
                "region_inner_radius varchar(10)not null," +
                "region_inner_height varchar(10)not null," +
                "region_dist_mean varchar(10)not null," +
                "region_dist_deviation varchar(10)not null," +
                "region_roundness varchar(10)not null," +
                "region_num_sides varchar(10)not null," +
                "region_connect_num varchar(10)not null," +
                "region_holes_num varchar(10)not null," +
                "region_area_holes varchar(10)not null," +
                "region_max_diameter varchar(10)not null," +
                "region_rect2_len1 varchar(10)not null," +
                "region_rect2_len2 varchar(10)not null," +
                "gray_area varchar(10)not null," +
                "gray_row varchar(10)not null," +
                "gray_column varchar(10)not null," +
                "gray_ra varchar(10)not null," +
                "gray_rb varchar(10)not null," +
                "gray_min varchar(10)not null," +
                "gray_max varchar(10)not null," +
                "gray_mean varchar(10)not null," +
                "gray_deviation varchar(10)not null," +
                "gray_plane_deviation varchar(10)not null," +
                "gray_anisotropy varchar(10)not null," +
                "gray_entropy varchar(10)not null,);";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Dispose();
                con.Close();
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            else
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
                return false;
            }
        }

        public bool createBrokenEdgeTable()
        {
            string sql = "";
            if (openDb())
            {
                cmd.Connection = con;

                sql = "Create table broken_edge_value_info(" +
                     "glass_id varchar(10)not null," +
                     "broken_edge_defect_id int IDENTITY(1,1) primary key not null," +
                     "brokenedge_xcnt varchar(10)not null," +
                     "brokenedge_ycnt varchar(10)not null," +
                     "brokenedge_width varchar(10)not null," +
                     "brokenedge_lenght varchar(10)not null,);";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Dispose();
                con.Close();
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            else
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
                return false;
            }
        }
        /// <summary>
        ///   创建截边信息值表
        /// </summary>
        public bool createCutEdgeTable()
        {
             string sql = "";
            if (openDb())
            {
                cmd.Connection = con;
                sql = " Create table cut_edge_value_info(" +
                    "glass_id varchar(10)not null," +
                    "cut_edge_defect_id int IDENTITY(1,1) primary key not null," +
                    "cutedge_xs varchar(10)not null," +
                    "cutedge_xe varchar(10)not null," +
                    "cutedge_ys varchar(10)not null," +
                    "cutedge_ye varchar(10)not null," +
                    "cutedge_x_lenght varchar(10)not null," +
                    "cutedge_y_lenght varchar(10)not null," +
                    "cutedge_lenght varchar(10)not null," +
                    "cutedge_position varchar(10)not null,);";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Dispose();
                con.Close();
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            else
            {
                cmd.Dispose();
                con.Dispose();
                con.Close();
                return false;
            }
        }
        public bool insertDb(string tableName,string cloumnName,string data)
        {
                string sql = "";
                string str = "";
                if (openDb())
                {
                    cmd.Connection = con;
                    //insert into Table_1 (学号,姓名)values (555,'kk');
                    sql = "insert into ";
                    str = tableName;
                    sql += str + "(";
                    str = cloumnName;
                    sql += str + ")";
                    //   str = " ' "+ data +" '";
                    str = data;
                    sql += " values (" + str + ")";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Dispose();
                    con.Close();
                    if (i >= 1)
                        return true;
                    else
                        return false;
                   
                }
                else
                {
                    cmd.Dispose();
                    con.Dispose();
                    con.Close();
                    return false;
                }
               
             
        }
        public void deleteDb(string tableName, string strCondition)
        {
                string sql = "";
                string str = "";
                if (openDb())
                {
                    cmd.Connection = con;
                    //DELETE FROM [Table_1] where 学号 = 1;
                    sql = "DELETE FROM ";
                    str = tableName;
                    sql += str;
                    sql += " where " + strCondition;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Dispose();
                    con.Close();
                }
                        
        }
        public void updateDb(string tableName,string updateData,string condition)
        {
             
                string sql = "";
                string str = "";
                if (openDb())
                {
                    cmd.Connection = con;
                    //UPDATE [Table_1] set 学号 = 6  where 学号 = 2;
                    sql = "UPDATE ";
                    str = tableName;
                    sql += str + " set ";
                    //Console.WriteLine("请输入要修改的数据，如money=0,多个用逗号隔开");
                    sql += updateData;
                    //  Console.WriteLine("请输入要修改的条件");
                    sql += " where " + condition;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
        }
        public void search(string cloumnName,string tableName)
        {
          
            string sql = "";
            if (openDb())
            {
                cmd.Connection = con;
                //select 姓名 from Table_1 
                sql = "select ";
                // Console.WriteLine("请输入你要查询的列名，多个用逗号隔开");
                sql += cloumnName;
                // Console.WriteLine("请输入你要查询的表名，多个用逗号隔开");
                sql += " from " + tableName;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
                        
        }

        public void close()
        {
            con.Dispose();
            con.Close();
          
        }
                   
     
    }   
}

