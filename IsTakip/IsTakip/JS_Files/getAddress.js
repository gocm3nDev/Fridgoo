if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(onSuccess, onError);
} else {
    alert("Tarayıcınızda konum desteği bulunmamakta. Yeni nesil bir tarayıcıya geçmeniz gerek.")
}

function onSuccess(position) {
    let latitude = position.coords.latitude;
    let longitude = position.coords.longitude;

    const api_key = '48d9235d1375468893bbd72977df7943';
    const url = `https://api.opencagedata.com/geocode/v1/json?q=${latitude}+${longitude}&key=${api_key}`;

    fetch(url)
        .then(response => response.json())
        .then(result => {
            let details = result.results[0].components;

            let {
                country,
                postcode,
                province,
                state,
                city,
                city_district,
                county,
                suburb,
                neighbourhood,
                road,
                house_number
            } = details;

            let sehir = province || state || city || county;
            let mahalle = city_district || suburb || county;

            let fullAddress = "${country}, ${sehir}, ${city_district}, ${mahalle}, ${neighbourhood}, ${house_number}, ${postcode}";


            fetch('/Adres/Kaydet', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ adres: fullAddress })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        console.log("Adres başarıyla kaydedildi.");
                    } else {
                        console.log("Adres kaydı sırasında bir hata oluştu.");
                    }
                });
        });
}

function onError(error) {
    if (error.code == 1) {
        alert("Lütfen konum izni verin.");
    } else if (error.code == 2) {
        alert("Konum alınamadı!");
    } else {
        alert("Bir hata oluştu! Lütfen sayfayı yenileyin.");
    }
}
