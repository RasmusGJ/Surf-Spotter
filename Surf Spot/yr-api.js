var airPressure = document.querySelector('.airPressure');

fetch('https://api.met.no/weatherapi/locationforecast/2.0/compact.json?lat=56.263920&lon=9.501785')
.then(response => response.json())
.then(data => {
    console.log(data)
    var airPressureValue = data['properties']['timeseries']['0']['data']['instant']['details']['air_pressure_at_sea_level'];
    console.log(airPressureValue)
    airPressure.innerHTML = airPressureValue;
})

.catch(err => alert("error"))

