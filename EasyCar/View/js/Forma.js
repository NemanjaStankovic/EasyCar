class Forma {
    constructor() {
    }

    crtajOpcije(listPlaceva, imeDropDown)
    {
        var se = document.getElementById(imeDropDown);
        var op;
        listPlaceva.forEach(plac => {
            op = document.createElement("option");
            op.innerHTML = plac.ime;
            op.value = plac.ime;
            se.appendChild(op);
        }) 
    }
}
