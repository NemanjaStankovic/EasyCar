document.getElementById("dugmeZaPretragu").onclick = ev => pretrazi();

function pretrazi()
{
    var plac = document.getElementById("placeviPretraga").value;
    var godisteOd = document.getElementById("godOdPretraga").value;
    if (godisteOd == '')
        godisteOd = 0;
    var godisteDo = document.getElementById("godDoPretraga").value;
    if (godisteDo == '')
        godisteDo = 0;
    var opadajuce = document.getElementById("opadajuce").value;
    var cenaOd = document.getElementById("cenaOdPretraga").value;
    if (cenaOd == '')
        cenaOd = 0;
    var cenaDo = document.getElementById("cenaDoPretraga").value;
    if (cenaDo == '')
        cenaDo = 0;
    var marka = document.getElementById("markaPretraga").value;
    if (marka == '')
        marka = "Sve";
    var model = document.getElementById("modelPretraga").value;
    if (model == '')
        model = "Sve";
    var kriterijum = document.getElementById("opadajuce").value;
    var el = document.getElementById("prvi");
    while(el.childElementCount!=0)
        el.removeChild(el.firstChild);

    fetch("https://localhost:7276/Car/Filtriraj/" + plac + "/" + godisteOd + "/" + godisteDo + "/" + opadajuce + "/" + cenaOd + "/" + cenaDo + "/" + marka + "/" + model + "/" + kriterijum + "/")
        .then(p => {
            p.json().then(cars => {
                cars.forEach(car => {
                    let c = new Car(car.id, car.marka, car.model, car.godProizvodnje, car.predjenihKM, car.cena, car.photo, car.prodavac);
                    c.crtajAuto(el);
                })
            })
        })
}