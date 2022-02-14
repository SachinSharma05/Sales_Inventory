
$(document).ready(function () {
    $(".Address").on('blur', function () {
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById(this.id));
        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            console.log(place.address_components);
            for (var i = 0; i < place.address_components.length; i++) {
                var component = place.address_components[i];
                var addressType = component.types[0];
                switch (addressType) {
                    case 'street_number':
                        break;
                    case 'route':
                        break;
                    case 'locality': $("#City").val(component.long_name)
                        break;
                    case 'administrative_area_level_1': $("#State").val(component.short_name)
                        break;
                    case 'postal_code': $("#PostalCode").val(component.long_name)
                        break;
                    case 'country':
                        break;
                }
            }

        });
    });
    $("#Address1").on('blur', function () {
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('Address1'));
        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            console.log(place.address_components);
            for (var i = 0; i < place.address_components.length; i++) {
                var component = place.address_components[i];
                var addressType = component.types[0];
                switch (addressType) {
                    case 'street_number':
                        break;
                    case 'route':
                        break;
                    case 'locality': $("#City1").val(component.long_name)
                        break;
                    case 'administrative_area_level_1': $("#State1").val(component.short_name)
                        break;
                    case 'postal_code': $("#PostalCode1").val(component.long_name)
                        break;
                    case 'country':
                        break;
                }
            }

        });
    });
});




function validateForm(id) {
    var valid = true;
    var input = $("#" + id).find('input:text, input:password,input:checkbox ,input:file, select, textarea');
    for (i = 0; i < input.length; i++) {
        if ($(input[i]).val() == "" && $(input[i]).hasClass("required")) {
            $(input[i]).addClass("invalid");
            valid = false;
        }
        else {
            $(input[i]).removeClass("invalid");
        }
    }
    return valid;
}
function validateByClassName(name) {
    
    var valid = true;
    var input = $("." + name).find('input:text, input:password,input:checkbox ,input:file, select, textarea');
    for (i = 0; i < input.length; i++) {
        if ($(input[i]).val() == "" && $(input[i]).hasClass("required")) {
            $(input[i]).addClass("invalid");
            valid = false;
        }
        else {
            $(input[i]).removeClass("invalid");
        }
    }
    return valid;
}
$('.StopChar').bind('keypress', function (e) {
    return (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) ? false : true;
})
$('.StopNumber').keydown(function (e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = e.keyCode;
        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    }
});
$('.currency').keydown(function (e) {
    var string = numeral($(this).val()).format('($0,0)');
    $(this).val(string);
});
$('.EmailValid').change(function () {
    
    email = $(this).val();
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        $(this).addClass("invalid");
        $(this).val('');
        $(this).focus();
        return false;
    } else {
        $(this).removeClass("invalid");
        return true;

    }
});

function ValidEmail(email) {

    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        $(this).addClass("invalid");
        return false;
    } else {
        $(this).removeClass("invalid");
        return true;

    }
}

$(function () {
    $('.date').mask('00/00/0000');
    $('.time').mask('00:00:00');
    $('.date_time').mask('00/00/0000 00:00:00');
    $('.cep').mask('00000-000');
    $('.phone').mask('0000-0000');
    $('.phone_with_ddd').mask('(00) 0000-0000');
    $('.phone_us').mask('(000) 000-0000');
    $('.mixed').mask('AAA 000-S0S');
    $('.ip_address').mask('099.099.099.099');
    $('.percent').mask('##0,00%', { reverse: true });
    $('.clear-if-not-match').mask("00/00/0000", { clearIfNotMatch: true });
    $('.placeholder').mask("00/00/0000", { placeholder: "__/__/____" });
    $('.fallback').mask("00r00r0000", {
        translation: {
            'r': {
                pattern: /[\/]/,
                fallback: '/'
            },
            placeholder: "__/__/____"
        }
    });

    $('.selectonfocus').mask("00/00/0000", { selectOnFocus: true });

    $('.cep_with_callback').mask('00000-000', {
        onComplete: function (cep) {
            console.log('Mask is done!:', cep);
        },
        onKeyPress: function (cep, event, currentField, options) {
            console.log('An key was pressed!:', cep, ' event: ', event, 'currentField: ', currentField.attr('class'), ' options: ', options);
        },
        onInvalid: function (val, e, field, invalid, options) {
            var error = invalid[0];
            console.log("Digit: ", error.v, " is invalid for the position: ", error.p, ". We expect something like: ", error.e);
        }
    });

    $('.crazy_cep').mask('00000-000', {
        onKeyPress: function (cep, e, field, options) {
            var masks = ['00000-000', '0-00-00-00'];
            mask = (cep.length > 7) ? masks[1] : masks[0];
            $('.crazy_cep').mask(mask, options);
        }
    });

    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.money').mask('#.##0,00', { reverse: true });

    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
    spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

    $('.sp_celphones').mask(SPMaskBehavior, spOptions);

    $(".bt-mask-it").click(function () {
        $(".mask-on-div").mask("000.000.000-00");
        $(".mask-on-div").fadeOut(500).fadeIn(500)
    })

    $('pre').each(function (i, e) { hljs.highlightBlock(e) });
});