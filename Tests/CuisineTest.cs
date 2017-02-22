using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantListApp
{
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=food_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CuisineEmptyAtFirst()
    {
      //Arrange, Act
      int result = Cuisine.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Cuisine firstCuisine = new Cuisine("mexican");
      Cuisine secondCuisine = new Cuisine("mexican");

      //Assert
      Assert.Equal(firstCuisine, secondCuisine);
    }

    [Fact]
    public void Test_Save_SavesCuisineToDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("mexican");
      testCuisine.Save();

      //Act
      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCuisineObject()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("mexican");
      testCuisine.Save();

      //Act
      Cuisine savedCuisine = Cuisine.GetAll()[0];

      int result = savedCuisine.GetId();
      int testId = testCuisine.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCuisineInDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("mexican");
      testCuisine.Save();

      //Act
      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      //Assert
      Assert.Equal(testCuisine, foundCuisine);
    }

    [Fact]
   public void Test_GetRestaurant_RetrievesAllRestaurantsWithCuisine()
   {
     Cuisine testCuisine = new Cuisine("mexican");
     testCuisine.Save();

     Restaurant firstRestaurant = new Restaurant("Larry's Tacos", 3, "Bend", testCuisine.GetId());
     firstRestaurant.Save();
     Restaurant secondRestaurant = new Restaurant("Brandon's Burritos", 2, "Mill Creek", testCuisine.GetId());
     secondRestaurant.Save();


     List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
     List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

     Assert.Equal(testRestaurantList, resultRestaurantList);
   }

    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
    }
  }
}
