$(document).ready(function(){
  $("li.new-restaurant").click(function(event) {
    event.preventDefault();
    $(".restaurant-form").show();
  });

  $("li.new-cuisine").click(function(event) {
    event.preventDefault();
    $(".category-form").show();
  });

  $(".navbar-brand").click(function(event) {
    event.preventDefault();
    $(".category-form").hide();
    $(".restaurant-form").hide();
  });
});
