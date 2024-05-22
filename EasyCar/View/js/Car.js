document.getElementById("dugmeDodaj").onclick = ev => this.dodajAuto();

class Car {
    constructor(id, marka, model, godP, predjKM, cena, slika, prodavac) {
        this.id = id;
        this.marka = marka;
        this.model = model;
        this.godP = godP;
        this.predjKM = predjKM;
        this.cena = cena;
        this.slika = slika;
        this.prodavac = prodavac;
    }
    izmena()
    {
        console.log(this.id);
        fetch("https://localhost:7276/Car/PromeniAuto/", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "id": this.id,
                "marka": document.getElementById("marka").value,
                "model": document.getElementById("model").value,
                "godProizvodnje": document.getElementById("god").value,
                "predjenihKM": document.getElementById("km").value,
                "cena": document.getElementById("cena").value,
                "ime": document.getElementById("myDropdown").value,
                "photo": null
            })
        }).then(p => {
            console.log(p);
            if (p.status == 200) {
                alert("uspesno ste izmenili auto!");
                location.reload();
            }
            else {
                alert("Doslo je do greske!");
            }
        });
    }
    brisanje() {
        fetch("https://localhost:7276/Car/ObrisiAuto/" + this.id, {
            method: 'DELETE',
        })
            .then(p => {
                console.log(p);
                if (p.status == 200) {
                    alert("uspesno ste obrisali auto!");
                    location.reload();
                }
                else {
                    alert("Doslo je do greske!");
                }
            });
    }
    izmeniAuto() {
        document.getElementById("marka").value = this.marka;
        document.getElementById("model").value = this.model;
        document.getElementById("god").value = this.godP;
        document.getElementById("km").value = this.predjKM;
        document.getElementById("cena").value = this.cena;
        if(this.slika!=null)
            document.getElementById("slika").value = this.slika;
        document.getElementById("myDropdown").value = this.prodavac;

        var dugme = document.getElementById("dugmeDodaj");
        dugme.style.visibility = 'hidden';

        var dugme2 = document.createElement('input');
        var roditelj = document.getElementById("nesto");
        dugme2.id = "forma2";
        dugme2.value = "Azuriraj";
        dugme2.type = "button";
        dugme2.class = "btn btn-outline-dark mt-auto";

        var dete1 = document.getElementById("forma2");
        if(dete1!=null)
            roditelj.removeChild(dete1);

        dugme2.onclick =ev => this.izmena();
        roditelj.appendChild(dugme2);

        var dugme3 = document.createElement('input');
        dugme3.id = "forma3";
        dugme3.value = "Obrisi";
        dugme3.type = "button";
        dugme3.class = "btn btn-outline-dark mt-auto";

        dete1 = document.getElementById("forma3");
        if(dete1!=null)
            roditelj.removeChild(dete1);



        dugme3.onclick = ev => this.brisanje();
        roditelj.appendChild(dugme3);
    }
    crtajAuto(gde) {
        var info = document.createElement("div");
        gde.appendChild(info);
        info.id = "deteZaBrisanje";
        info.innerHTML = `<div class="col mb-5">
                        <div class="card h-100">
                            <!-- Sale badge-->
                            <div class="badge bg-dark text-white position-absolute" style="top: 0.5rem; right: 0.5rem">${this.cena}€</div>
                            <!-- Product image-->
                            <img class="card-img-top" src="https://dummyimage.com/450x300/dee2e6/6c757d.jpg" alt="..." />
                            <!-- Product details-->
                            <div class="card-body p-4">
                                <div class="text-center">
                                    <!-- Product name-->
                                    <h5 class="fw-bolder">${this.marka} ${this.model}</h5>
                                    <h6 class="fw-bolder">${this.godP}.</h6>
                                    <h6 class="fw-bolder">${this.predjKM}km</h6>
                                </div>
                            </div>
                            <!-- Product actions-->
                            <div class="card-footer p-4 pt-0 border-top-0 bg-transparent">
                                <div id="roditelj" class="text-center"><a class="btn btn-outline-dark mt-auto" href="#">${this.prodavac}</a></div>
                            </div>
                        </div>
                    </div>`
        var dugme = document.createElement("div");
        dugme.className = "text-center";
        dugme.id = "dugme";
        var a = document.createElement("a");
        a.className = "btn btn-outline-dark mt-auto";
        a.href = "#";   
        a.innerHTML = "Izmeni";
        info.querySelector("#roditelj").appendChild(dugme);
        dugme.appendChild(a);
        dugme.onclick = ev => this.izmeniAuto();
        //dugme.addEventListener("click", this.izmeniAuto());
    }
}

function dodajAuto() {
    console.log("NESTO0");
    fetch("https://localhost:7276/Car/NoviAuto/" + document.getElementById("myDropdown").value, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "marka": document.getElementById("marka").value,
            "model": document.getElementById("model").value,
            "godProizvodnje": document.getElementById("god").value,
            "predjenihKM": document.getElementById("km").value,
            "cena": document.getElementById("cena").value,
            "ime": document.getElementById("myDropdown").value,
            "photo": null
        })
    }).then(p => {
        console.log(p);
        if (p.status == 200) {
            alert("uspesno ste dodali auto!");
            location.reload();
        }
        else {
            alert("Doslo je do greske!");
        }
    });
}