/*

~~~ ROCKET ZONE ~~~

*/
jQuery.extend(jQuery.easing,
    {
        customEaseInQuad: function (x, t, b, c, d) {
            return c * (t /= d) * (t + 0.2) + b;
        }
    })
var rocket_extended = false;
var rocket_loaded = false;
var rocket_clicked = false;

let rocket_img_url = 'https://cdn.factorio.com/assets/img/web/rocket-loop-halfsize.png';
// easing = 'linear';
let easing = 'customEaseInQuad';
let rocket_offset;
let window_height;

function animate_rocket() {
    $('#rocket').animate({ bottom: rocket_offset }, rocket_offset, easing, function () {
        $('#rocket').hide();
    });
    $("html").delay(100).animate({ scrollTop: 0 }, rocket_offset - window_height / 2, easing);
}

$('#rocket').on('mouseover', function (e) {
    $('<img src="' + rocket_img_url + '">').on('load', function () {
        console.log('rocket loaded on hover');
        rocket_loaded = true;
        $('#rocket').addClass('rocket-animating')
    })
})
$('#rocket').on('mouseout', function () {
    if (!rocket_clicked) {
        $('#rocket').removeClass('rocket-animating')
    }
})

$('#rocket').on('click', function (e) {
    if (rocket_clicked) return;
    rocket_clicked = true;
    rocket_offset = $("#rocket").offset().top + 448 / 2;
    window_height = $(window).height();
    if (!rocket_loaded) {
        $('<img src="' + rocket_img_url + '">').on('load', function () {
            console.log('rocket loaded');
            rocket_loaded = true;
            $('#rocket').addClass('rocket-animating')
            if (rocket_extended) {
                console.log('animating rocket from img load');
                animate_rocket();
            }
        });
    }
    $('#rocket').animate({ height: 200 }, 400, 'linear', function () {
        $('#rocket').css({
            height: 224,
            bottom: -(224 - 200),
            'z-index': 200
        });
        rocket_extended = true;
        if (rocket_loaded) {
            console.log('animating rocket from animate');
            animate_rocket();
        }
    });
});