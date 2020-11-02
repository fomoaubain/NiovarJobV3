// chemin : https://niovar.solutions
// chemin : ..

var chemin = 'https://niovar.solutions';
 //   var chemin = '..';
var prefix = '..'; 

if (typeof edit == 'boolean' &&  edit == true) {
  var chemin = 'https://niovar.solutions/..';
   // var chemin = '../..';
  var prefix = '../..';
}


$(document).ready(function () {
  
// charge les pays au  chargement de la page
    var stateOptions = " <option selected  value=''>Veuiller choisir le pays</option>";
    $.getJSON(chemin+'/Json/countries.json', function (result) {
        $.each(result, function (i, data) {
            //<option value='countrycode'>contryname</option>
            console.log("a");
            if (data.name == valuePays) {
                stateOptions += "<option value='" + valuePays + "' selected >"
                    + valuePays + "</option>";
            } else {
                stateOptions += "<option value='"
                    + data.name +
                    "' >"
                    + data.name +
                    "</option>";
            }
        });
        $('.country').html(stateOptions);
    });


    // charge les province au  chargement de la page
    if (valueProvince != null) { 
    var stateOptionsProvince = " <option selected  value=''>Veuiller choisir la province</option>";

        $.getJSON(chemin+'/Json/countrie_states.json', function (result) {
        $.each(result, function (i, item) {
            if (valuePays == item.name) {
                $.each(item.states, function (j, province) {
                    if (valueProvince == province.state_code) {
                        stateOptionsProvince += "<option value='"
                            + province.state_code +
                            "' selected>"
                            + province.name +
                            "</option>";
                    } else {
                        stateOptionsProvince += "<option value='"
                            + province.state_code +
                            "'>"
                            + province.name +
                            "</option>";
                    }
                   
                });

            }

        });
        $('.state').html(stateOptionsProvince);
    });
    }

// charge les ville au  chargement de la page
    if (valueVille != null) {
        var val = valueProvince;
        var stateOptionsVille = " <option selected  value=''>Veuiller choisir la ville</option>";
        $.ajax({
            type: 'GET',
            url: prefix+'/Offre/ListeVilleProvince',
            data: { "id": val },
            dataType: 'json',
            success: function (data) {
                $.each(data.datas, function (i, item) {
                    console.log("name:" + item.name);
                    if (valueVille == item.name) {
                        stateOptionsVille += "<option value='"
                            + item.name +
                            "' selected>"
                            + item.name +
                            "</option>";
                    } else {
                        stateOptionsVille += "<option value='"
                            + item.name +
                            "'>"
                            + item.name +
                            "</option>";
                    }
                });

                $('.city').html(stateOptionsVille);
            },
            error: function (xhr, status, error) {
                //var err = eval("(" + xhr.responseText + ")");
                console.log("aaaaa:" + error)
                $('.city').html(stateOptionsVille);
            }
        });
    }

    });


$(".country").on('change', function (e) {
    var optionSelected = $("option:selected", this);
    var choice = this.value;
    console.log("choice:" + choice);
    var stateOptions = " <option selected  value=''>Veuiller choisir la province</option>";
    console.log("b");
    $.getJSON(chemin+'/Json/countrie_states.json', function (result) {
        $.each(result, function (i, item) {
            if (choice == item.name) {
                $.each(item.states, function (j, province) {
                    stateOptions += "<option value='"
                        + province.state_code +
                        "'>"
                        + province.name +
                        "</option>";
                });
                
            }

        });
        $('.state').html(stateOptions);
    });
});


$(".state").on('change', function (e) {
    var optionSelected = $("option:selected", this);
    var choice = this.value;
    console.log("choice:" + choice);
    var stateOptions = " <option selected  value=''>Veuiller choisir la ville</option>";
    $.ajax({
        type: 'GET',
        url: prefix+'/Offre/ListeVilleProvince',
        data: { "id": choice },
        dataType: 'json',
        success: function (data) {
            console.log("data:" + data.datas);
            $.each(data.datas, function (i, item) {
                console.log("name:" + item.name);
                stateOptions += "<option value='"
                        + item.name +
                        "'>"
                        + item.name +
                        "</option>";   
            });

            $('.city').html(stateOptions);
        },
        error: function (xhr, status, error) {
            //var err = eval("(" + xhr.responseText + ")");
            console.log("aaaaa:" + error)
            $('.city').html(stateOptions);
        }
    });
});




