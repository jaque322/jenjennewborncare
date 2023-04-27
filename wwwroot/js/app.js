$(document).ready(function () {

    $('#hero-slider').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        dots: false,
        items: 1,
        navText: ['PREV', 'NEXT'],
        smartSpeed: 1000,
        responsive: {
            0: {
                nav: false,
            },
            768: {
                nav: true
            },


        }
    });
    /*project slider*/
    //$('#projects-slider').owlCarousel({
    //    loop: true,
    //    nav: false,
    //    items: 2,
    //    dots: true,
    //    smartSpeed: 600,
    //    center: true,
    //    autoplay: true,
    //    autoplayTimeout: 4000,
    //    responsive: {
    //        0: {
    //            items: 1
    //        },
    //        768: {
    //            items: 2,
    //            margin: 8,
    //        }
    //    }
    //});

    $('#portafolio').owlCarousel({
        items: 1,
        loop: true,
        margin: 10,
        video: true,
        lazyLoad: true,
        center: true,
        autoplay: true,
        autoplayTimeout: 5000,
        autoplayHoverPause: true,
        nav: true,
        dots: true,
        responsive: {
            0: {
                items: 1
            },
            480: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });


    $('.reviews-slider').owlCarousel({
        loop: true,
        nav: false,
        dots: true,
        smartSpeed: 900,
        items: 1,
        margin: 24,
        autoplay: true,
        autoplayTimeout: 4000,
    })
});