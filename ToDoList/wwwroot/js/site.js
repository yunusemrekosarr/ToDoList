$('.btn-close-white').on('click', function () {
    $('.details').removeClass('col-md-4');
    $('.details').addClass('none');
    $('.main').removeClass('col-md-8');
    $('.main').addClass('col-md-12');
});


$('.openbtn').off().on('click', function (e) {
    $("#mySidenav").toggleClass('open');

    if ($("#mySidenav").hasClass("open")) {
        $("#mySidenav").css("width", "250px");
        $("#main").css("marginLeft", "250px");
        $(".openbtn").html('&times;');      
    }
    else {
        $("#mySidenav").css("width", "0");
        $("#main").css("marginLeft", "0");
        $(".openbtn").html('&#9776;');  
    } 
});

$('#showdone').on('click', function (e) {
    $('li').removeClass('none')
    $('.task-list-item-label:not(.done)').parent().addClass('none');
});

$('#showall').on('click', function (e) {
    $('li').removeClass('none');
});

$('#showundone').on('click', function (e) {
    $('li').removeClass('none')
    $('.task-list-item-label.done').parent().addClass('none');
});