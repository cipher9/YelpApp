using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace YelpApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string address { get; set; }
            public double review_rating { get; set; }
            public int postal_code { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public double stars { get; set; }
            public int review_count { get; set; }
            public int num_checkins { get; set; }
            public bool is_open { get; set; }
            public double distance { get; set; }
            public static string sort_option { get; set; }
        }

        public class YelpUser
        {
            public double average_stars { get; set; }
            public int useful { get; set; }
            public string user_id { get; set; }
            public int cool { get; set; }
            public string name { get; set; }
            public int funny { get; set; }
            public string yelping_since { get; set; }
            public static string current_user_id { get; set; }
            public static double user_latitude { get; set; }
            public static double user_longitude { get; set; }
        }

        public class Reviews
        {
            public string review_date { get; set; }
            public string name { get; set; }
            public double stars { get; set; }
            public string review_text { get; set; }
            public int funny { get; set; }
            public int useful { get; set; }
            public int cool { get; set; }
        }

        public class Reviewer
        {
            public string user_name { get; set; }
            public string business_name { get; set; }
            public string city { get; set; }
            public string review_text { get; set; }
        }

        public class Favorite
        {
            public string favorite_name { get; set; }
            public double favorite_stars { get; set; }
            public string favorite_city { get; set; }
            public int favorite_postal_code { get; set; }
            public string favorite_address { get; set; }
            public string favorite_business_id { get; set; }
        }

        public class Distance
        {
            public static double user_lat { get; set; }
            public static double user_long { get; set; }
            public static double business_lat { get; set; }
            public static double business_long { get; set; }
            public static double actual_distance { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns1Grid();
            addColumns2Grid();
            addColumns3Grid();
            addColumns4Grid();
        }
       
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb; password=1234";     // add password 
        }

        private void addState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            stateList.Items.Add(reader.GetString(0));
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // Sort options combobox
        private void sortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (sortCB.SelectedIndex > -1)
            {
                Business.sort_option = "name";
                if (sortCB.SelectedIndex.Equals(1))
                    Business.sort_option = "stars";
                else if (sortCB.SelectedIndex.Equals(2))
                    Business.sort_option = "review_count";
                else if (sortCB.SelectedIndex.Equals(3))
                    Business.sort_option = "review_rating";
                else if (sortCB.SelectedIndex.Equals(4))
                    Business.sort_option = "num_checkins";
                else if (sortCB.SelectedIndex.Equals(5))
                    Business.sort_option = "name";
                searchButton_Click(sender, e);
            }
        }

        // build columns for the business datagrid
        private void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 200;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("address");
            col2.Header = "Address";
            col2.Width = 225;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 100;
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("state");
            col4.Header = "State";
            col4.Width = 50;
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("distance");
            col5.Header = "Distance (miles)";
            col5.Width = 100;
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("stars");
            col6.Header = "Stars";
            col6.Width = 100;
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("review_count");
            col7.Header = "# of Reviews";
            col7.Width = 100;
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("review_rating");
            col8.Header = "Avg Review Rating";
            col8.Width = 100;
            businessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Binding = new Binding("num_checkins");
            col9.Header = "Total Checkins";
            col9.Width = 100;
            businessGrid.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn();
            col10.Binding = new Binding("bid");
            col10.Header = "";
            col10.Width = 0;
            businessGrid.Columns.Add(col10);
        }

        

        // executes the query and fails nicely
        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            myf(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addCity(NpgsqlDataReader R)
        {
            cityListBox.Items.Add(R.GetString(0));
        }

        // when a state is selected the cityListBox will populate with cities from selected state
        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityListBox.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT DISTINCT city FROM Business WHERE state = '" + stateList.SelectedItem.ToString() + "' ORDER BY city";
                executeQuery(sqlStr, addCity);
            }
        }

        private void addZipcode(NpgsqlDataReader R)
        {
            zipcodeListBox.Items.Add(R.GetInt32(0));
        }

        // when a city is selected the zipcodeListBox will populate with zipcodes from selected city
        private void cityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipcodeListBox.Items.Clear();
            if (cityListBox.SelectedIndex > -1)
            {
                string sqlStr = "SELECT DISTINCT postal_code FROM Business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityListBox.SelectedItem.ToString() + "' ORDER BY postal_code;";
                executeQuery(sqlStr, addZipcode);
            }
        }

        private void addCategories(NpgsqlDataReader R)
        {
            businessCatLB.Items.Add(R.GetString(0));
        }

        // when zipcode is selected the businessCatLB will populate with the business categories from that zipcode
        private void zipcodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessCatLB.Items.Clear();
            if (zipcodeListBox.SelectedIndex > -1)
            {
                string sqlStr = "SELECT DISTINCT category_name FROM Business, Categories WHERE state = '" + stateList.SelectedItem.ToString() + "' and city = '" + cityListBox.SelectedItem.ToString() + 
                                "' AND postal_code = '" + zipcodeListBox.SelectedItem.ToString() + "' AND business.business_id = categories.business_id  ORDER BY category_name;";
                executeQuery(sqlStr, addCategories);
            }
        }

        private void addBusinessLocation(NpgsqlDataReader R)
        {
            Distance.business_lat = R.GetDouble(0);
            Distance.business_long = R.GetDouble(1);
        }

        private void addUserLocation(NpgsqlDataReader R)
        {
            if (R.IsDBNull(0))
                Distance.user_lat = 0;
            else
                Distance.user_lat = R.GetDouble(0);
            if (R.IsDBNull(1))
                Distance.user_long = 0;
            else
                Distance.user_long = R.GetDouble(1);
        }

        private void calculateDistance(NpgsqlDataReader R)
        {
            Distance.actual_distance = Math.Round(R.GetDouble(0), 2);
        }

        private void addGridRow(NpgsqlDataReader R)
        {
            string sqlStr1 = "SELECT latitude, longitude FROM Business WHERE business_id = '" + R.GetString(8) + "';";
            executeQuery(sqlStr1, addBusinessLocation);
            string sqlStr2 = "SELECT latitude, longitude FROM yelpUser WHERE user_id = '" + YelpUser.current_user_id + "';";
            executeQuery(sqlStr2, addUserLocation);
            string sqlStr3 = "SELECT myDistance('" + Distance.user_lat + "','" + Distance.user_long + "','" + Distance.business_lat + "','" + Distance.business_long + "');";
            executeQuery(sqlStr3, calculateDistance);
            businessGrid.Items.Add(new Business() { name = R.GetString(0), address = R.GetString(1), city = R.GetString(2), state = R.GetString(3), distance = Distance.actual_distance, stars = R.GetDouble(4), 
                                                    review_count = R.GetInt32(5), review_rating = Math.Round(R.GetDouble(6), 2), num_checkins = R.GetInt32(7), bid = R.GetString(8) });
            int numB = businessGrid.Items.Count;
            numBus.Text = numB.ToString();
        }

        // need to add descending order for highest rating and most checkin
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            Business B = new Business();
            businessGrid.Items.Clear();
            if (zipcodeListBox.SelectedIndex > -1 && businessCatLB.SelectedIndex > -1)
            {
                var selectedItems = new List<string>();
                foreach (var item in searchCatLB.Items)
                    selectedItems.Add(item.ToString());
                StringBuilder str = new StringBuilder();

                str.Append("SELECT name, address, city, state, stars, review_count, review_rating, num_checkins, business_id FROM business WHERE business.business_id IN (SELECT business_id FROM categories WHERE state = '" + stateList.SelectedItem.ToString() +
                 "' AND city = '" + cityListBox.SelectedItem.ToString() + "' AND postal_code = '" + zipcodeListBox.SelectedItem.ToString() + "' AND category_name = '" + selectedItems[0] + "') ");
                for (int i = 1; i < selectedItems.Count; i++)
                {
                    str.Append("AND business.business_id IN (SELECT business_id FROM categories WHERE category_name = '" + selectedItems[i] + "') ");
                }
                string sort = Business.sort_option.ToString();
                str.Append(" ORDER BY " + sort.ToString() + ";'");
                string sqlStr1 = str.ToString();
                executeQuery(sqlStr1, addGridRow);
            }
            else if (zipcodeListBox.SelectedIndex > -1)
            {
                string sqlStr = "SELECT name, address, city, state, stars, review_count, review_rating, num_checkins, business_id FROM Business WHERE state = '" + stateList.SelectedItem.ToString() +
                                "' AND city = '" + cityListBox.SelectedItem.ToString() + "' AND postal_code = '" + zipcodeListBox.SelectedItem.ToString() + "' ORDER BY name;";
                executeQuery(sqlStr, addGridRow);
            }
            
        }

        // It works
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in businessCatLB.SelectedItems)
            {
                if (!searchCatLB.Items.Contains(item))
                    searchCatLB.Items.Add(item);
                else
                    break;
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < searchCatLB.SelectedItems.Count; i++)
                searchCatLB.Items.Remove(searchCatLB.SelectedItems[i]);
        }

        private void addBusinessName(NpgsqlDataReader R)
        {
            busNameTB.Text = R.GetString(0);
            addressTB.Text = R.GetString(1) + ", " + R.GetString(2) + ", " + R.GetString(3);
            hoursTB.Text = String.Empty;
            
            hoursTB.Text = "Today (" + DateTime.Today.DayOfWeek.ToString() + ")";
            hoursTB.Text += "\n Opens: " + R.GetTimeSpan(4).ToString();
            hoursTB.Text += " Closes: " + R.GetTimeSpan(5).ToString();
        }

        private void addBusinessCategory(NpgsqlDataReader R)
        {
            selectedBusCatLB.Items.Add(R.GetString(0));
        }

        private void businessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    //busNameTB addressTB hoursTB
                    object item = businessGrid.SelectedItem;
                    string ID = (businessGrid.SelectedCells[9].Column.GetCellContent(item) as TextBlock).Text;
                    string sqlStr1 = "SELECT DISTINCT name, address, city, state, open_time, close_time FROM Business, Hours WHERE Business.business_id = Hours.business_id AND Hours.business_id = '" + ID + "';";
                    executeQuery(sqlStr1, addBusinessName);
                    string sqlStr2 = "SELECT DISTINCT category_name " +
                                    "FROM Categories " +
                                    "WHERE business_id = '" + B.bid.ToString() + "';";
                    executeQuery(sqlStr2, addBusinessCategory);
                }
            }
        }

        private void showRevButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    BusinessDetails businessWindow = new BusinessDetails(B.bid.ToString());
                    businessWindow.Show();
                }
            }
        }

        private void addFavorite(NpgsqlDataReader R)
        {

        }

        private void addToFavButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    string sqlStr = "INSERT INTO Favorites (user_id, business_id) " +
                                    "VALUES ('" + YelpUser.current_user_id + "','" + B.bid.ToString() + "');";
                    executeQuery(sqlStr, addFavorite);
                    updateFavorites();
                }
            }
        }
        private void businessCatLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //-------------------------------------User Information Tab------------------------------------------------------//
        // Friends datagrid
        private void addColumns1Grid()
        {
            // table yelpUser
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "User Name";
            col1.Width = 150;
            friendsDG.Columns.Add(col1);

            // table yelpUser
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("average_stars");
            col2.Header = "Avg Stars";
            col2.Width = 90;
            friendsDG.Columns.Add(col2);

            // table yelpUser
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("yelping_since");
            col3.Header = "Yelping Since";
            col3.Width = 150;
            friendsDG.Columns.Add(col3);
        }

        // What are my friends reviewing? datagrid
        private void addColumns3Grid()
        {
            // table Friendship
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("user_name");
            col1.Header = "User Name";
            col1.Width = 90;
            friendRevDG.Columns.Add(col1);

            // table Business
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("business_name");
            col2.Header = "Business";
            col2.Width = 150;
            friendRevDG.Columns.Add(col2);

            // table Business
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 100;
            friendRevDG.Columns.Add(col3);

            // table Review
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("review_text");
            col4.Header = "Text";
            col4.Width = 400;
            friendRevDG.Columns.Add(col4);
        }

        // Favorite Businesses datagrid
        private void addColumns4Grid()
        {
            // table Business
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("favorite_name");
            col1.Header = "Business Name";
            col1.Width = 90;
            favoriteBusDG.Columns.Add(col1);

            // table Review
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("favorite_stars");
            col2.Header = "Stars";
            col2.Width = 45;
            favoriteBusDG.Columns.Add(col2);

            // table Business
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("favorite_city");
            col3.Header = "City";
            col3.Width = 80;
            favoriteBusDG.Columns.Add(col3);

            // table Business
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("favorite_postal_code");
            col4.Header = "Zipcode";
            col4.Width = 80;
            favoriteBusDG.Columns.Add(col4);

            // table Business
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("favorite_address");
            col5.Header = "Address";
            col5.Width = 400;
            favoriteBusDG.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("favorite_business_id");
            col6.Header = "bid";
            col6.Width = 0;
            favoriteBusDG.Columns.Add(col6);
        }

        private void addUserID(NpgsqlDataReader R)
        {
            userIDLB.Items.Add(R.GetString(0));
        }

        private void userNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            userIDLB.Items.Clear();
            string sqlStr = "SELECT user_id FROM yelpUser WHERE name = '" + userNameTB.Text.ToString() + "';";
            executeQuery(sqlStr, addUserID);
        }
        
        // What are my friends reviewing? datagrid on User Information tab
        private void populateFriendsRev(NpgsqlDataReader R)
        {
            friendRevDG.Items.Add( new Reviewer()
            {
                user_name = R.GetString(0),
                business_name = R.GetString(1),
                city = R.GetString(2),
                review_text = R.GetString(3)
            });
        }

        // Friends datagrid on User Information tab
        private void populateFriends(NpgsqlDataReader R)
        {
            friendsDG.Items.Add(new YelpUser()
            {
                name = R.GetString(0),
                average_stars = R.GetDouble(1),
                yelping_since = R.GetDate(2).ToString()
            });
        }

        // User Information textboxes on User Information tab
        private void populateUserInformation(NpgsqlDataReader R)
        {
            userNameUI.Text = R.GetString(0);
            starsTB.Text = R.GetDouble(1).ToString();
            yelpSinceTB.Text = R.GetDate(2).ToString();
            funnyTB.Text = R.GetInt32(3).ToString();
            coolTB.Text = R.GetInt32(4).ToString();
            usefulTB.Text = R.GetInt32(5).ToString();
            if (R.IsDBNull(6))
                latTB.Text = "0";
            else
                latTB.Text = R.GetDouble(6).ToString();
            if (R.IsDBNull(7))
                longTB.Text = "0";
            else
                longTB.Text = R.GetDouble(7).ToString();
        }

        private void populateFavorites(NpgsqlDataReader R)
        {
            while(R.Read())
                favoriteBusDG.Items.Add(new Favorite()
                {
                    favorite_name = R.GetString(0),
                    favorite_stars = R.GetDouble(1),
                    favorite_city = R.GetString(2),
                    favorite_postal_code = R.GetInt32(3),
                    favorite_address = R.GetString(4),
                    favorite_business_id = R.GetString(5)
                });
        }

        private void updateFavorites()
        {
            favoriteBusDG.Items.Clear();
            string sqlStr = "SELECT name, stars, city, postal_code, address, F.business_id " +
                                "FROM Favorites as F INNER JOIN Business as B ON B.business_id = F.business_id " +
                                "WHERE user_id = '" + YelpUser.current_user_id + "';";
            executeQuery(sqlStr, populateFavorites);
        }

        private void userIDLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            favoriteBusDG.Items.Clear();
            friendRevDG.Items.Clear();
            friendsDG.Items.Clear();
            if(userIDLB.SelectedIndex > -1)
            {
                YelpUser.current_user_id = userIDLB.SelectedItem.ToString();
                string sqlStr1 = "SELECT name, average_stars, yelping_since, funny, cool, useful, latitude, longitude FROM yelpUser WHERE user_id = '" + userIDLB.SelectedItem.ToString() + "';";
                executeQuery(sqlStr1, populateUserInformation);
                string sqlStr2 = "SELECT name, average_stars, yelping_since FROM yelpUser, Friendship WHERE Friendship.user_id = '" + userIDLB.SelectedItem.ToString() + "' AND Friendship.users_friend_id = yelpUser.user_id;";
                executeQuery(sqlStr2, populateFriends);
                string sqlStr3 = "SELECT X.name as User_Name, Y.name as Business_Name, y.city, review_text, stars, useful, funny, cool, review_date " +
                                "FROM Review as R3 " +
                                "INNER JOIN " +
                                    "(SELECT R2.user_id, MIN(review_id) as review_id " +
                                    "FROM Review as R2 " +
                                    "INNER JOIN " +
                                        "(SELECT R1.user_id, MAX(R1.review_date) as recent_date " +
                                        "FROM(SELECT users_friend_id FROM Friendship WHERE user_id = '" + userIDLB.SelectedItem.ToString() + "') as F " +
                                        "LEFT JOIN Review as R1 ON F.users_friend_id = R1.user_id " +
                                        "GROUP BY R1.user_id) as A ON R2.user_id = A.user_id " +
                                    "WHERE R2.review_date = recent_date GROUP BY R2.user_id) AS B ON R3.user_id = B.user_id AND R3.review_id = B.review_id " +
                                "INNER JOIN(SELECT name, user_id FROM yelpUser) as X ON X.user_id = R3.user_id " +
                                "INNER JOIN(SELECT name, city, business_id FROM Business) as Y ON Y.business_id = R3.business_id;";
                executeQuery(sqlStr3, populateFriendsRev);

                string sqlStr4 = "SELECT name, stars, city, postal_code, address, F.business_id " +
                                "FROM Favorites as F INNER JOIN Business as B ON B.business_id = F.business_id " +
                                "WHERE user_id = '" + YelpUser.current_user_id + "';";
                executeQuery(sqlStr4, populateFavorites);
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            latTB.IsReadOnly = false;
            longTB.IsReadOnly = false;
        }

        private void updateLatLong(NpgsqlDataReader R)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr = "UPDATE yelpUser " +
                            "SET latitude = '" + latTB.Text + "', longitude = '" + longTB.Text +
                            "'WHERE user_id = '" + YelpUser.current_user_id + "';";
            executeQuery(sqlStr, updateLatLong);
        }

        private void favoriteRemoved(NpgsqlDataReader R)
        {

        }

        private void removeFavButton_Click(object sender, RoutedEventArgs e)
        {
            if (favoriteBusDG.SelectedIndex > -1)
            {
                Favorite F = favoriteBusDG.Items[favoriteBusDG.SelectedIndex] as Favorite;
                if ((F.favorite_business_id != null) && (F.favorite_business_id.ToString().CompareTo("") != 0))
                {
                    string sqlStr1 = "DELETE FROM Favorites " +
                                    "WHERE user_id = '" + YelpUser.current_user_id + "' AND business_id = '" + F.favorite_business_id.ToString() + "';";
                    executeQuery(sqlStr1, favoriteRemoved);
                    favoriteBusDG.Items.Clear();
                    string sqlStr2 = "SELECT name, stars, city, postal_code, address, F.business_id " +
                                "FROM Favorites as F INNER JOIN Business as B ON B.business_id = F.business_id " +
                                "WHERE user_id = '" + YelpUser.current_user_id + "';";
                    executeQuery(sqlStr2, populateFavorites);
                }
            }
        }

        private void upsertCheckin(NpgsqlDataReader R)
        {

        }

        private void checkInButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    string sqlStr = "INSERT INTO Checkin (business_id, checkin_day, checkin_hour, checkin_count) " +
                                    "VALUES ('" + B.bid + "','" + DateTime.Today.DayOfWeek.ToString() + "','" + DateTime.Now.ToString("HH:00") + "', 1) " +
                                    "ON CONFLICT (business_id,checkin_day,checkin_hour) DO UPDATE " +
                                    "SET checkin_count = Checkin.checkin_count + 1 " +
                                    "WHERE Checkin.business_id = '" + B.bid + "' AND Checkin.checkin_day = '" + DateTime.Today.DayOfWeek.ToString() + "' AND Checkin.checkin_hour = '" + DateTime.Now.ToString("HH:00") + "';";
                    businessGrid.Items.Clear();
                    executeQuery(sqlStr, upsertCheckin);
                    searchButton_Click(sender, e);
                }
            }
        }
    }
}