        var map;
        var centerCords = {
        lat: 55.3980029,
        lng: 10.3814147
        };
        var markerLocations = [{
        lat: 57.044290,
        lng: 8.480431,
        title: 'Klitmøller Strand',
        },
        {
        lat: 56.010760,
        lng: 8.106947,
        title: 'Ringkøbing Fjord',
        }
        ,
        {
            lat: 22.039073,
            lng: -159.335492,
            title: 'Kauai',
        }
        ,
        {
            lat: 20.898691,
            lng: -156.444709,
            title: 'Maui',
        }
        ,
        {
            lat: -28.042766,
            lng: 153.438623,
            title: 'Gold Coast',
        }
        ,
        {
            lat: 57.363698,
            lng: 9.696541,
            title: 'Løkken',
        }
        ];
        
        var airPressureValue = " ";
        var airTemperatureValue = " ";
        var windSpeedValue = " ";
        var probPercipitationValue = " ";
        var contentHTML = " ";

        function dataHentning(lat, lng, title){
            //console.log(lat + " : " + lng);
            fetch('https://api.met.no/weatherapi/locationforecast/2.0/compact.json?lat=' + lat + '&lon=' + lng)
            .then(response => response.json())
            .then(data => {
                //console.log(data)
                airPressureValue = data['properties']['timeseries']['0']['data']['instant']['details']['air_pressure_at_sea_level'];
                airTemperatureValue = data['properties']['timeseries']['0']['data']['instant']['details']['air_temperature'];
                windSpeedValue = data['properties']['timeseries']['0']['data']['instant']['details']['wind_speed'];
                probPercipitationValue = data['properties']['timeseries']['0']['data']['instant']['details']['wind_speed'];

                // airPressureValue = 'Air Pressure: ' + airPressureValue + ' p';
                // airTemperatureValue = 'Temperature: ' + airTemperatureValue + ' °C';
                // windSpeedValue = 'Wind Speed: ' + windSpeedValue + ' m/s';
                // probPercipitationValue = 'Percipitation Probability : ' + probPercipitationValue + ' %';

            })          

            .catch(err => alert("error"))
            contentHTML =
            "<div class='infowindowHeader'>" +
            "<h2>" + title + "</h2>" +
            "</div>" +
           "<table>" +
           "<tr>" +
           "<td>Wave Height: </td>" +
           "<td>" + airTemperatureValue + "</td>" +
            "</tr>" +
            "<tr> " +
                "<td>Wave Period: </td>" +
                "<td>asd</td>" +
            "</tr>" +
            "<tr>" +
                "<td>Wind Speed: </td>" +
                "<td>asd</td>" +
            "</tr>" +
            "<tr>" +
                "<td>Water Temperature: </td>" +
                "<td>asd</td>" +
                "</tr>" +
            "<tr>" +
                "<td>Weather assesment: </td>" +
                "<td>asd</td>" +
            "</tr>" +
            "</table>";
            return contentHTML;
        }

        function myMap() {
            map = new google.maps.Map(document.getElementById("googleMap"), {
            center: centerCords,
            zoom: 6.5,
            mapTypeId: google.maps.MapTypeId.SATELLITE,
            disableDefaultUI: true
            });

            var infowindow = new google.maps.InfoWindow();

            for(var i = 0; i < markerLocations.length; i++){
                var marker = new google.maps.Marker({
                    position: markerLocations[i],
                    map: map,
                    title: markerLocations[i].title
                });  
                google.maps.event.addListener(marker, 'click', (function(marker, i) {
			        return function() {
                        infowindow.setContent(dataHentning(markerLocations[i].lat, markerLocations[i].lng, markerLocations[i].title));
                        infowindow.open(map, marker);
			        }
                }) (marker, i));   
                google.maps.event.addListener(marker, 'mouseover', (function(marker, i) {
			        return function() {
                        infowindow.setContent(dataHentning(markerLocations[i].lat, markerLocations[i].lng));
			        }
		        }) (marker, i));              
            }                    
        }
