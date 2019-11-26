using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
//using Newtonsoft.Json;

namespace MainSite.Models
{
    public class Database
    {
        public static SqlConnection con;
        private static void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            con = new SqlConnection(constr);
        }
        public static void SaveData(GetData P)
        {
            //
            // Select Statment
            // P.State =  

            connection();
            SqlCommand cmd = new SqlCommand("save_data", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TickerName", P.TickerName.ToUpper());
            cmd.Parameters.AddWithValue("@Date", P.Date);
            cmd.Parameters.AddWithValue("@StartTime", P.StartTime);
            cmd.Parameters.AddWithValue("@EndTime", P.EndTime);
            cmd.Parameters.AddWithValue("@Quantity", P.Quantity);
            cmd.Parameters.AddWithValue("@StartPrice", P.StartPrice);
            cmd.Parameters.AddWithValue("@EndPrice", P.EndPrice);
            cmd.Parameters.AddWithValue("@BuySell", P.BuySell);
            cmd.Parameters.AddWithValue("@Profit", P.Profit);
            cmd.Parameters.AddWithValue("@Comments", P.Comments);
            cmd.Parameters.AddWithValue("@Post_Date", P.post_date);
            cmd.Parameters.AddWithValue("@ChartImage", P.chartImage);
            cmd.Parameters.AddWithValue("@TotalViews", 0);
            int row = cmd.ExecuteNonQuery();
            con.Close();

        }
        public static void EditData(GetData P)
        {
            //
            // Select Statment
            // P.State =  

            connection();
            SqlCommand cmd = new SqlCommand("edit_data", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(P.ID));
            cmd.Parameters.AddWithValue("@TickerName", P.TickerName.ToUpper());
            cmd.Parameters.AddWithValue("@Date", P.Date);
            cmd.Parameters.AddWithValue("@StartTime", P.StartTime);
            cmd.Parameters.AddWithValue("@EndTime", P.EndTime);
            cmd.Parameters.AddWithValue("@Quantity", P.Quantity);
            cmd.Parameters.AddWithValue("@StartPrice", P.StartPrice);
            cmd.Parameters.AddWithValue("@EndPrice", P.EndPrice);
            cmd.Parameters.AddWithValue("@BuySell", P.BuySell);
            cmd.Parameters.AddWithValue("@Profit", P.Profit);
            cmd.Parameters.AddWithValue("@Comments", P.Comments);
            cmd.Parameters.AddWithValue("@Post_Date", P.post_date);
            cmd.Parameters.AddWithValue("@ChartImage", P.chartImage);
            cmd.Parameters.AddWithValue("@TotalViews", P.TotalViews);
            int row = cmd.ExecuteNonQuery();
            con.Close();

        }
        public static List<GetData> ShowData()
        {
            List<GetData> data = new List<GetData>();
            connection();
            SqlCommand cmd = new SqlCommand("show_data", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    GetData p = new GetData
                    {
                        ID = Convert.ToString(dr["ID"]),
                        TickerName = Convert.ToString(dr["TickerName"]),
                        Date = Convert.ToString(dr["Date"]),
                        StartTime = Convert.ToString(dr["StartTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        StartPrice = float.Parse(Convert.ToString(dr["StartPrice"])),
                        EndPrice = float.Parse(Convert.ToString(dr["EndPrice"])),
                        BuySell = Convert.ToString(dr["BuySell"]),
                        Profit = Convert.ToString(dr["Profit"]),
                        Quantity = float.Parse(Convert.ToString(dr["Quantity"])),
                        Comments = Convert.ToString(dr["Comments"]),
                        post_date = Convert.ToString(dr["Post_Date"]),
                        chartImage = Convert.ToString(dr["ChartImage"]),
                        TotalViews = Convert.ToInt32(dr["TotalViews"]),
                        Believe = Convert.ToInt32(dr["Believe"]),
                        Likes = Convert.ToInt32(dr["Likes"]),
                        Dislikes = Convert.ToInt32(dr["Dislikes"])
                    };
                    data.Add(p);
                }
            }

            con.Close();
            return data;

        }
        public static UserData UserData(List<GetData> l)
        {
            l = l.OrderBy(x => Convert.ToDateTime(x.EndTime)).OrderBy(x =>  x.Date).ToList();
            UserData u = new UserData();
            //float avg = 100;
            u.total_trd = l.Count.ToString();
            u.win_trd = 0;
            u.lose_trd = 0;
            u.largest_winning = 0;
            u.largest_losing = 0;
            u.gross_profit = 0;
            u.gross_loss = 0;
            u.consecutive_win = 0;
            u.consecutive_lose = 0;            
            int tempwin = 0;
            int templose = 0;
            foreach (GetData i in l)
            {
                
                float profit = float.Parse(i.Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                //float pct = float.Parse((100 * Convert.ToInt32(i.Profit.Split(' ').ToArray()[0].Trim('(', ')')) / (i.Quantity * i.StartPrice)).ToString("0.00"));
                //avg = avg + Convert.ToInt32(i.Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                if (i.Profit.Split(' ').ToArray()[1] == "Profit")
                {
                     
                    u.win_trd += 1;
                    if (profit >= u.largest_winning)
                    {
                        u.largest_winning = profit;
                    }

                    u.gross_profit += profit;

                }
                else
                {
                    u.lose_trd += 1;
                    if (profit <= u.largest_losing)
                    {
                        u.largest_losing = profit;
                    }
                    u.gross_loss += profit;
                }
                var o=i.Profit.Split(' ').ToArray()[0];
                var p = o.Trim('(', ')');
                u.tot_gain = u.tot_gain +float.Parse(i.Profit.Split(' ').ToArray()[0].Trim('(', ')'));

                if(i.Profit.Split(' ').ToArray()[1] == "Profit")
                {
                    templose = 0;
                    tempwin += 1;
                    if(u.consecutive_win < tempwin)
                    {
                        u.consecutive_win = tempwin;
                    }
                }
                else
                {
                    tempwin = 0;
                    templose += 1;
                    if (u.consecutive_lose < templose)
                    {
                        u.consecutive_lose = templose;
                    }
                }

            }
            float a = float.Parse(u.win_trd.ToString()) / float.Parse(u.total_trd);
            u.win_pct = float.Parse((a * 100).ToString("0.00"));
            u.avg_gainn = float.Parse((u.gross_profit/u.win_trd).ToString("0.00"));
            u.avg_gain = float.Parse((u.tot_gain / Convert.ToInt32(u.total_trd)).ToString("0.00"));
            u.tot_gain = float.Parse(u.tot_gain.ToString("0.00"));

            

            return u;
        }
        public static void IncreaseCount(string id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("increase_count", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            int row = cmd.ExecuteNonQuery();
            con.Close();
        }

        public static string checkComment(string cmt)
        {
            string s = cmt == null ? "*No Description Available*" : cmt.ToString();
            return (s);
        }
        public static string checkImage(string id)
        {
            List<GetData> datas = ShowData();
            if (datas.Any())
            {
                GetData data = datas.Select(x => x).Where(x => x.ID == id).ToList()[0];
                return (data.chartImage);
            }
            return ("1.jpg");
        }
        public static void DeleteData(string id)
        {
            //List<GetData> datas = Database.ShowData();
            //GetData P = datas.Select(x => x).Where(x => x.ID == id).ToList()[0];
            connection();
            SqlCommand cmd = new SqlCommand("delete_data", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));           
            int row = cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void Believe(string id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("add_believe", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            int row = cmd.ExecuteNonQuery();
            con.Close();

        }
        public static void Like(string id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("add_like", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            int row = cmd.ExecuteNonQuery();
            con.Close();

        }
        public static void Dislike(string id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("add_dislike", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            int row = cmd.ExecuteNonQuery();
            con.Close();

        }
        public static List<string> Datapoints()
        {
            
            float price = 0;
            int i = 0;
            List<datapoints> d = new List<datapoints>();
            List<string> dp = new List<string>();
            List<GetData> l = ShowData().OrderBy(x => x.Date).ToList();
            foreach(GetData g in l)
            {
                i = i + 1;
                price = price + float.Parse(g.Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                string date = g.Date;
               // d.Add(new datapoints(1,1));
                dp.Add("{ x : " + i + ", y : "+ price +" },");

            }
            
            return (dp); 
        }
    }
}