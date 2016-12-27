$(document).ready(function () {
	// логин
	$("#btnLogin").click(function () {
		var nickName = $("#txtUserName").val();
		if (nickName) {
			// формируем ссылку с параметрами, по которой идет обращение
			var href = "/Home?user=" + encodeURIComponent(nickName);
			href = href + "&logOn=true";
			$("#LoginButton").attr("href", href).click();

			//установка поля с ником пользователя
			$("#Username").text(nickName);
		}
	});
});

//при успешном входе загружаем сообщения
function LoginOnSuccess(result) {
	
}

//при ошибке отображаем сообщение об ошибке при логине
function LoginOnFailure(result) {
	
}