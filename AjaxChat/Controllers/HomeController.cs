using AjaxChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AjaxChat.Controllers
{
	public class HomeController : Controller
	{
		private static ChatModel _chatModel;

		public ActionResult Index(string user, bool? logOn, bool? logOff, string chatMessage)
		{
			try
			{
				if (_chatModel == null)
					_chatModel = new ChatModel();

				//оставляем только последние 90 сообщений
				if (_chatModel.Messages.Count > 100)
					_chatModel.Messages.RemoveRange(0, 90);

				// если обычный запрос, просто возвращаем представление
				if (!Request.IsAjaxRequest())
				{
					return View(_chatModel);
				}
				else if (logOn != null && (bool)logOn)
				{
					//проверяем, существует ли уже такой пользователь
					if (_chatModel.Users.FirstOrDefault(u => u.Name == user) != null)
					{
						throw new Exception("Пользователь с таким ником уже существует");
					}
					else if (_chatModel.Users.Count > 10)
					{
						throw new Exception("Чат заполнен");
					}
					else
					{
						// добавляем в список нового пользователя
						_chatModel.Users.Add(new ChatUser()
						{
							Name = user,
							LoginTime = DateTime.Now,
							LastPing = DateTime.Now
						});

						// добавляем в список ссообщений сообщение о новом пользователе
						_chatModel.Messages.Add(new ChatMessage()
						{
							Text = user + " вошел в чат",
							Date = DateTime.Now
						});
					}

					return PartialView("_ChatRoom", _chatModel);
				}
				else if (logOff != null && (bool)logOff)
				{
					LogOff(_chatModel.Users.FirstOrDefault(u => u.Name == user));
					return PartialView("_ChatRoom", _chatModel);
				}
				else
				{
					ChatUser currentUser = _chatModel.Users.FirstOrDefault(u => u.Name == user);

					//для каждлого пользователя запоминаем воемя последнего обновления
					currentUser.LastPing = DateTime.Now;

					// удаляем неаквтивных пользователей
					List<ChatUser> removeThese = new List<ChatUser>();
					foreach (ChatUser usr in _chatModel.Users)
					{
						TimeSpan span = DateTime.Now - usr.LastPing;
						if (span.TotalSeconds > 15)
							removeThese.Add(usr);
					}
					foreach (ChatUser u in removeThese)
					{
						LogOff(u);
					}

					// добавляем в список сообщений новое сообщение
					if (!string.IsNullOrEmpty(chatMessage))
					{
						_chatModel.Messages.Add(new ChatMessage()
						{
							User = currentUser,
							Text = chatMessage,
							Date = DateTime.Now
						});
					}

					return PartialView("_History", _chatModel);
				}
			}
			catch (Exception ex)
			{
				//в случае ошибки посылаем статусный код 500
				Response.StatusCode = 500;
				return Content(ex.Message);
			}
		}

		// при выходе пользователя удаляем его из списка
		public void LogOff(ChatUser user)
		{
			_chatModel.Users.Remove(user);
			_chatModel.Messages.Add(new ChatMessage()
			{
				Text = user.Name + " покинул чат.",
				Date = DateTime.Now
			});
		}
	}
}