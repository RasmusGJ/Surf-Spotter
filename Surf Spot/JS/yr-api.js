var airPress = document.querySelector('.airPress');
var airTemp = document.querySelector('.airTemp');
var windSpeed = document.querySelector('.windSpeed');
var probPercipitation = document.querySelector('.probPercipitation');



fetch('https://api.met.no/weatherapi/locationforecast/2.0/compact.json?lat=56.263920&lon=9.501785')
.then(response => response.json())
.then(data => {
    console.log(data)
    var airPressureValue = data['properties']['timeseries']['0']['data']['instant']['details']['air_pressure_at_sea_level'];
    var airTemperatureValue = data['properties']['timeseries']['0']['data']['instant']['details']['air_temperature'];
    var windSpeedValue = data['properties']['timeseries']['0']['data']['instant']['details']['wind_speed'];
    var probPercipitationValue = data['properties']['timeseries']['0']['data']['instant']['details']['wind_speed'];

    airPress.innerHTML = 'Air Pressure: ' + airPressureValue + ' p';
    airTemp.innerHTML = 'Temperature: ' + airTemperatureValue + ' °C';
    windSpeed.innerHTML = 'Wind Speed: ' + windSpeedValue + ' m/s';
    probPercipitation.innerHTML = 'Percipitation Probability : ' + probPercipitationValue + ' %';

})

.catch(err => alert("error"))

