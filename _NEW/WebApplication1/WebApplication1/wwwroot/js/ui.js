$(document).ready(function () {
  $(".hoverable-cell").hover(
    function () {
      $(this).addClass("hoverable");
    },
    function () {
      $(this).removeClass("hoverable");
    }
  );
});
