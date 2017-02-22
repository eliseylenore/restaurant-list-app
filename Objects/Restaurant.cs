using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantListApp
{
    public class Restaurant
    {
        private int _id;
        private string _name;
        private int _rating;
        private string _city;
        private int _cuisineId;

        public Restaurant(string Name, int Rating, string City, int CuisineId, int Id = 0)
        {
            _id = Id;
            _name = Name;
            _rating = Rating;
            _city = City;
            _cuisineId = CuisineId;
        }
        public override bool Equals(System.Object otherRestaurant)
        {
            if (!(otherRestaurant is Restaurant))
            {
                return false;
            }
            else
            {
                Restaurant newRestaurant = (Restaurant) otherRestaurant;
                bool idEquality = this.GetId() == newRestaurant.GetId();
                bool nameEquality = (this.GetName() == newRestaurant.GetName());
                bool ratingEquality = (this.GetRating() == newRestaurant.GetRating());
                bool cityEquality = (this.GetCity() == newRestaurant.GetCity());
                bool cuisineEquality = this.GetCuisineId() == newRestaurant.GetCuisineId();
                return (idEquality && nameEquality && ratingEquality && cityEquality && cuisineEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }
        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public int GetRating()
        {
            return _rating;
        }

        public void SetRating(int newRating)
        {
            _rating = newRating;
        }

        public string GetCity()
        {
            return _city;
        }

        public void SetCity(string newCity)
        {
            _city = newCity;
        }

        public int GetCuisineId()
        {
            return _cuisineId;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, rating, city, cuisine_id) OUTPUT INSERTED.id VALUES (@RestaurantName, @RestaurantRating, @RestaurantCity, @RestaurantCuisineId);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@RestaurantName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);

            SqlParameter ratingParameter = new SqlParameter();
            ratingParameter.ParameterName = "@RestaurantRating";
            ratingParameter.Value = this.GetRating();
            cmd.Parameters.Add(ratingParameter);

            SqlParameter cityParameter = new SqlParameter();
            cityParameter.ParameterName = "@RestaurantCity";
            cityParameter.Value = this.GetCity();
            cmd.Parameters.Add(cityParameter);

            SqlParameter cuisineIdParameter = new SqlParameter();
            cuisineIdParameter.ParameterName = "@RestaurantCuisineId";
            cuisineIdParameter.Value = this.GetCuisineId();
            cmd.Parameters.Add(cuisineIdParameter);


            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }
        public static List<Restaurant> GetAll()
        {
            List<Restaurant> allRestaurants = new List<Restaurant>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int restaurantId = rdr.GetInt32(0);
                string restaurantName = rdr.GetString(1);
                int restaurantRating = rdr.GetInt32(2);
                string restaurantCity = rdr.GetString(3);
                int restaurantCuisineId = rdr.GetInt32(4);
                Restaurant newRestaurant = new Restaurant(restaurantName, restaurantRating, restaurantCity, restaurantCuisineId, restaurantId);
                allRestaurants.Add(newRestaurant);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allRestaurants;
        }
        public static Restaurant Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @RestaurantId;", conn);
            SqlParameter restaurantIdParameter = new SqlParameter();

            restaurantIdParameter.ParameterName = "@RestaurantId";
            restaurantIdParameter.Value = id.ToString();
            cmd.Parameters.Add(restaurantIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundRestaurantId = 0;
            string foundRestaurantName = null;
            int foundRestaurantRating = 0;
            string foundRestaurantCity = null;
            int foundRestaurantCuisineId = 0;
            while(rdr.Read())
            {
                foundRestaurantId = rdr.GetInt32(0);
                foundRestaurantName = rdr.GetString(1);
                foundRestaurantRating = rdr.GetInt32(2);
                foundRestaurantCity = rdr.GetString(3);
                foundRestaurantCuisineId = rdr.GetInt32(4);
            }
            Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantRating, foundRestaurantCity, foundRestaurantCuisineId, foundRestaurantId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundRestaurant;
        }

    }
}
