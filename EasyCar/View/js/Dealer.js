document.getElementById("dugmeDodajDealera").onclick = ev => this.dodajDealera();

class Dealer {
    constructor(id, ime, adresa, email, telefon) {
        this.id = id;
        this.ime = ime;
        this.adresa = adresa;
        this.email = email;
        this.telefon = telefon;
    }
    crtajDealera(gde) {
        var info = document.createElement("div");
        info.id = "roditeljDealer" + this.ime;
        info.class = "card-body p-4";
        gde.appendChild(info);
        console.log("Crtam " + this.ime);
        info.innerHTML = `
                                    <h6 class="fw-bolder">${this.ime}</h6>
                                    <h6 class="fw-bolder">${this.adresa}.</h6>
                                    <h6 class="fw-bolder">${this.email}   ${this.telefon}</h6>`
        var dugme = document.createElement("div");
        dugme.id = "dugmeD";
        var a = document.createElement("a");
        a.className = "btn btn-outline-dark mt-auto";
        a.href = "#";
        a.innerHTML = "Izmeni";
        document.querySelector("#roditeljDealer"+this.ime).appendChild(dugme);
        dugme.appendChild(a);
        dugme.onclick = ev => this.izmeniDealera();
    }
    izmeniDealera() {
            document.getElementById("ime").value = this.ime;
            document.getElementById("adresa").value = this.adresa;
            document.getElementById("email").value = this.email;
            document.getElementById("telefon").value = this.telefon;

            var dugme = document.getElementById("dugmeDodajDealera");
            dugme.style.visibility = 'hidden';

            var dugme2 = document.createElement('input');
            var roditelj = document.getElementById("nestoDrugo");
            dugme2.id = "forma22";
            dugme2.value = "Azuriraj";
            dugme2.type = "button";
            dugme2.class = "btn btn-outline-dark mt-auto";

            var dete1 = document.getElementById("forma22");
            if (dete1 != null)
                roditelj.removeChild(dete1);

            dugme2.onclick = ev => this.izmena();
            roditelj.appendChild(dugme2);

            var dugme3 = document.createElement('input');
            dugme3.id = "forma32";
            dugme3.value = "Obrisi";
            dugme3.type = "button";
            dugme3.class = "btn btn-outline-dark mt-auto";

            dete1 = document.getElementById("forma32");
            if (dete1 != null)
                roditelj.removeChild(dete1);



            dugme3.onclick = ev => this.brisanje();
            roditelj.appendChild(dugme3);
    }
    izmena() {
        console.log(this.id);
        fetch("https://localhost:7276/Dealer/PromeniDealera/", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "id": this.id,
                "ime": document.getElementById("ime").value,
                "adresa": document.getElementById("adresa").value,
                "email": document.getElementById("email").value,
                "telefon": document.getElementById("telefon").value
            })
        }).then(p => {
            console.log(p);
            if (p.status == 200) {
                alert("uspesno ste izmenili dealership!");
                location.reload();
            }
            else {
                alert("Doslo je do greske!");
            }
        });
    }
    brisanje() {
        fetch("https://localhost:7276/Dealer/ObrisiDealera/" + this.id, {
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
}
function dodajDealera() {
    fetch("https://localhost:7276/Dealer/DodajDealera/", {
        method: "POST",
        headers:
        {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "ime": document.getElementById("ime").value,
            "adresa": document.getElementById("adresa").value,
            "email": document.getElementById("email").value,
            "telefon": document.getElementById("telefon").value,
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

