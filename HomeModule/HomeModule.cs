using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace RestaurantListApp
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {

        Dictionary<string, object> model = new Dictionary<string, object>();
        var AllCuisines = Cuisine.GetAll();
        var AllRestaurants = Restaurant.GetAll();
        model.Add("cuisines", AllCuisines);
        model.Add("restaurants", AllRestaurants);
        return View["index.cshtml", model];
      };
      Get["/restaurants"] = _ => {
        List<Restaurant> AllRestaurants = Restaurant.GetAll();
        return View["restaurants.cshtml", AllRestaurants];
      };
      // Get["/cuisines"] = _ => {
      //     List<Cuisine> AllCuisines = Cuisine.GetAll();
      //     return View["cuisines.cshtml", AllCuisines];
      // };
      Get["/cuisines/new"] = _ => {
        return View["cuisine_form.cshtml"];
      };
      Post["/cuisines/new"] = _ => {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine-name"]);
        newCuisine.Save();
        return View["success.cshtml"];
      };
      Get["/restaurants/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var AllCuisines = Cuisine.GetAll();
        var AllRestaurants = Restaurant.GetAll();
        model.Add("cuisines", AllCuisines);
        model.Add("restaurants", AllRestaurants);
        return View["restaurants_form.cshtml", model];
      };
      Post["/restaurants/new"] = _ => {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], Request.Form["restaurant-rating"], Request.Form["restaurant-city"], Request.Form["cuisine-id"]);
        List<Restaurant> AllRestaurants = Restaurant.GetAll();
        newRestaurant.Save();
        return View["success.cshtml", AllRestaurants];
      };
      Post["/restaurants/delete"] = _ => {
        Restaurant.DeleteAll();
        return View["restaurants_cleared.cshtml"];
      };
      Post["/cuisines/delete"] = _ => {
        Cuisine.DeleteAll();
        return View["cuisines_cleared.cshtml"];
      };
      Get["/cuisines/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedCuisine = Cuisine.Find(parameters.id);
        var CuisineRestaurants = SelectedCuisine.GetRestaurants();
        model.Add("cuisine", SelectedCuisine);
        model.Add("restaurants", CuisineRestaurants);
        return View["cuisine.cshtml", model];
      };
      Get["cuisine/edit/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        return View["cuisine_edit.cshtml", SelectedCuisine];
      };
      Patch["cuisine/edit/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        SelectedCuisine.Update(Request.Form["cuisine-name"]);
        return View["success.cshtml"];
      };
      Get["cuisines/delete/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        return View["cuisine_delete.cshtml", SelectedCuisine];
      };
      Delete["cuisines/delete/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        SelectedCuisine.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
