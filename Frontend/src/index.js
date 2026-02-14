//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
//-------------- MOSTRAR / OCULTAR MENU LATERAL IZQUIERDO  ------------//
document.addEventListener("DOMContentLoaded", () => {
  const menuBtn = document.querySelector(".menu-burguer");
  const sidebar = document.querySelector(".sidebar");

  menuBtn.addEventListener("click", () => {
    sidebar.classList.remove('d-none')
    sidebar.classList.toggle("open");
  });
});

//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
//--------------  MOSTRAR INPUT Y ACTIVAR BUSCADOR AUTMATICO  ----------//
document.addEventListener("DOMContentLoaded", () => {
  const btnLupa = document.getElementById("search");          // la lupa
  const inputBuscar = document.getElementById("buscarSearch"); // el input
  const resultadosContainer = document.createElement("div");   // contenedor resultados
  resultadosContainer.id = "resultadosBusqueda";
  resultadosContainer.style.position = "absolute";
  resultadosContainer.style.top = "40px";
  resultadosContainer.style.left = "10px";
  resultadosContainer.style.width = "90%";
  resultadosContainer.style.background = "#fff";
  resultadosContainer.style.border = "1px solid #ccc";
  resultadosContainer.style.zIndex = "1000";
  resultadosContainer.style.display = "none";
  document.body.appendChild(resultadosContainer);

  // Función para mostrar resultados
  function mostrarResultados(resultados) {
    if (!resultados.length) {
      resultadosContainer.innerHTML = "<p style='padding:8px;'>Sin resultados</p>";
    } else {
      resultadosContainer.innerHTML = resultados.map(r => `
        <div style="padding:8px; border-bottom:1px solid #eee; cursor:pointer;">
          ${r.nombre} (${r.tabla})
        </div>
      `).join("");
    }
    resultadosContainer.style.display = "block";
  }

  // Función para buscar en BD (simulada con fetch)
  async function buscarEnBD(query) {
    try {
      const response = await fetch(`/buscar?q=${encodeURIComponent(query)}`);
      if (!response.ok) throw new Error("Error en la búsqueda");
      const data = await response.json(); // el backend devuelve [{nombre:"X", tabla:"Usuarios"}, ...]
      mostrarResultados(data);
    } catch (err) {
      console.error("Error:", err);
    }
  }

  // Evento de lupa
  btnLupa.addEventListener("click", () => {
    const isMobile = window.innerWidth < 768;
    if (isMobile) {
      if (inputBuscar.style.display === "none" || inputBuscar.style.display === "") {
        inputBuscar.style.display = "flex";  
        btnLupa.classList.add("open");
      } else {
        inputBuscar.style.display = "none";  
        btnLupa.classList.remove("open");
        resultadosContainer.style.display = "none"; // ocultar resultados
      }
    } else {
      // en escritorio no oculta/mostrar, siempre visible
      inputBuscar.focus();
    }
  });

  // Evento al escribir
  inputBuscar.addEventListener("input", (e) => {
    const query = e.target.value.trim();
    if (query.length > 1) {
      buscarEnBD(query); // consulta automática
    } else {
      resultadosContainer.style.display = "none";
    }
  });

  // Ajustar según ancho al inicio
  function ajustarBuscador() {
    if (window.innerWidth >= 768) {
      inputBuscar.style.display = "flex";
    } else {
      inputBuscar.style.display = "none";
      resultadosContainer.style.display = "none";
    }
  }
  window.addEventListener("resize", ajustarBuscador);
  ajustarBuscador();
});

//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
//-------------- MOSTRAR / OCULTAR MENU DE NOTIFICACIONES  ------------//
document.addEventListener("DOMContentLoaded", () => {
  const notifyUsers = document.getElementById("notificaciones-user");          // la lupa
  const containerNotifUsers = document.getElementById("container-notify-users");

  notifyUsers.addEventListener('click', () => {
    if (containerNotifUsers.style.display === "none") {
      containerNotifUsers.style.display = "flex";  // lo muestra
      notifyUsers.classList.toggle("open");
    } else {
      containerNotifUsers.style.display = "none";   // lo oculta
    }
});
});

//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
//-------------- MOSTRAR / OCULTAR CONTAINER CUENTA USUARIOS  ------------//
document.addEventListener("DOMContentLoaded", () => {
  const showAccount = document.getElementById("showAccount");          
  const containerCuenta = document.getElementById("contenedorCuenta");

  showAccount.addEventListener("click", () => {
    containerCuenta.classList.toggle("open");
  });
});

//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
document.addEventListener("DOMContentLoaded", () => {

  // CONTENEDORES
  const contenedores = {
    home: document.getElementById("contenedor-Home"),
    publicar: document.getElementById("contenedor-Publicacion"),
    historial: document.getElementById("contenedor-Historial"),
    misPublisher: document.getElementById("contenedor-MisPublisher"),
    misVideos: document.getElementById("contenedor-MisVideos"),
    misImagenes: document.getElementById("contenedor-MisImagenes"),
    miConfiguracion: document.getElementById("contenedor-Configuracion"),
    miReportDenuncias: document.getElementById("report-denuncias"),
    miHistoryDenuncias: document.getElementById("historial-denuncias"),
    miHelpAyuda: document.getElementById("contenedor-help"),
    miAccountPersonal: document.getElementById("contenedor-account-personal"),
    miFormSuscripcion: document.getElementById("container-form-suscrip"),
    miLoginUser: document.getElementById("container-form-login"),
    miValidarPass: document.getElementById("form-validate-pass"),
    miSendCode: document.getElementById("form-send-code"),
    miAddAccountPers: document.getElementById("contenedor-add-account")
  };

  // BOTONES DEL MENÚ
  const botones = {
    home: document.getElementById("casaHome"),
    publicar: document.getElementById("camaraVideo"),
    historial: document.getElementById("miHistory"),
    misPublisher: document.getElementById("misPublicaciones"),
    misVideos: document.getElementById("misVideos"),
    misImagenes: document.getElementById("misImages"),
    miConfiguracion: document.getElementById("misConfigures"),
    miReportDenuncias: document.getElementById("misDenuncias"),
    miHistoryDenuncias: document.getElementById("miHistorialDenuncias"),
    miHelpAyuda: document.getElementById("helpAyudaWeb"),
    miAccountPersonal: document.getElementById("btnCuentaProfessionals"),
    miFormSuscripcion: document.getElementById("btnSuscripcionCuenta"),
    miLoginUser: document.getElementById("validate-submit-pass"),
    miValidarPass: document.getElementById("validar-pass"),
    miSendCode: document.getElementById("validate-submit-pass"),
    miAddAccountPers: document.getElementById("btnCambiarCuenta")
  };

  // FUNCIÓN ÚNICA PARA MOSTRAR CONTENEDOR
  function mostrarContenedor(nombre) {

    // Ocultar todos
    Object.values(contenedores).forEach(c => {
      c.classList.remove("activo");
    });

    // Quitar activo a todos los botones
    Object.values(botones).forEach(b => {
      b.classList.remove("menu-activo");
    });

    // Mostrar el elegido
    const cont = contenedores[nombre];
    const btn = botones[nombre];

    if (cont) {
      cont.classList.add("activo");
    }

    if (btn) {
      btn.classList.add("menu-activo");
    }
  }

  // EVENTOS DE MENÚ
  botones.home?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("home");
  });

  botones.publicar?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("publicar");
  });

  botones.historial?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("historial");
  });

  botones.misPublisher?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("misPublisher");
  });

  botones.misVideos?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("misVideos");
  });

  botones.misImagenes?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("misImagenes");
  });

  botones.miConfiguracion?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miConfiguracion");
  });

   botones.miReportDenuncias?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miReportDenuncias");
  });

  botones.miHistoryDenuncias?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miHistoryDenuncias");
  });

  botones.miHelpAyuda?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miHelpAyuda");
  });

  botones.miAccountPersonal?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miAccountPersonal");
  });

  botones.miFormSuscripcion?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miFormSuscripcion");
  });

  botones.miLoginUser?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miLoginUser");
  });

  botones.miValidarPass?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miValidarPass");
  });

  botones.miSendCode?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miSendCode");
  });

  
  botones.miAddAccountPers?.addEventListener("click", e => {
    e.preventDefault();
    mostrarContenedor("miAddAccountPers");
  });

  // BOTONES X DE CIERRE
  document.addEventListener("click", e => {

    if (e.target.id === "cerrarHome") cerrar("home");
    if (e.target.id === "publicarCerrar") cerrar("publicar");
    if (e.target.id === "historialCerrar") cerrar("historial");
    if (e.target.id === "misPublisherCerrar") cerrar("misPublisher");
    if (e.target.id === "misVideosCerrar") cerrar("misVideos");
    if (e.target.id === "misImagenesCerrar") cerrar("misImagenes");
    if (e.target.id === "miConfiguracionCerrar") cerrar("miConfiguracion");
    if (e.target.id === "miDenunciasCerrar") cerrar("miReportDenuncias");
    if (e.target.id === "miHistoryDenunciasCerrar") cerrar("miHistoryDenuncias");
    if (e.target.id === "miHelpAyudaCerrar") cerrar("miHelpAyuda");
    if (e.target.id === "miCuentaPersonalCerrar") cerrar("miAccountPersonal");
    if (e.target.id === "miRegisterCerrar") cerrar("miSuscripPersonal");
    if (e.target.id === "miLoginCerrar") cerrar("miLoginUser");
    if (e.target.id === "miAddCuentaCerrar") cerrar("miAddAccountPers");
  });

  function cerrar(nombre) {
    const cont = contenedores[nombre];
    const btn = botones[nombre];

    if (cont) cont.classList.remove("activo");
    if (btn) btn.classList.remove("menu-activo");
  }

});

//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
//--------------------------MOSTRAR DENUNCIAS
// document.addEventListener("DOMContentLoaded", () => {

//   const mostrarDenuncias = document.getElementById("report-post-main");
//   const containerDenuncias = document.getElementById("report-denuncias");

//   if (!mostrarDenuncias || !containerDenuncias) {
//     console.error("No se encontraron los elementos de Denuncias");
//     return;
//   }

//   mostrarDenuncias.addEventListener("click", () => {
//     containerDenuncias.classList.toggle("activo");
//   });

// });

// document.addEventListener("click", function (e) {

//   // BOTÓN DESDE MAIN O MENÚ
//   const btn = e.target.closest(".btn-abrir-denuncia, #view-report-main");

//   if (btn) {
//     const overlay = document.getElementById("overlay-denuncias");
//     overlay.classList.add("activo");
//     return;
//   }

//   // CERRAR
//   if (e.target.id === "cerrar-denuncias" ||
//       e.target.id === "overlay-denuncias") {

//     document.getElementById("overlay-denuncias")
//             .classList.remove("activo");
//   }

// });




//--OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO--//
//--------------------------MOSTRAR REDES SOCIALES ---------------------//
document.addEventListener("click", function (e) {
  const link = e.target.closest(".social-link");
  if (!link) return;

  e.preventDefault();

  const app = link.dataset.app;

  const links = {
    instagram: {
      app: "instagram://app",
      web: "https://www.instagram.com/"
    },
    facebook: {
      app: "fb://facewebmodal/f?href=https://www.facebook.com/",
      web: "https://www.facebook.com/"
    },
    youtube: {
      app: "youtube://www.youtube.com/",
      web: "https://www.youtube.com/"
    },
    linkedin: {
      app: "linkedin://home",
      web: "https://www.linkedin.com/"
    }
  };

  if (!links[app]) return;

  const start = Date.now();
  window.location.href = links[app].app;

  setTimeout(() => {
    if (Date.now() - start < 2000) {
      window.location.href = links[app].web;
    }
  }, 1500);
});


















// containerReport.classList.toggle("open");


