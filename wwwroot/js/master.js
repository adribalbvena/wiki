$(window).ready(function () {

	//let contElement = $(".myTd").length;
	
	//var urlCurrent = window.location;
	//var url = "https://localhost:44349/Home/SearchArticle";

	//if (urlCurrent == url) {

	//	if (contElement == 0) {
	//		swal("Error en busqueda!", "No se consiguio nada", "error");
	//	} else {
	//		swal("Busqueda exitosa!", "Elementos encontrados: " + contElement, "success");
	//	}
	//}


	/*Funcionalidad de busquedas en tablas*/
	$("#myInput").on("keyup", function () {
		var value = $(this).val().toLowerCase();
		$("#myTable tr").filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
		});
	});
	

});