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

//Отображаем сообщение об ошибке
function ChatOnFailure(result) {
	
}

// при успешном получении ответа с сервера
function ChatOnSuccess(result) {
	
}

//скролл к низу окна
function Scroll() {
	var win = $('#Messages');
	var height = win[0].scrollHeight;
	win.scrollTop(height);
}

//отображение времени последнего обновления чата
function ShowLastRefresh() {
	var dt = new Date();
	var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
	$("#LastRefresh").text("Последнее обновление было в " + time);
}