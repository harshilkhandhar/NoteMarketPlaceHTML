/* =========================================
            Show/Hide password flid
============================================ */
$(".toggle-password").click(function() {

    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
      input.attr("type", "text");
    } else {
      input.attr("type", "password");
    }
  });


/* =========================================
            FAQ Accordian
============================================ */
$(document).ready(function () {
  // Add minus icon for collapse element which is open by default
  $(".collapse.show").each(function () {
      $(this).prev(".card-header").find("#img-faq").attr("src", "/Content/images/plus.png");
      $(this).parentsUntil(".card").css({
          "border": "1px solid #d1d1d1"
      });

  });

  // Toggle plus minus icon on show hide of collapse element
  $(".collapse").on('show.bs.collapse', function () {

      $(this).prev(".card-header").find(".img-faq").attr("src", "/Content/images/minus.png").css({
          "height": "23px",
          "width": "35px"
      });
      $(this).prev(".card-header").css({
          "background": "white"
      });
      $(this).parent(".card").css("border","1px solid #d1d1d1");

  }).on('hide.bs.collapse', function () {
      $(this).prev(".card-header").find(".img-faq").attr("src", "/Content/images/plus.png").css({
          "height": "38px",
          "width": "38px"
      });
      $(this).prev(".card-header").css({
          "background": "#f3f3f3"
      });
      $(this).parent(".card").css("border","none");

  });
});

/* =========================================
              Navigation
============================================ */
/* Show & Hide White Navigation */
$(function() {

    //show/hide nav on page load
    showHideNav();
    $(window).scroll(function() {
        //show/hide nav on window's scroll
        showHideNav();
    });
  
    function showHideNav() {
        if( $(window).scrollTop()> 50 ) {
            // Show White nav
            $("#home-nav").addClass("white-nav-top");
  
            //SHow dark logo
            $("#navigation-logo img").attr("src","/Content/images/logo.png");
  
        }else{
            // Hide White nav
            $("#home-nav").removeClass("white-nav-top");
            $("#navigation-logo img").attr("src","/Content/images/top-logo.png");
  
        }
    } 
  });

/* =========================================
                Mobile Menu
============================================ */
$(document).ready(function () {

    //Show Mobile Navbar
    $("#mobile-nav-open-btn").click(function () {

        $("#mobile-nav").css("height", "100%");
        $("#mobile-nav-open-btn").css("display", "none");

    });

    //Hide Mobile Navbar
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {

        $("#mobile-nav").css("height", "0");
        $("#mobile-nav-open-btn").css("display", "block");

    });

});

$(document).ready(function () {


    if ($(window).width() < 991) {
        $('#navbarSupportedContent').addClass('animate_animated animateslideInLeft animate_faster');
    } else {
        $('#navbarSupportedContent').removeClass('animate_animated animateslideInLeft animate_faster');
    }

});

/* =========================================
                Text Box Enable
============================================ */
$(function(){
    $("input[name='inlineRadioOptions']").click(function(){
        if($("#inlineRadio2").is(":checked")){
            $("#txtPassword").removeAttr("disabled");
            $("#txtPassword").focus();
        }
        else{
            $("#txtPassword").attr("disabled","disabled");
        }
    });
});