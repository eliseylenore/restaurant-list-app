$(document).ready(function(){
  $("li.new-restaurant").click(function(event) {
    event.preventDefault();
    $(".restaurant-form").show();
  });
});
