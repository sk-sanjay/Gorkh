$('#imgModal').on('show.bs.modal', function (event) {
    debugger;
    var name1 = $(event.relatedTarget).attr("data-name");
    $(".linkname").html(name1);
    $('#imgid').attr('src', $(event.relatedTarget).data('src'));
    var href = $(event.relatedTarget).data('href');
    if (href != '' && href != null) {
        $('#link').attr('href', href);
        $('#divlink').show();
    }
    else {
        $('#divlink').hide();
    }
});

var owl = $(".hero-carousal");
owl.owlCarousel({
  smartSpeed: 1000,
  items: 1,
  loop: true,
  margin: 0,
  nav: false,
  dots: true,
  autoplay:true,
  autoplayTimeout:60000,
    autoplayHoverPause: true,
    rtl: true,
    
});
$('.play').on('click',function(){
    owl.trigger('play.owl.autoplay',[5000])
})
$('.stop').on('click',function(){
    owl.trigger('stop.owl.autoplay')
})


$('.service-slide').owlCarousel({
  smartSpeed: 500,
  items: 3,
  margin: 5,
  nav: true,
  dots: true,
  loop: true,
  autoplay:true,
  autoplayTimeout:5000,
    autoplayHoverPause: true,
    rtl: true,
  responsive:{
    0:{items:1},
    600:{items:2},
    769:{items:3}
  }
});

$('.logoSlider').owlCarousel({
    smartSpeed: 500,
    margin: 5,
    nav: true,
    dots: false,
    loop: true,
    autoplay: true,
    autoplayTimeout: 5000,
    autoplayHoverPause: true,
    rtl: true,
    responsive: {
        0: { items: 1 },
        600: { items: 2 },
        769: { items: 6 }
    }
});

$('.pro-slide').owlCarousel({
  smartSpeed: 500,
  items: 3,
  margin: 16,
  nav: true,
  dots: true,
  loop: true,
  autoplay:true,
  autoplayTimeout:5000,
  autoplayHoverPause:true,
    stagePadding: 16,
    rtl: true,
  responsive:{
    0:{items:1, stagePadding: 10},
    600:{items:2},
    769:{items:3}
  }
});

$('.related-slide').owlCarousel({
  smartSpeed: 500,
  items: 4,
  margin: 16,
  nav: false,
    dots: true,
    rtl: true,
  responsive:{
    0:{items:1},
    600:{items:2},
    769:{items:4}
  }
});


$('.prolike').on('click', function(){
    $(this).toggleClass('act');
});


$('.view ul li').on('click', function(){
    $('.view ul li.active').removeClass('active');
    $(this).addClass('active');
});

$('.gView').click(function(){
  $('.prView').removeClass('act');
});

$('.lView').click(function(){
  $('.prView').addClass('act');
});


$(function () {
  $(".accordion-content:not(:first-of-type)").css("display", "none");
  //$(".js-accordion-title:first-of-type").addClass("open");

  $(".js-accordion-title").click(function () {
    $(".open").not(this).removeClass("open").next().slideUp(300);
    $(this).toggleClass("open").next().slideToggle(300);
  });
});



$(".product-cat-link ul li a").each(function(){
  if($(this).parent().find('ul').length > 0){
   $(this).parent().prepend('<span class="subDropAlt"></span>');
   $(this).parent().addClass('has-sub');
  }else{
  }
});

$('.subDropAlt').click(function(){
  $(this).parent().find('> ul').slideToggle();
});
